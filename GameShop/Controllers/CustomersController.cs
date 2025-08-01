using GameShop.DTO;
using GameShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // GET: api/customers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CustomerDto>>> GetAll()
        {
            var customers = await _customerService.GetAllAsync();
            return Ok(customers);
        }

        // GET: api/customers/{id}
        [HttpGet("{id:int}")]
        public async Task<ActionResult<CustomerDto>> GetById(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        // GET: api/customers/{id}/games
        [HttpGet("{id:int}/games")]
        public async Task<ActionResult<CustomerWithGamesDto>> GetWithGames(int id)
        {
            var customer = await _customerService.GetWithGamesByIdAsync(id);
            if (customer == null) return NotFound();
            return Ok(customer);
        }

        // POST: api/customers
        [HttpPost]
        public async Task<ActionResult<CustomerDto>> Create(CustomerCreateDto dto)
        {
            var created = await _customerService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
        }

        // PUT: api/customers/{id}
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, CustomerUpdateDto dto)
        {
            var updated = await _customerService.UpdateAsync(id, dto);
            if (!updated) return NotFound();
            return NoContent();
        }

        // DELETE: api/customers/{id}
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _customerService.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }

        // POST /api/customers/{customerId}/games/{gameId}
        [HttpPost("{customerId}/games/{gameId}")]
        public async Task<IActionResult> AssignGameToCustomer(int customerId, int gameId)
        {
            var success = await _customerService.AssignGameToCustomerAsync(customerId, gameId);
            if (!success) return NotFound();
            return NoContent();
        }

        // DELETE /api/customers/{customerId}/games/{gameId}
        [HttpDelete("{customerId}/games/{gameId}")]
        public async Task<IActionResult> RemoveGameFromCustomer(int customerId, int gameId)
        {
            var success = await _customerService.RemoveGameFromCustomerAsync(customerId, gameId);
            if (!success) return NotFound();
            return NoContent();
        }

    }
}
