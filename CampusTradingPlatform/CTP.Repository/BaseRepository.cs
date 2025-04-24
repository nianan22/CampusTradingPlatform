using EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Repository
{
    public class BaseRepository<T>where T : class,new()
    {
        public readonly MyDbContext myDbContext;
        public BaseRepository(MyDbContext myDbContext)
        {
            this.myDbContext = myDbContext;
        }
        public Task<bool> AddEntity(T entity)
        {
            myDbContext.Set<T>().Add(entity);
            return Task.FromResult<bool>(true);
        }

        public Task<bool> DeleteEntity(T entity)
        {

            myDbContext.Set<T>().Remove(entity);
            return Task.FromResult<bool>(true);

        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return myDbContext.Set<T>().Where(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orederbyLambda, bool isAsc)
        {
            var temp = myDbContext.Set<T>().Where<T>(whereLambda);
            totalCount = temp.Count();
            if (isAsc)
            {
                // 升序
                temp = temp.OrderBy<T, S>(orederbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            else
            {
                temp = temp.OrderByDescending<T, S>(orederbyLambda).Skip<T>((pageIndex - 1) * pageSize).Take<T>(pageSize);
            }
            return temp;
            
        }

        public Task<bool> UpdateEntity(T entity)
        {
            myDbContext.Set<T>().Update(entity);
            return Task.FromResult<bool>(true);
        }
    }
}
