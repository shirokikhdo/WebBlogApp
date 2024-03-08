﻿namespace WebBlogApp.Models
{
    public class UserProfile
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Description { get; set; }
        public byte[]? Photo { get; set; }
        public int SubsCount { get; set; }
    }
}