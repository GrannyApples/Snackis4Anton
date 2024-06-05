using System.ComponentModel.DataAnnotations;

namespace Snackis4Anton.Models
{
    public class Message
    {
        public int Id { get; set; }
        [Required]
        public string Text { get; set; }
        [Required]
        public string SenderId { get; set; }
        [Required]
        public string ReceiverId { get; set; }
        public bool IsRead { get; set; } = false;
        public DateTime SendDate { get; set; }
    }
}
