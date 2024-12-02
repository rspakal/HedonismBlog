using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesLibrary;
using ServicesLibrary.Models.User;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HedonismBlog.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<HomeController> _logger;

        public UserController(IUserService userService, ILogger<HomeController> logger)
        {
            _userService = userService;
            _logger = logger;

        }

        [HttpGet("users")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Index()
        {
            var _userPreviewModels = await _userService.GetAll();
            return View(_userPreviewModels);
        }


        [HttpGet("account")]
        [Authorize]
        public async Task<IActionResult> Account()
        {
            var _contextUser = HttpContext.User;
            var _email = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _userAccountModel = await _userService.GetAccountData(_email);
            return View(_userAccountModel);
        }

        [HttpPost("account")]
        [Authorize]
        public async Task<IActionResult> Update(UserAccountModel _userAccountModel) 
        {
            if (!ModelState.IsValid) 
            {
                return View("Info", _userAccountModel);
            }

            var _contextUser = HttpContext.User;
            var _email = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            await _userService.UpdateAccountData(_userAccountModel, _email);
            _logger.LogInformation($"User action: {HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value} updated its data");
            return RedirectToAction("Account", "User");
        }


        [HttpGet("user/role")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> AssignRole([FromQuery] int userId)
        {
            var _userAssignRoleModel = await _userService.AssignRole(userId);
            return View(_userAssignRoleModel);
        }


        [HttpPost("user/role")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> AssignRole(UserAssignRoleModel userAssignRoleModel)
        {
            await _userService.AssignRole(userAssignRoleModel);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' assigned '{userAssignRoleModel.SelectedRole}' role to '{userAssignRoleModel.Email}'");
            return RedirectToAction("AssignRole", "User", new { userId = userAssignRoleModel.Id});
        }


        [HttpGet("user/delete")]
        [Authorize(Roles = "administrator")]
        public IActionResult Delete(int id)
        {        
            return View();
        }

    }
}
