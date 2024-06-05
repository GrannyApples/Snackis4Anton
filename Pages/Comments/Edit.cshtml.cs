using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis4Anton.Data;
using Snackis4Anton.Models;

namespace Snackis4Anton.Pages.Comments
{
    public class EditModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        

        public EditModel(ApplicationDbContext context)
        {
            _context = context;
        }
        [BindProperty]
        public Comment Comment { get; set; }
        public async Task<IActionResult> OnGetAsync(int id)
        {
            Comment = await _context.Comments.FindAsync(id);

            if (Comment == null)
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

            var commentToUpdate = await _context.Comments.FindAsync(id);
            if (commentToUpdate == null)
            {
                return NotFound();
            }

            commentToUpdate.Text = Comment.Text;

            await _context.SaveChangesAsync();

            return RedirectToPage("/Posts/PostDetails", new { id });
        }
    }
}
