using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using HedonismBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HedonismBlog.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository; 
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;

        public UserController(IUserRepository userRepository, IRoleRepository roleRepository, ILogger<HomeController> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository =  roleRepository;
            _logger = logger;
            _mapper = mapper;

        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Index()
        {
            var users = await _userRepository.GetAllAsNoTracking();
            var userViewModels = _mapper.Map<List<UserViewModel>>(users);
            return View(userViewModels);
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public IActionResult Get(int id)
        {
            var user = _userRepository.Get(id);
            return View(user);
        }

        [Authorize(Roles = "administrator")]
        public IActionResult GetAll()
        {
            var users = _userRepository.GetAllAsNoTracking();
            return View(users);
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> Info()
        {
            var contextUser = HttpContext.User;
            var email = contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _userRepository.Get(email);
            var userViewModel = _mapper.Map<UserViewModel>(user);
            return View(userViewModel);
        }

        [HttpPost]
        [Authorize]
        public IActionResult SubmitUpdate(UserViewModel userViewModel) 
        {
            if (!ModelState.IsValid) 
            {
                return View("Info", userViewModel);
            }
            var user = _mapper.Map<User>(userViewModel); 
            _userRepository.Update(user);
            _logger.LogInformation($"User action: {HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value} updated its data");
            return RedirectToAction("Info", "User");
        }

        [HttpGet]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> AssignRole(int Id)
        {
            var _roles = await _roleRepository.GetAll();
            var _roleViewModels = _mapper.Map<IEnumerable<RoleViewModel>>(_roles);
            var _user = await _userRepository.Get(Id);
            var _userViewModel = _mapper.Map<UserViewModel>(_user);
            _roleViewModels.Where(r => r.Name == _user.Role.Name).ToList().ForEach(r => r.IsSelected = true);
            _userViewModel.Roles = _roleViewModels.ToList();
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
            }
            return View(_userViewModel);
        }

        [HttpPost]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> SubmitAssignRole(UserViewModel userViewModel)
        {
            var _role = await _roleRepository.GetRolesByName(userViewModel.RoleName);
            var _user = await _userRepository.Get(userViewModel.Id);
            _user.Role = _role;
            await _userRepository.Update(_user);
            TempData["Message"] = $"The role was successfully assigned";
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' assigned '{_role.Name}' role to '{_user.Email}'");
            return RedirectToAction("AssignRole", "User", new { id = userViewModel.Id});
        }


        [HttpGet]
        [Authorize(Roles = "administrator")]
        public IActionResult Delete(int id)
        {
            _userRepository.Delete(id);
            return View();
        }

    }
}
