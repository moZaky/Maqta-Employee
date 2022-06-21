using MAQTA.DAL.Contracts;
using MAQTA.DAL.Entities;
using MAQTA.DAL.Repository;

namespace MAQTA.DAL
{
    public class UnitOfWork:IUnitOfWork
    {
        private MAQTADbContext _dbContext;
        BaseRepository<ApplicationUser> _applicationUserRepository;

        public UnitOfWork(MAQTADbContext dbContext)
        {
            this._dbContext = dbContext;
        }
        
        public IRepository<ApplicationUser> ApplicationUserRepository { get { return _applicationUserRepository ?? (_applicationUserRepository = new BaseRepository<ApplicationUser>(_dbContext)); } }
        public void Commit()
        {
            _dbContext.SaveChanges();
        }
    }
}
