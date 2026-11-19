using System.ComponentModel.DataAnnotations;

namespace CampusGear.Api.Dtos
{
    public class CreateBookingRequest
    {
        public int EquipmentId { get; set; }
        public int UserId { get; set; }
        public DateTime StartDate { get; set; }

        [Range(1, 4, ErrorMessage = "Duration must be between 1 to 4 weeks")]
        public int Duration { get; set; }
    }
}
