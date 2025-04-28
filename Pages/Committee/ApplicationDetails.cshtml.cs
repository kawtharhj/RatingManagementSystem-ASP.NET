using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using RatingManagementSystem.Data.Models;
using RatingManagementSystem.Data;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.IO.Compression;
using NuGet.Packaging;

namespace RatingManagementSystem.Pages.Committee
{
    [Authorize(Policy ="AccessApplication")]
    public class ApplicationDetailsModel : PageModel
    {
		private readonly IWebHostEnvironment _environment;

		public RatingSystemDbContext Context { get; set; }
		public UserManager<UserData> UserManager { get; set; }

		[BindProperty(SupportsGet = true)]
		public ApplicationData appdata { get; set; }

		[BindProperty(SupportsGet = true)]
		public IEnumerable<Thesis> jornal { get; set; }

		public List<(string propertyName, string filepath)> pdfFiles { get; set; } = new List<(string propertyName, string filepath)>();
		public ApplicationDetailsModel(RatingSystemDbContext db, UserManager<UserData> userManager, IWebHostEnvironment environment)
		{
			Context = db;
			UserManager = userManager;
			_environment = environment;
		}

		public async Task<IActionResult> OnGet(int id)
		{
			appdata = await Context.Applications.FirstOrDefaultAsync(a => a.ApplicationID == id);
			jornal = await Context.AppThesis.Where(a => a.ApplicationID == id).ToListAsync();
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
				var originalJor = await Context.AppThesis
			   .Include(a => a.Results) 
			   .FirstOrDefaultAsync(a => a.Id == jor.Id);

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
			if (app == null) return Page();

			app.ApplicationStatus = "Results Are Available";
			Context.Applications.Update(app);

			await Context.SaveChangesAsync();
			return RedirectToPage("/Admin/Decision", new { Id = app.ApplicationID });
		}


		public async Task<IActionResult> OnPostDownload(int ApplicationID)
		{
			var uploadsPath = Path.Combine(_environment.WebRootPath, "Uploads");

			var appdataD = await Context.Applications.FindAsync(ApplicationID);
			string folderPath = appdataD.FolderPath;

			if (!string.IsNullOrEmpty(folderPath) && Directory.Exists(folderPath))
			{
				string folderName = Path.GetFileName(folderPath);
				string zipFileName = $"{folderName}_Converted.zip";
				string tempZipFilePath = Path.Combine(Path.GetTempPath(), zipFileName);

				try
				{
					ZipFile.CreateFromDirectory(folderPath, tempZipFilePath);

					var zipFileBytes = System.IO.File.ReadAllBytes(tempZipFilePath);

					return File(zipFileBytes, "application/zip", zipFileName);
				}
				finally
				{
					if (System.IO.File.Exists(tempZipFilePath))
					{
						System.IO.File.Delete(tempZipFilePath);
					}
				}
			}
			ModelState.AddModelError(string.Empty, "Invalid folder path.");
			return Page();

		}
	}
}




