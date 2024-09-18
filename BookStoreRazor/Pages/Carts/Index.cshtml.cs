using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using BookStoreRazor.Data;
using BookStoreRazor.Models;

namespace BookStoreRazor.Pages.Carts
{
    public class IndexModel : PageModel
    {
        public IList<ShoppingCartItem> ShoppingCartItem { get;set; } = default!;

        public async Task OnGetAsync()
        {
            // help me rest api and give list
        }
    }
}
