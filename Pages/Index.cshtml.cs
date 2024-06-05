using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Snackis4Anton.Models;
using Snackis4Anton.Services;

namespace Snackis4Anton.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApiService _apiService;

        public IndexModel(ApiService apiService)
        {
            _apiService = apiService;
        }

        public IList<Post> Posts { get; set; }

        //[BindProperty]
        //public IFormFile Image { get; set; }

        public async Task OnGetAsync()
        {
            Posts = await _apiService.GetPostsAsync();
        }
    }
}
