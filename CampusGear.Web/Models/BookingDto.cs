namespace CampusGear.Web.Models
{
    public class BookingDto
    {
        public int BookingId { get; set; }

        public int EquipmentId { get; set; }

        public string EquipmentName { get; set; } = string.Empty;

        public int Duration { get; set; }

        public string Status { get; set; } = string.Empty;
    }
}