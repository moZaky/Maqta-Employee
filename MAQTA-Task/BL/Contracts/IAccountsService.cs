using MAQTA.DAL.Entities;
using MAQTA.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MAQTA.BL.Contracts
{
    public interface IAccountsService
    {
        IEnumerable<ApplicationUser>? GetAppUsers();
        bool ChangeUserPassword(ApplicationUser user, string password);
        Task<ResponseModel> UpdateUserInfo(ApplicationUser updatedUser, ApplicationUser oldUser);
        Task<ResponseModel> ActivateUser(ApplicationUser user);
        Task<ResponseModel> DeactivateUser(ApplicationUser user);
    }
}
