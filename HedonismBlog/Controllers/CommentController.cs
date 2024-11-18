using AutoMapper;
using BlogDALLibrary.Models;
using BlogDALLibrary.Repositories;
using HedonismBlog.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace HedonismBlog.Controllers
{

    [Authorize]
    public class CommentController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;

        public CommentController(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository, ILogger<HomeController> logger, IMapper mapper)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _logger = logger;
            _mapper = mapper;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(PostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                TempData["Errors"]  = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();
                return RedirectToAction("View", "Post", new { Id = postViewModel.Id });
            }
            var post = await _postRepository.Get(postViewModel.Id);
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var user = await _userRepository.GetByEmail(userEmail);
            var comment = _mapper.Map<Comment>(postViewModel.NewComment);
            comment.User = user;
            comment.Post = post;
            comment.TimeStamp = DateTime.Now.ToString();
            await _commentRepository.Create(comment);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' added comment");
            return RedirectToAction("View", "Post", new { Id = postViewModel.Id });
        }

        public IActionResult Get(int id)
        {
            var comment = _commentRepository.GetById(id);
            return View(comment);
        }

        public IActionResult GetAll(int id)
        {
            var comments = _commentRepository.GetAll();
            return View(comments);
        }

        public IActionResult Edit(Comment comment)
        {
            _commentRepository.Update(comment);
            return View();
        }

        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _commentRepository.GetById(id);
            var postId = comment.PostId;
            await _commentRepository.Delete(id);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' deleted comment");
            return RedirectToAction("View", "Post", new { Id = postId });
        }

    }
}
