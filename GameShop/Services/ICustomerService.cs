using GameShop.DTO;

namespace GameShop.Services
{
    // Interface for customer-related operations
    public interface ICustomerService
    {
        // Get all customers
        Task<List<CustomerDto>> GetAllAsync();
        // Get a customer by ID
        Task<CustomerDto?> GetByIdAsync(int id);
        // Get a customer with their games by ID
        Task<CustomerWithGamesDto?> GetWithGamesByIdAsync(int id);
        // Create a new customer
        Task<CustomerDto> CreateAsync(CustomerCreateDto dto);
        // Update an existing customer
        Task<bool> UpdateAsync(int id, CustomerUpdateDto dto);
        // Delete a customer by ID
        Task<bool> DeleteAsync(int id);
        // Assign a game to a customer
        Task<bool> AssignGameToCustomerAsync(int customerId, int gameId);
        // Remove a game from a customer
        Task<bool> RemoveGameFromCustomerAsync(int customerId, int gameId);

    }
}
