using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary;
using ServicesLibrary.Models.Post;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class CommentController : BlogAPIBaseController
    {
        private readonly ICommentService _commentService;

        public CommentController(ICommentService commentService)
        {
            _commentService = commentService;
        }

        /// <summary>
        /// Adds a new comment to a post.
        /// </summary>
        /// <param name="postViewModel">Model for post view.</param>
        /// <returns>Adding new comment result .</returns>
        /// <response code="200">New comment was added.</response>
        /// <response code="400">If PostViewModel is null.</response>
        [HttpPost("comment/create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] PostViewModel postViewModel)
        {

            if (postViewModel == null)
            {
                return BadRequest("PostViewModel cannot be null.");
            }

            await _commentService.CreateAsync(postViewModel);
            return Ok();
        }

        /// <summary>
        /// Edits an existing comment. Allowed to comment author, administartor and moderator
        /// </summary>
        /// <param name="postViewModel">Model for post view.</param>
        /// <returns>Editing comment result .</returns>
        /// <response code="200">Comment was edited.</response>
        /// <response code="400">If PostViewModel is null.</response>
        [HttpPut("comment/edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] PostViewModel postViewModel)
        {
            if (postViewModel == null)
            {
                return BadRequest("PostViewModel cannot be null.");
            }

            var _currentUserEmail = GetClaimValue(ClaimTypes.Email);
            var _currentUserRole = GetClaimValue(ClaimTypes.Role);

            await _commentService.Update(postViewModel, _currentUserEmail, _currentUserRole);
            return Ok();
        }

        /// <summary>
        /// Delets a comment. Allowed to comment author, administartor and moderator
        /// </summary>
        /// <param name="id">Comment id.</param>
        /// <returns>Deleting comment result .</returns>
        /// <response code="200">Comment was deleted.</response>
        /// <response code="400">If Id has incorrect value.</response>
        [HttpDelete("comment/delete")]
        [Authorize]
        public async Task<IActionResult> Delete([FromQuery] int id)
        {

            if (id < 1)
            {
                return BadRequest("Id cannot be less then 1.");
            }

            var _currentUserEmail = GetClaimValue(ClaimTypes.Email);
            var _currentUserRole = GetClaimValue(ClaimTypes.Role);

            await _commentService.DeleteAsync(id, _currentUserEmail, _currentUserRole);
            return Ok();
        }
    }

}
