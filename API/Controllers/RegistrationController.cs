using AutoMapper;
using BlogDALLibrary.Exceptions;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;
using ServicesLibrary;
using ServicesLibrary.Models.User;
using System.Threading.Tasks;

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

        /// <summary>
        /// Registers new user
        /// </summary>
        /// <param name="userRegistrationModel">Model for user registration.</param>
        /// <returns>Registration result.</returns>
        /// <response code="200">New user was registered.</response>
        /// <response code="400">If UserRegistrationModel is null.</response>
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] UserRegistrationModel userRegistrationModel)
        {
            if (userRegistrationModel == null) 
            {
                return BadRequest("UserRegistrationModel cannot be null");
            }
            try
            {
                await _userService.Register(userRegistrationModel);
                return Ok();
            }
            catch (UniqueConstraintException ex)
            {
                return BadRequest(ex.Message);
            }

        }
    }
}
