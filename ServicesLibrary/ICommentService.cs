using BlogDALLibrary.Entities;
using ServicesLibrary.Models;
using ServicesLibrary.Models.Comment;
using ServicesLibrary.Models.Post;
using ServicesLibrary.Models.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public interface ICommentService
    {
        public Task CreateAsync(PostViewModel postViewModel);
        public Task DeleteAsync(int Id, string currentUserEmail, string currentUserRole);
        public Task<Comment> GetAsNoTrackingAsync(int Id);
        public Task Update(PostViewModel postViewModel, string currentUserEmail, string currentUserRole);


    }
}
