using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace LoadExpressApi.Application.Abstraction.Repositories
{


    public interface IRepositoryAsync<T> where T : class, new()
    {
        Task AddAsync(T entity);
        //Task AddRangeAsync(List<T> entity);
        Task<T> GetAsync(Expression<Func<T, bool>> expression);
        Task<IList<T>> GetUnpaginatedListAsync(Expression<Func<T, bool>> expression = null);
        void Update(T entity);
        void UpdateRanges(List<T> entity);
        Task<bool> Exist(Expression<Func<T, bool>> expression);
        Task<int> SaveChangesAsync();
    }
}
