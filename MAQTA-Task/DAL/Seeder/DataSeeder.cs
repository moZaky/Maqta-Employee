using MAQTA.DAL.Entities;
using MAQTA.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace MAQTA.DAL.Seeder
{
    public class DataSeeder
    {
        private RoleManager<IdentityRole> _roleManager;
        private UserManager<ApplicationUser> _userManager;
        private MAQTADbContext _context;
        IConfiguration _configuration;

        public DataSeeder(MAQTADbContext context, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IConfiguration configuration)
        {
            _context = context;
            _roleManager = roleManager;
            _userManager = userManager;
            _configuration = configuration;
        }
        public void SeedData()
        {
            _context.Database.Migrate();
            SeedRoles();
            Seedusers();
        }
        private void SeedRoles()
        {
            if (!_roleManager.RoleExistsAsync("User").Result)
            {
                _ = _roleManager.CreateAsync(new IdentityRole("User")).Result;
            }

            if (!_roleManager.RoleExistsAsync("Admin").Result)
            {
                _ = _roleManager.CreateAsync(new IdentityRole("Admin")).Result;
            }

        }

        private void Seedusers()
        {
            if (_userManager.FindByNameAsync("admin").Result is null)
            {
                DefaultAccountModel defaultAccount = new DefaultAccountModel();
                _configuration.GetSection(DefaultAccountModel.DefaultAccount).Bind(defaultAccount);

                _ = _userManager.CreateAsync(new ApplicationUser() { UserName = defaultAccount.UserName }, defaultAccount.Password).Result;
                var user = _userManager.FindByNameAsync("admin").Result; ;
                _ = _userManager.AddToRoleAsync(user, "Admin").Result;
            }
        }
    }
}
