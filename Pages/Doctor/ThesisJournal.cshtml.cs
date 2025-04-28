using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RatingManagementSystem.Data.BindingModels;
using RatingManagementSystem.Data.Models;
using RatingManagementSystem.Data;
using RatingManagementSystem.Services;
using System;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data;

namespace RatingManagementSystem.Pages.Doctor
{
    [Authorize(Roles = "Doctor")]
    public class ThesisJournalModel : PageModel
    {
        private readonly RatingSystemDbContext dbContext;
        private readonly UserManager<UserData> userManager;
        private readonly AppRequestService appRequestService;

        [BindProperty(SupportsGet =true)]
        public int AppID { get; set; }

        [BindProperty(SupportsGet = true)]
        public ThesisBindingModel thesis { get; set; } 

        public List<string> Decide { get; set; } = new List<string>()
{
                "InsideThesis",
                "OutsideThesis",
                "One of The Topics of The Thesis",
                "Participation In Conference",
                "Patented",
};

        public ThesisJournalModel(RatingSystemDbContext db, UserManager<UserData> user, AppRequestService app)
        {
            dbContext = db;
            userManager = user;
            appRequestService = app;
        }

        public async Task<IActionResult> FromDetails(int applicationID)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return RedirectToPage("Details");
            AppID = applicationID;

            return Page();
        }
        public async Task<IActionResult> OnGet(int AppID)
        {
            var user = await userManager.GetUserAsync(User);
            if (user == null) return Page();
            var appnew = await dbContext.Applications.FindAsync(AppID);

            AppID = appnew.ApplicationID;

            if (AppID == null) return RedirectToPage("Details");
            

            return Page();
        }


        public async Task<IActionResult> OnPost()
        {
            if(!ModelState.IsValid) return Page();
            //var user = await userManager.GetUserAsync(User);
            //if (user == null) return Page();
            //var res = appRequestService.AddJournalDetails(AppID,thesis);
            //if (res == null) return Page();

            var newThesis = new Thesis()
            {
                DecideIf =appRequestService.JoinNames(thesis.DecideIf),
                ResearchTitle = thesis.ResearchTitle,
                Authors = thesis.Authors,
                Journal = thesis.Journal,
                JournalNumber = thesis.JournalNumber,
                JournalYear = thesis.JournalYear,
                FromPage = thesis.FromPage,
                ToPage = thesis.ToPage,
                MembersContributionTResearch = thesis.MembersContributionTResearch,
                ApplicationID = AppID
            };

                dbContext.AppThesis.Add(newThesis);
              
                await dbContext.SaveChangesAsync();

            var action = Request.Form["action"];
            if (action == "saveAndReload")
            {
                return RedirectToPage("ThesisJournal", new { AppID = AppID });
            }
            else if (action == "saveAndGoBack")
            {
                return RedirectToPage("MyApplications");
            }

            return Page();
        }
    }
}
