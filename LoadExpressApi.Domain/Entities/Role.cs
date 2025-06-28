using LoadExpressApi.Domain.Entities.Contracts;
using Microsoft.AspNetCore.Identity;

namespace LoadExpressApi.Domain.Entities;

public class Role : IdentityRole, IAuditableEntity, ISoftDelete
{
    public string Description { get; set; }
    public string? CreatedBy { get; set; }
    public DateTime? CreatedOn { get; set; }
    public string? LastModifiedBy { get; set; }
    public DateTime? LastModifiedOn { get; set; }
    public DateTime? DeletedOn { get; set; }
    public string? DeletedBy { get; set; }
    public bool IsDeleted { get; set; }
}