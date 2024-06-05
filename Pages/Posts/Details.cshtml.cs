using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis4Anton.Data;
using Snackis4Anton.Models;

namespace Snackis4Anton.Pages.Posts
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


        public Post Post { get; set; }
        public List<Comment> Comments { get; set; }
        [BindProperty]
        public Comment NewComment { get; set; }
        [BindProperty]
        public Message Message { get; set; }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);

            if (Post == null)
            {
                return NotFound();
            }

            Comments = Post.Comments;

            return Page();
        }

        public async Task<IActionResult> OnPostCommentAsync(int postId)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return Unauthorized();
            }

            NewComment.UserId = user.Id;
            NewComment.Author = user.UserName;
            NewComment.CreateDate = DateTime.Now;
            NewComment.PostId = postId;

            _context.Comments.Add(NewComment);
            await _context.SaveChangesAsync();

            return RedirectToPage(new { id = postId });
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
