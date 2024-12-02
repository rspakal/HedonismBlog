using BlogDALLibrary.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using ServicesLibrary;
using ServicesLibrary.Models.User;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace API.Controllers
{
    [ApiController]
    [Route("api")]
    public class AuthenticationController : ControllerBase
    {
        private readonly IUserService _userService;

        public AuthenticationController(IUserService userService)
        {
            _userService = userService;
        }

        /// <summary>
        /// User authentication.
        /// </summary>
        /// <param name="userLoginModel">Model for user login.</param>
        /// <returns>JWT token for loggedin user.</returns>
        /// <response code="200">Returns JWT token for loggedin user.</response>
        /// <response code="400">If UserLoginModel is null.</response>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginModel userLoginModel)
        {
            if (userLoginModel == null) 
            {
                return BadRequest("UserLoginModel cannot be null");
            }
            var _user = await _userService.Login(userLoginModel);
            return Ok(new { token = CreateToken(_user) });
        }

        private string CreateToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Argument 'UserLoginModel' is null");
            }
            var claims = new[]
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.Name)
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("the-very-long-secret-key-that-is-more-than-32-characters-long"));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: "hedonismBlog",
                audience: "hedonismBlogAPI",
                claims: claims,
                expires: DateTime.Now.AddMinutes(30),
                signingCredentials: creds);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

    }
}
