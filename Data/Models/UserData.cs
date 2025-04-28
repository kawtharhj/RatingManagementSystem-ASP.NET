using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace RatingManagementSystem.Data.Models
{
    public class UserData :IdentityUser
    {
        [Display(Name = "First Name")]
        public string? FirstName { get; set; }

        [Display(Name = "Last Name")]
        public string? LastName { get; set; }

        public string? Address { get; set; }
        public string? Phone { get; set; }
        public string? DoctorMajor { get; set; }
        public byte[]? ProfilePicture { get; set; }
        public string? isInCommittee { get; set; }
        public ICollection<ApplicationData>? Application { get; set; }

        public int? CommitteeID { get; set; }

        [ForeignKey("CommitteeID")]
        public CommitteeMember? CommitteeMember { get; set; }

       

    }
    


}
