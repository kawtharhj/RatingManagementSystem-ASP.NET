using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RatingManagementSystem.Data.Models
{
    public class ApplicationData
    {
        [Key]
        public int ApplicationID { get; set; }
        public string? ThesisRateType { get; set; } = string.Empty;

        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? DrMajor { get; set; }
        public string? DrInFds { get; set; }
        public string? YourFaculty { get; set; }  //optional

        public string? ThesisTitle { get; set; }
        public DateTime? DefenseDate { get; set; }
        public string? DefenseLocation { get; set; }
        public string? DefenseUniversity { get; set; }

        [Display(Name = "Decide if the Thesis is: ")]

        public DateTime CreatedAt { get; set; } = DateTime.Now; // for sorting - by time - later

        public string ApplicationStatus { get; set; } = "Successfully Sent";
        public string? Accepted { get; set; }
        public string? FinalResultNotes { get; set; }
        public string? DoctorsDecision { get; set; }
        public string? FolderPath { get; set; }
        public string? PhDThesisFilePath { get; set; }
        public string? MasterThesisFilePath { get; set; }
        public string? CVFilePath { get; set; }
        public List<string>? PublicationsFilePath { get; set; } = new List<string>();

        public string? ConferenceParticipationFilePath { get; set; }

        public string? PhDEquivalencyFilePath { get; set; }
        public string? RapportdeSoutenanceFilePath { get; set; }
        public string? BacCertificateFilePath { get; set; }
        public string? MasterCertificateFilePath { get; set; }
        public string? PhDCertificateFilePath { get; set; }
        public List<string>? OtherFilePath { get; set; } = new List<string>();

        [ForeignKey(nameof(UserData))]
        public string Id { get; set; }
        public UserData? UserData { get; set; }


        public ICollection<Thesis>? ThesisDetails { get; set; }

    }
}
