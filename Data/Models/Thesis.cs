using GrapeCity.Documents.Pdf.ViewerSupportApi.Localization;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RatingManagementSystem.Data.Models
{
    public class Thesis
    {
        public int Id { get; set; }
        public string DecideIf { get; set; }
        public string ResearchTitle { get; set; }
        [Display(Name = "Authors (Write Authors Name Separated by , )")]
        public string Authors { get; set; }

        public string Journal { get; set; }

        [Range(1, long.MaxValue, ErrorMessage = "The value must be greater than 0.")]
        public int JournalNumber { get; set; }
        public DateOnly JournalYear { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0.")]
        public int FromPage { get; set; }


        [Range(1, int.MaxValue, ErrorMessage = "The value must be greater than 0.")]
        public int ToPage { get; set; }
        public string MembersContributionTResearch { get; set; }


        [ForeignKey(nameof(ApplicationData))]
        public int ApplicationID { get; set; }
        public ApplicationData ApplicationData { get; set; }


        [ForeignKey(nameof(Results))]
        public int? RID { get; set; }
        public FinalResult? Results { get; set; } = new FinalResult();



    }
}
