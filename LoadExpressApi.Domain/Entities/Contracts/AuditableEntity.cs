namespace LoadExpressApi.Domain.Entities.Contracts;

public abstract class AuditableEntity : AuditableEntity<string>
{
}

public abstract class AuditableEntity<T> : BaseEntity, IAuditableEntity, ISoftDelete
{
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; private set; } = DateTime.UtcNow;
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; } = DateTime.UtcNow;
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }

    protected AuditableEntity() => CreatedOn = DateTime.UtcNow;
}