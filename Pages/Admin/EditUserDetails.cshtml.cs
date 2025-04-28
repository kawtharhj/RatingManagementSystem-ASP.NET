using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RatingManagementSystem.Data.Models;
using System.ComponentModel.DataAnnotations;

namespace RatingManagementSystem.Pages.Admin
{
    [Authorize(Roles ="Admin")]
    public class EditUserDetailsModel : PageModel
    {

        public UserManager<UserData> UserManager { get; set; }

        [BindProperty(SupportsGet = true)]
        public EditViewModel editUser { get; set; }


        public IEnumerable<SelectListItem> DrMajors { get; set; }
       = new List<SelectListItem>
{
            new SelectListItem{Value="Chemistry" ,Text="Chemistry"},
            new SelectListItem{Value="BioChemistry" , Text="BioChemistry"},
            new SelectListItem { Value = "Biology", Text = "Biology" },
            new SelectListItem { Value = "Statistics", Text = "Statistics" },
            new SelectListItem { Value = "Informatics", Text = "Informatics" },
            new SelectListItem { Value = "Physics", Text = "Physics" },
            new SelectListItem { Value = "Math", Text = "Math" },
            new SelectListItem { Value = "Electronic", Text = "Electronic" },
            new SelectListItem { Value = "Other", Text = "Other" },

};




        public EditUserDetailsModel(UserManager<UserData> user)
        {
            UserManager = user;

        }

        public async Task<IActionResult> OnGet(string id)
        {
            var usr = await UserManager.FindByIdAsync(id);
            if (usr == null) return NotFound();

            editUser = new EditViewModel
            {
                id = usr.Id,
                FirstName = usr.FirstName,
                LastName = usr.LastName,
                Email = usr.Email,
                PhoneNumber = usr.PhoneNumber,
                Major = usr.DoctorMajor,
                Password = usr.PasswordHash
               
            };

            return Page();
        }

        public async Task<IActionResult> OnPost(string id)
        {
            if (!ModelState.IsValid) return Page();

            var usr = await UserManager.FindByIdAsync(id);
            if (usr == null) return Page();

            var checkemail = await UserManager.FindByEmailAsync(editUser.Email);

            if (checkemail.Email != null && editUser.id != checkemail.Id)
            {
                ModelState.AddModelError("", "Email already assigned for another user");
            }

           

            usr.FirstName = editUser.FirstName;
            usr.LastName = editUser.LastName;
            usr.Phone = editUser.PhoneNumber;
            usr.DoctorMajor = editUser.Major;
            usr.Email = editUser.Email;
            usr.PasswordHash = editUser.Password;


            await UserManager.UpdateAsync(usr);
            return RedirectToPage("ManageUsers");

        }


    }








    public class EditViewModel
    {
        public string? id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }

        [EmailAddress]
        [RegularExpression(@"^[^@]+@ul\.edu\.lb$", ErrorMessage = "Only emails ending with @ul.edu.lb are allowed.")]
        [Display(Name = "Email")]
        public string? Email { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string? ConfirmPassword { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Major { get; set; }

    }
}

