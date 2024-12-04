using BlogDALLibrary.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesLibrary;
using ServicesLibrary.Models.User;
using System.Threading.Tasks;

namespace HedonismBlog.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUserService  _userService;
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(IUserService userService, ILogger<RegistrationController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register(UserRegistrationModel userRegistrationModel)
        {
            if (!ModelState.IsValid)
            {
                return View(userRegistrationModel);
            }
            try
            {
                await _userService.Register(userRegistrationModel);
                _logger.LogInformation($"User action:New user with email {userRegistrationModel.Email} registered");
                return View("RegisterSucceed");
            }
            catch (UniqueConstraintException ex)
            {
                ModelState.AddModelError(nameof(userRegistrationModel.Email), "A user with this email already exists.");
                return View(userRegistrationModel);
            }
        }
    }
}
