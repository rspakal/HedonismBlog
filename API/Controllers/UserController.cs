using AutoMapper;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary;
using ServicesLibrary.Models.User;
using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    public class UserController : BlogAPIBaseController
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// Shows all registered users.
        /// </summary>
        /// <returns>User short info models.</returns>
        /// <response code="200">Returns users short info data.</response>
        [HttpGet("users")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Index()
        {
            var _userPreviewModels = await _userService.GetAll();
            return Ok(_userPreviewModels);
        }

        /// <summary>
        /// Shows currently logged user account data.
        /// </summary>
        /// <returns>User data model.</returns>
        /// <response code="200">Returns full user data.</response>
        [HttpGet("account")]
        [Authorize]
        public async Task<IActionResult> Account()
        {
            var _currentUserEmail = GetClaimValue(ClaimTypes.Email);
            var _userAccountModel = await _userService.GetAccountData(_currentUserEmail);
            return Ok(_userAccountModel);
        }

        /// <summary>
        /// Updates currently logged user account data.
        /// </summary>
        /// <param name="userAccountModel">User data model.</param>
        /// <response code="200">Updated user account data.</response>
        /// <response code="400">If userAccountModel is null.</response>
        [HttpPost("account")]
        [Authorize]
        public async Task<IActionResult> Update(UserAccountModel userAccountModel)
        {
            if (userAccountModel == null)
            {
                return BadRequest("UserAccountModel cannot be null.");
            }

            var _currentUserEmail = GetClaimValue(ClaimTypes.Email);
            await _userService.UpdateAccountData(userAccountModel, _currentUserEmail);
            return Ok();
        }

        /// <summary>
        /// Shows user role data.
        /// </summary>
        /// <param name="userId">User id.</param>
        /// <returns>User role data.</returns>
        /// <response code="200">User role data.</response>
        /// <response code="400">If user id has incorrect value.</response>
        [HttpGet("user/role")]
        [Authorize(Roles = "administrator")]

        public async Task<IActionResult> AssignRole(int userId)
        {
            if (userId < 1)
            {
                return BadRequest("Id cannot be less then 1.");
            }

            var _userAssignRoleModel = await _userService.AssignRole(userId); ;
            return Ok(_userAssignRoleModel);

        }

        /// <summary>
        /// Assigns role to user.
        /// </summary>
        /// <param name="userAssignRoleModel">Assign role to user model.</param>
        /// <response code="200">Assigned role to user.</response>
        /// <response code="400">If userAssignRoleModel is null.</response>
        [HttpPut("user/role")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> AssignRole(UserAssignRoleModel userAssignRoleModel)
        {
            if (userAssignRoleModel == null)
            {
                return BadRequest("UserAssignRoleModel cannot be null.");
            }

            await _userService.AssignRole(userAssignRoleModel);
            return Ok();
        }
    }
}
