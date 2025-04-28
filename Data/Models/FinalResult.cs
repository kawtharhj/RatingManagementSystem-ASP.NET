using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RatingManagementSystem.Data.Models
{
    public class FinalResult
    {
        [Key]
        public int Id { get; set; }
        public string? Desicion_ThesisOriginal { get; set; }
        public string? Desicion_ParticipationInConference { get; set; }
        public string? Desicion_Patented { get; set; }
        public string? Desicion_PublishedInInternationalMagazine { get; set; }
        public string? Desicion_Authentic { get; set; }

        public string? Desicion_PresentedInInternationalConference { get; set; }


        [ForeignKey(nameof(Thesis))]
        public int? TID { get; set; }
        public Thesis? Thesis { get; set; }

    }
}
