using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ST10058357_PROG7311_POE2.Models;


namespace ST10058357_PROG7311_POE2.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly UserManager<User> _userManager;

        public CreateModel(ApplicationDbContext context, IWebHostEnvironment webHostEnvironment, UserManager<User> userManager)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
            _userManager = userManager;
        }



        [BindProperty]
        public Product Product { get; set; } = default!;
        public User Farmer { get; set; }
        public IFormFile ProductImage { get; set; }
        public List<ProductCategory> ProductCategories { get; set; }
        public List<ProductSubCategory> ProductSubCategories { get; set; }


        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        public async Task<IActionResult> OnGetAsync()
        {
            ProductCategories = await _context.ProductCategory.ToListAsync();
            ProductSubCategories = await _context.ProductSubCategory.ToListAsync();

            Farmer = await _userManager.GetUserAsync(User);

            return Page();
        }


        public async Task<JsonResult> OnGetSubCategories(int categoryId)
        {
            var subCategories = await _context.ProductSubCategory
                .Where(sc => sc.CategoryId == categoryId)
                .Select(sc => new { subCategoryId = sc.SubCategoryId, name = sc.Name })
                .ToListAsync();

            return new JsonResult(subCategories);
        }


        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
            Farmer = await _userManager.GetUserAsync(User);
            if (Farmer == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            Product.FarmerId = Farmer.Id;

            if (!ModelState.IsValid)
            {
                ProductCategories = _context.ProductCategory.ToList();
                return Page();
            }

            if (ProductImage != null)
            {
                // File extension (e.g., .jpeg, .png)
                var extension = Path.GetExtension(ProductImage.FileName);
                if (!AllowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("ProductImage", "Unsupported file type. Please upload an image with one of the following extensions: .jpg, .jpeg, .png");
                    ProductCategories = _context.ProductCategory.ToList();
                    return Page();
                }

                // Extract the name without extension
                var fileName = Path.GetFileNameWithoutExtension(ProductImage.FileName);

                // Ensure the directory exists
                var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/products");
                if (!Directory.Exists(uploadsFolder))
                {
                    Directory.CreateDirectory(uploadsFolder);
                }

                // Combine the base path (wwwroot/images/products) with the unique file name and the extension.
                var uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    // Copy the contents of the uploaded file to the file stream asynchronously.
                    await ProductImage.CopyToAsync(fileStream);
                }

                // Set the ImagePath for the product
                Product.ImagePath = Path.Combine("images/products", uniqueFileName);
                Product.FarmerId = Farmer.Id;
            }

            // Validate the ModelState
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Product.Add(Product);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }

    }
}
