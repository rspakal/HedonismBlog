using AutoMapper;
using BlogDALLibrary.Repositories;
using HedonismBlog.ViewModels;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HedonismBlog.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<AuthenticationController> _logger;

        public AuthenticationController(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, ILogger<AuthenticationController> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
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
        public async Task<IActionResult> LogIn(UserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Login");
            }
            var user = await _userRepository.Get(viewModel.Email);
            if (user == null)
            {
                ViewBag.Message = $"No user with '{viewModel.Email}' email is registered";
                return View("Login");
            }

            if (user.Password != viewModel.Password)
            {
                ViewBag.Message = $"Wrong password for '{viewModel.Email}'";
                return View("Login");
            }

            var claims = new List<Claim> 
            {
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.Email, user.Email)

            };
            ClaimsIdentity claimsIdentity = new ClaimsIdentity(
                claims,
                "AppCookie",
                ClaimsIdentity.DefaultNameClaimType,
                ClaimsIdentity.DefaultRoleClaimType);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity));
            _logger.LogInformation($"User action: {viewModel.Email} signed in");
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

        [Route("AccessDenied")]
        public IActionResult AccessDenied()
        {
            _logger.LogInformation($"User action: {HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value} tried to get resourse with restrictions");

            return View();
        }
    }
}
