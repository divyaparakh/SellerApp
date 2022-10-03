using Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace Auth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _config;
        private TimeSpan ExpiryDuration = new TimeSpan(30, 20, 30, 0);

        public AuthController(IConfiguration config)
        {
            _config = config ?? throw new ArgumentNullException(nameof(config));
        }

        [HttpPost("login")]
        public JsonResult Post(string UserName, string Password)
        {
            if (UserName == "username" && Password == "password")
            {
                var token = BuildToken(UserName);
                return new JsonResult(new Response<string> { Code = HttpStatusCode.OK, Message = "Success", Data = token });
            }
            else
                return new JsonResult(new Response<string> { Code = HttpStatusCode.NotFound, Message = "Invalid user" });
        }
        private string BuildToken(string userName)
        {
            string key = _config["Jwt:Key"];
            string issuer = _config["Jwt:Issuer"];
            string aud = _config["Jwt:Aud1"];
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Aud,aud),
                new Claim(JwtRegisteredClaimNames.Iss,issuer),
                new Claim(JwtRegisteredClaimNames.UniqueName, userName),
            };

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var tokenDescriptor = new JwtSecurityToken(issuer, aud, claims,
                expires: DateTime.UtcNow.AddMinutes(10), signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }
    }
}