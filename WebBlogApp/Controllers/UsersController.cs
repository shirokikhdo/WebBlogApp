using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebBlogApp.Models;
using WebBlogApp.Services;

namespace WebBlogApp.Controllers
{
    [Route("[controller]")]
    [ApiController]
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly UsersService _userService;

        public UsersController(UsersService userService)
        {
            _userService = userService;
        }

        [HttpGet("all/{name}")]
        public IActionResult GetUsersByName(string name)
        {
            var users = _userService.GetUsersByName(name);
            var result = Ok(users);
            return result;
        }

        [HttpPost("subs/{userId}")]
        public IActionResult Subscribe(int userId)
        {
            var user = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (user == null)
                return NotFound();
            if(user.Id != userId)
                _userService.Subscribe(user.Id, userId);
            else
                return BadRequest();
            var result = Ok();
            return result;
        }

        [HttpGet("{userId}")]
        public IActionResult Get(int userId)
        {
            var userProfile = _userService.GetUserProfileById(userId);
            var result = Ok(userProfile);
            return result;
        }

        [HttpPost("create")]
        public IActionResult CreateUsers([FromBody] List<UserModel> userModels)
        {
            var user = _userService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (user == null)
                return NotFound();
            if(user.Id != 1)
                return BadRequest();
            var users = _userService.Create(userModels);
            var result = Ok(users);
            return result;
        }
    }
}