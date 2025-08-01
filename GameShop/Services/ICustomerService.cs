using GameShop.DTO;

namespace GameShop.Services
{
    public interface ICustomerService
    {
        Task<List<CustomerDto>> GetAllAsync();
        Task<CustomerDto?> GetByIdAsync(int id);
        Task<CustomerWithGamesDto?> GetWithGamesByIdAsync(int id);
        Task<CustomerDto> CreateAsync(CustomerCreateDto dto);
        Task<bool> UpdateAsync(int id, CustomerUpdateDto dto);
        Task<bool> DeleteAsync(int id);

        Task<bool> AssignGameToCustomerAsync(int customerId, int gameId);
        Task<bool> RemoveGameFromCustomerAsync(int customerId, int gameId);

    }
}
