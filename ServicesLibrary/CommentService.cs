using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using ServicesLibrary.Models;
using ServicesLibrary.Models.Comment;
using ServicesLibrary.Models.Post;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Reflection.PortableExecutable;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public class CommentService : ICommentService
    {
        private readonly IMapper _mapper;
        private readonly IUserRepository _userRepository;
        private readonly IPostRepository _postRepository;
        private readonly ICommentRepository _commentRepository;
        public CommentService(IMapper mapper,IUserRepository userRepository, ICommentRepository commentRepository, IPostRepository postRepository)
        {
            _mapper = mapper;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            _postRepository = postRepository;

        }
        public async Task CreateAsync(PostViewModel postViewModel)
        {
            var _post = await _postRepository.Get(postViewModel.Id);
            var _user = await _userRepository.Get(postViewModel.NewComment.UserEmail);
            var _comment = _mapper.Map<Comment>(postViewModel.NewComment);
            _comment.User = _user;
            _comment.Post = _post;
            _comment.TimeStamp = DateTime.Now.ToString();
            await _commentRepository.Create(_comment);
        }

        public async Task DeleteAsync(int commentId, string currentUserEmail, string currentUserRole)
        {
            var _comment = await _commentRepository.Get(commentId);
            if (_comment.User.Email != currentUserEmail && currentUserEmail != "administrator" && currentUserRole != "moderator")
            {
                return;
            }

            await _commentRepository.Delete(commentId);
        }

        public async Task<Comment> GetAsNoTrackingAsync(int id)
        {
            return await _commentRepository.GetAsNoTracking(id);
        }
        public async Task Update(PostViewModel postViewModel, string currentUserEmail, string currentUserRole)
        {

            if (postViewModel.UserEmail != currentUserEmail && currentUserEmail != "administrator" && currentUserRole != "moderator")
            {
                return;
            }
            var _comment = _mapper.Map<Comment>(postViewModel.NewComment);
            await _commentRepository.Update(_comment);
        }

    }
}
