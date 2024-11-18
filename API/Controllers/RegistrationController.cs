using API.APIModels;
using AutoMapper;
using BlogDALLibrary.Models;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Reflection;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IMapper _mapper;
        public RegistrationController(IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;
            _mapper = mapper;

        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationAPIModel userRegistrationAPIModel )
        {
            if ((await _userRepository.GetByEmail(userRegistrationAPIModel.Email)) != null)
            {
                return Conflict(new { message = "User with that email is already exists" });
            }
            var role = await _roleRepository.GetRolesByName("user");
            if (role == null)
                throw new NullReferenceException($"No role with the name \"user\" in DB");
            var _user = _mapper.Map<User>(userRegistrationAPIModel);
            _user.Role = role;
            await _userRepository.Create(_user);
            return Ok(new { message = "New user registered" });
        }
    }
}
