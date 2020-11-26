using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RawCoding.Shop.Application.Products;

namespace RawCoding.Shop.UI.Pages
{
    public class IndexModel : PageModel
    {
        public IEnumerable<dynamic> Products { get; set; }

        public void OnGet()
        {
        }
    }
}