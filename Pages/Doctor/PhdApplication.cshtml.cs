using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.BindingModels;
using RatingManagementSystem.Data.Models;
using RatingManagementSystem.Services;
using RatingManagementSystem.Data;

namespace RatingManagementSystem.Pages.Doctor
{
    [Authorize(Roles = "Doctor ,Admin")]
    public class PhdApplicationModel : PageModel
    {
        private readonly UserManager<UserData> userManager;
        private readonly AppRequestService appService;
        private readonly RatingSystemDbContext db; 

        public UserData userd { get; set; }

        [BindProperty(SupportsGet = true)]
        public AppBindingModel appl { get; set; }

        public IEnumerable<SelectListItem> Majors { get; set; }
        = new List<SelectListItem>
{
            new SelectListItem{Value="Chemistry" ,Text="Chemistry"},
            new SelectListItem{Value="BioChemistry" , Text="BioChemistry"},
            new SelectListItem { Value = "Biology", Text = "Biology" },
            new SelectListItem { Value = "Statistics", Text = "Statistics" },
            new SelectListItem { Value = "Informatics", Text = "Informatics" },
            new SelectListItem { Value = "Physics", Text = "Physics" },
            new SelectListItem { Value = "Math", Text = "Math" },
            new SelectListItem { Value = "Electronic", Text = "Electronic" },
            new SelectListItem { Value = "Other", Text = "Other" },

};
        public List<string> Decide { get; set; } = new List<string>()
{
                "InsideThesis",
                "OutsideThesis",
                "One of The Topics of The Thesis",
                "Participation In Conference",
                "Patented",
};


        public PhdApplicationModel(UserManager<UserData> user, AppRequestService aps,RatingSystemDbContext dbcon)
        {
            userManager = user;
            appService = aps;
            db = dbcon;
        }


        public async Task<IActionResult> OnGet()
        {
            var userd = await userManager.GetUserAsync(User);

            if (userd == null)
            {
                return RedirectToPage("/Account/Login");
            }

            return Page();
        }





        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) { return Page(); }

            var userpp = await userManager.GetUserAsync(User);

            string uID = userpp.Id;
            if (uID == null) return Page();

            var res = await appService.AddNewApplication(uID, appl);

            var apps = db.Applications.Where(a => a.Id == uID)
                .OrderByDescending(b=> b.CreatedAt)
                .FirstOrDefault();
            int AppID = apps.ApplicationID;

           
            if (res == false) return Page();

            return RedirectToPage("ThesisJournal", new { AppID = AppID });

        }





    }
}
