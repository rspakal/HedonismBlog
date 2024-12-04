using BlogDALLibrary.Exceptions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary;
using ServicesLibrary.Models;
using System;
using System.Threading.Tasks;

namespace API.Controllers
{
    [Route("api")]
    [ApiController]
    [Authorize(Roles = "administrator")]
    public class RoleController : ControllerBase
    {
        private readonly IRoleService _roleService;                  

        public RoleController(IRoleService roleService)
        {
            _roleService = roleService;
        }

        /// <summary>
        /// Shows all roles
        /// </summary>
        /// <returns>All roles.</returns>
        /// <response code="200">All roles.</response>
        [HttpGet("roles")]
        public async Task<IActionResult> Index()
        {
            var _roleModels = await _roleService.GetAllAsync();
            return Ok(_roleModels);
        }

        /// <summary>
        /// Creates a new role.
        /// </summary>
        /// <param name="roleModel">Role model.</param>
        /// <returns>Creatinging role result .</returns>
        /// <response code="200">Role was created.</response>
        /// <response code="400">If roleModel is null.</response>
        [HttpPost("role/create")]
        public async Task<IActionResult> Create(RoleModel roleModel)
        {
            if (roleModel == null) 
            {
                return BadRequest("RoleMode cannot be null");
            }

            try
            {
                await _roleService.CreateAsync(roleModel);
                return Ok();
            }
            catch (UniqueConstraintException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Edits an existing role.
        /// </summary>
        /// <param name="roleModel">Role model.</param>
        /// <returns>Editing role result .</returns>
        /// <response code="200">Role was edited.</response>
        /// <response code="400">If roleModel is null.</response>
        [HttpPut("role/edit")]
        public async Task<IActionResult> Edit(RoleModel roleModel)
        {
            if (roleModel == null)
            {
                return BadRequest("RoleModel cannot be null.");
            }
            await _roleService.Update(roleModel);
            return Ok();
        }

        /// <summary>
        /// Delets an existing role.
        /// </summary>
        /// <param name="id">Role id.</param>
        /// <returns>Deleting role result .</returns>
        /// <response code="200">Role was deleted.</response>
        /// <response code="400">If id has incorrect value.</response>
        [HttpDelete("role/delete")]
        public async Task<IActionResult> Delete(int id)
        {
            if (id < 1)
            {
                return BadRequest("Id cannot be less then 1");
            }
            await _roleService.DeleteAsync(id);
            return Ok();
        }
    }
}
