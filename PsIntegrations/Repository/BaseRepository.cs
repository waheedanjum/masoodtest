using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

using Repository.DBContext;

using Microsoft.EntityFrameworkCore;
using Repository.Entities;
using System.Threading.Tasks;

namespace Repository
{

    public class Repository<T> : Interface.IRepository<T> where T : GeneralBaseEntity
    {
        private readonly ParagonIntegrationDBContext context;
        private DbSet<T> entities;
        public Repository(ParagonIntegrationDBContext context)
        {
            this.context = context;
            entities = context.Set<T>();
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            return await entities.ToListAsync();
        }
        public async Task<T> Get(int id)
        {
            return await entities.SingleOrDefaultAsync(s => s.Id == id);
        }
        public async Task Insert(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await entities.AddAsync(entity);
        }
        public async Task Update(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            await context.SaveChangesAsync();
        }
        public void Delete(T entity)
        {
            if (entity == null)
            {
                throw new ArgumentNullException("entity");
            }
            entities.Remove(entity);
        }
        public async Task<IEnumerable<T>> GetMany(Expression<Func<T, bool>> predicate)
        {
            return await entities.Where(predicate).ToListAsync();
        }
        public async Task<T> GetFirstOrDefault(Expression<Func<T, bool>> predicate)
        {
            return await entities.FirstOrDefaultAsync(predicate);
        }
        public async Task<IEnumerable<T>> GetWithQuery(string query)
        {
            return await entities.FromSqlRaw(query).ToListAsync();
        }
        public async Task<IEnumerable<T>> GetManyPaging(Expression<Func<T, bool>> predicate, int page, int pageSize)
        {
            return await entities.Where(predicate).Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();
        }
        public async Task<int> GetManyCount(Expression<Func<T, bool>> predicate)
        {
            return await entities.CountAsync(predicate);
        }
    }
}
