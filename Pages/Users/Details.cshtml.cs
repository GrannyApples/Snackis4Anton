using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis4Anton.Data;
using Snackis4Anton.Models;

namespace Snackis4Anton.Pages.Users
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public ApplicationUser UserDetail { get; set; }
        public List<Post> Posts { get; set; }
        public List<Comment> Comments { get; set; }
        [BindProperty]
        public Message Message { get; set; }

        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            UserDetail = await _userManager.FindByIdAsync(id);

            if (UserDetail == null)
            {
                return NotFound();
            }
            Posts = await _context.Posts
                .Include(p => p.Comments)
                .Where(p => p.UserId == id)
                .ToListAsync();

            Comments = await _context.Comments
                .Include(c => c.Post)
                .Where(c => c.UserId == id)
                .ToListAsync();
            //UserDetail = await _userManager.Users.FirstOrDefaultAsync(u => u.Id == id);
            //if (UserDetail == null)
            //{
            //    return NotFound();
            //}

            //Posts = await _context.Posts.Where(p => p.UserId == id).ToListAsync();
            //Comments = await _context.Comments.Where(c => c.UserId == id).ToListAsync();

            return Page();
        }

        public async Task<IActionResult> OnPostSendDMAsync()
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

            Message.SenderId = sender.Id;
            Message.SendDate = DateTime.Now;

            _context.Messages.Add(Message);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = Message.ReceiverId });
        }
    }
}
