using System.ComponentModel.DataAnnotations;

namespace CampusGear.models
{
    public class User
    {
        public int UserId { get; set; }

        [Required]
        [StringLength(80)]
        [Display(Name ="First Name")]
        public string FirstName { get; set; }

        [Required]
        [StringLength(80)]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [StringLength(20)]
        [Display(Name = "Student Number")]
        public string StudentNumber { get; set; }



        public ICollection<Booking> Bookings { get; set; }
    }
}
