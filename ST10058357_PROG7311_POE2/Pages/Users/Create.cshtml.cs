using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.Encodings.Web;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.WebUtilities;
using ST10058357_PROG7311_POE2.Models;
using ST10058357_PROG7311_POE2.Areas.Identity.Pages.Account;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Hosting;

namespace ST10058357_PROG7311_POE2.Pages.Users
{
    public class CreateModel : PageModel
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<User> _userManager;

        private readonly SignInManager<User> _signInManager;
        private readonly IUserStore<User> _userStore;
        private readonly IUserEmailStore<User> _emailStore;
        private readonly ILogger<RegisterModel> _logger;
        private readonly IEmailSender _emailSender;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public CreateModel(UserManager<User> userManager,
            IUserStore<User> userStore,
            SignInManager<User> signInManager,
            ILogger<RegisterModel> logger,
            IEmailSender emailSender,
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
            _emailSender = emailSender;
            _webHostEnvironment = webHostEnvironment;
        }


        private static readonly HashSet<string> AllowedExtensions = new HashSet<string>(StringComparer.OrdinalIgnoreCase)
        {
            ".jpg",
            ".jpeg",
            ".png"
        };

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public IList<AuthenticationScheme> ExternalLogins { get; set; }
        public IFormFile? ProfileImage { get; set; }
        public string uniqueFileName { get; set; }

        public class InputModel
        {
            [Required]
            public string UserName { get; set; }

            [Required]
            [EmailAddress]
            public string Email { get; set; }

            [Required]
            [DataType(DataType.Password)]
            public string Password { get; set; }

            [Required]
            public string FirstName { get; set; }

            [Required]
            public string LastName { get; set; }

            [Required]
            public int Age { get; set; }

            [Required]
            public string PhoneNumber { get; set; }
            [Required]
            public string Address { get; set; }
            


        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            var employee = await _userManager.GetUserAsync(User);
            if (employee == null)
            {
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            returnUrl ??= Url.Content("~/");
            ExternalLogins = (await _signInManager.GetExternalAuthenticationSchemesAsync()).ToList();
            if (ModelState.IsValid)
            {
                if (ProfileImage != null)
                {
                    // File extension (e.g., .jpeg, .png)
                    var extension = Path.GetExtension(ProfileImage.FileName);
                    if (!AllowedExtensions.Contains(extension))
                    {
                        ModelState.AddModelError("ProfileImage", "Unsupported file type. Please upload an image with one of the following extensions: .jpg, .jpeg, .png");
                        return Page();
                    }
                    // Extract the name without extension
                    var fileName = Path.GetFileNameWithoutExtension(ProfileImage.FileName);

                    // Ensure the directory exists
                    var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "images/profilePics");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Combine the base path (wwwroot/images/products) with the unique file name and the extension.
                    uniqueFileName = $"{fileName}_{DateTime.Now:yyyyMMddHHmmss}{extension}";
                    var filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        // Copy the contents of the uploaded file to the file stream asynchronously.
                        await ProfileImage.CopyToAsync(fileStream);
                    }

                }

                var user = CreateUser();

                user.FirstName = Input.FirstName;
                user.LastName = Input.LastName;
                user.UserName = Input.UserName;
                user.Age = Input.Age;
                user.PhoneNumber = Input.PhoneNumber;
                user.Role = "Farmer";
                user.RegistrationDate = DateOnly.FromDateTime(DateTime.Now);
                user.RegEmployeeUserID = employee.Id;
                user.RegEmployeeName = employee.FirstName + " " + employee.LastName;
                user.Address = Input.Address;
                user.ImagePath = Path.Combine("images/profilePics", uniqueFileName);
                


                await _userStore.SetUserNameAsync(user, Input.UserName, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                var result = await _userManager.CreateAsync(user, Input.Password);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    var userId = await _userManager.GetUserIdAsync(user);
                    var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
                    var callbackUrl = Url.Page(
                        "/Account/ConfirmEmail",
                    pageHandler: null,
                        values: new { area = "Identity", userId = userId, code = code, returnUrl = returnUrl },
                        protocol: Request.Scheme);

                    await _emailSender.SendEmailAsync(Input.Email, "Confirm your email",
                        $"Please confirm your account by <a href='{HtmlEncoder.Default.Encode(callbackUrl)}'>clicking here</a>.");

                    if (_userManager.Options.SignIn.RequireConfirmedAccount)
                    {
                        return RedirectToPage("RegisterConfirmation", new { email = Input.Email, returnUrl = returnUrl });
                    }
                    /*else
                    {
                        await _signInManager.SignInAsync(user, isPersistent: false);
                        return LocalRedirect(returnUrl);
                    }*/
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }

            }

           /* var user = new User
            {
                UserName = Input.UserName,
                Email = Input.Email,
                FirstName = Input.FirstName,
                LastName = Input.LastName,
                Age = Input.Age,
                PhoneNumber = Input.PhoneNumber,
                Role = "Farmer",  // Assign the Farmer role
                RegistrationDate = DateTime.Now,
                Address = Input.Address,
                RegEmployeeUserID = employee.Id,
                RegEmployeeName = employee.FirstName + " " + employee.LastName,
                NormalizedEmail = Input.Email.ToUpper(),
                NormalizedUserName = Input.UserName.ToUpper(),
            };

            // Hash the password before creating the user
            var hashedPassword = _userManager.PasswordHasher.HashPassword(user, Input.Password);
            user.PasswordHash = hashedPassword;

            _context.User.Add(user);
            await _context.SaveChangesAsync();*/

            return RedirectToPage("./Index");
        }

        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(User)}'. " +
                    $"Ensure that '{nameof(User)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<User> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<User>)_userStore;
        }
    }
}
