using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.Models;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Pages.Doctor
{
    [Authorize(Roles = "Doctor ,Admin")]

    public class EditModel : PageModel
    {
        
        public RatingSystemDbContext dbContext { get; set; }
        [BindProperty(SupportsGet = true)]
        public ApplicationData appl { get; set; }

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

        public List<string> Decide { get; set; } = new List<string>()
{
                "InsideThesis",
                "OutsideThesis",
                "One of The Topics of The Thesis",
                "Participation In Conference",
                "Patented",
};


        public EditModel(RatingSystemDbContext dbcon)
        {
            dbContext = dbcon;
        }

        public async Task<IActionResult> OnGet(int id)
        {
            appl = dbContext.Applications.Find(id);

            return Page();

        }

        public async Task<IActionResult> OnPost()
        {
            if (!ModelState.IsValid) return Page();

            dbContext.Applications.Update(appl);
            await dbContext.SaveChangesAsync();
            return RedirectToPage("MyApplications");

        }
    }
}
