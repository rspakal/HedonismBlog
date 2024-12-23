﻿using BlogDALLibrary.Entities;
using ServicesLibrary.Models;
using ServicesLibrary.Models.Post;
using ServicesLibrary.Models.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public interface IPostService
    {
        public Task<PostCreateModel> CreateAsync();
        public Task CreateAsync(PostCreateModel postCreateModel);
        public Task DeleteAsync(int postId, string currentUserEmail, string currentUserRole);
        public Task<PostViewModel>GetAsync(int postId);
        public Task<List<PostPreviewModel>> GetAllAsync();
        public Task<PostUpdateModel> Update(int postId, string currentUserEmail, string currentUserRole);
        public Task Update(PostUpdateModel postUpdateModel, string currentUserEmail, string currentUserRole);


    }
}
