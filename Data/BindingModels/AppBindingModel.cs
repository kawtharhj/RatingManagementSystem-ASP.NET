namespace RatingManagementSystem.Data.BindingModels
{
    public class AppBindingModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string? ThesisRateType { get; set; } = string.Empty;

        public string DrMajor { get; set; }
        public string DrInFds { get; set; }
        public string? YourFaculty { get; set; }  //optional

        public string ThesisTitle { get; set; }
        public DateTime DefenseDate { get; set; }
        public string DefenseLocation { get; set; }
        public string DefenseUniversity { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public IFormFile PhDThesis { get; set; }
        public IFormFile MasterThesis { get; set; }
        public IFormFile CV { get; set; }
        public List<IFormFile> Publications { get; set; } = new List<IFormFile>();
        public IFormFile ConferenceParticipation { get; set; }
        public IFormFile PhDEquivalency { get; set; }
        public IFormFile RapportdeSoutenance { get; set; }
        public IFormFile BacCertificate { get; set; }
        public IFormFile MasterCertificate { get; set; }
        public IFormFile PhDCertificate { get; set; }
        public List<IFormFile>? Other { get; set; } = new List<IFormFile>();

    }
}
