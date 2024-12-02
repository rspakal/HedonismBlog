using API.APIModels.User;
using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using ServicesLibrary;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using ServicesLibrary.Models.User;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class RegistrationController : ControllerBase
    {
        private readonly IUserService _userService;
        public RegistrationController(IUserService userService, IUserRepository userRepository, IRoleRepository roleRepository, IMapper mapper)
        {
            _userService = userService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel userRegistrationModel )
        {
            try
            {
                await _userService.Register(userRegistrationModel);
                return Ok();
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
