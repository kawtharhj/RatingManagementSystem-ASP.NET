using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RatingManagementSystem.Data.Models;
using RatingManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;

namespace RatingManagementSystem.Pages.Committee.Apps
{
    [Authorize(Policy = "AccessApplication")]
    public class MathAppsModel : PageModel
    {
        private readonly UserManager<UserData> userManager;
        private readonly RatingSystemDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty(SupportsGet = true)]
        public List<ApplicationData> ApplicationData { get; set; }

        public MathAppsModel(UserManager<UserData> usr, RoleManager<IdentityRole> identrole, RatingSystemDbContext db)
        {
            userManager = usr;
            roleManager = identrole;
            dbContext = db;
        }
        public async Task<IActionResult> OnGet()
        {
            var usr = await userManager.GetUserAsync(User);
            var member = await dbContext.CommitteeMembers.FirstOrDefaultAsync(c => c.UserID == usr.Id);
            DateTime date = member.Joined;

            if (usr == null || member == null || date == null) return Page();

            ApplicationData = await dbContext.Applications.Where(a => a.DrMajor == "Math" && a.CreatedAt > date && a.ApplicationStatus == "Verified").ToListAsync();

            return Page();
        }
    }
}
