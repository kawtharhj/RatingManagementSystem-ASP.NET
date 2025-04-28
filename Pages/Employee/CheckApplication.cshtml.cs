using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Pages.Employee
{
    [Authorize(Roles = "Admin,Employee")]
    public class CheckApplicationModel : PageModel
    {
        private readonly RatingSystemDbContext db;
        public UserManager<UserData> userManager;

        [BindProperty(SupportsGet = true)]
        public IEnumerable<ApplicationData> AllApps { get; set; }
        public CheckApplicationModel(RatingSystemDbContext dbc, UserManager<UserData> usr)
        {
            db = dbc;
            userManager = usr;
        }

        public void OnGet(string filterValue, string SortOrder)
        {

            if (!string.IsNullOrEmpty(filterValue))
            {
                if (SortOrder == "Newest")
                {
                    AllApps = db.Applications.Where(a => a.DrMajor == filterValue).OrderByDescending(a => a.CreatedAt).ToList();
                }
                else
                {
                    AllApps = db.Applications.Where(a => a.DrMajor == filterValue).OrderBy(a => a.CreatedAt).ToList();
                }
            }
            else
            {
                if (SortOrder == "Newest")
                {
                    AllApps = db.Applications.OrderByDescending(a => a.CreatedAt).ToList();
                }

                else AllApps = db.Applications.OrderBy(a => a.CreatedAt).ToList();
            }
        }
    }
}
