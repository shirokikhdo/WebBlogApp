using System;
using System.Collections.Generic;
using System.Linq;
using WebBlogApp.Data;
using WebBlogApp.Models;

namespace WebBlogApp.Services
{
    public class NewsService
    {
        private readonly WebBlogDataContext _dataContext;
        private readonly NoSqlDataService _noSqlDataService;

        public NewsService(WebBlogDataContext dataContext, NoSqlDataService noSqlDataService)
        {
            _dataContext = dataContext;
            _noSqlDataService = noSqlDataService;
        }

        public List<NewsModel> GetByAuthor(int authorId)
        {
            var newsModels = _dataContext.News
                .Where(x=>x.AuthorId == authorId)
                .OrderBy(x=>x.PostDate)
                .Reverse()
                .Select(ToModel)
                .ToList();
            return newsModels;
        }

        public NewsModel Create(NewsModel newsModel, int userId)
        {
            var news = new News()
            {
                AuthorId = userId,
                Text = newsModel.Text,
                Image = newsModel.Image,
                PostDate = DateTime.Now
            };

            _dataContext.News.Add(news);
            _dataContext.SaveChanges();

            newsModel.Id = news.Id;
            newsModel.PostDate = news.PostDate;

            return newsModel;
        }

        public List<NewsModel> Create(List<NewsModel> newsModels, int userId)
        {
            foreach (var newsModel in newsModels)
            {
                var news = new News()
                {
                    AuthorId = userId,
                    Text = newsModel.Text,
                    Image = newsModel.Image,
                    PostDate = DateTime.Now
                };
                _dataContext.News.Add(news);
            }
            _dataContext.SaveChanges();
            return newsModels;
        }

        public NewsModel Update(NewsModel newsModel, int userId)
        {
            var news = _dataContext.News
                .FirstOrDefault(x=>x.Id == newsModel.Id && x.AuthorId == userId);

            if (news == null)
                return null;

            news.Text = newsModel.Text;
            news.Image = newsModel.Image;

            _dataContext.News.Update(news);
            _dataContext.SaveChanges();

            newsModel = ToModel(news);

            return newsModel;
        }

        public void Delete(int newsId, int userId)
        {
            var news = _dataContext.News
                .FirstOrDefault(x=>x.Id == newsId && x.AuthorId== userId);
            _dataContext.News.Remove(news);
            _dataContext.SaveChanges();
        }

        public List<NewsModel> GetNewsForUser(int userId)
        {
            var subs = _noSqlDataService.GetUserSubs(userId);
            var newsModels = new List<NewsModel>();
            if(subs is null)
                return newsModels;
            foreach(var sub in subs.Users)
            {
                var newsByAuthor = _dataContext.News.Where(x=>x.AuthorId == sub.Id);
                newsModels.AddRange(newsByAuthor.Select(ToModel));
            }
            newsModels.Sort(new NewsModelComparer());
            return newsModels;
        }

        public void SetLike(int newsId, int userId) => _noSqlDataService.SetNewsLikes(userId, newsId);

        private NewsModel ToModel(News news)
        {
            var likes = _noSqlDataService.GetNewsLikes(news.Id);
            var newsModel = new NewsModel()
            {
                Id = news.Id,
                Text = news.Text,
                Image = news.Image,
                PostDate = news.PostDate,
                LikesCount = likes?.Users.Count ?? 0
            };
            return newsModel;
        }
    }
}