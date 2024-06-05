using System.ComponentModel.DataAnnotations.Schema;

namespace Snackis4Anton.Models
{
    public class Comment
    {
        public int Id { get; set; }
        public string? Text { get; set; }
        public string? UserId { get; set; }
        public string? Author { get; set; }
        public DateTime CreateDate { get; set; }
        //[ForeignKey("Post")]
        public int PostId { get; set; }
        public string? Image { get; set; }
        public Post? Post { get; set; }
        public bool IsReported { get; set; } = false;
    }
}
