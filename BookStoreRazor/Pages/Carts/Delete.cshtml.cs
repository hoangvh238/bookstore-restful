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
    public class DeleteModel : PageModel
    {
        private readonly BookStoreRazor.Data.ApiDbContext _context;

        public DeleteModel(BookStoreRazor.Data.ApiDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ShoppingCartItem ShoppingCartItem { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingcartitem = await _context.ShoppingCartItems.FirstOrDefaultAsync(m => m.Id == id);

            if (shoppingcartitem == null)
            {
                return NotFound();
            }
            else
            {
                ShoppingCartItem = shoppingcartitem;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var shoppingcartitem = await _context.ShoppingCartItems.FindAsync(id);
            if (shoppingcartitem != null)
            {
                ShoppingCartItem = shoppingcartitem;
                _context.ShoppingCartItems.Remove(ShoppingCartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
