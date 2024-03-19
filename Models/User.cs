using System.ComponentModel.DataAnnotations;

namespace InventoryItems.Models
{
    public class User
    {
        public int UserID { get; set; }
        
        [Required]
        [StringLength(255)]
        public string Username { get; set; }

        [Required]
        [StringLength(255)]
        public string Password { get; set; }

        public string AccessToken { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(255)]
        public string Email { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime UpdateAt { get; set; }
    }
}
