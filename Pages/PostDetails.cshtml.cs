//using Microsoft.AspNetCore.Mvc;
//using Microsoft.AspNetCore.Mvc.RazorPages;
//using Microsoft.EntityFrameworkCore;
//using System.Threading.Tasks;
//using Snackis4Anton.Data;
//using Snackis4Anton.Models;
//using Microsoft.AspNetCore.Identity;

//namespace Snackis4Anton.Pages
//{
//    public class PostDetailsModel : PageModel
//    {
//        private readonly ApplicationDbContext _context;
//        private readonly UserManager<ApplicationUser> _userManager;

//        public PostDetailsModel(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
//        {
//            _context = context;
//            _userManager = userManager;
//        }

//        public Post Post { get; set; }
//        public List<Comment> Comments { get; set; }
//        [BindProperty]
//        public Comment NewComment { get; set; }
//        [BindProperty]
//        public Message Message { get; set; }

//        public async Task<IActionResult> OnGetAsync(int id)
//        {
//            Post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);

//            if (Post == null)
//            {
//                return NotFound();
//            }

//            Comments = Post.Comments;

//            return Page();
//        }
//        public async Task<IActionResult> OnPostAsync()
//        {
//            if (!ModelState.IsValid)
//            {
//                return Page();
//            }

//            var post = await _context.Posts.FindAsync(NewComment.PostId);
//            if (post == null)
//            {
//                return NotFound("Post not found.");
//            }

//            NewComment.CreateDate = DateTime.Now;
//            NewComment.Author = User.Identity.Name;
//            NewComment.UserId = _userManager.GetUserId(User);

//            _context.Comments.Add(NewComment);
//            await _context.SaveChangesAsync();

//            return RedirectToPage("/Posts/PostDetails", new { id = NewComment.PostId });
//        }

//        public async Task<IActionResult> OnPostDeletePostAsync(int id)
//        {
//            var post = await _context.Posts.Include(p => p.Comments).FirstOrDefaultAsync(p => p.Id == id);
//            if (post == null)
//            {
//                return NotFound();
//            }

//            if (User.IsInRole("Admin") || User.Identity.Name == post.Author)
//            {
//                _context.Comments.RemoveRange(post.Comments);
//                _context.Posts.Remove(post);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToPage("/Index");
//        }

//        public async Task<IActionResult> OnPostDeleteCommentAsync(int id)
//        {
//            var comment = await _context.Comments.FindAsync(id);
//            if (comment == null)
//            {
//                return NotFound();
//            }

//            if (User.IsInRole("Admin") || User.Identity.Name == comment.Author)
//            {
//                _context.Comments.Remove(comment);
//                await _context.SaveChangesAsync();
//            }

//            return RedirectToPage("/Posts/PostDetails", new { id = comment.PostId });
//        }

//    }
//}
