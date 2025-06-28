using LoadExpressApi.Application.Abstraction.Repositories;
using LoadExpressApi.Persistence.Context;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoadExpressApi.Persistence.Repositories
{
    public class RepositoryAsync<T> : IRepositoryAsync<T> where T : class, new()
    {
        private readonly ApplicationDbContext _context;
        internal DbSet<T> DbSet;
        public RepositoryAsync(ApplicationDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public async Task AddAsync(T entity) => await DbSet.AddAsync(entity);

        public async Task AddRangeAsync(List<T> entity) => await DbSet.AddRangeAsync(entity);

        public async Task<bool> Exist(Expression<Func<T, bool>> expression) => await DbSet.AnyAsync(expression);

        public async Task<T> GetAsync(Expression<Func<T, bool>> expression) => await DbSet.SingleOrDefaultAsync(expression);

        //public async Task<IList<T>> GetUnpaginatedListAsync(Expression<Func<T, bool>> expression = null) => await DbSet.AsNoTracking().Where(expression).ToListAsync();

        public async Task<IList<T>> GetUnpaginatedListAsync(Expression<Func<T, bool>> expression = null)
        {
            var query = DbSet.AsNoTracking();
            if (expression != null)
                query = query.Where(expression);
            return await query.ToListAsync();
        }

        public async Task<int> SaveChangesAsync() => await _context.SaveChangesAsync();

        public void Update(T entity) => _context.Entry(entity).State = EntityState.Modified;

        public void UpdateRanges(List<T> entity) => DbSet.UpdateRange(entity);
    }
}
