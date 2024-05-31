using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ST10058357_PROG7311_POE2.Models;

namespace ST10058357_PROG7311_POE2.Pages.Products
{
    [Authorize]
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;


        public IndexModel(ApplicationDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }


        /// <summary>
        /// Variables for display and filtering
        /// </summary>
        public IList<Product> Product { get;set; } = default!;
        public IList<ProductCategory> ProductCategories { get; set; } = default!;
        public IList<ProductSubCategory> ProductSubCategories { get; set; } = default!;
        public IList<User> Farmers { get; set; } = default!;



        [BindProperty(SupportsGet = true)]
        public int? CategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? SubCategoryId { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateOnly? StartDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public DateOnly? EndDate { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? FarmerId { get; set; }






        /// <summary>
        /// Method to get categories & subcategories, return filtered seach IF employee, else just farmers's products
        /// </summary>
        /// <returns></returns>
        public async Task OnGetAsync()
        {
            if (!await IsFarmer())
            {
                // Load categories and subcategories for filter dropdowns
                ProductCategories = await _context.ProductCategory.ToListAsync();
                ProductSubCategories = await _context.ProductSubCategory.ToListAsync();
                Farmers = await _context.User.Where(u => u.Role == "Farmer").ToListAsync();

                var productsQuery = _context.Product.AsQueryable();

                if (FarmerId.HasValue)
                {
                    productsQuery = productsQuery.Where(p => p.FarmerId == FarmerId.Value);
                }

                if (CategoryId.HasValue)
                {
                    productsQuery = productsQuery.Where(p => p.CategoryId == CategoryId.Value);
                }

                if (SubCategoryId.HasValue)
                {
                    productsQuery = productsQuery.Where(p => p.SubCategoryId == SubCategoryId.Value);
                }

                if (StartDate.HasValue)
                {
                    productsQuery = productsQuery.Where(p => p.ProductionDate >= StartDate.Value);
                }

                if (EndDate.HasValue)
                {
                    productsQuery = productsQuery.Where(p => p.ProductionDate <= EndDate.Value);
                }



                Product = await productsQuery.ToListAsync();
            }
            else
            {
                var user = await _userManager.GetUserAsync(User);
                var userId = await _userManager.GetUserIdAsync(user);
                int farmerId = int.Parse(userId);
                Product = await _context.Product
                    .Where(p => p.FarmerId == farmerId)
                    .ToListAsync();
            }
        }

        public async Task<bool> IsFarmer()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return false;
            }else if(user.Role == "Farmer")
            {
                return true;
            }
            else
            {
                return false;
            }
            
           // return await _userManager.IsInRoleAsync(user, "Farmer");
        }

        public async Task<string> GetFarmerName(int productId)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product != null) 
            { 
                var farmer = await _context.User.FindAsync(product.FarmerId);
                if (farmer != null)
                {
                    return farmer?.FirstName + " " + farmer?.LastName;
                }
            }
            return "Unknown";
        }

        public async Task<string> GetCategoryName(int productId)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product != null)
            {
                var category = await _context.ProductCategory.FindAsync(product.CategoryId);
                if (category != null)
                {
                    return category?.Name;
                }
            }
            return "Unknown";
        }
        public async Task<string> GetSubCategoryName(int productId)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product != null)
            {
                var subCategory = await _context.ProductSubCategory.FindAsync(product.SubCategoryId);
                if(subCategory != null)
                {
                    return subCategory?.Name;
                }
            }
            return "Unknown";
        }
        public async Task<string> GetFarmerLocation(int productId)
        {
            var product = await _context.Product.FindAsync(productId);
            if (product != null)
            {
                var farmer = await _context.User.FindAsync(product.FarmerId);
                if (farmer != null)
                {
                    return farmer?.Address;
                }
            }
            return "Unknown";
        }




    }
}
