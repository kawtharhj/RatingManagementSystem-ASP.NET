namespace RatingManagementSystem.Data.BindingModels
{
    public class ResultsBindingModel
    {
        public int Id { get; set; }
        public string? Desicion_ThesisOriginal { get; set; }
        public string? Desicion_ParticipationInConference { get; set; }
        public string? Desicion_Patented { get; set; }
        public string? Desicion_PublishedInInternationalMagazine { get; set; }
        public string? Desicion_Authentic { get; set; }

        public string? Desicion_PresentedInInternationalConference { get; set; }

    }
}
