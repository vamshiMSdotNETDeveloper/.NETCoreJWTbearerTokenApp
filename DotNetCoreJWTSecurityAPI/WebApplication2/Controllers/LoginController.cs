using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebApplication2.ADO;
using WebApplication2.Models;

namespace WebApplication2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private IConfiguration _config;

        public LoginController(IConfiguration config)
        {
            _config = config;
        }

      
        public IActionResult Login(string userid="vamshi",string password="vamshi")
        {
            JWTUserModel usermodel = new JWTUserModel();
            IActionResult Response = Unauthorized();

            usermodel = AuthenticateUser(userid,password);
            if(usermodel!=null)
            {
                var tokenstr = GenerateJWTtoken(usermodel);
                Response=Ok (new { token = tokenstr });
            }

            return Response;
        }

        private JWTUserModel AuthenticateUser(string userid, string password)
        {
            DAL dal = new DAL(); JWTUserModel userModel = new JWTUserModel();
            userModel=dal.GetLoggedUserDetail(userid);
            return userModel;
        }


        private string GenerateJWTtoken(JWTUserModel userModel)
        {
            var securitykey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:key"]));
            var credentials = new SigningCredentials(securitykey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,userModel.userid),
                new Claim(JwtRegisteredClaimNames.Sub,userModel.role),
            };

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:issuer"],
                audience: _config["Jwt:issuer"],
                claims,
                expires: DateTime.Now.AddMinutes(120),
                signingCredentials: credentials
                );
            var encodetoken = new JwtSecurityTokenHandler().WriteToken(token);
            return encodetoken;
        }

        [HttpPost]
        public string Post()
        {
            var identity = HttpContext.User.Identity as ClaimsIdentity;
            IList<Claim> claim = identity.Claims.ToList();
            var username = claim[0].Value.ToString();
            return "Welcome to: "+username;
        }
    }
}
