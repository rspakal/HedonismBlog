using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ServicesLibrary;
using ServicesLibrary.Models.Post;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HedonismBlog.Controllers
{
    public class PostController : Controller
    {
        private readonly IPostService _postService;
        private readonly ILogger<HomeController> _logger;

        public PostController(IPostService postService, ILogger<HomeController> logger)
        {
            _postService = postService;
            _logger = logger;
        }

        [HttpGet]
        [Route("posts")]
        public async Task<IActionResult> Index()
        {
            var _postPreviewModels = await _postService.GetAllAsync();
            return View(_postPreviewModels);
        }

        [HttpGet]
        [Route("post")]
        public async Task<IActionResult> View([FromQuery] int id)
        {
            var _postViewModel = await _postService.GetAsync(id);
            return View(_postViewModel);
        }

        [HttpGet]
        [Authorize]
        [Route("post/create")]
        public async Task<IActionResult> Create()
        {
            
            var _postCreateModel = await _postService.CreateAsync();
            return View(_postCreateModel);
        }

        [HttpPost]
        [Authorize]
        [Route("post/create")]
        public async Task<IActionResult> Create(PostCreateModel postCreateModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", postCreateModel);
            }
            await _postService.CreateAsync(postCreateModel);
            _logger.LogInformation($"User action: '{postCreateModel.UserEmail}' created post '{postCreateModel.Title}'");
            return RedirectToAction("Index", "Post");
        }

        [HttpGet]
        [Authorize]
        [Route("post/edit")]
        public async Task<IActionResult> Edit([FromQuery] int id)
        {
            var _contextUserEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _contextUserRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            var _postCreateModel = await _postService.Update(id, _contextUserEmail, _contextUserRole); 
            return View(_postCreateModel);
        }

        [HttpPost]
        [Authorize]
        [Route("post/edit")]
        public async Task<IActionResult> Edit(PostUpdateModel postUpdateModel)
        {
            var _contextUserEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _contextUserRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (!ModelState.IsValid)
            {
                return View("Edit", postUpdateModel);
            }

            await _postService.Update(postUpdateModel, _contextUserEmail, _contextUserRole);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' edited post '{postUpdateModel.Title}'");
            return RedirectToAction("Index", "Post");
        }


        [Authorize]
        [Route("post/delete")]

        public async Task<IActionResult> Delete([FromQuery] int id)
        {
            var _contextUserEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _contextUserRole = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            await _postService.DeleteAsync(id, _contextUserEmail, _contextUserRole);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' deleted post '{id}'");
            return RedirectToAction("Index", "Post");
        }

    }
}
