using BlogDALLibrary.Entities;
using ServicesLibrary.Models;
using ServicesLibrary.Models.User;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public interface ITagService
    {
        public Task CreateAsync(TagModel tagModel);
        public Task DeleteAsync(int tagId);
        public Task<TagModel>GetAsync(int tagId);
        public Task<List<TagModel>> GetAllAsync();
        public Task Update(TagModel tagModel);


    }
}
