using System.ComponentModel.DataAnnotations;

namespace CampusGear.models
{
    public class Equipment
    {
        public int EquipmentId { get; set; }

        [Required]
        [StringLength (100)]
        [Display(Name ="Equipment Name")]
        public string EquipmentName { get; set; }

        
      public int CategoryId {  get; set; }
        public Category Category{ get; set; }
        
        [StringLength(200)]
        public string Description { get; set; }

        [Required]
        public string Location {  get; set; }
        [Required]
        public string Condition { get; set; }

        public Boolean IsAvailable { get; set; }


        public ICollection<Booking> Bookings { get; set; }
    }
}
