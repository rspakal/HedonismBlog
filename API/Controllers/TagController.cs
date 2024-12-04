using BlogDALLibrary.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary;
using ServicesLibrary.Models;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Authorize(Roles = "administrator")]
    public class TagController : BlogAPIBaseController
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        /// <summary>
        /// Shows all tags.
        /// </summary>
        /// <returns>All existing tags .</returns>
        /// <response code="200">All existing tags.</response>
        [HttpGet("tags")]
        public async Task<IActionResult> Index()
        {
            var _tagModels = await _tagService.GetAllAsync();
            return Ok(_tagModels);
        }

        /// <summary>
        /// Creates a tag.
        /// </summary>
        /// <param name="tagModel">Model for tag.</param>
        /// <returns>Creating tag result .</returns>
        /// <response code="200">Tag was created.</response>
        /// <response code="400">If tagModel is null or tag with the same name already exists</response>
        [HttpPost("tag/create")]
        public async Task<IActionResult> Create([FromBody] TagModel tagModel)
        {
            if (tagModel == null)
            {
                return BadRequest("TagModel cannot be null.");
            }
            try
            {
                await _tagService.CreateAsync(tagModel);
                return Ok();
            }
            catch (UniqueConstraintException ex) 
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Edits an existing tag.
        /// </summary>
        /// <param name="tagModel">Model for tag.</param>
        /// <returns>Editing tag result .</returns>
        /// <response code="200">Tag was edited.</response>
        /// <response code="400">If tagModel is null.</response>
        [HttpPut("tag/edit")]
        public async Task<IActionResult> Edit([FromBody] TagModel tagModel)
        {
            if (tagModel == null)
            {
                return BadRequest("TagModel cannot be null.");
            }

            await _tagService.Update(tagModel);
            return Ok();
        }

        /// <summary>
        /// Delets an existing tag.
        /// </summary>
        /// <param name="id">Tag id.</param>
        /// <returns>Deleting tag result .</returns>
        /// <response code="200">Tag was deleted.</response>
        /// <response code="400">If id has incorrect value.</response>
        [HttpDelete("tag/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest("Id cannot be less then 1.");
            }

            await _tagService.DeleteAsync(id);
            return Ok();
        }
    }
}
