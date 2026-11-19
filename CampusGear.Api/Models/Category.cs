namespace CampusGear.Api.Models
{
    //placeholder for category model, once merged, my model folder can deleted.
    public class Category
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; } = string.Empty;

        public ICollection<Equipment> Equipment { get; set; } = new List<Equipment>();
    }
}
