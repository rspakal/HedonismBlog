using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary;
using ServicesLibrary.Models.Post;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class PostController : BlogAPIBaseController
    {
        private readonly IPostService _postService;

        public PostController(IPostService postService)
        {
            _postService = postService;
        }

        /// <summary>
        /// Shows all post previews. Allowed to all
        /// </summary>
        /// <returns>All posts preview.</returns>
        /// <response code="200">Returns all post preview.</response>
        [HttpGet("posts")]
        public async Task<IActionResult> Index()
        {
            var _postPreviewModels = await _postService.GetAllAsync();
            return Ok(_postPreviewModels);
        }

        /// <summary>
        /// Shows post with content and comments.
        /// </summary>
        /// <param name="id">Post id.</param>
        /// <returns>Post view.</returns>
        /// <response code="200">Returns Post view.</response>
        /// <response code="400">If post id has incorrect value.</response>
        [HttpGet("post")]
        public async Task<IActionResult> View([FromQuery] int id)
        {
            if (id < 1)
            {
                return BadRequest("Id cannot be less then 1.");

            }

            var _postViewAPIModel = await _postService.GetAsync(id);
            return Ok(_postViewAPIModel);
        }

        /// <summary>
        /// Creates new post.
        /// </summary>
        /// <param name="postCreateModel">Model fro creating new post.</param>
        /// <returns>Creating post result.</returns>
        /// <response code="200">Post was created.</response>
        /// <response code="400">If PostCreateModel is null.</response>
        [HttpPost("post/create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] PostCreateModel postCreateModel)
        {

            if (postCreateModel == null)
            {
                return BadRequest("PostCreateModel cannot be null.");
            }

            await _postService.CreateAsync(postCreateModel);
            return Ok();
        }


        /// <summary>
        /// Edits existing post. Allowed to comment author, administartor and moderator
        /// </summary>
        /// <param name="postUpdateModel">Model for editing existing post.</param>
        /// <returns>Editing post result.</returns>
        /// <response code="200">Post was edited.</response>
        /// <response code="400">If PostUpdateModel is null.</response>
        [HttpPut("post/edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] PostUpdateModel postUpdateModel)
        {
            if (postUpdateModel == null)
            {
                return BadRequest("PostCreateModel cannot be null.");
            }

            var _currentUserEmail = GetClaimValue(ClaimTypes.Email);
            var _currentUserRole = GetClaimValue(ClaimTypes.Role);

            await _postService.Update(postUpdateModel, _currentUserEmail, _currentUserRole);
            return Ok();
        }

        /// <summary>
        /// Delets a post. Allowed to comment author, administartor and moderator
        /// </summary>
        /// <param name="postId">Post id.</param>
        /// <returns>Deleting post result.</returns>
        /// <response code="200">Post was deleted.</response>
        /// <response code="400">If post id has incorrect value.</response>
        [HttpDelete("post/delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] int postId)
        {
            if (postId < 1)
            {
                return BadRequest("Id cannot be less then 1.");
            }
            var _currentUserEmail = GetClaimValue(ClaimTypes.Email);
            var _currentUserRole = GetClaimValue(ClaimTypes.Role);

            await _postService.DeleteAsync(postId, _currentUserEmail, _currentUserRole);
            return Ok();

        }
    }
}
