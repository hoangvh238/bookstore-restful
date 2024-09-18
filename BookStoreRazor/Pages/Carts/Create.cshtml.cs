using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookStoreRazor.Data;
using BookStoreRazor.Models;

namespace BookStoreRazor.Pages.Carts
{
    public class CreateModel : PageModel
    {
        private readonly BookStoreRazor.Data.ApiDbContext _context;

        public CreateModel(BookStoreRazor.Data.ApiDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ShoppingCartItem ShoppingCartItem { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.ShoppingCartItems.Add(ShoppingCartItem);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
