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
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public UserController(IUserService userService, IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userService = userService;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet("users")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> Index()
        {
            var _userPreviewModels = await _userService.GetAll();
            return Ok(_userPreviewModels);
        }

        [HttpGet("account")]
        [Authorize]
        public async Task<IActionResult> Account()
        {
            var _contextUser = HttpContext.User;
            var _email = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _userAccountModel = await _userService.GetAccountData(_email);
            return Ok(_userAccountModel);
        }

        [HttpPost("account")]
        [Authorize]
        public async Task<IActionResult> Update(UserAccountModel userAccountModel)
        {
            if (userAccountModel == null)
            {
                throw new ArgumentNullException(nameof(userAccountModel), "Argument 'UserAccountAPIMode' is null");
            }
            try
            {
                var _currenttUser = HttpContext.User;
                var _currentUserEmail = _currenttUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
                await _userService.UpdateAccountData(userAccountModel, _currentUserEmail);
                return Ok();
            }
            catch (Exception)
            {
                throw;
            }
        }


        [HttpGet("user/role")]
        [Authorize(Roles = "administrator")]

        public async Task<IActionResult> AssignRole(int userId)
        {
            if (userId < 1)
            {
                throw new ArgumentNullException(nameof(userId), "Argument id is incorrect");
            }

            var _userAssignRoleModel = await _userService.AssignRole(userId); ;
            return Ok(_userAssignRoleModel);

        }

        [HttpPut("user/role")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> AssignRole(UserAssignRoleModel userAssignRoleModel)
        {
            if (userAssignRoleModel == null)
            {
                throw new ArgumentNullException(nameof(userAssignRoleModel), "Argument 'UserAssignRoleAPIMode' is null");
            }

            await _userService.AssignRole(userAssignRoleModel);
            return Ok();
        }
    }
}
