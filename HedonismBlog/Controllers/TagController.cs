using AutoMapper;
using BlogDALLibrary.Exceptions;
using BlogDALLibrary.Repositories;
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
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        private readonly ITagService _tagService;
        private readonly ILogger<HomeController> _logger;


        public TagController(ITagService tagService, ITagRepository tagRepository, ILogger<HomeController> logger, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _logger = logger;
            _tagService = tagService;
            _mapper = mapper;
        }

        [HttpGet]
        [Route("tags")]
        public async Task<IActionResult> Index()
        {
            var _tagModels = await _tagService.GetAllAsync();
            return View(_tagModels);
        }

        [HttpGet]
        [Route("tag/create")]
        public IActionResult Create()
        {
            return View();
        }


        [HttpPost]
        [Route("tag/create")]
        public async Task<IActionResult> Create(TagModel tagModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create");
            }

            try
            {
                await _tagService.CreateAsync(tagModel);
                _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' created '{tagModel.Text}' tag'");
                return RedirectToAction("Index", "Tag");
            }
            catch (UniqueConstraintException ex)
            {
                ModelState.AddModelError(nameof(tagModel.Text), "Same tag already exists.");
                return View(tagModel);
            }
        }

        [HttpGet]
        [Route("tag/edit")]
        public async Task<IActionResult> Edit([FromQuery] int id)
        {
            var _tagModel = await _tagService.GetAsync(id);
            return View(_tagModel);
        }

        [HttpPost]
        [Route("tag/edit")]
        public async Task<IActionResult> Edit(TagModel tagModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }

            await _tagService.Update(tagModel);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' edited '{tagModel.Text}' tag'");
            return RedirectToAction("Index", "Tag");
        }


        public async Task<IActionResult> Delete(int id)
        {
            await _tagService.DeleteAsync(id);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' deleted '{id}' tag'");
            return RedirectToAction("Index", "Tag");
        }

    }
}
