using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data.Models;
using static RatingManagementSystem.Data.BindingModels.RolesData;

namespace RatingManagementSystem.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class ManageUsersRolesModel : PageModel
    {

        private readonly UserManager<UserData> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty(SupportsGet = true)]
        public UserRolesBindingModel userData { get; set; }

        [BindProperty(SupportsGet = true)]
        public RolesDataBindingModel rolesData { get; set; }

        public ManageUsersRolesModel(UserManager<UserData> usr, RoleManager<IdentityRole> identrole)
        {
            userManager = usr;
            roleManager = identrole;
        }
        public async Task<IActionResult> OnGet(string id)
        {
            var user = await userManager.FindByIdAsync(id);
            if (user == null) return NotFound();

            var roles = await roleManager.Roles.ToListAsync();

            userData = new UserRolesBindingModel
            {
                Id = user.Id,
                Email = user.Email,
                Roles = roles.Select(role => new RolesDataBindingModel
                {
                    RoleID = role.Id,
                    RoleName = role.Name,
                    isSelected = userManager.IsInRoleAsync(user, role.Name).Result

                }).ToList()
            };

            return Page();
        }

        public async Task<IActionResult> OnPost()
        {
            var user = await userManager.FindByIdAsync(userData.Id);
            if (user == null) return NotFound();

            var useroles = await userManager.GetRolesAsync(user);

            foreach (var role in userData.Roles)
            {
                if (useroles.Any(r => r == role.RoleName) && !role.isSelected)
                    await userManager.RemoveFromRoleAsync(user, role.RoleName);

                if (!useroles.Any(r => r == role.RoleName) && role.isSelected)
                    await userManager.AddToRoleAsync(user, role.RoleName);
            }

            return RedirectToPage("ManageUsers");
        }
    }
}
