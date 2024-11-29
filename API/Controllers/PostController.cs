using API.APIModels.Post;
using AutoMapper;
using BlogDALLibrary.Models;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly IMapper _mapper;

        public PostController(IUserRepository userRepository, IPostRepository postRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _mapper = mapper;
        }

        [HttpGet("posts")]
        public async Task<IActionResult> GetAll()
        {
            var _posts = await _postRepository.GetAllAsNoTracking();
            var _postPreviewAPIModels = _mapper.Map<List<PostPreviewAPIModel>>(_posts);
            return Ok(_postPreviewAPIModels);
        }

        [HttpGet("post/view")]
        [Authorize]
        public async Task<IActionResult> View(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id), "Post id is zero or null");
            }
            var _post = await _postRepository.GetAsNoTracking(id);
            var _postViewAPIModel = _mapper.Map<PostViewAPIModel>(_post);
            return Ok(_postViewAPIModel);
        }

        [HttpPost("post/create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] PostCreateAPIModel postCreateAPIModel)
        {
            if (postCreateAPIModel == null)
            {
                throw new ArgumentNullException(nameof(postCreateAPIModel), "Aegument 'PostCreateAPIModel' is null");
            }
            var _user = _mapper.Map<Post>(postCreateAPIModel);
            await _postRepository.Create(_user);
            return Ok();
        }

        [HttpPut("post/update")]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] PostUpdateAPIModel postUpdateAPIModel)
        {
            if (postUpdateAPIModel == null)
            {
                throw new ArgumentNullException(nameof(postUpdateAPIModel), "Argument 'PostUpdateAPIModel' is null");
            }
            var _contextUser = HttpContext.User;
            var _contextUserEmail = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _contextUserRole = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;

            if (_contextUserEmail != postUpdateAPIModel.UserEmail && _contextUserRole != "administrator" && _contextUserRole == "moderator")
            {
                return Forbid();
            }
            var _post = _mapper.Map<Post>(postUpdateAPIModel);
            await _postRepository.Update(_post);
            return Ok();
        }

        [HttpDelete("post/delete")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id), "Post id is zero or null");
            }
            var _post = await _postRepository.Get(id);
            var _contextUser = HttpContext.User;
            var _contextUserEmail = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _contextUserRole = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (_contextUserEmail != _post.User.Email && _contextUserRole != "administrator" && _contextUserRole == "moderator")
            {
                return Forbid();
            }
            await _postRepository.Delete(_post);
            return Ok();
        }
    }
}
