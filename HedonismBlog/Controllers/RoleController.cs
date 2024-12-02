using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesLibrary;
using ServicesLibrary.Models;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HedonismBlog.Controllers
{
    [Authorize(Roles = "administrator")]
    public class RoleController : Controller
    {
        private readonly IRoleService _roleService;
        private readonly ILogger<HomeController> _logger;

        public RoleController(IRoleService roleService, ILogger<HomeController> logger)
        {
            _roleService = roleService;
            _logger = logger;
        }

        [HttpGet]
        [Route("roles")]
        public async Task<IActionResult> Index()
        {
            var _roleViewModels = await _roleService.GetAllAsync();
            return View(_roleViewModels);
        }

        [HttpGet]
        [Route("role/create")]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [Route("role/create")]
        public async Task<IActionResult> Create(RoleModel roleModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }
            await _roleService.CreateAsync(roleModel);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' created '{roleModel.Name}' role'");
            return RedirectToAction("Index", "Role");
        }

        [HttpGet]
        [Route("role/edit")]
        public async Task<IActionResult> Edit([FromQuery]int id)
        {
            var _roleModel = await _roleService.GetAsync(id);
            return View(_roleModel);
        }

        [HttpPost]
        [Route("role/edit")]
        public async Task<IActionResult> Edit(RoleModel roleModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }
            await _roleService.Update(roleModel);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' updated '{roleModel.Name}' role'");
            return RedirectToAction("Edit", "Role", new { roleModel.Id });
        }

        [HttpGet]
        [Route("role/delete")]
        public async Task<IActionResult> Delete([FromQuery]int id) 
        {
            await _roleService.DeleteAsync(id);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' deleted '{id}' role'");
            return RedirectToAction("Index", "Role");
        }

    }
}
