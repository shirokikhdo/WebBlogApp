using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using WebBlogApp.Models;
using WebBlogApp.Services;

namespace WebBlogApp.Controllers
{
    [Authorize]
    [Route("[controller]")]
    [ApiController]
    public class NewsController : ControllerBase
    {
        private NewsService _newsService;
        private UsersService _usersService;

        public NewsController(NewsService newsService, UsersService usersService)
        {
            _newsService = newsService;
            _usersService = usersService;
        }

        [HttpGet("{userId}")]
        public IActionResult GetByAuthor(int userId)
        {
            var news = _newsService.GetByAuthor(userId);
            var result = Ok(news);
            return result;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var user = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (user == null)
                return NotFound();
            var news = _newsService.GetNewsForUser(user.Id);
            var result = Ok(news);
            return result;
        }

        [HttpPost]
        public IActionResult Create([FromBody] NewsModel newsModel)
        {
            var user = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (user == null)
                return NotFound();
            var newNewsModel = _newsService.Create(newsModel, user.Id);
            var result = Ok(newNewsModel);
            return result;
        }

        [HttpPost("create")]
        public IActionResult Create([FromBody] List<NewsModel> newsModels)
        {
            var user = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (user == null)
                return NotFound();
            var newNewsModel = _newsService.Create(newsModels, user.Id);
            var result = Ok(newNewsModel);
            return result;
        }

        [HttpPatch]
        public IActionResult Update([FromBody] NewsModel newsModel)
        {
            var user = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (user == null)
                return NotFound();
            var newNewsModel = _newsService.Update(newsModel, user.Id);
            var result = Ok(newNewsModel);
            return result;
        }

        [HttpDelete("{newsId}")]
        public IActionResult Delete(int newsId)
        {
            var user = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (user == null)
                return NotFound();
            _newsService.Delete(newsId, user.Id);
            var result = Ok();
            return result;
        }

        [HttpPost("like/{newsId}")]
        public IActionResult SetLike(int newsId)
        {
            var user = _usersService.GetUserByLogin(HttpContext.User.Identity.Name);
            if (user == null)
                return NotFound();
            _newsService.SetLike(newsId, user.Id);
            var result = Ok();
            return result;
        }
    }
}