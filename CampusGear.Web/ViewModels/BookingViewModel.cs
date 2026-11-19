using System.ComponentModel.DataAnnotations;
namespace CampusGear.Web.ViewModels
{
    public class BookingViewModel
    {
        [Required]
        public int EquipmentId { get; set; }

        public string EquipmentName { get; set; } = string.Empty;

        [Required(ErrorMessage = "Please select a booking duration.")]
        [Range(1,4, ErrorMessage = "Duration must be between 1 and 4 weeks.")]
        [Display(Name = "Booking Duration (Weeks)")]
        public int? Duration { get; set; }
    }
}