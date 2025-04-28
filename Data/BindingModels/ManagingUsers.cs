namespace RatingManagementSystem.Data.BindingModels
{
    public class ManagingUsers
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public IEnumerable<string>? Roles { get; set; }

    }
}
