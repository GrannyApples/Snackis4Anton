using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis4Anton.Data;
using Snackis4Anton.Models;

namespace Snackis4Anton.Pages.Users
{
    [Authorize]
    public class SendMessageModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public SendMessageModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Message NewMessage { get; set; }
        public string ReceiverId { get; set; }
        public string SenderId { get; set; }

        public string ReceiverName { get; set; }
        public async Task<IActionResult> OnGetAsync(string receiverId)
        {
            if (string.IsNullOrEmpty(receiverId))
            {
                return BadRequest("Receiver ID is required.");
            }
            var sender = await _userManager.GetUserAsync(User);
            var receiver = await _userManager.FindByIdAsync(receiverId);
            if (receiver == null)
            {
                return NotFound("Receiver not found.");
            }
            SenderId = sender.Id;
            ReceiverName = receiver.UserName;
            ReceiverId = receiver.Id;
            NewMessage = new Message();
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(string receiverId)
        {

            if (!ModelState.IsValid)
            {

                return Page();
            }

            var sender = await _userManager.GetUserAsync(User);
            if (sender == null)
            {
                return Unauthorized();
            }

            NewMessage.SenderId = sender.Id;
            NewMessage.ReceiverId = receiverId;
            NewMessage.SendDate = DateTime.Now;

            _context.Messages.Add(NewMessage);
            await _context.SaveChangesAsync();

            return RedirectToPage("/Users/Details", new {Id = receiverId});
        }
    }
}
