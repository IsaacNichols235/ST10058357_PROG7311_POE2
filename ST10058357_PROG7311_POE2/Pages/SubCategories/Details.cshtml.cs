using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ST10058357_PROG7311_POE2.Models;

namespace ST10058357_PROG7311_POE2.Pages.SubCategories
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;

        public DetailsModel(ApplicationDbContext context)
        {
            _context = context;
        }

        public ProductSubCategory ProductSubCategory { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var productsubcategory = await _context.ProductSubCategory.FirstOrDefaultAsync(m => m.SubCategoryId == id);
            if (productsubcategory == null)
            {
                return NotFound();
            }
            else
            {
                ProductSubCategory = productsubcategory;
            }
            return Page();
        }
    }
}
