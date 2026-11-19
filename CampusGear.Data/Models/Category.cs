using System.ComponentModel.DataAnnotations;

namespace CampusGear.models
{
    public class Category
    {
        public  int  CategoryId { get; set; }

        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }

        public ICollection<Equipment> Equipment { get; set; }

    }
}
