using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesLibrary;
using ServicesLibrary.Models.User;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HedonismBlog.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserService _userService;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IUserService userService, ILogger<AuthenticationController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Route("login")]
        public IActionResult LogIn()
        {
            return View();
        }


        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> LogIn(UserLoginModel userLoginModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login");
            }
            var _user = await _userService.Login(userLoginModel);
            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Role, _user.Role.Name),
                new Claim(ClaimTypes.Email, _user.Email)

            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                "AppCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            _logger.LogInformation($"User action: {_user.Email} signed in");
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [Route("logout")]
        public async Task<IActionResult> LogOut()
        {
            await HttpContext.SignOutAsync();
            _logger.LogInformation($"User action: {HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value} signed out");
            return RedirectToAction("Index", "Home");
        }

        [Route("denied")]
        public IActionResult AccessDenied()
        {
            _logger.LogInformation($"User action: {HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value} tried to get resourse with restrictions");

            return View();
        }
    }
}
