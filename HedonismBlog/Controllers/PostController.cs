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
    public class PostController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;

        public PostController(IUserRepository userRepository, IPostRepository postRepository, ITagRepository tagRepository, IMapper mapper, ILogger<HomeController> logger)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var posts = await _postRepository.GetAllAsNoTracking();
            var postViewModels = _mapper.Map<List<PostViewModel>>(posts);
            //TO DO: REMOVE EECEPTION COUSING CODE
            //var y = 3 - 3;
            //var x = 5 / y;

            return View(postViewModels);
        }

        public async Task<IActionResult> View(int id)
        {
            var post = await _postRepository.Get(id);
            var postViewModel = _mapper.Map<PostViewModel>(post);
            if (TempData["Errors"] != null)
            {
                var errors = TempData["Errors"] as IEnumerable<string>;
                foreach (var error in errors)
                {
                    ModelState.AddModelError(string.Empty, error);
                }
            }

            return View(postViewModel);
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var tags = await _tagRepository.GetAll();
            var tagViewModels = _mapper.Map<List<TagViewModel>>(tags);
            var userEmail = HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var postViewModel = new PostViewModel
            {
                UserEmail = userEmail,
                Tags = tagViewModels.ToList()
            };
            return View(postViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitCreate(PostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Create", postViewModel);
            }
            var user = await _userRepository.GetByEmail(postViewModel.UserEmail);
            postViewModel.Tags = postViewModel.Tags.Where(t => t.IsSelected == true).ToList();
            var post = _mapper.Map<Post>(postViewModel);
            post.User = user;
            await _postRepository.Create(post);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' created post '{postViewModel.Title}'");
            return RedirectToAction("Index", "Post");
        }

        [Authorize]
        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var tags = await _tagRepository.GetAllAsNoTracking();
            var tagViewModels = _mapper.Map<List<TagViewModel>>(tags);
            var post = await _postRepository.GetAsNoTracking(id);
            var postViewModel = _mapper.Map<PostViewModel>(post);
            foreach (var tag in tagViewModels)
            {
                if (post.Tags.Any(x => x.Id == tag.Id))
                {
                    tag.IsSelected = true;
                }
            }
            postViewModel.Tags = tagViewModels;
            return View(postViewModel);
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> SubmitEdit(PostViewModel postViewModel)
        {
            if (!ModelState.IsValid)
            {
                return View("Edit", postViewModel);
            }
            var user = await _userRepository.GetByEmail(postViewModel.UserEmail);
            postViewModel.Tags = postViewModel.Tags.Where(t => t.IsSelected == true).ToList();
            var post = _mapper.Map<Post>(postViewModel);
            post.User = user;
            await _postRepository.Update(post);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' edited post '{postViewModel.Title}'");
            return RedirectToAction("Index", "Post");
        }

        public IActionResult Get(int id)
        {
            var post = _postRepository.Get(id);
            return View(post);
        }
        public IActionResult GetAll(int id)
        {
            var allPosts = _postRepository.GetAll();
            return View(allPosts);
        }

        public IActionResult GetByUserId(int userId) 
        {
            var userPosts = _postRepository.GetByUserId(userId);
            return View(userPosts);
        }

        [Authorize]
        public IActionResult Update(Post post)
        {
            _postRepository.Update(post);
            return View();
        }

        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            var _post = await _postRepository.Get(id);
            await _postRepository.Delete(_post);
            _logger.LogInformation($"User action: '{HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value}' deleted post '{_post.Title}'");
            return RedirectToAction("Index", "Post");
        }

    }
}
