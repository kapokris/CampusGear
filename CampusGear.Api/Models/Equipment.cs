namespace CampusGear.Api.Models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }
        public string EquipmentName { get; set; } = string.Empty;

        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        public string Description { get; set; } = string.Empty;
        public string Location { get; set; } = string.Empty;
        public string Condition { get; set; } = string.Empty;

        // False = out of circulation (e.g. under maintenance), so it can't be booked.
        public bool IsAvailable { get; set; }

        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
    }
}
