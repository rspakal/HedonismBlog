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
    public class TagController : Controller
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        private readonly ILogger<HomeController> _logger;


        public TagController(ITagRepository tagRepository, ILogger<HomeController> logger, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _tagRepository.GetAll();
            var tagViewModels = _mapper.Map<List<TagViewModel>>(tags);
            return View(tagViewModels);
        }

        [HttpGet]
        public IActionResult Create() 
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> SubmitCreate(TagViewModel viewModel)
        {
            if (!ModelState.IsValid) 
            {
                return View("Create");
            }

            if ((await _tagRepository.GetByName(viewModel.Text) != null))
            {
                ViewBag.Message = $"The tag '{viewModel.Text.ToLower()}' is already exist";
                return View("Create");
            }

            var tag = _mapper.Map<Tag>(viewModel);
            await _tagRepository.Create(tag);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' created '{viewModel.Text}' tag'");
            return RedirectToAction("Index", "Tag");
        }

        [HttpGet]
        public IActionResult Get(int id)
        {
            var tag = _tagRepository.GetById(id);
            return View(tag);
        }
        public IActionResult GetAll()
        {
            var tags = _tagRepository.GetAll();
            return View(tags);
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _tagRepository.GetById(id);
            var tagViewModel = _mapper.Map<TagViewModel>(tag);
            return View(tagViewModel);
        }

        public async Task<IActionResult> SubmitEdit(TagViewModel tagViewModel) 
        {
            if (!ModelState.IsValid)
            {
                return View("Edit");
            }
            var tag = _mapper.Map<Tag>(tagViewModel);
            await _tagRepository.Update(tag);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' edited '{tagViewModel.Text}' tag'");
            return RedirectToAction("Index", "Tag");
        }

        public async Task<IActionResult> Delete(int id)
        {
            var _tag = await _tagRepository.GetById(id);
            await _tagRepository.Delete(id);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' deleted '{_tag.Text}' tag'");
            return RedirectToAction("Index", "Tag");
        }

    }
}
