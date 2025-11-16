# Booking Flow Documentation

## Overview

This flight booking system implements a high-performance, cache-first seat assignment mechanism designed to handle high-concurrency booking scenarios without database locks. The system uses Redis as the primary seat allocation store and falls back to pessimistic database locking when Redis is unavailable.

## Architecture

### Core Design Principle

**Cache-First Seat Pre-Allocation**: Seats are pre-allocated and stored in Redis when a flight is created. During booking, seat IDs are atomically popped from Redis, eliminating the need for database locks and improving throughput under high concurrency.

### Components

- **Primary Path**: Redis-based atomic seat assignment (lock-free)
- **Fallback Path**: Database pessimistic locking (when Redis unavailable)
- **Recovery Mechanism**: Automatic cache rebuild when Redis comes back online

## Booking Flow

### 1. Happy Path (Redis Available)

```  
Request → Validate Flight → Pop Seat from Redis → Create Booking → Update DB → Clear Cache  
```  

**Steps:**

1. **Flight Validation**: Verify flight exists in database
2. **Atomic Seat Assignment**: Pop a seat ID from Redis list using `PopAsync`
- Key format: `flight:{flightId}:seats`
- Returns: Next available seat ID or `null` if cache empty
3. **Booking Creation**: Create booking with assigned seat ID
4. **Database Update**:
    - Decrease available seats counter
    - Persist booking record
5. **Cache Invalidation**: Clear flight search cache

**Performance Benefits:**
- No database locks required
- Atomic operations via Redis
- High throughput for concurrent requests

### 2. Fallback Path (Redis Unavailable or Cache Miss)

```  
Request → Validate Flight → Detect Cache Miss → Acquire DB Lock → Assign Seat → Create Booking → Trigger Rebuild  
```  

**Steps:**

1. **Cache Miss Detection**: `PopAsync` returns `null`
2. **Pre-flight Check**: Verify `flight.AvailableSeats > 0`
3. **Health Check**: Ping Redis to determine if it's operational
4. **Pessimistic Locking**: Acquire row-level lock on flight record
    - Uses `GetWithLockAsync` (likely `SELECT ... FOR UPDATE`)
5. **Manual Seat Assignment**:
    - Query database for booked seats
    - Calculate next available seat number
6. **Booking Creation**: Create booking and commit transaction
7. **Async Rebuild** (if Redis is healthy): Trigger cache reconstruction

**Fallback Triggers:**
- Redis is down/unreachable
- Cache not yet populated for flight
- Cache depleted (all seats popped but booking failed)

### 3. Cache Rebuild Process

```  
Acquire Distributed Lock → Check if Already Rebuilt → Query Free Seats → Bulk Push to Redis  
```  

**Steps:**

1. **Distributed Lock**: Acquire RedLock to prevent concurrent rebuilds
    - Key format: `booking:lock:{flightId}`
- Expiration: 5 seconds
    - Wait/Retry: 5s wait, 3s retry interval
2. **Duplicate Check**: Verify cache doesn't already exist
3. **Seat Calculation**:
    - Query all booked seat numbers from database
    - Calculate total seats: `bookedCount + availableSeats`
- Generate list: `[1..totalSeats] - bookedSeats`
4. **Bulk Population**: Push all available seats to Redis list
5. **Resume Normal Flow**: Subsequent bookings use Redis path

## Concurrency Handling

### Race Condition Mitigation

1. **Redis Atomic Operations**: `PopAsync` ensures only one request gets each seat ID
2. **Database Locks**: `GetWithLockAsync` prevents double-booking during fallback
3. **Distributed Locks**: RedLock prevents duplicate cache rebuilds across instances
4. **Optimistic Seat Counter**: `AvailableSeats` acts as a fast pre-check  
