using MAQTA.BL.Contracts;
using MAQTA.constants;
using MAQTA.DAL.Entities;
using MAQTA.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;


namespace MAQTA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UserManager<ApplicationUser> _userManager;
        private SignInManager<ApplicationUser> _signInManager;
        private readonly IAccountsService _accountsService;
        public IConfiguration _configuration { get; }

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IAccountsService accountsService, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _accountsService = accountsService;
            _configuration = configuration;
        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Login")]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {

            var result = await _signInManager.PasswordSignInAsync(loginModel.UserName, loginModel.Password, true, false);

            if (result.Succeeded)
            {
                var passKey = _configuration["PasswordKey"];
                var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(passKey));
                var user = await _userManager.FindByNameAsync(loginModel.UserName);
                string? rolename = _userManager.GetRolesAsync(user).Result?.FirstOrDefault();

                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);

                var tokeOptions = new JwtSecurityToken(
                        claims: new List<Claim>()        {
                                new Claim(ClaimTypes.Name, "MAQTA"),
                                new Claim(ClaimTypes.Role,rolename),
                                new Claim("UserName",user.UserName)

                    },
                    expires: DateTime.Now.AddDays(1),
                    signingCredentials: signinCredentials
                    );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);

                return Ok(new ResponseModel
                {
                    Data = tokenString,
                    Status = Enums.StatusCode.Succeeded

                });
            }

            return Ok(new ResponseModel
            {
                Data = string.Empty,
                Status = Enums.StatusCode.NotFound,
                Message = AppConstants.INVALID_PASSWORD
            });

        }
       
        [HttpGet]
        [Route("LoggedInUser")]
        [Authorize]
        public async Task<IActionResult> GetLoggedInUser()
        {
            ClaimsPrincipal currentUser = this.User;
            string userEmail = currentUser.Claims.FirstOrDefault(s => s.Type == "UserName")?.Value;
            var user = await _userManager.FindByNameAsync(userEmail);

            return Ok(user);

        }
        [HttpPost]
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IActionResult> Register(RegisetModel model)
        {
            var appUser = await _userManager.CreateAsync(new ApplicationUser() { UserName = model.UserName, PhoneNumber = model.PhoneNumber, Email = model.Email, UserType = Enums.UserType.User, IsActive = true }, model.Password);
            if (appUser.Succeeded)
            {
                var newUser = await _userManager.FindByNameAsync(model.UserName);
                _ = await _userManager.AddToRoleAsync(newUser, "User");
                return Ok(new ResponseModel
                {
                    Status = Enums.StatusCode.Created,
                });
            }
            else
            {
                return Ok(new ResponseModel
                {
                    Status = appUser.Errors?.ElementAt(0).Code== "DuplicateUserName"? Enums.StatusCode.Duplicate:Enums.StatusCode.Error,
                    Message = AppConstants.GENERIC_ERROR_MSG
                });
            }

        }
        [HttpPost]
        [Authorize]
        [Route("Update")]
        public async Task<IActionResult> Update(RegisetModel model)
        {
            var user = await _userManager.FindByNameAsync(model.UserName);
            if (user != null)
            {
                var result = await _accountsService.UpdateUserInfo(model,user);
                return Ok(result);
            }

            else
            {
                return Ok(new ResponseModel
                {
                    Status = Enums.StatusCode.NotFound
                });
            }

        }
        [Authorize]
        [HttpGet]
        [Route("Activate")]
        public async Task<IActionResult> Activate(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user != null)
            {
                var result = await _accountsService.ActivateUser(user);
                return Ok(result);
            }

            else
            {
                return Ok(new ResponseModel
                {
                    Status = Enums.StatusCode.NotFound
                });
            }

        }
        [Authorize]
        [HttpGet]
        [Route("Deactivate")]
        public async Task<IActionResult> Deactivate(string email)
        {
            var user = await _userManager.FindByNameAsync(email);
            if (user != null)
            {
                var result = await _accountsService.DeactivateUser(user);
                return Ok(result);
            }

            else
            {
                return Ok(new ResponseModel
                {
                    Status = Enums.StatusCode.NotFound
                });
            }
        }
        [Authorize]
        [HttpPost]
        [Route("LogOut")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Ok();
        }

    }
}
