namespace CampusGear.Api.Dtos
{
    public class BookingDto
    {
        public int BookingId { get; set; }
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; } = string.Empty;
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int Duration { get; set; }
        public string Status { get; set; } = string.Empty;
    }
}
