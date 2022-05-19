
using Repository.Entities;

using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Repository.Interface
{
    public interface IRepository<T> where T : GeneralBaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> Get(int id);
        Task Insert(T entity);
        Task Update(T entity);
        void Delete(T entity);
        Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> predicate);
        Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate);
        Task<IEnumerable<T>> GetWithQuery(string query);
        Task<IEnumerable<T>> GetManyPaging(Expression<Func<T, bool>> predicate, int page, int pageSize);
        Task<int> GetManyCount(Expression<Func<T, bool>> predicate);
    }
}
