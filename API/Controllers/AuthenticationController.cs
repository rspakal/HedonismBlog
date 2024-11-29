using API.APIModels.User;
using AutoMapper;
using BlogDALLibrary.Entities;
using BlogDALLibrary.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
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
        private readonly IUserRepository _userRepository;
        private readonly IMapper  _mapper;
        public AuthenticationController(IUserRepository userRepository, IMapper mapper)
        {
            _userRepository = userRepository;
            _mapper = mapper;
        }

        private string CreateToken(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "Argument 'User' is null");
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

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginAPIModel userLoginAPIModel)
        {
            var _user = await _userRepository.Get(userLoginAPIModel.Email);
            if (_user == null || _user.Password != userLoginAPIModel.Password) 
            {
                return Unauthorized(new { message = "Wrong email or password." });
            }
            return Ok(new { token = CreateToken(_user) });
        }

    }
}
