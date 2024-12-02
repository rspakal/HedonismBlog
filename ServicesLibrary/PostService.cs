using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using ServicesLibrary.Models;
using ServicesLibrary.Models.Post;
using ServicesLibrary.Models.User;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public class PostService : IPostService
    {
        private readonly IMapper _mapper;
        private readonly IPostRepository _postRepository;
        private readonly ITagRepository _tagRepository;
        private readonly IUserRepository _userRepository;
        public PostService(IMapper mapper, IPostRepository postRepository, ITagRepository tagRepository, IUserRepository userRepository)
        {
            _mapper = mapper;
            _postRepository = postRepository;
            _tagRepository = tagRepository;
            _userRepository = userRepository;
        }

        public async Task<PostCreateModel> CreateAsync()
        {
            var _tags = await _tagRepository.GetAll();
            var _tagModels = _mapper.Map<List<TagModel>>(_tags);
            var _postCreateModel = new PostCreateModel
            {
                Tags = _tagModels.ToList()
            };
            return _postCreateModel;
        }

        public async Task CreateAsync(PostCreateModel postCreateModel)
        {
            var _tags = await _tagRepository.GetAllAsNoTracking();
            var _user = await _userRepository.Get(postCreateModel.UserEmail);
            var _tagModels = _mapper.Map<List<TagModel>>(_tags);
            postCreateModel.Tags = _tagModels;
            var _post = _mapper.Map<Post>(postCreateModel);
            _post.User = _user;
            await _postRepository.Create(_post);
        }

        public async Task DeleteAsync(int postId)
        {
            await _postRepository.Delete(postId);
        }

        public async Task<List<PostPreviewModel>> GetAllAsync()
        {
           var _posts = await _postRepository.GetAllAsNoTracking();
           var _postPreviewModels = _mapper.Map<List<PostPreviewModel>>(_posts);
            return _postPreviewModels;
        }

        public async Task<PostViewModel> GetAsync(int postId)
        {
            var _post = await _postRepository.Get(postId);
            var _postViewModel = _mapper.Map<PostViewModel>(_post);
            return _postViewModel;
        }

        public async Task<PostUpdateModel> Update(int postId)
        {
            var _tags = await _tagRepository.GetAllAsNoTracking();
            var _tagModels = _mapper.Map<List<TagModel>>(_tags);
            var _post = await _postRepository.GetAsNoTracking(postId);
            var _postUpdateModel = _mapper.Map<PostUpdateModel>(_post);
            foreach (var tag in _tagModels)
            {
                if (_post.Tags.Any(t => t.Id == tag.Id))
                {
                    tag.IsSelected = true;
                }
            }
            _postUpdateModel.Tags = _tagModels;
            return _postUpdateModel;
        }

        public async Task Update(PostUpdateModel postUpdateModel)
        {
            var _user = await _userRepository.Get(postUpdateModel.UserEmail);
            postUpdateModel.Tags = postUpdateModel.Tags.Where(t => t.IsSelected == true).ToList();
            var _post = _mapper.Map<Post>(postUpdateModel);
            _post.User = _user;
            await _postRepository.Update(_post);
        }

    }
}
