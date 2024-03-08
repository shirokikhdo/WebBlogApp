using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using WebBlogApp.Models;
using WebBlogApp.Services;
using System.Security.Claims;
using WebBlogApp.Data;
using Microsoft.AspNetCore.Authorization;

namespace WebBlogApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private UsersService _usersService;

        public AccountController(UsersService usersService)
        {
            _usersService = usersService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            var userEmail = HttpContext.User.Identity.Name;
            var user = _usersService.GetUserByLogin(userEmail);
            if (user is null)
                return NotFound();
            var profile = _usersService.ToProfile(user);
            var result = Ok(profile);
            return result;
        }

        [AllowAnonymous]
        [HttpPost]
        public object Create(UserModel userModel)
        {
           var user = _usersService.Create(userModel);
            var result = Ok(user);
            return result;
        }

        [HttpPatch]
        public object Update(UserModel userModel)
        {
            var userEmail = HttpContext.User.Identity.Name;
            var user = _usersService.GetUserByLogin(userEmail);
            if(user != null && user.Id != userModel.Id)
                return BadRequest();
            _usersService.Update(user, userModel);
            var result = Ok(userModel);
            return result;
        }

        [HttpDelete]
        public IActionResult Delete()
        {
            var userEmail = HttpContext.User.Identity.Name;
            var user = _usersService.GetUserByLogin(userEmail);
            _usersService.Delete(user);
            return Ok();
        }

        [AllowAnonymous]
        [HttpPost("token")]
        public IActionResult GetToken()
        {
            var userData = _usersService.GetUserLoginPassFromBasicAuth(Request);
            (ClaimsIdentity claims, int id)? identity = _usersService.GetIdentity(userData.login, userData.password);
            if (identity == null) return NotFound("Login or password is incorrect");
            var now = DateTime.UtcNow;
            var jwt = new JwtSecurityToken(
                issuer: AuthOptions.ISSUER,
                audience: AuthOptions.AUDIENCE,
                notBefore: now,
                claims: identity?.claims.Claims,
                expires: now.AddMinutes(AuthOptions.LIFETIME),
                signingCredentials: new SigningCredentials(AuthOptions.GetSymmetricSecurityKey(), SecurityAlgorithms.HmacSha256));
            var encodedJwt = new JwtSecurityTokenHandler().WriteToken(jwt);
            var tokenModel = new AuthToken(
                minutes: AuthOptions.LIFETIME,
                accessToken: encodedJwt,
                userName: userData.login,
                userId: identity.Value.id);
            var result = Ok(tokenModel);
            return result;
        }
    }
}