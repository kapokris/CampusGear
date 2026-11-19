namespace CampusGear.Web.Models
{
    public class StaffBookingDto
    {
        public int BookingId { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; } = "";
        public int UserId { get; set; }
        public int EquipmentId { get; set; }
    }
}
