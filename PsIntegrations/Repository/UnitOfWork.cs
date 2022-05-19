using Microsoft.EntityFrameworkCore;

using Repository.Entities;
using Repository.Interface;

namespace Repository.DBContext
{
    public class UnitOfWork : IUnitOfWork
    {
        private ParagonIntegrationDBContext context;

        public UnitOfWork(ParagonIntegrationDBContext context)
        {
            this.context = context;
        }

        // Repositories are exposed via 
        // getter properties  to the calling components

        public IRepository<ParagonJWT> ParagonJWTRepository
        {
            get
            {
                return new Repository<ParagonJWT>(context);
            }
        }
        public void SaveChangesAsync()
        {
            this.context.SaveChangesAsync();
        }
        public void Save()
        {
            this.context.SaveChanges();
        }
    }
}