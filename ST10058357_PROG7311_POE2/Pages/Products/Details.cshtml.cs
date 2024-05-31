using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ST10058357_PROG7311_POE2.Models;

namespace ST10058357_PROG7311_POE2.Pages.Products
{
    public class DetailsModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        public DetailsModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public Product Product { get; set; } = default!;

        public String FarmerName { get; set; }
        public String CategoryName { get; set; }
        public String SubCategoryName { get; set; }
        public String FarmerLocation { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {

            if (id == null)
            {
                return NotFound();
            }
            
            var product = await _context.Product.FirstOrDefaultAsync(m => m.ProductId == id);
            
            if (product == null)
            {
                return NotFound();
            }
            else
            {
                Product = product;
                
                // Retrieve the FarmerName
                var farmer = await _userManager.FindByIdAsync(product.FarmerId.ToString());
                FarmerName = farmer?.FirstName + " " + farmer?.LastName ?? "Unknown";
                FarmerLocation = farmer?.Address ?? "Unknown";

                // Retrieve the CategoryName
                var category = await _context.ProductCategory
                    .FirstOrDefaultAsync(c => c.CategoryId == product.CategoryId);
                CategoryName = category?.Name ?? "Unknown";

                // Retrieve the SubCategoryName
                var subCategory = await _context.ProductSubCategory
                    .FirstOrDefaultAsync(sc => sc.SubCategoryId == product.SubCategoryId);
                SubCategoryName = subCategory?.Name ?? "Unknown";
            }
            return Page();
        }

        public async Task<bool> IsProductOwner()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user?.Id == Product.FarmerId)
            {
                return true;
            }
            else
            {
                return false;
            }

            // return await _userManager.IsInRoleAsync(user, "Farmer");
        }
    }
}
