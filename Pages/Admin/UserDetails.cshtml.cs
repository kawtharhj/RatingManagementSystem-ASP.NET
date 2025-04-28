using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Pages.Admin
{
    public class UserDetailsModel : PageModel
    {
        private readonly RatingSystemDbContext dbcontext;
        private readonly UserManager<UserData> userManager;

        public UserDetailsModel(RatingSystemDbContext db, UserManager<UserData> user)
        {
            userManager = user;
            dbcontext = db;
        }

        public async Task<IActionResult> OnGet(string id)
        {
            if (id == null) return NotFound();

            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            ViewData["FirstName"] = user?.FirstName;
            ViewData["LastName"] = user?.LastName;
            ViewData["Address"] = user?.Address;
            ViewData["Email"] = user?.Email;
            ViewData["PhoneNumber"] = user?.Phone;

            return Page();

        }
    }
}
