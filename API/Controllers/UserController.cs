using API.APIModels.User;
using AutoMapper;
using BlogDALLibrary.Models;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class UserController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public UserController(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;
        }

        [HttpGet("users")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> GetAll()
        {
            var _users = await _userRepository.GetAllAsNoTracking();
            var _userPreviewModels = _mapper.Map<List<UserPreviewAPIModel>>(_users);
            return Ok(_userPreviewModels);
        }

        [HttpGet("user/account")]
        [Authorize]
        public async Task<IActionResult> Account()
        {
            var _contextUser = HttpContext.User;
            var _contextUserEmail = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = await _userRepository.Get(_contextUserEmail);
            var _userAccountAPIModel = _mapper.Map<UserAccountAPIModel>(_user);
            return Ok(_userAccountAPIModel);
        }

        [HttpPut("user/accountupdate")]
        [Authorize]
        public async Task<IActionResult> Update(UserAccountAPIModel userAccountAPIModel)
        {
            if (userAccountAPIModel == null)
            {
                throw new ArgumentNullException(nameof(userAccountAPIModel), "Argument 'UserAccountAPIMode' is null");
            }

            var _contextUser = HttpContext.User;
            var _contextUserEmail = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (_contextUserEmail != userAccountAPIModel.Email)
            {
                return Forbid();
            }

            var _user = _mapper.Map<User>(userAccountAPIModel);
            await _userRepository.Update(_user);
            return Ok();

        }

        [HttpGet("user/assignrole")]
        [Authorize(Roles = "administrator")]

        public async Task<IActionResult> AssignRole(int id)
        {
            if (id == 0)
            {
                throw new ArgumentNullException(nameof(id), "Argument id is zero or null");
            }

            var _user = await _userRepository.Get(id);
            var _roles = await _roleRepository.GetAll();
            var _userAssignRoleAPIModel = _mapper.Map<UserAssignRoleAPIModel>(_user);
            _roles
                .Where(key => !_userAssignRoleAPIModel.Roles.ContainsKey(key.Name))
                .ToList()
                .ForEach(key => _userAssignRoleAPIModel.Roles[key.Name] = false);
            return Ok(_userAssignRoleAPIModel);

        }

        [HttpPut("user/assignrole")]
        [Authorize(Roles = "administrator")]
        public async Task<IActionResult> AssignRole(UserAssignRoleAPIModel userAssignRoleAPIModel)
        {
            if (userAssignRoleAPIModel == null)
            {
                throw new ArgumentNullException(nameof(userAssignRoleAPIModel), "Argument 'UserAssignRoleAPIMode' is null");
            }

            var _role = await _roleRepository.GetRolesByName(userAssignRoleAPIModel.Roles.FirstOrDefault(r => r.Value == true).Key ?? "user");
            var _user = await _userRepository.Get(userAssignRoleAPIModel.Email);
            _user.Role = _role;
            await _userRepository.Update(_user);
            return Ok();
        }
    }
}
