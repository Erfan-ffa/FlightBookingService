using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UseLowerCaseNaming : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_FlightId",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Flights_FlightId1",
                table: "Bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_Bookings_Passengers_PassengerId",
                table: "Bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Passengers",
                table: "Passengers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Flights",
                table: "Flights");

            migrationBuilder.DropIndex(
                name: "IX_Flights_FlightNumber",
                table: "Flights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings");

            migrationBuilder.DropIndex(
                name: "IX_Bookings_FlightId1",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "FlightId1",
                table: "Bookings");

            migrationBuilder.RenameTable(
                name: "Passengers",
                newName: "passengers");

            migrationBuilder.RenameTable(
                name: "Flights",
                newName: "flights");

            migrationBuilder.RenameTable(
                name: "Bookings",
                newName: "bookings");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "passengers",
                newName: "phonenumber");

            migrationBuilder.RenameColumn(
                name: "PassportNumber",
                table: "passengers",
                newName: "passportnumber");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "passengers",
                newName: "modifiedat");

            migrationBuilder.RenameColumn(
                name: "Fullname",
                table: "passengers",
                newName: "fullname");

            migrationBuilder.RenameColumn(
                name: "Email",
                table: "passengers",
                newName: "email");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "passengers",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "passengers",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "flights",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Origin",
                table: "flights",
                newName: "origin");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "flights",
                newName: "modifiedat");

            migrationBuilder.RenameColumn(
                name: "FlightNumber",
                table: "flights",
                newName: "flightnumber");

            migrationBuilder.RenameColumn(
                name: "Destination",
                table: "flights",
                newName: "destination");

            migrationBuilder.RenameColumn(
                name: "DepartureTime",
                table: "flights",
                newName: "departuretime");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "flights",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "AvailableSeats",
                table: "flights",
                newName: "availableseats");

            migrationBuilder.RenameColumn(
                name: "ArrivalTime",
                table: "flights",
                newName: "arrivaltime");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "flights",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "SeatNumber",
                table: "bookings",
                newName: "seatnumber");

            migrationBuilder.RenameColumn(
                name: "PassengerId",
                table: "bookings",
                newName: "passengerid");

            migrationBuilder.RenameColumn(
                name: "ModifiedAt",
                table: "bookings",
                newName: "modifiedat");

            migrationBuilder.RenameColumn(
                name: "FlightId",
                table: "bookings",
                newName: "flightid");

            migrationBuilder.RenameColumn(
                name: "CreatedAt",
                table: "bookings",
                newName: "createdat");

            migrationBuilder.RenameColumn(
                name: "BookingDate",
                table: "bookings",
                newName: "bookingdate");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "bookings",
                newName: "id");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_PassengerId",
                table: "bookings",
                newName: "IX_bookings_passengerid");

            migrationBuilder.RenameIndex(
                name: "IX_Bookings_FlightId",
                table: "bookings",
                newName: "IX_bookings_flightid");

            migrationBuilder.AlterColumn<string>(
                name: "passportnumber",
                table: "passengers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(9)",
                oldMaxLength: 9);

            migrationBuilder.AlterColumn<string>(
                name: "fullname",
                table: "passengers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(250)",
                oldMaxLength: 250);

            migrationBuilder.AlterColumn<string>(
                name: "email",
                table: "passengers",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(150)",
                oldMaxLength: 150);

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "flights",
                type: "numeric",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric(18,2)",
                oldPrecision: 18,
                oldScale: 2);

            migrationBuilder.AlterColumn<string>(
                name: "origin",
                table: "flights",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "flightnumber",
                table: "flights",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "destination",
                table: "flights",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(100)",
                oldMaxLength: 100);

            migrationBuilder.AddPrimaryKey(
                name: "PK_passengers",
                table: "passengers",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_flights",
                table: "flights",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_bookings",
                table: "bookings",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_flights_flightid",
                table: "bookings",
                column: "flightid",
                principalTable: "flights",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_bookings_passengers_passengerid",
                table: "bookings",
                column: "passengerid",
                principalTable: "passengers",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_bookings_flights_flightid",
                table: "bookings");

            migrationBuilder.DropForeignKey(
                name: "FK_bookings_passengers_passengerid",
                table: "bookings");

            migrationBuilder.DropPrimaryKey(
                name: "PK_passengers",
                table: "passengers");

            migrationBuilder.DropPrimaryKey(
                name: "PK_flights",
                table: "flights");

            migrationBuilder.DropPrimaryKey(
                name: "PK_bookings",
                table: "bookings");

            migrationBuilder.RenameTable(
                name: "passengers",
                newName: "Passengers");

            migrationBuilder.RenameTable(
                name: "flights",
                newName: "Flights");

            migrationBuilder.RenameTable(
                name: "bookings",
                newName: "Bookings");

            migrationBuilder.RenameColumn(
                name: "phonenumber",
                table: "Passengers",
                newName: "PhoneNumber");

            migrationBuilder.RenameColumn(
                name: "passportnumber",
                table: "Passengers",
                newName: "PassportNumber");

            migrationBuilder.RenameColumn(
                name: "modifiedat",
                table: "Passengers",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "fullname",
                table: "Passengers",
                newName: "Fullname");

            migrationBuilder.RenameColumn(
                name: "email",
                table: "Passengers",
                newName: "Email");

            migrationBuilder.RenameColumn(
                name: "createdat",
                table: "Passengers",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Passengers",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "Flights",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "origin",
                table: "Flights",
                newName: "Origin");

            migrationBuilder.RenameColumn(
                name: "modifiedat",
                table: "Flights",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "flightnumber",
                table: "Flights",
                newName: "FlightNumber");

            migrationBuilder.RenameColumn(
                name: "destination",
                table: "Flights",
                newName: "Destination");

            migrationBuilder.RenameColumn(
                name: "departuretime",
                table: "Flights",
                newName: "DepartureTime");

            migrationBuilder.RenameColumn(
                name: "createdat",
                table: "Flights",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "availableseats",
                table: "Flights",
                newName: "AvailableSeats");

            migrationBuilder.RenameColumn(
                name: "arrivaltime",
                table: "Flights",
                newName: "ArrivalTime");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Flights",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "seatnumber",
                table: "Bookings",
                newName: "SeatNumber");

            migrationBuilder.RenameColumn(
                name: "passengerid",
                table: "Bookings",
                newName: "PassengerId");

            migrationBuilder.RenameColumn(
                name: "modifiedat",
                table: "Bookings",
                newName: "ModifiedAt");

            migrationBuilder.RenameColumn(
                name: "flightid",
                table: "Bookings",
                newName: "FlightId");

            migrationBuilder.RenameColumn(
                name: "createdat",
                table: "Bookings",
                newName: "CreatedAt");

            migrationBuilder.RenameColumn(
                name: "bookingdate",
                table: "Bookings",
                newName: "BookingDate");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Bookings",
                newName: "Id");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_passengerid",
                table: "Bookings",
                newName: "IX_Bookings_PassengerId");

            migrationBuilder.RenameIndex(
                name: "IX_bookings_flightid",
                table: "Bookings",
                newName: "IX_Bookings_FlightId");

            migrationBuilder.AlterColumn<string>(
                name: "PassportNumber",
                table: "Passengers",
                type: "character varying(9)",
                maxLength: 9,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Fullname",
                table: "Passengers",
                type: "character varying(250)",
                maxLength: 250,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Email",
                table: "Passengers",
                type: "character varying(150)",
                maxLength: 150,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "Flights",
                type: "numeric(18,2)",
                precision: 18,
                scale: 2,
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "numeric");

            migrationBuilder.AlterColumn<string>(
                name: "Origin",
                table: "Flights",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FlightNumber",
                table: "Flights",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Destination",
                table: "Flights",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<long>(
                name: "FlightId1",
                table: "Bookings",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Passengers",
                table: "Passengers",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Flights",
                table: "Flights",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Bookings",
                table: "Bookings",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Flights_FlightNumber",
                table: "Flights",
                column: "FlightNumber");

            migrationBuilder.CreateIndex(
                name: "IX_Bookings_FlightId1",
                table: "Bookings",
                column: "FlightId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_FlightId",
                table: "Bookings",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Flights_FlightId1",
                table: "Bookings",
                column: "FlightId1",
                principalTable: "Flights",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Bookings_Passengers_PassengerId",
                table: "Bookings",
                column: "PassengerId",
                principalTable: "Passengers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
