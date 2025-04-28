using GrapeCity.Documents.Pdf.ViewerSupportApi.Localization;
using System.ComponentModel.DataAnnotations;

namespace RatingManagementSystem.Data.BindingModels
{
    public class ThesisBindingModel
    {

        [Display(Name = "Decide if the Thesis is: ")]
        public List<string> DecideIf { get; set; }
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

        /// For Thesis Part //

        public string? Desicion_ThesisOriginal { get; set; }
        public string? Desicion_ParticipationInConference { get; set; }
        public string? Desicion_Patented { get; set; }
        public string? Desicion_PublishedInInternationalMagazine { get; set; }
        public string? Desicion_Authentic { get; set; }

        public string? Desicion_PresentedInInternationalConference { get; set; }

    }
}
