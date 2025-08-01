using GameShop.Data;
using GameShop.DTO;
using GameShop.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace GameShop.Services
{
    public class CustomerService : ICustomerService
    {
        private readonly ApplicationDbContext _context;
        private readonly IPasswordHasher<Customer> _passwordHasher;

        public CustomerService(ApplicationDbContext context, IPasswordHasher<Customer> passwordHasher)
        {
            _context = context;
            _passwordHasher = passwordHasher;
        }

        public async Task<List<CustomerDto>> GetAllAsync()
        {
            return await _context.Customers
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    Email = c.Email,
                    DateJoined = c.DateJoined
                }).ToListAsync();
        }

        public async Task<CustomerDto?> GetByIdAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return null;

            return new CustomerDto
            {
                Id = customer.Id,
                UserName = customer.UserName,
                Email = customer.Email,
                DateJoined = customer.DateJoined
            };
        }

        public async Task<CustomerWithGamesDto?> GetWithGamesByIdAsync(int id)
        {
            return await _context.Customers
                .Where(c => c.Id == id)
                .Include(c => c.Games)
                .Select(c => new CustomerWithGamesDto
                {
                    Id = c.Id,
                    UserName = c.UserName,
                    Email = c.Email,
                    DateJoined = c.DateJoined,
                    Games = c.Games!.Select(g => new GameDto
                    {
                        GameId = g.GameId,
                        Title = g.Title,
                        Genre = g.Genre
                    }).ToList()
                }).FirstOrDefaultAsync();
        }

        public async Task<CustomerDto> CreateAsync(CustomerCreateDto dto)
        {
            var customer = new Customer
            {
                UserName = dto.UserName,
                Email = dto.Email,
                DateJoined = dto.DateJoined
            };

            customer.Password = _passwordHasher.HashPassword(customer, dto.Password);

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return new CustomerDto
            {
                Id = customer.Id,
                UserName = customer.UserName,
                Email = customer.Email,
                DateJoined = customer.DateJoined
            };
        }

        public async Task<bool> UpdateAsync(int id, CustomerUpdateDto dto)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            customer.UserName = dto.UserName;
            customer.Email = dto.Email;

            if (!string.IsNullOrEmpty(dto.Password))
            {
                customer.Password = _passwordHasher.HashPassword(customer, dto.Password);
            }

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null) return false;

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> AssignGameToCustomerAsync(int customerId, int gameId)
        {
            var customer = await _context.Customers.FindAsync(customerId);
            var game = await _context.Games.FindAsync(gameId);

            if (customer == null || game == null) return false;

            // Assign game to customer
            game.CustomerId = customerId;

            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> RemoveGameFromCustomerAsync(int customerId, int gameId)
        {
            var game = await _context.Games
                .Where(g => g.GameId == gameId && g.CustomerId == customerId)
                .FirstOrDefaultAsync();

            if (game == null) return false;

            // Remove association
            game.CustomerId = null;

            await _context.SaveChangesAsync();
            return true;
        }

    }
}
