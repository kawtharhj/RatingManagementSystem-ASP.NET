using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.BindingModels;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class ManageUsersModel : PageModel
    {

        private readonly RatingSystemDbContext dbcontext;
        private readonly UserManager<UserData> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty(SupportsGet = true)]
        public List<ManagingUsers> usersManage { get; set; }

        public ManageUsersModel(RatingSystemDbContext db, UserManager<UserData> usr, RoleManager<IdentityRole> identrole)
        {
            dbcontext = db;
            userManager = usr;
            roleManager = identrole;
        }

        public async Task<IActionResult> OnGet()
        {
            usersManage = await userManager.Users.Select(user => new ManagingUsers
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Roles = userManager.GetRolesAsync(user).Result


            }).ToListAsync();

            return Page();
        }
    }
}
