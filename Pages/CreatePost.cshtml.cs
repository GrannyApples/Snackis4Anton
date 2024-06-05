using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis4Anton.Data;
using Snackis4Anton.Models;

namespace Snackis4Anton.Pages
{
    public class CreatePostModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public CreatePostModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        [BindProperty]
        public Post Post { get; set; }
        [BindProperty]
        public IFormFile? Image { get; set; }
        public bool IsAdmin { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsAdmin)
            {
                IsAdmin = false;
                return Page();
            }

            IsAdmin = true;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null || !user.IsAdmin)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            Post.UserId = user.Id;
            Post.Author = user.UserName;
            Post.CreateDate = DateTime.Now;

            if (Image != null)
            {
                var fileName = Path.GetFileNameWithoutExtension(Image.FileName);
                var extension = Path.GetExtension(Image.FileName);
                Post.Image = $"{fileName}_{DateTime.Now.ToString("yyyyMMddHHmmss")}{extension}";

                var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images", Post.Image);
                using (var stream = new FileStream(path, FileMode.Create))
                {
                    await Image.CopyToAsync(stream);
                }
            }

            _context.Posts.Add(Post);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
