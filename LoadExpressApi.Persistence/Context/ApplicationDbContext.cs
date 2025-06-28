using LoadExpressApi.Application.Common.Interface;
using Microsoft.EntityFrameworkCore;

namespace LoadExpressApi.Persistence.Context
{
    public class ApplicationDbContext(DbContextOptions dbContextOptions, ICurrentUser currentUser, ISerializerService serializer) : BaseDbContext(dbContextOptions, currentUser, serializer)
    {
       
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("1791 Assessment");
        }
    }
}
