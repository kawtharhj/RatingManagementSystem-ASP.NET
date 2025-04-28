using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Pages.Doctor
{
    [Authorize(Roles = "Doctor ,Admin")]
    public class MyApplicationsModel : PageModel
    {
        private readonly UserManager<UserData> userManager;
        private readonly RatingSystemDbContext db;



        [BindProperty(SupportsGet = true)]
        public List<ApplicationData> appdata { get; set; }

        [BindProperty(SupportsGet =true)]
        public List<FinalResult> finalresult { get; set; }

          [BindProperty(SupportsGet =true)]
        public List<Thesis> thesis { get; set; }


        public MyApplicationsModel(UserManager<UserData> us, RatingSystemDbContext con)
        {
            userManager = us;
            db = con;
        }


        public async Task<IActionResult> OnGet()
        {
            var input = await userManager.GetUserAsync(User);
            string uId = input.Id;

            if (input == null)
            {
                return RedirectToPage("PhdApplication");
            }
            appdata = await db.Applications.Where(a => a.Id == uId).ToListAsync();
           
           
            await db.SaveChangesAsync();

            return Page();
        }

    }
}
