using System.Collections.Generic;
using WebBlogApp.Models;

namespace WebBlogApp.Data
{
    public class NewsModelComparer : IComparer<NewsModel>
    {
        public int Compare(NewsModel? x, NewsModel? y)
        {
            if(x.PostDate > y.PostDate) return -1;
            if(x.PostDate < y.PostDate) return 1;
            return 0;
        }
    }
}