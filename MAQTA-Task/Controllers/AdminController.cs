using MAQTA.BL.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace MAQTA.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly IAccountsService _accountsService;
        public AdminController(IAccountsService accountsService)
        {
            _accountsService = accountsService;
        }
        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("UsersList")]
       
        public IActionResult GetUsers()
        {
            var users = _accountsService.GetAppUsers();
            return Ok(users);
        }
    }
}
