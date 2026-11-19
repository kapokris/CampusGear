namespace CampusGear.Api.Models
{
    public class Booking
    {
        public int BookingId { get; set; }

        // Length of the booking in weeks (Ravneet's rule: 1 to 4).
        public int Duration { get; set; }

        // NOT in Ravneet's model yet. The double-booking rule needs to know WHEN a booking is.
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // "Pending", "Approved", "Cancelled" or "Returned"
        public string Status { get; set; } = "Pending";

        public int UserId { get; set; }
        public User? User { get; set; }

        public int EquipmentId { get; set; }
        public Equipment? Equipment { get; set; }
    }
}
