using System.ComponentModel.DataAnnotations;

namespace CampusGear.models
{
    public class Booking
    {
        public int BookingId{ get; set; }
        

        [Required]
        [Range(1, 4, ErrorMessage = "Duration must be between 1 to 4 weeks")]
        public int Duration { get; set; }

        public string Status { get; set; }
        public int UserId { get; set; }
        public User User { get; set; }
        public int EquipmentId {  get; set; }
        public Equipment Equipment { get; set; }
    }
}
