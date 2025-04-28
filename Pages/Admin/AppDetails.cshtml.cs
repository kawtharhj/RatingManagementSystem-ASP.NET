using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
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
    public class AppDetailsModel : PageModel
    {

        public RatingSystemDbContext Context { get; set; }
        public UserManager<UserData> UserManager { get; set; }

        [BindProperty(SupportsGet = true)]
        public ApplicationData appdata { get; set; }

        [BindProperty(SupportsGet =true)]
        public IEnumerable<Thesis> jornal {  get; set; }
        public List<(string propertyName, string filepath)> pdfFiles { get; set; } = new List<(string propertyName, string filepath)>();
        public AppDetailsModel(RatingSystemDbContext db, UserManager<UserData> userManager)
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
           // pdfFiles.Add(("Publications", appdata.PublicationsFilePath));
            pdfFiles.Add(("ConferenceParticipation", appdata.ConferenceParticipationFilePath));
            pdfFiles.Add(("PhDEquivalency", appdata.PhDEquivalencyFilePath));
            pdfFiles.Add(("RapportdeSoutenance", appdata.RapportdeSoutenanceFilePath));
            pdfFiles.Add(("BacCertificate", appdata.BacCertificateFilePath));
            pdfFiles.Add(("MasterCertificate", appdata.MasterCertificateFilePath));
            pdfFiles.Add(("PhDCertificate", appdata.PhDCertificateFilePath));
            //     pdfFiles.Add(("Other", appdata.OtherFilePath));

            int counter = 0, counter2 = 0;
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
            //
           

            return Page();
        }

		public async Task<IActionResult> OnPost(int id)
		{
			foreach (var jor in jornal)
			{
				var originalJor = await Context.AppThesis.FindAsync(jor.Id);

				if (originalJor != null)
				{
					if (originalJor.Results == null)
					{
						originalJor.Results = new FinalResult
						{
							TID = jor.Id,
							Desicion_ThesisOriginal = jor.Results.Desicion_ThesisOriginal,
							Desicion_ParticipationInConference = jor.Results.Desicion_ParticipationInConference,
							Desicion_Authentic = jor.Results.Desicion_Authentic,
							Desicion_Patented = jor.Results.Desicion_Patented,
							Desicion_PublishedInInternationalMagazine = jor.Results.Desicion_PublishedInInternationalMagazine,
							Desicion_PresentedInInternationalConference = jor.Results.Desicion_PresentedInInternationalConference
						};
						Context.Results.Add(originalJor.Results);
					}

					else
					{
						originalJor.Results.Desicion_ThesisOriginal = jor.Results.Desicion_ThesisOriginal;
						originalJor.Results.Desicion_ParticipationInConference = jor.Results.Desicion_ParticipationInConference;
						originalJor.Results.Desicion_Authentic = jor.Results.Desicion_Authentic;
						originalJor.Results.Desicion_Patented = jor.Results.Desicion_Patented;
						originalJor.Results.Desicion_PublishedInInternationalMagazine = jor.Results.Desicion_PublishedInInternationalMagazine;
						originalJor.Results.Desicion_PresentedInInternationalConference = jor.Results.Desicion_PresentedInInternationalConference;
						originalJor.Results.TID = jor.Id;

						Context.Results.Update(originalJor.Results); // Explicitly mark the Results entity as updated
					}

				}
			}
			var app = Context.Applications.Find(id);
			await Context.SaveChangesAsync();
			return RedirectToPage("Decision", new { Id = app.ApplicationID });
		}
	}


}
