using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RatingManagementSystem.Data.Models;
using RatingManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace RatingManagementSystem.Pages.Committee
{
    [Authorize(Roles = "Admin")]
    public class BiologyModel : PageModel
    {
        private readonly UserManager<UserData> userManager;
        private readonly RatingSystemDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty(SupportsGet = true)]
        public List<UserData> UserInfoComit { get; set; }
        [BindProperty(SupportsGet = true)]
        public List<CommitteeMember> Members { get; set; }

        public BiologyModel(UserManager<UserData> usr, RoleManager<IdentityRole> identrole, RatingSystemDbContext db)
        {
            userManager = usr;
            roleManager = identrole;
            dbContext = db;
        }

        public async Task<IActionResult> OnGet()
        {
            UserInfoComit = await dbContext.Users.Where(a => a.DoctorMajor == "Biology").ToListAsync();
            Members = await dbContext.CommitteeMembers.ToListAsync();
            return Page();
        }
        public async Task<IActionResult> OnPost(List<UserData> UserInfoComit)
        {
            if (!ModelState.IsValid) return Page();

            foreach (var user in UserInfoComit)
            {
                var userID = await userManager.FindByIdAsync(user.Id);
                var isInComt = await dbContext.CommitteeMembers.FirstOrDefaultAsync(u => u.UserID == user.Id);
                var existuser = await userManager.FindByIdAsync(user.Id);

                if (user.isInCommittee == "true")
                {
                    var userclaim = await userManager.GetClaimsAsync(userID);

                    if (!userclaim.Any(c => c.Type == "Major" && c.Value == "Biology"))
                    {
                        string name = user.FirstName + " " + user.LastName;

                        var cmtMember = new CommitteeMember
                        {
                            Name = name,
                            UserID = user.Id
                        };

                        dbContext.CommitteeMembers.Add(cmtMember);
                        await dbContext.SaveChangesAsync();
                        await userManager.AddClaimAsync(userID, new Claim("Major", "Biology"));
                    }
                    else if (userclaim.Any(c => c.Type == "Major" && c.Value == "Biology") && isInComt == null)
                    {
                        string name = existuser.FirstName + " " + existuser.LastName;

                        var cmtMember = new CommitteeMember
                        {
                            Name = name,
                            UserID = user.Id
                        };
                        dbContext.CommitteeMembers.Add(cmtMember);
                        await dbContext.SaveChangesAsync();
                    }


                }
                else
                {
                    var existingMember = await dbContext.CommitteeMembers.FirstOrDefaultAsync(a => a.UserID == user.Id);
                    if (existingMember != null)
                    {
                        dbContext.CommitteeMembers.Remove(existingMember);
                        await dbContext.SaveChangesAsync();
                        var claimRemove = new Claim("Major", "Biology");
                        await userManager.RemoveClaimAsync(userID, claimRemove);
                    }

                }
            }
            await dbContext.SaveChangesAsync();
			return RedirectToPage("/Admin/ChooseCommittees");
		}
    }
}





