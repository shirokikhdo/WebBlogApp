using LiteDB;
using System;
using System.Collections.Generic;
using System.Linq;

namespace WebBlogApp.Data
{
    public class NoSqlDataService
    {
        private const string DB_PATH = "WebBlogApp_NoSQLDB.db";
        private const string SUBS_COLLECTION = "SubsCollection";
        private const string NEWS_LIKES_COLLECTION = "NewsLikesCollection";

        public UserSubs GetUserSubs(int userId)
        {
            using (var db = new LiteDatabase(DB_PATH))
            {
                var subs = db.GetCollection<UserSubs>(SUBS_COLLECTION);
                var userSubs = subs.FindOne(x=>x.Id == userId);
                return userSubs;
            }
        }

        public UserSubs SetUserSubs(int from, int to)
        {
            using (var db = new LiteDatabase(DB_PATH))
            {
                var subs = db.GetCollection<UserSubs>(SUBS_COLLECTION);
                var userSubs = subs.FindOne(x => x.Id == from);
                var sub = new UserSub()
                {
                    Id = to,
                    Date = DateTime.Now
                };
                if (userSubs != null && !userSubs.Users.Select(x=>x.Id).Contains(to))
                {
                    userSubs.Users.Add(sub);
                    subs.Update(userSubs);
                }
                else
                {
                    var newUserSubs = new UserSubs()
                    {
                        Id = from,
                        Users = new List<UserSub>() { sub }
                    };
                    subs.Insert(newUserSubs);
                    subs.EnsureIndex(x => x.Id);
                    userSubs = newUserSubs;
                }
                return userSubs;
            }
        }

        public NewsLike GetNewsLikes(int newsId)
        {
            using (var db = new LiteDatabase(DB_PATH))
            {
                var likes = db.GetCollection<NewsLike>(NEWS_LIKES_COLLECTION);
                var newsLikes = likes.FindOne(x => x.NewsId == newsId);
                return newsLikes;
            }
        }

        public NewsLike SetNewsLikes(int from, int newsId)
        {
            using (var db = new LiteDatabase(DB_PATH))
            {
                var likes = db.GetCollection<NewsLike>(NEWS_LIKES_COLLECTION);
                var newsLikes = likes.FindOne(x => x.NewsId == newsId);
                if (newsLikes != null && !newsLikes.Users.Contains(from))
                {
                    newsLikes.Users.Add(from);
                    likes.Update(newsLikes);
                }
                else
                {
                    var newNewsLikes = new NewsLike()
                    {
                        NewsId = newsId,
                        Users = new List<int>() { from }
                    };
                    likes.Insert(newNewsLikes);
                    likes.EnsureIndex(x => x.NewsId);
                    newsLikes = newNewsLikes;
                }
                return newsLikes;
            }
        }
    }
}