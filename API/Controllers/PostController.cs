using BlogDALLibrary.Models;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class PostController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;

        public PostController(IUserRepository userRepository, IPostRepository postRepository)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
        }

        [HttpGet("posts")]
        public async Task<IActionResult> GetAll()
        {
            var _posts = await _postRepository.GetAllAsNoTracking();
            return Ok(_posts);
        }

        [HttpGet("post/view")]
        [Authorize]
        public async Task<IActionResult> View(int id)
        {
            var _post = await _postRepository.Get(id);
            return Ok(_post);
        }

        [HttpPost("post/create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] Post post)
        {
            await _postRepository.Create(post);
            return Ok();
        }

        [HttpPut("post/update")]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] Post post)
        {
            await _postRepository.Update(post);
            return Ok();
        }

        [HttpDelete("post/delete")]
        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _postRepository.Delete(id);
            return Ok();
        }
    }
}
