using Contracts;
using Entities;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;

namespace Repository
{
    public abstract class RepositoryBase<T> : IRepositoryBase<T> where T : class
    {
        protected RepositoryContext APIRepositoryContext { get; set; }

        public RepositoryBase(RepositoryContext repositoryContext)
        {
            APIRepositoryContext = repositoryContext;
        }

        public IQueryable<T> FindAll() => APIRepositoryContext.Set<T>().AsNoTracking();

        public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression) => APIRepositoryContext.Set<T>().Where(expression).AsNoTracking();

        public void Create(T entity)
        {
            this.APIRepositoryContext.Set<T>().Add(entity);
        }

    }
}
