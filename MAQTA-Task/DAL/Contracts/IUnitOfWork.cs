using MAQTA.DAL.Entities;

namespace MAQTA.DAL.Contracts
{
    public interface IUnitOfWork
    {
        public IRepository<ApplicationUser> ApplicationUserRepository { get; }
        void Commit();
    }
}
