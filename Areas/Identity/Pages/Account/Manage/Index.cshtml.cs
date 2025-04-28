// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Areas.Identity.Pages.Account.Manage
{
    public class IndexModel : PageModel
    {
        private readonly UserManager<UserData> _userManager;
        private readonly SignInManager<UserData> _signInManager;

        public IndexModel(
            UserManager<UserData> userManager,
            SignInManager<UserData> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IEnumerable<SelectListItem> Majors { get; set; }
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


        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [TempData]
        public string StatusMessage { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        [BindProperty]
        public InputModel Input { get; set; }

        /// <summary>
        ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public class InputModel
        {
            /// <summary>
            ///     This API supports the ASP.NET Core Identity default UI infrastructure and is not intended to be used
            ///     directly from your code. This API may change or be removed in future releases.
            /// </summary>
            [Phone]
            [Display(Name = "Phone number")]
            public string PhoneNumber { get; set; }

            [Display(Name = "First Name")]
            public string? FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string? LastName { get; set; }

            public byte[]? ProfilePicture {  get; set; }
            public string? DrMajor {  get; set; }
        }

        private async Task LoadAsync(UserData user)
        {
            var userName = await _userManager.GetUserNameAsync(user);
            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);

            Username = userName;

            Input = new InputModel
            {
                PhoneNumber = phoneNumber,
                FirstName = user.FirstName,
                LastName = user.LastName,
                ProfilePicture= user.ProfilePicture,
                DrMajor = user.DoctorMajor
                
            };
        }

        public async Task<IActionResult> OnGetAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            await LoadAsync(user);
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                return NotFound($"Unable to load user with ID '{_userManager.GetUserId(User)}'.");
            }

            if (!ModelState.IsValid)
            {
                await LoadAsync(user);
                return Page();
            }

            var existingMajor =  _userManager.GetUserAsync(User).Result.DoctorMajor;

            if (Input.DrMajor != existingMajor)
            {
                user.DoctorMajor = Input.DrMajor;
            }
            else
            {
                user.DoctorMajor = existingMajor;
            }

            var phoneNumber = await _userManager.GetPhoneNumberAsync(user);
            if (Input.PhoneNumber != phoneNumber)
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, Input.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    StatusMessage = "Unexpected error when trying to set phone number.";
                    return RedirectToPage();
                }
            }
            if (!string.IsNullOrWhiteSpace(Input.FirstName))
            {
                user.FirstName = Input.FirstName;
            }
            if (!string.IsNullOrWhiteSpace(Input.LastName))
            {
                user.LastName = Input.LastName;
            }

            var result = await _userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                StatusMessage = "Unexpected error when trying to update profile.";
                return RedirectToPage();
            }

            if (Request.Form.Files.Count > 0)
            {
                var file = Request.Form.Files.FirstOrDefault();
                using(var datastream = new MemoryStream())
                {
                  await  file.CopyToAsync(datastream);
                    user.ProfilePicture = datastream.ToArray();
                }
                await _userManager.UpdateAsync(user);

            }

            await _signInManager.RefreshSignInAsync(user);
            StatusMessage = "Your profile has been updated";
            return RedirectToPage();
        }
    }
}
