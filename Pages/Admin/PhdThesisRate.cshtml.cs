using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class PhdThesisRateModel : PageModel
    {
        private readonly RatingSystemDbContext db;
        private readonly UserManager<UserData> userManager;
        [BindProperty(SupportsGet = true)]
        public List<ApplicationData> ApplicationData { get; set; }
        public PhdThesisRateModel(RatingSystemDbContext con, UserManager<UserData> user)
        {
            db = con;
            userManager = user;
        }
        public async Task<IActionResult> OnGet(string filterValue)
        {
            var user = await userManager.GetUserAsync(User);

            if (!string.IsNullOrEmpty(filterValue))
            {
                ApplicationData = db.Applications.Where(a => a.DrMajor == filterValue && a.ApplicationStatus == "In-Progress" && a.ThesisRateType == "PHDThesisRate").ToList();
            }
            else
            {

                ApplicationData = db.Applications.Where(a => a.ApplicationStatus == "In-Progress" && a.ThesisRateType == "PhdThesisRate").ToList();
            }


            return Page();
        }

        public async Task<IActionResult> OnPost(int ApplicationID)
        {
            if (!ModelState.IsValid) return Page();
            var appdetails = await db.Applications.FindAsync(ApplicationID);
            if (appdetails == null) return Page();

            appdetails.ApplicationStatus = "Verified";
            db.Update(appdetails);
            await db.SaveChangesAsync();

            return RedirectToPage("/Index");
        }
    }
}
