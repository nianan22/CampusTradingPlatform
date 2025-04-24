using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTP.IRepository
{
    public interface IBaseRepository<T> where T : class, new()
    {
        IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda);
        IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orederbyLambda, bool isAsc);

        Task<bool> DeleteEntity(T entity);

        Task<bool> UpdateEntity(T entity);

        Task<bool> AddEntity(T entity);
    }
}
