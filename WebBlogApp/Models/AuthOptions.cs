using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace WebBlogApp.Models
{
    public class AuthOptions
    {
        public const string ISSUER = "AuthServer";
        public const string AUDIENCE = "AuthClient";
        public const int LIFETIME = 10;

        private const string KEY = "SecretKeyForAspNetWithReactWebBlogApp";

        public static SymmetricSecurityKey GetSymmetricSecurityKey()
        {
            var bytes = Encoding.ASCII.GetBytes(KEY);
            var key = new SymmetricSecurityKey(bytes);
            return key;
        }
    }
}