using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace RatingManagementSystem.Data.Models
{
    public class CommitteeMember
    {
        [Key]
        public int Id { get; set; }
        public string Name { get; set; }
        public string UserID { get; set; }

        public DateTime Joined { get; set; } = DateTime.Now;

        [ForeignKey("UserID")]
        public UserData Members { get; set; }

    }
}
