using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Pages.Employee
{
    [Authorize(Roles = "Admin,Employee")]
    public class DeleteModel : PageModel
    {
        public RatingSystemDbContext db { get; set; }
        [BindProperty(SupportsGet = true)]
        public ApplicationData appdata { get; set; }

        private readonly UserManager<UserData> userManager;

        public DeleteModel(UserManager<UserData> usr, RatingSystemDbContext dbb)
        {
            db = dbb;
            userManager = usr;
        }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            var app = await db.Applications.FirstOrDefaultAsync(m => m.ApplicationID == id);
            await userManager.GetUserAsync(User);
            appdata.FolderPath = app.FolderPath;
            if (app == null)
            {
                return NotFound();
            }
            else
            {
                appdata = app;
            }
            return Page();
        }

        public async Task<IActionResult> OnPost(int id)
        {
            await userManager.GetUserAsync(User);


            appdata = await db.Applications.FindAsync(id);
            var folderpath = appdata.FolderPath;

            if (appdata == null)
            {
                return Page();
            }
            if (Directory.Exists(folderpath))
            {
                Directory.Delete(folderpath, true);
            }

            db.Applications.Remove(appdata);
            await db.SaveChangesAsync();


            return RedirectToPage("/Index");

        }
    }
}