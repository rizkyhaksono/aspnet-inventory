using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace InventoryItems.Models
{
    public class Item
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryID { get; set; }

        [Required]
        public string ItemName { get; set; }

        [Required]
        public string ItemDescription { get; set; }
        public int ItemQuantity { get; set; }
        public decimal ItemPrice { get; set; }

        [Required]
        public string Category { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
