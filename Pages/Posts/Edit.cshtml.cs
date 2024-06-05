using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis4Anton.Data;
using Snackis4Anton.Models;

namespace Snackis4Anton.Pages.Posts
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        [BindProperty]
        public Post Post { get; set; }

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync(int id)
        {
            Post = await _context.Posts.FindAsync(id);

            if (Post == null)
            {
                return NotFound();
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            var postToUpdate = await _context.Posts.FindAsync(id);
            if (postToUpdate == null)
            {
                return NotFound();
            }

            postToUpdate.Title = Post.Title;
            postToUpdate.Text = Post.Text;

            await _context.SaveChangesAsync();

            return RedirectToPage("/Posts/PostDetails", new { id });
        }
    }
}
