using MAQTA.BL.Contracts;
using MAQTA.DAL.Entities;
using Microsoft.AspNetCore.Identity;
using MAQTA.DAL.Contracts;
using MAQTA.Models;
using MAQTA.constants;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Linq;
using System;

namespace MAQTA.BL
{
    public class AccountsService : IAccountsService
    {
        private IUnitOfWork _unitOfWork;
        private UserManager<ApplicationUser> _userManager;
        private readonly ILogger<AccountsService> _logger;
        public AccountsService(IUnitOfWork unitOfWork, UserManager<ApplicationUser> userManager, ILogger<AccountsService> logger)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
            _logger = logger;
        }



        public IEnumerable<ApplicationUser>? GetAppUsers()
        {
            try
            {
                return _unitOfWork.ApplicationUserRepository.Get().Where(s => s.UserType == Enums.UserType.User).Select(x => new ApplicationUser()
                {
                    Id = x.Id,
                    UserName = x.UserName,
                    Email = x.Email,
                    PhoneNumber = x.PhoneNumber,
                    IsActive = x.IsActive,

                })?.ToList();
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return Enumerable.Empty<ApplicationUser>();

            }

        }

        public async Task<ResponseModel> UpdateUserInfo(ApplicationUser newUserInfo, ApplicationUser oldUser)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(oldUser.Id);

                user.PhoneNumber = newUserInfo.PhoneNumber;
                user.Email = newUserInfo.Email;
                user.FirstName = newUserInfo.FirstName;
                user.LastName = newUserInfo.LastName;
                var result = await _userManager.UpdateAsync(user);
                return result.Succeeded ?
                        await Task.FromResult(new ResponseModel { Status = Enums.StatusCode.Succeeded }) :
                        await Task.FromResult(new ResponseModel { Status = Enums.StatusCode.Error, Message = AppConstants.GENERIC_ERROR_MSG, Data = result.Errors });


            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return new ResponseModel { Status = Enums.StatusCode.Error, Message = AppConstants.GENERIC_ERROR_MSG };
            }

        }



        public bool ChangeUserPassword(ApplicationUser user, string password)
        {
            if (string.IsNullOrEmpty(password)) return false;
            var passwordHasher = new PasswordHasher<ApplicationUser>();

            var isChangedPassword = passwordHasher.VerifyHashedPassword(user, user.PasswordHash, password);
            //if faild..means password changed
            return isChangedPassword == PasswordVerificationResult.Failed;
        }

        public async Task<ResponseModel> ActivateUser(ApplicationUser user)
        {
            try
            {
                var appUser = await _userManager.FindByIdAsync(user.Id);
                appUser.IsActive = true;

                var result = await _userManager.UpdateAsync(appUser);
                return result.Succeeded ?
                        await Task.FromResult(new ResponseModel { Status = Enums.StatusCode.Succeeded }) :
                        await Task.FromResult(new ResponseModel { Status = Enums.StatusCode.Error, Message = AppConstants.GENERIC_ERROR_MSG, Data = result.Errors });

            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return await Task.FromResult(new ResponseModel { Status = Enums.StatusCode.Error, Message = AppConstants.GENERIC_ERROR_MSG });
            }
        }

        public async Task<ResponseModel> DeactivateUser(ApplicationUser user)
        {
            try
            {

                var appUser = await _userManager.FindByIdAsync(user.Id);
                appUser.IsActive = false;

                var result = await _userManager.UpdateAsync(appUser);
                return result.Succeeded ?
                        await Task.FromResult(new ResponseModel { Status = Enums.StatusCode.Succeeded }) :
                        await Task.FromResult(new ResponseModel { Status = Enums.StatusCode.Error, Message = AppConstants.GENERIC_ERROR_MSG, Data = result.Errors });
            }
            catch (Exception e)
            {
                _logger.LogError(e.Message, e);

                return await Task.FromResult(new ResponseModel { Status = Enums.StatusCode.Error, Message = AppConstants.GENERIC_ERROR_MSG });
            }
        }
    }
}
