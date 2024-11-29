using API.APIModels.Comment;
using AutoMapper;
using BlogDALLibrary.Models;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    public class CommentController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private ICommentRepository _commentRepository;
        private IMapper _mapper;
        public CommentController(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _postRepository = postRepository;
            _commentRepository = commentRepository;
            _mapper = mapper;

        }

        [HttpPost("comment/create")]
        [Authorize]
        public async Task<IActionResult> Create([FromBody] CommentAPIModel commentAPIModel)
        {

            if (commentAPIModel == null)
            {
                throw new ArgumentNullException(nameof(commentAPIModel), "Argument 'CommentAPIModel' is null");
            }
            var _comment = _mapper.Map<Comment>(commentAPIModel);
            _comment.User = await _userRepository.Get(commentAPIModel.UserEmail);
            _comment.Post = await _postRepository.Get(commentAPIModel.PostId);
            await _commentRepository.Create(_comment);
            return Ok();
        }

        [HttpPut("comment/edit")]
        [Authorize]
        public async Task<IActionResult> Edit([FromBody] CommentAPIModel commentAPIModel)
        {

            if (commentAPIModel == null)
            {
                throw new ArgumentNullException(nameof(commentAPIModel), "Argument 'CommentAPIModel' is null");
            }

            var _contextUser = HttpContext.User;
            var _contextUserEmail = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _contextUserRole = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (_contextUserEmail != commentAPIModel.UserEmail)
            {
                return Forbid();
            }

            var _comment = _mapper.Map<Comment>(commentAPIModel);
            _comment.User = await _userRepository.Get(commentAPIModel.UserEmail);
            _comment.Post = await _postRepository.Get(commentAPIModel.PostId);
            await _commentRepository.Update(_comment);
            return Ok();
        }

        [HttpDelete("comment/delete")]
        [Authorize]
        public async Task<IActionResult> Delete(int id)
        {

            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id), "Argument 'id' is zero or null");
            }
            var _comment = await _commentRepository.Get(id);
            var _contextUser = HttpContext.User;
            var _contextUserEmail = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _contextUserRole = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (_contextUserEmail != _comment.User.Email && _contextUserRole != "administrator" && _contextUserRole == "moderator")
            {
                return Forbid();
            }
            _commentRepository.Delete(_comment);
            return Ok();
        }
    }

}
