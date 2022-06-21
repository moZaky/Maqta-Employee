using MAQTA.DAL.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MAQTA.DAL
{
    public class MAQTADbContext: IdentityDbContext<ApplicationUser>
    {
        public MAQTADbContext(DbContextOptions<MAQTADbContext> options)
            : base(options)
        {

        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseLazyLoadingProxies();
        }
    }
}
