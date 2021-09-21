using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using webapi.Dto;
using webapi.Repository;

namespace Controllers.Controllers
{
    [Route("authen")]
    [ApiController]
    public class AuthenController : ControllerBase
    {
        private readonly IUserRepository userRepository;
        private readonly IConfiguration configuration;
        public AuthenController(IUserRepository userRepository, IConfiguration configuration)
        {
            this.userRepository = userRepository;
            this.configuration = configuration;
        }

        [HttpPost("signin")]
        public ActionResult<SignInResponseDto> SignIn([FromBody] SignInRequestDto data)
        {
            var user = userRepository.GetUserByUsername(data.Username);

            if (user == null) return StatusCode(StatusCodes.Status401Unauthorized, "Username or Password is invalid.");
            if (user.Password != data.Password) return StatusCode(StatusCodes.Status401Unauthorized, "Username or Password is invalid.");

            var userClaims = new List<Claim>
            {
                new Claim("Id" , user.Id.ToString()),
                new Claim("Firstname" , user.Firstname),
                new Claim("Lastname" , user.Lastname),
            };

            var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["JWT:Secret"]));

            var tokenData = new JwtSecurityToken(
                claims: userClaims,
                expires: DateTime.Now.AddHours(6),
                signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            );

            return StatusCode(StatusCodes.Status200OK, new SignInResponseDto()
            {
                Token = new JwtSecurityTokenHandler().WriteToken(tokenData)
            });
        }
    }
}