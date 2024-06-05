using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis4Anton.Data;
using Snackis4Anton.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;


namespace Snackis4Anton.Pages.Users
{
    [Authorize]
    public class MessagesModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public MessagesModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public IList<Message> Inbox { get; set; }
        public IList<Message> Sent { get; set; }



        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);

            if (user == null)
            {
                return Unauthorized();
            }

            Inbox = await _context.Messages.Where(m => m.ReceiverId == user.Id).ToListAsync();
            Sent = await _context.Messages.Where(m => m.SenderId == user.Id).ToListAsync();

            return Page();
        }
    }
}
