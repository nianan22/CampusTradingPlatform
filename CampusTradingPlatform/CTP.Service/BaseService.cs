using Entity;
using CTP.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace CTP.Service
{
    public class BaseService<T> where T : class, new()
    {
        protected IBaseRepository<T> repository { get; set; }

        public BaseService(IBaseRepository<T> repository)
        {
            this.repository = repository;
        }

        public Task<bool> AddEntity(T entity)
        {
            return repository.AddEntity(entity);
        }

        public Task<bool> DeleteEntity(T entity)
        {
            return repository.DeleteEntity(entity);
        }

        public IQueryable<T> LoadEntities(Expression<Func<T, bool>> whereLambda)
        {
            return repository.LoadEntities(whereLambda);
        }

        public IQueryable<T> LoadPageEntities<S>(int pageIndex, int pageSize, out int totalCount, Expression<Func<T, bool>> whereLambda, Expression<Func<T, S>> orederbyLambda, bool isAsc)
        {
            return repository.LoadPageEntities(pageIndex, pageSize, out totalCount, whereLambda, orederbyLambda, isAsc);
        }

        public Task<bool> UpdateEntity(T entity)
        {
            return repository.UpdateEntity(entity);
        }
    }
}