using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.BindingModels;
using RatingManagementSystem.Data.Models;

namespace RatingManagementSystem.Pages.Employee
{
    [Authorize(Roles ="Admin,Employee")]
    public class DetailsViewModel : PageModel
    {

        public RatingSystemDbContext Context { get; set; }
        public UserManager<UserData> UserManager { get; set; }

        [BindProperty(SupportsGet = true)]
        public ApplicationData appdata { get; set; }

        [BindProperty(SupportsGet = true)]
        public IEnumerable<Thesis> jornal {  get; set; }
        public List<(string propertyName, string filepath)> pdfFiles { get; set; } = new List<(string propertyName, string filepath)>();
        public DetailsViewModel(RatingSystemDbContext db, UserManager<UserData> userManager)
        {
            Context = db;
            UserManager = userManager;

        }

        public async Task<IActionResult> OnGet(int id)
        {
            appdata = await Context.Applications.FirstOrDefaultAsync(a => a.ApplicationID == id);
            jornal = await Context.AppThesis.Where(a=>a.ApplicationID == id).ToListAsync();
            var usr = await Context.Users.FirstOrDefaultAsync(b => b.Id == appdata.Id);


            var input = await UserManager.GetUserAsync(User);
            if (appdata == null) return Page();
            pdfFiles.Add(("PhDThesis", appdata.PhDThesisFilePath));
            pdfFiles.Add(("MasterThesis", appdata.MasterThesisFilePath));
            pdfFiles.Add(("CV", appdata.CVFilePath));
            //pdfFiles.Add(("Publications", appdata.PublicationsFilePath));
            pdfFiles.Add(("ConferenceParticipation", appdata.ConferenceParticipationFilePath));
            pdfFiles.Add(("PhDEquivalency", appdata.PhDEquivalencyFilePath));
            pdfFiles.Add(("RapportdeSoutenance", appdata.RapportdeSoutenanceFilePath));
            pdfFiles.Add(("BacCertificate", appdata.BacCertificateFilePath));
            pdfFiles.Add(("MasterCertificate", appdata.MasterCertificateFilePath));
            pdfFiles.Add(("PhDCertificate", appdata.PhDCertificateFilePath));
            //     pdfFiles.Add(("Other", appdata.OtherFilePath));

            int counter = 0 ,counter2=0;
            foreach (var file in appdata.PublicationsFilePath)
            {
                counter++;
                pdfFiles.Add(($"Publications-{counter}", file));
            }

            foreach (var file2 in appdata.OtherFilePath)
            {
                counter2++;
                pdfFiles.Add(($"Other-{counter2}", file2));
            }

            ViewData["Email"] = usr?.Email;
            ViewData["PhoneNumber"] = usr?.Phone;
            return Page();
        }


        public async Task<IActionResult> OnPost(int id)
        {
            var app = await Context.Applications.FindAsync(id);
            app.ApplicationStatus = "In-Progress";

            Context.Update(app);
            await Context.SaveChangesAsync();

            return RedirectToPage("CheckApplication");
        }


    }
}
