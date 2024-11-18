using API.APIModels;
using AutoMapper;
using BlogDALLibrary.Models;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(Roles = "administrator")]
    public class TagController : ControllerBase
    {
        private readonly ITagRepository _tagRepository;
        private readonly IMapper _mapper;
        public TagController(ITagRepository tagRepository, IMapper mapper)
        {
            _tagRepository = tagRepository;
            _mapper = mapper;
        }

        [HttpGet("tags")]
        public async Task<IActionResult> Tags()
        {
            try
            {
                var _tags = await _tagRepository.GetAll();
                var _tagAPIModels = _mapper.Map<List<TagAPIModel>>(_tags);
                return Ok(_tagAPIModels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");
            }
        }

        [HttpPost("tag/create")]
        public async Task<IActionResult> Create(TagAPIModel tagAPIModel)
        {
            var _tag = _mapper.Map<Tag>(tagAPIModel);
            try
            {
                await _tagRepository.Create(_tag);
                return Ok();

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");
            }
        }

        [HttpPut("tag/update")]
        public async Task<IActionResult> Update(TagAPIModel tagAPIModel)
        {
            var _tag = _mapper.Map<Tag>(tagAPIModel);
            try
            {
                await _tagRepository.Update(_tag);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");

            }
        }

        [HttpDelete("tag/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                await _tagRepository.Delete(id);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");
            }
        }
    }
}
