using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using WebBlogApp.Data;
using WebBlogApp.Models;

namespace WebBlogApp.Services
{
    public class UsersService
    {
        private const string AUTORIZATION = "Authorization";
        private const string BASIC = "Basic";
        private const string ISO = "iso-8859-1";
        private const string TOKEN = "Token";

        private readonly WebBlogDataContext _dataContext;
        private readonly NoSqlDataService _noSqlDataService;

        public UsersService(WebBlogDataContext dataContext, NoSqlDataService noSqlDataService)
        {
            _dataContext = dataContext;
            _noSqlDataService = noSqlDataService;

        }

        public UserModel Create(UserModel userModel)
        {
            var user = new User()
            {
                Name = userModel.Name,
                Email = userModel.Email,
                Password = userModel.Password,
                Description = userModel.Description,
                Photo = userModel.Photo
            };

            _dataContext.Users.Add(user);
            _dataContext.SaveChanges();

            userModel.Id = user.Id;

            return userModel;
        }

        public List<UserModel> Create(List<UserModel> userModels)
        {
            foreach (var userModel in userModels)
            {
                var user = new User()
                {
                    Name = userModel.Name,
                    Email = userModel.Email,
                    Password = userModel.Password,
                    Description = userModel.Description,
                    Photo = userModel.Photo
                };
                _dataContext.Users.Add(user);
            }
            _dataContext.SaveChanges();

            return userModels;
        }

        public UserModel Update(User user, UserModel userModel)
        {
            user.Name = userModel.Name;
            user.Email = userModel.Email;
            user.Password = userModel.Password;
            user.Description = userModel.Description;
            user.Photo = userModel.Photo;

            _dataContext.Users.Update(user);
            _dataContext.SaveChanges();

            return userModel;
        }

        public void Delete(User user)
        {
            _dataContext.Users.Remove(user);
            _dataContext.SaveChanges();
        }

        public (string login, string password) GetUserLoginPassFromBasicAuth(HttpRequest request)
        {
            var userName = string.Empty;
            var password = string.Empty;
            var authHeader = request.Headers[AUTORIZATION].ToString();
            if (authHeader != null && authHeader.StartsWith(BASIC))
            {
                var encodedUserNamePass = authHeader.Replace(BASIC, string.Empty);
                var encoding = Encoding.GetEncoding(ISO);
                var namePassArray = encoding.GetString(Convert.FromBase64String(encodedUserNamePass)).Split(':');
                userName = namePassArray[0];
                password = namePassArray[1];
            }
            return (userName, password);
        }

        public (ClaimsIdentity identity, int id)? GetIdentity(string email, string password)
        {
            var currentUser = GetUserByLogin(email);

            if (currentUser == null || !VerifyHashedPassword(currentUser.Password, password))
                return null;

            var claims = new List<Claim>()
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, currentUser.Email)
            };

            var claimsIdentity = new ClaimsIdentity(claims, TOKEN, ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            return (claimsIdentity, currentUser.Id);
        }

        public User? GetUserByLogin(string email)
        {
            var result = _dataContext.Users.FirstOrDefault(x => x.Email == email);
            return result;
        }

        public UserProfile? GetUserProfileById(int userId)
        {
            var user = _dataContext.Users.FirstOrDefault(x => x.Id == userId);
            if(user == null) return null;
            var result = ToProfile(user);
            return result;
        }

        public void Subscribe(int from, int to) => _noSqlDataService.SetUserSubs(from, to);

        public List<UserShortModel> GetUsersByName(string name)
        {
            var result = _dataContext.Users.Where(x => x.Name.ToLower().StartsWith(name.ToLower()))
                .Select(ToShortModel)
                .ToList();
            return result;
        }

        private bool VerifyHashedPassword(string password1, string password2)
        {
            var result = password1 == password2;
            return result;
        }

        private UserModel ToModel(User user)
        {
            var userModel = new UserModel()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Description = user.Description,
                Photo = user.Photo
            };
            return userModel;
        }

        public UserProfile ToProfile(User user)
        {
            var userSubs = _noSqlDataService.GetUserSubs(user.Id);
            var userProfile = new UserProfile()
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Description = user.Description,
                Photo = user.Photo,
                SubsCount = userSubs?.Users.Count ?? 0
            };
            return userProfile;
        }

        private UserShortModel ToShortModel(User user)
        {
            var shortModel = new UserShortModel()
            {
                Id = user.Id,
                Name = user.Name,
                Description = new string(user.Description.Take(50).ToArray()),
                Photo = user.Photo
            };
            return shortModel;
        }
    }
}   