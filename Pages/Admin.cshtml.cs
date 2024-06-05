using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Snackis4Anton.Data;
using Snackis4Anton.Models;

namespace Snackis4Anton.Pages
{
    public class AdminModel : PageModel
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly ApplicationDbContext _context;

        public AdminModel(UserManager<ApplicationUser> userManager, ApplicationDbContext context)
        {
            _userManager = userManager;
            _context = context;
        }

        public List<UserViewModel> Users { get; set; }

        public bool IsReported { get; set; }
        public bool IsAdmin { get; set; }

        public class UserViewModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public bool IsAdmin { get; set; }
            public List<Post>? Posts { get; set; }

            public List<Comment>? Comments { get; set; }
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser == null || !currentUser.IsAdmin)
            {
                IsAdmin = false;
                return Page();
            }

            IsAdmin = true;

            var users = await _userManager.Users.ToListAsync();
            Users = users.Select(u => new UserViewModel
            {
                Id = u.Id,
                Email = u.Email,
                IsAdmin = u.IsAdmin,
                Posts = _context.Posts.Where(p => p.UserId == u.Id).ToList(),
                Comments = _context.Comments.Where(c => c.UserId == u.Id).ToList()
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostToggleAdminAsync(string id)
        {
            var user = await _userManager.FindByIdAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            user.IsAdmin = !user.IsAdmin;
            await _userManager.UpdateAsync(user);

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostToggleReportedPostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound();
            }

            post.IsReported = !post.IsReported;
            _context.Posts.Update(post);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostToggleReportedCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            comment.IsReported = !comment.IsReported;
            _context.Comments.Update(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
        public async Task<IActionResult> OnPostDeletePostAsync(int postId)
        {
            var post = await _context.Posts.FindAsync(postId);
            if (post == null)
            {
                return NotFound();
            }

            _context.Posts.Remove(post);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }

        public async Task<IActionResult> OnPostDeleteCommentAsync(int commentId)
        {
            var comment = await _context.Comments.FindAsync(commentId);
            if (comment == null)
            {
                return NotFound();
            }

            _context.Comments.Remove(comment);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
