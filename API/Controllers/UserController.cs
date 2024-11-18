using API.APIModels;
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
            return Ok(_users);
        }

        [HttpGet("user/account")]
        [Authorize]
        public async Task<IActionResult> Account()
        {
            var _contextUser = HttpContext.User;
            var _email = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var _user = await _userRepository.GetByEmail(_email);
            var _userInfoAPIModel = _mapper.Map<UserInfoAPIModel>(_user);
            return Ok(_userInfoAPIModel);
        }

        [HttpPut("user/update")]
        [Authorize]
        public async Task<IActionResult> Update(UserInfoAPIModel userInfoAPIModel)
        {
            var _contextUser = HttpContext.User;
            var _contextUserEmail = _contextUser.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            if (_contextUserEmail != userInfoAPIModel.Email)
            {
                return Forbid();
            }
            var _user = _mapper.Map<User>(userInfoAPIModel);
            try
            {
                await _userRepository.Update(_user);
                return Ok();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Server internal error: {ex.Message}");
            }
        }

        [HttpGet("user/assignrole")]
        public async Task<IActionResult> AssignRole(int id)
        {
            var _user = await _userRepository.Get(id);
            var _roles = await _roleRepository.GetAllAsync();
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
            var _role = await _roleRepository.GetRolesByName(userAssignRoleAPIModel.Roles.FirstOrDefault(r => r.Value == true).Key ?? "user");
            var _user = await _userRepository.Get(userAssignRoleAPIModel.Id);
            _user.Role = _role;
            await _userRepository.Update(_user);
            return Ok();
        }
    }
}
