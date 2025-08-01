namespace GameShop.DTO
{
    public class CustomerDto
    {
        public int Id { get; set; }
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateJoined { get; set; }
    }
}
