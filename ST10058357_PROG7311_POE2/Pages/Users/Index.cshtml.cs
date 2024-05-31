using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ST10058357_PROG7311_POE2.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ST10058357_PROG7311_POE2.Pages.Users
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

        // public IList<User> User { get;set; } = default!;
        public IList<User> Farmers { get; set; } = new List<User>();

        public User CurrentFarmer { get; set; }
        public bool IsFarmer { get; set; }

        public async Task OnGetAsync()
        {
            //User = await _context.User.ToListAsync();
            // Get the current logged-in employee's ID
            var user = await _userManager.GetUserAsync((System.Security.Claims.ClaimsPrincipal)User);
            if (user == null)
            {
                RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            IsFarmer = await IsUserFarmer(user);

            if (IsFarmer)
            {
                CurrentFarmer = user;
            }
            // if user is employee
            else
            {
                // Fetch farmers
                var farmers = await _context.User
                    .Where(u => u.Role == "Farmer")
                    .ToListAsync();

                foreach (var farmer in farmers)
                {

                    var regEmployee = await _userManager.FindByIdAsync(farmer.RegEmployeeUserID.ToString());
                    
                    // Count the number of products associated with the farmer's user ID
                    int productCount = await _context.Product
                        .Where(p => p.FarmerId == farmer.Id)
                        .CountAsync();


                    Farmers.Add(new User
                    {
                        UserName = farmer.UserName,
                        Email = farmer.Email,
                        FirstName = farmer.FirstName,
                        LastName = farmer.LastName,
                        PhoneNumber = farmer.PhoneNumber,
                        Age = farmer.Age,
                        RegEmployeeName = regEmployee != null ? $"{regEmployee.FirstName} {regEmployee.LastName}" : "Unknown",
                        ImagePath = farmer.ImagePath,
                        RegistrationDate = farmer.RegistrationDate,
                        Address = farmer.Address,
                        ProductCount = productCount,
                    });
                }
            }

        }

        public async Task<bool> IsUserFarmer(User user)
        {
            if (user == null)
            {
                return false;
            }
            else if (user.Role == "Farmer")
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
