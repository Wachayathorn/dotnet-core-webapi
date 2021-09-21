using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using webapi.Entities;
using webapi.Repository;

namespace webapi.Middleware
{
    public class AuthenMiddleware
    {
        private readonly RequestDelegate next;
        private readonly IConfiguration configuration;
        private readonly IUserRepository userRepository;

        public AuthenMiddleware(RequestDelegate next, IConfiguration configuration, IUserRepository userRepository)
        {
            this.next = next;
            this.configuration = configuration;
            this.userRepository = userRepository;
        }

        public async Task Invoke(HttpContext context)
        {
            var isIgnore = this.IsIgnore(context);
            if (!isIgnore)
            {
                var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
                if (token != null)
                {
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.ASCII.GetBytes(configuration["JWT:Secret"]);
                    tokenHandler.ValidateToken(token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                    }, out SecurityToken validatedToken);
                    var jwtToken = (JwtSecurityToken)validatedToken;
                    context.Items["User"] = new User()
                    {
                        Id = Guid.Parse(jwtToken.Claims.First(x => x.Type == "Id").Value),
                        Firstname = jwtToken.Claims.First(x => x.Type == "Firstname").Value,
                        Lastname = jwtToken.Claims.First(x => x.Type == "Lastname").Value,
                    };
                    await next(context);
                }
                else
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await context.Response.WriteAsync("Unauthorized");
                }
            }
            else
            {
                await next(context);
            }
        }

        public Boolean IsIgnore(HttpContext context)
        {
            var path = context.Request.Path.ToString();
            if (path.Contains("authen"))
            {
                return true;
            }
            return false;
        }
    }
}