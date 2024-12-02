using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using ServicesLibrary.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ServicesLibrary
{
    public class TagService : ITagService
    {
        private readonly IMapper _mapper;
        private readonly ITagRepository _tagRepository;
        public TagService(IMapper mapper, ITagRepository tagRepository)
        {
            _mapper = mapper;
            _tagRepository = tagRepository;
        }
        public async Task CreateAsync(TagModel tagModel)
        {
            if ((await _tagRepository.Get(tagModel.Text) != null))
            {
                throw new Exception($"{tagModel.Text} is already exist");
            }

            var _tag = _mapper.Map<Tag>(tagModel);
            await _tagRepository.Create(_tag);
        }

        public async Task DeleteAsync(int tagId)
        {
            await _tagRepository.Delete(tagId);
        }

        public async Task<List<TagModel>> GetAllAsync()
        {
            var _tags = await _tagRepository.GetAll();
            var _tagModels = _mapper.Map<List<TagModel>>(_tags);
            return _tagModels;
        }

        public async Task<TagModel> GetAsync(int tagId)
        {
            var _tag = await _tagRepository.Get(tagId);
            var _tagModel = _mapper.Map<TagModel>(_tag);
            return _tagModel;
        }

        public async Task Update(TagModel tagModel)
        {
            var _role = _mapper.Map<Tag>(tagModel);
            await _tagRepository.Update(_role);
        }

    }
}
