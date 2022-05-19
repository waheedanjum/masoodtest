
using Repository.Entities;

namespace Repository.Interface
{
    public interface IUnitOfWork
    {
        IRepository<ParagonJWT> ParagonJWTRepository { get; }
        void Save();
        void SaveChangesAsync();
    }
}
