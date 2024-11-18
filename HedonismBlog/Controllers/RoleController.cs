using AutoMapper;
using BlogDALLibrary.Models;
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
    [Authorize(Roles = "administrator")]
    public class RoleController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;

        public RoleController(IRoleRepository roleRepository, ILogger<HomeController> logger, IMapper mapper)
        {
            _roleRepository = roleRepository;
            _logger = logger;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var roles = await _roleRepository.GetAll();
            var roleViewModels = _mapper.Map<List<RoleViewModel>>(roles);
            return View(roleViewModels);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SubmitCreate(RoleViewModel roleViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }
            roleViewModel.Name = roleViewModel.Name.ToLower();
            if ((await _roleRepository.GetByName(roleViewModel.Name)) != null)
            {
                ViewBag.Message = $"The role '{roleViewModel.Name}' is already exist";
                return View("Create");
            }
            var role = _mapper.Map<Role>(roleViewModel);
            await _roleRepository.Create(role);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' created '{roleViewModel.Name}' role'");
            return RedirectToAction("Index", "Role");
        }

        public async Task<IActionResult> Edit(int id)
        {
            var role = await _roleRepository.GetById(id);
            var roleViewModel = _mapper.Map<RoleViewModel>(role);
            return View(roleViewModel);
        }

        [HttpPost]
        public async Task<IActionResult> SubmitEdit(RoleViewModel roleViewModel)
        {
            var role = await _roleRepository.GetByName(roleViewModel.Name);
            if (role != null && role.Id != roleViewModel.Id)
            {
                ViewBag.Message = $"The role '{roleViewModel.Name}' is already exist";
                return View("Edit", new { Id = roleViewModel.Id });
            }
            var updatedRole = _mapper.Map<Role>(roleViewModel);
            await _roleRepository.Update(updatedRole);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' updated '{roleViewModel.Name}' role'");
            return RedirectToAction("Index", "Role");
        }

        [HttpGet]
        public IActionResult GetAll() 
        {
            return View();
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var role = _roleRepository.GetById(id);
            return View(role);
        }

        public async Task<IActionResult> Delete(int id) 
        {
            var _role = await _roleRepository.GetById(id);
            await _roleRepository.Delete(_role);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' deleted '{_role.Name}' role'");

            return RedirectToAction("Index", "Role");
        }

    }
}
