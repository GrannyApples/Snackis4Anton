using Microsoft.AspNetCore.Identity;
namespace Snackis4Anton.Models

{
    public class ApplicationUser :IdentityUser
    {
        public bool IsAdmin { get; set; }
        //public string? Image { get; set; }
        public DateTime JoindeDate { get; set; } = DateTime.Now;
    }
}
