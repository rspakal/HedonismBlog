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
    [Authorize]
    public class CommentController : Controller
    {
        private readonly ICommentService _commentService;
        private readonly ILogger<HomeController> _logger;

        public CommentController(ICommentService commentService, ILogger<HomeController> logger)
        {
            _commentService = commentService;
            _logger = logger;
        }

        [HttpPost]
        [Route("comment/create")]
        public async Task<IActionResult> Create(PostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction("View", "Post", new { postViewModel.Id });
            }
            await _commentService.CreateAsync(postViewModel);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' added comment");
            return RedirectToAction("View", "Post", new { postViewModel.Id });
        }

        [HttpPost]
        [Route("comment/edit")]
        public IActionResult Edit(PostViewModel postViewModel)
        {
            _commentService.Update(postViewModel);
            return View();
        }


        [HttpGet]
        [Route("comment/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            var _comment = await _commentService.GetAsNoTrackingAsync(id);
            await _commentService.DeleteAsync(id);

            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' deleted comment");
            return RedirectToAction("View", "Post", new { id = _comment.PostId });
        }

    }
}
