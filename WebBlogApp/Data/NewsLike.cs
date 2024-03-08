using System.Collections.Generic;

namespace WebBlogApp.Data
{
    public class NewsLike
    {
        public int NewsId { get; set; }
        public List<int> Users { get; set; }
    }
}