using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using HedonismBlog.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace HedonismBlog.Controllers
{
    public class RegistrationController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<RegistrationController> _logger;

        public RegistrationController(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper, ILogger<RegistrationController> logger)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        [Route("SubmitRegister")]
        public async Task<IActionResult> SubmitRegister(UserViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                return View(viewModel);
            }

            if ((await _userRepository.Get(viewModel.Email)) != null)
            {
                ViewBag.Message = $"The user with '{viewModel.Email}' address is already exist";
                return View("Register");
            }
            var user = _mapper.Map<User>(viewModel);
            var role = await _roleRepository.GetRolesByName("user");
            if (role == null)
                throw new NullReferenceException($"No role with the name \"user\" in DB");
            user.Role = role;
            await _userRepository.Create(user);
            _logger.LogInformation($"User action:New user with email {viewModel.Email} registered");
            return View("RegisterSucceed");
        }
    }
}
