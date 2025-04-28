using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Pages.Admin
{
    //[Authorize(Roles = "Admin")]
	[Authorize(Policy = "AccessApplication")]
	public class DecisionModel : PageModel
    {

        private readonly UserManager<UserData> userManager;
        public RatingSystemDbContext dbcontext { get; set; }

        [BindProperty(SupportsGet = true)]
        public ApplicationData appdata { get; set; }

        public DecisionModel(UserManager<UserData> user, RatingSystemDbContext db)
        {
            userManager = user;
            dbcontext = db;
        }

        public async Task<IActionResult> OnGet(int Id)
        {
            //var user = await userManager.GetUserAsync(User);
            //var isInComt = await dbcontext.CommitteeMembers.FirstOrDefaultAsync(u => u.UserID == user.Id);

            ////if (isInComt != null)
            ////{
            ////    return RedirectToPage("/Index");
            ////}
            appdata = await dbcontext.Applications.FirstOrDefaultAsync(a => a.ApplicationID == Id);


            return Page();
        }


        public async Task<IActionResult> OnPost(int Id)
        {
           
            var app = await dbcontext.Applications.FirstOrDefaultAsync(a => a.ApplicationID == Id);
            app.Accepted = appdata.Accepted;
            app.FinalResultNotes = appdata.FinalResultNotes;
            app.ApplicationStatus = "Results Are Available";
            app.DoctorsDecision = appdata.DoctorsDecision;
            dbcontext.Update(app);
            await dbcontext.SaveChangesAsync();

            return RedirectToPage("/Index");

        }
    }
}
