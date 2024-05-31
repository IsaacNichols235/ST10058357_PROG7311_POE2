using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ST10058357_PROG7311_POE2.Models;

namespace ST10058357_PROG7311_POE2.Pages
{
    //[Authorize]
    public class IndexModel : PageModel
    {
        private readonly ILogger<IndexModel> _logger;
        private readonly UserManager<User> _userManager;
        public User CurrentUser { get; set; }


        
        public IndexModel(ILogger<IndexModel> logger, UserManager<User> userManager)
        {
            _logger = logger;
            this._userManager = userManager;
        }

        //public void OnGet()
        //{
        //    var user = _userManager.GetUserAsync(User);
        //    if (user != null)
        //    {
        //        //User saved during the log in operation
        //        ViewData["UserID"] = _userManager.GetUserId(this.User);
        //        // Perform actions with the user object
        //        // For example, fetch claims, roles, etc.
        //    }
        //}

        public async Task OnGetAsync()
        {
          CurrentUser = await _userManager.GetUserAsync(User);
           if (CurrentUser != null)
           {
               //User saved during the log in operation
              ViewData["UserID"]=_userManager.GetUserId(this.User);
               // Perform actions with the user object
                // For example, fetch claims, roles, etc.
            }
        }
    }
}
