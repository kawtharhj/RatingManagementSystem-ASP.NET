using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.BindingModels;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Pages.Doctor
{
    [Authorize(Roles = "Doctor ,Admin")]
    public class DetailsModel : PageModel
    {

        private readonly UserManager<UserData> userManager;
        public RatingSystemDbContext db { get; set; }

        [BindProperty(SupportsGet = true)]
        public ApplicationData appdata { get; set; }

        [BindProperty(SupportsGet =true)]
        public IEnumerable<Thesis> jornaldata { get; set; }




        public DetailsModel(UserManager<UserData> usr, RatingSystemDbContext dbc)
        {
            userManager = usr;
            db = dbc;
        }
        public async Task<IActionResult> OnGet(int id)
        {
            appdata = await db.Applications.FirstOrDefaultAsync(a => a.ApplicationID == id);
            jornaldata = await db.AppThesis.Where(j => j.ApplicationID == id)
				.AsNoTracking()
				.Include(j=>j.Results)
                .ToListAsync();
            
           

            var usr = await userManager.GetUserAsync(User);
            var input = await userManager.GetUserAsync(User);
            if (appdata == null) return Page();
            var jo = jornaldata;

            ViewData["Email"] = usr?.Email;
            ViewData["PhoneNumber"] = usr?.Phone;

            return Page();
        }
    }
}
