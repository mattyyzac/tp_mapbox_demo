using Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;

namespace App.Pages
{
    public class IndexModel : PageModel
    {
        [BindProperty]
        public string MapboxToken { get; set; }

        private readonly IOptions<AppSettings> _options;
        public IndexModel(IOptions<AppSettings> options)
        {
            this._options = options;
        }

        public void OnGet()
        {
            this.MapboxToken = this._options.Value.MapboxToken;
        }
    }
}
