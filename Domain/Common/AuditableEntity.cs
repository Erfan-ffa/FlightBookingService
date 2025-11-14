namespace Domain.Common;

public interface IEntity
{
}

public abstract class AuditableEntity<TKey> : IEntity
{
    public TKey Id { get; protected set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? ModifiedAt { get; set; }
}


public abstract class AuditableEntity : AuditableEntity<long>
{

}