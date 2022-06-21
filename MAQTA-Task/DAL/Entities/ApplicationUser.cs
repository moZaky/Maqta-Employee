using MAQTA.Enums;
using Microsoft.AspNetCore.Identity;

namespace MAQTA.DAL.Entities
{
    public class ApplicationUser: IdentityUser  
    {
        public UserType UserType { get; set; }
        public bool IsActive { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }

    }
}
