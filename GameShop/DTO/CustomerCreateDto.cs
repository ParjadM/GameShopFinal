namespace GameShop.DTO
{
    public class CustomerCreateDto
    {
        public string UserName { get; set; } = null!;
        public string Email { get; set; } = null!;
        public DateTime DateJoined { get; set; }
        public string Password { get; set; } = null!;
    }

}
