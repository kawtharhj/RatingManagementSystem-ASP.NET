using Microsoft.AspNetCore.Identity;
using RatingManagementSystem.Data;
using RatingManagementSystem.Data.BindingModels;
using RatingManagementSystem.Data.Models;
using RatingManagementSystem.Pages.Doctor;
using System.Drawing;
using System.IO.Compression;

namespace RatingManagementSystem.Services
{
    public class AppRequestService
    {
        public RatingSystemDbContext _dbContext { get; set; }
        public AppRequestService(RatingSystemDbContext dbContext)
        {

            _dbContext = dbContext;

        }
        public async Task<bool> AddNewApplication(string id, AppBindingModel app)
        {
            if (app == null) return false;

            var newApp = new ApplicationData()
            {
                FirstName = app.FirstName,
                LastName = app.LastName,
                ThesisRateType = app.ThesisRateType,
                DrMajor = app.DrMajor,
                DrInFds = app.DrInFds,
                YourFaculty = app.YourFaculty,
                ThesisTitle = app.ThesisTitle,
                DefenseDate = app.DefenseDate,
                DefenseLocation = app.DefenseLocation,
                DefenseUniversity = app.DefenseUniversity,
                CreatedAt = app.CreatedAt,
                Id = id,
                PublicationsFilePath= new List<string>(),
                OtherFilePath= new List<string>(),

            };

            var foldername = $"Uploads/{app.FirstName}_{app.LastName}_{app.DrMajor}_{DateTime.Now:yyyyMMddHHmmss}";
            var uploadpath = Path.Combine(Directory.GetCurrentDirectory(), foldername);
            newApp.FolderPath = foldername; //replace =uploadpath by = foldername

            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }

            var properties = typeof(AppBindingModel).GetProperties();
            foreach (var property in properties)
            {
                if (property.PropertyType == typeof(IFormFile))
                {
                    var file = property.GetValue(app) as IFormFile;
                    if (file != null && file.FileName.EndsWith(".pdf"))
                    {
                        var filename = file.FileName;
                        var filepath = Path.Combine(uploadpath, filename);
                        await using (var stream = new FileStream(filepath, FileMode.Create))
                        {
                            await file.CopyToAsync(stream);
                        }
                        var propertyname = $"{property.Name}FilePath";
                        var propertyInfo = typeof(ApplicationData).GetProperty(propertyname);
                        propertyInfo?.SetValue(newApp, filename); //replace filepath with filename
                        //////
                        ///

                 
                    }
                }
            }

            if (app.Publications != null && app.Publications.Count > 0)
            {
                foreach (var publicationFile in app.Publications)
                {
                    if (publicationFile != null && publicationFile.FileName.EndsWith(".pdf"))
                    {
                        var publicationFilename = publicationFile.FileName;
                        var publicationFilepath = Path.Combine(uploadpath, publicationFilename);

                        using (var publicationStream = new FileStream(publicationFilepath, FileMode.Create))
                        {
                            await publicationFile.CopyToAsync(publicationStream);
                        }

                        newApp.PublicationsFilePath.Add(publicationFilename); // Store the file path in the list
                    }
                }
            }

            if (app.Other != null && app.Other.Count > 0)
            {
                foreach (var OtherFile in app.Other)
                {
                    if (OtherFile != null)
                    {
                        var OtherFilename = OtherFile.FileName;
                        var OtherFilepath = Path.Combine(uploadpath, OtherFilename);

                        using (var OtherStream = new FileStream(OtherFilepath, FileMode.Create))
                        {
                            await OtherFile.CopyToAsync(OtherStream);
                        }

                        newApp.OtherFilePath.Add(OtherFilename); // Store the file path in the list
                    }
                }
            }

            _dbContext.Applications.Add(newApp);
            await _dbContext.SaveChangesAsync();
            return true;
        }
        public string JoinNames(List<string> dec)
        {

            var str = string.Join(",", dec);
            return str;
        }
        public async Task<bool> AddJournalDetails(int appId, ThesisBindingModel jor)
        {

            if (appId == null) return false;
            if (jor == null) return false;

            var newThesis = new Thesis()
            {
                DecideIf = JoinNames(jor.DecideIf),
                ResearchTitle = jor.ResearchTitle,
                Authors = jor.Authors,
                Journal = jor.Journal,
                JournalNumber = jor.JournalNumber,
                JournalYear = jor.JournalYear,
                FromPage = jor.FromPage,
                ToPage = jor.ToPage,
                MembersContributionTResearch=jor.MembersContributionTResearch,
                ApplicationID = appId
            };

            try
            {
                _dbContext.AppThesis.Add(newThesis);
                Console.WriteLine("Successfully saved");
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                // Log the exception details for further investigation
                Console.WriteLine(ex.Message);
                return false;
            }
        }



        public void CompressFolder(string sourceFolder)
        {
            string folderName = Path.GetFileName(sourceFolder);
            string uniqueFileName = $"{folderName}_Converted.zip";
            string destinationZipFilePath = Path.Combine(folderName, uniqueFileName);


            if (Directory.Exists(destinationZipFilePath))
            {
				Directory.Delete(destinationZipFilePath, true);
			}
            ZipFile.CreateFromDirectory(sourceFolder, destinationZipFilePath);
        }

      
    }







}
