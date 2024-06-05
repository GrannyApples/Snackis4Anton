﻿namespace Snackis4Anton.Models
{
    public class Post
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public string? Text { get; set; }
        public string? Author { get; set; }
        public string? Image { get; set; }
        public DateTime CreateDate { get; set; }
        public string? UserId { get; set; }
        public bool IsReported { get; set; } = false;
        public List<Comment>? Comments { get; set; } = new List<Comment>();
    }
}
