using BlogDALLibrary.Models;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private ICommentRepository _commentRepository;
        public CommentController(ICommentRepository commentRepository) 
        {
            _commentRepository = commentRepository;
        }

        [HttpPost("comment/create")]
        //[Authorize]
        public async Task<IActionResult> Create([FromBody] Comment comment)
        {
            await _commentRepository.Create(comment);
            return Ok();
        }

        [HttpPut("comment/edit")]
        //[Authorize]
        public async Task<IActionResult> Edit([FromBody] Comment comment)
        {
            await _commentRepository.Update(comment);
            return Ok();
        }

        [HttpDelete("comment/delete")]
        //[Authorize]
        public async Task<IActionResult> Delete(int id)
        {
            await _commentRepository.Delete(id);
            return Ok();
        }


    }
}
