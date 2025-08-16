using System.ComponentModel.DataAnnotations;

namespace GameShop.Models
{
    // Represents a customer in the game shop
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        // The username must be between 3 and 50 characters
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        // The email must be a valid email address
        public string Email { get; set; } = null!;

        [Required]
        // The date the customer joined the shop
        public DateTime DateJoined { get; set; }

        [Required]
        [StringLength(100)]
        // The password must be between 6 and 100 characters
        public string Password { get; set; } = null!;
        
        // The customer's games collection
        public ICollection<Game>? Games { get; set; }
    }
}
