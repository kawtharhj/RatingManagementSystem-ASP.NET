using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using NuGet.Versioning;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.Models;
using System.Drawing;
using System.Security.Claims;

namespace RatingManagementSystem.Pages.Committee
{
    [Authorize(Roles ="Admin")]
    public class InfoModel : PageModel
    {
        private readonly UserManager<UserData> userManager;
        private readonly RatingSystemDbContext dbContext;
        private readonly RoleManager<IdentityRole> roleManager;

        [BindProperty(SupportsGet =true)]
        public List<UserData> UserInfoComit { get; set; }
        [BindProperty(SupportsGet =true)]
        public List<CommitteeMember> Members {  get; set; }


        public InfoModel(UserManager<UserData> usr, RoleManager<IdentityRole> identrole, RatingSystemDbContext db)
        {
            userManager = usr;
            roleManager = identrole;
            dbContext = db;
        }

        public async Task<IActionResult> OnGet()
        {
            UserInfoComit = await dbContext.Users.Where(a=>a.DoctorMajor=="Informatics").ToListAsync();
            Members = await dbContext.CommitteeMembers.ToListAsync();
            return Page();
        }
          public async Task<IActionResult> OnPost(List<UserData> UserInfoComit)
        {
            if (!ModelState.IsValid) return Page();

            foreach (var user in UserInfoComit)
            {
                var userID = await userManager.FindByIdAsync(user.Id);
                if (user.isInCommittee == "true")
                {
                    var userclaim = await userManager.GetClaimsAsync(userID);
                    var isInComt = await dbContext.CommitteeMembers.FirstOrDefaultAsync(u => u.UserID == user.Id);
                    var existuser = await userManager.FindByIdAsync(user.Id);


                    if (!userclaim.Any(c => c.Type == "Major" && c.Value == "Informatics"))
                    {
                        string name = user.FirstName + " " + user.LastName;
                        
                        var cmtMember = new CommitteeMember
                        {
                            Name = name,
                            UserID = user.Id
                        };

                        dbContext.CommitteeMembers.Add(cmtMember);
                        await dbContext.SaveChangesAsync();
                      await userManager.AddClaimAsync(userID, new Claim("Major", "Informatics"));
                    }
                    else if (userclaim.Any(c => c.Type == "Major" && c.Value == "Informatics") && isInComt == null)
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
                    if(existingMember != null)
                    {
                      dbContext.CommitteeMembers.Remove(existingMember);
                      await dbContext.SaveChangesAsync();
                        var claimRemove = new Claim("Major", "Informatics");
                        await userManager.RemoveClaimAsync(userID, claimRemove);
                    }
                   
                }
            }
            await dbContext.SaveChangesAsync();
            return RedirectToPage("/Admin/ChooseCommittees");
        }
    }
}




