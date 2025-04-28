namespace RatingManagementSystem.Data.Models
{
    public class EmailSetting
    {
        public string Host { get; set; } = "";
        public int Port { get; set; }
        public bool UseSSL { get; set; }
        public string UserName { get; set; } = "";
        public string Password { get; set; } = "";
        public string SenderName { get; set; } = "";
        public string SenderEmail { get; set; } = "";
    }
}
