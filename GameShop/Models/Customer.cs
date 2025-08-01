using System.ComponentModel.DataAnnotations;

namespace GameShop.Models
{
    public class Customer
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string UserName { get; set; } = null!;

        [Required]
        [EmailAddress]
        public string Email { get; set; } = null!;

        [Required]
        public DateTime DateJoined { get; set; }

        [Required]
        [StringLength(100)]
        public string Password { get; set; } = null!;

       
        public ICollection<Game>? Games { get; set; }
    }
}
