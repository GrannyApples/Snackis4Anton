namespace Snackis4Anton.Models
{
    public class ReportedItem
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Title { get; set; }
        public string Text { get; set; }
        public string ReportedBy { get; set; }
    }
}
