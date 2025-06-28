using LoadExpressApi.Application.Auditing;
using LoadExpressApi.Application.Common;
using LoadExpressApi.Application.Common.Interface;
using LoadExpressApi.Domain.Entities;
using LoadExpressApi.Domain.Entities.Contracts;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using LoadExpressApi.Domain.Entities;

namespace LoadExpressApi.Persistence.Context
{
    public abstract class BaseDbContext : IdentityDbContext<User, Role, string, IdentityUserClaim<string>, IdentityUserRole<string>, IdentityUserLogin<string>, IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        private readonly ICurrentUser _currentUser;
        private readonly ISerializerService _serializer;
        protected BaseDbContext(DbContextOptions options, ICurrentUser currentUser, ISerializerService serializer) : base(options)
        {
            _currentUser = currentUser;
            _serializer = serializer;
        }
        public DbSet<Trail> AuditTrails => Set<Trail>();
        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.AppendGlobalQueryFilter<ISoftDelete>(x => x.DeletedOn == null);
            base.OnModelCreating(builder);
            builder.ApplyConfigurationsFromAssembly(GetType().Assembly);
            builder.HasDefaultSchema("Identity");
            builder.Entity<User>(entity => entity.ToTable(name: "Users"));
            builder.Entity<Role>(entity => entity.ToTable(name: "Roles"));
            builder.Entity<IdentityUserRole<string>>(entity => entity.ToTable(name: "UserRoles"));
            builder.Entity<IdentityUserClaim<string>>(entity => entity.ToTable(name: "UserClaims"));
            builder.Entity<IdentityUserLogin<string>>(entity => entity.ToTable(name: "UserLogins"));
            builder.Entity<IdentityRoleClaim<string>>(entity => entity.ToTable(name: "RoleClaims"));
            builder.Entity<IdentityUserToken<string>>(entity => entity.ToTable(name: "UserTokens"));
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            var userId = _currentUser.GetUserId();

            var auditEntries = HandleAuditingBeforeSaveChanges(userId);
            var result = await base.SaveChangesAsync(cancellationToken);
            await HandleAuditingAfterSaveChangesAsync(auditEntries, cancellationToken);

            return result;
        }


        private List<AuditTrail> HandleAuditingBeforeSaveChanges(string userId)
        {
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>().ToList())
            {
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedBy = userId;
                        entry.Entity.LastModifiedBy = userId;
                        break;

                    case EntityState.Modified:
                        entry.Entity.LastModifiedOn = DateTime.UtcNow;
                        entry.Entity.LastModifiedBy = userId;
                        break;

                    case EntityState.Deleted:
                        if (entry.Entity is ISoftDelete softDelete)
                        {
                            softDelete.DeletedBy = userId;
                            softDelete.DeletedOn = DateTime.UtcNow;
                            entry.State = EntityState.Modified;
                        }

                        break;
                }
            }

            ChangeTracker.DetectChanges();

            var trailEntries = new List<AuditTrail>();
            foreach (var entry in ChangeTracker.Entries<IAuditableEntity>()
                .Where(e => e.State is EntityState.Added or EntityState.Deleted or EntityState.Modified)
                .ToList())
            {
                var trailEntry = new AuditTrail(entry, _serializer)
                {
                    TableName = entry.Entity.GetType().Name,
                    UserId = userId
                };
                trailEntries.Add(trailEntry);
                foreach (var property in entry.Properties)
                {
                    if (property.IsTemporary)
                    {
                        trailEntry.TemporaryProperties.Add(property);
                        continue;
                    }

                    var propertyName = property.Metadata.Name;
                    if (property.Metadata.IsPrimaryKey())
                    {
                        trailEntry.KeyValues[propertyName] = property.CurrentValue;
                        continue;
                    }

                    switch (entry.State)
                    {
                        case EntityState.Added:
                            trailEntry.TrailType = TrailType.Create;
                            trailEntry.NewValues[propertyName] = property.CurrentValue;
                            trailEntry.IPAddress = GetIPAddress.GetLocalIPAddress();
                            break;

                        case EntityState.Deleted:
                            trailEntry.TrailType = TrailType.Delete;
                            trailEntry.OldValues[propertyName] = property.OriginalValue;
                            trailEntry.IPAddress = GetIPAddress.GetLocalIPAddress();
                            break;

                        case EntityState.Modified:
                            if (property.IsModified && entry.Entity is ISoftDelete && property.OriginalValue == null && property.CurrentValue != null)
                            {
                                trailEntry.ChangedColumns.Add(propertyName);
                                trailEntry.TrailType = TrailType.Delete;
                                trailEntry.OldValues[propertyName] = property.OriginalValue;
                                trailEntry.NewValues[propertyName] = property.CurrentValue;
                                trailEntry.IPAddress = GetIPAddress.GetLocalIPAddress();
                            }
                            else if (property.IsModified && property.OriginalValue?.Equals(property.CurrentValue) == false)
                            {
                                trailEntry.ChangedColumns.Add(propertyName);
                                trailEntry.TrailType = TrailType.Update;
                                trailEntry.OldValues[propertyName] = property.OriginalValue;
                                trailEntry.NewValues[propertyName] = property.CurrentValue;
                            }

                            break;
                    }
                }
            }

            foreach (var auditEntry in trailEntries.Where(e => !e.HasTemporaryProperties))
            {
                var auditEntity = auditEntry.ToAuditTrail();
                auditEntity.OldValues = SensitiveInfo.MaskSensitiveInfo(auditEntity.OldValues);
                auditEntity.NewValues = SensitiveInfo.MaskSensitiveInfo(auditEntity.NewValues);
                AuditTrails.Add(auditEntity);
            }

            return trailEntries.Where(e => e.HasTemporaryProperties).ToList();
        }

        private Task HandleAuditingAfterSaveChangesAsync(List<AuditTrail> trailEntries, CancellationToken cancellationToken = new())
        {
            if (trailEntries == null || trailEntries.Count == 0)
            {
                return Task.CompletedTask;
            }

            foreach (var entry in trailEntries)
            {
                foreach (var prop in entry.TemporaryProperties)
                {
                    if (prop.Metadata.IsPrimaryKey())
                    {
                        entry.KeyValues[prop.Metadata.Name] = prop.CurrentValue;
                        entry.IPAddress = GetIPAddress.GetLocalIPAddress();
                    }
                    else
                    {
                        entry.IPAddress = GetIPAddress.GetLocalIPAddress();
                        entry.NewValues[prop.Metadata.Name] = prop.CurrentValue;
                    }
                }
                var auditEntry = entry.ToAuditTrail();
                auditEntry.OldValues = SensitiveInfo.MaskSensitiveInfo(auditEntry.OldValues);
                auditEntry.NewValues = SensitiveInfo.MaskSensitiveInfo(auditEntry.NewValues);
                AuditTrails.Add(entry.ToAuditTrail());
            }

            return SaveChangesAsync(cancellationToken);
        }
    }
}
