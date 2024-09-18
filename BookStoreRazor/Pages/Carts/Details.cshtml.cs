﻿using System;
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
    public class DetailsModel : PageModel
    {
        private readonly BookStoreRazor.Data.ApiDbContext _context;

        public DetailsModel(BookStoreRazor.Data.ApiDbContext context)
        {
            _context = context;
        }

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
    }
}
