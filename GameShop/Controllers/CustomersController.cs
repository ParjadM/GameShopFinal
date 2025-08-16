using GameShop.DTO;
using GameShop.Services;
using Microsoft.AspNetCore.Mvc;

namespace GameShop.Controllers
{
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        /// <summary>
        /// Retrieves a list of all customers and renders the Index view.
        /// </summary>
        /// <returns>An IActionResult that renders a view displaying the list of all customers.</returns>
        /// <example>
        /// GET: /Customers
        /// </example>
        public async Task<IActionResult> Index()
        {
            var customers = await _customerService.GetAllAsync();
            return View(customers);  
        }

        /// <summary>
        /// Retrieves a specific customer by their ID and renders the Details view.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to display.</param>
        /// <returns>
        /// An <see cref="IActionResult"/> that renders the details view for the customer.
        /// Returns a (HTTP 404) if the customer is not found.
        /// </returns>
        /// <example>
        /// GET: /Customers/Details/5
        /// </example>
        public async Task<IActionResult> Details(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) return NotFound();
            return View(customer); 
        }

        /// <summary>
        /// Renders the view for creating a new customer.
        /// </summary>
        /// <returns>Displays the customer creation form.</returns>
        /// <example>
        /// GET: /Customers/Create
        /// </example>
        public IActionResult Create()
        {
            return View();
        }

        /// <summary>
        /// Handles the HTTP POST request to create a new customer using the submitted form data.
        /// </summary>
        /// <param name="dto">The customer data submitted from the creation form, bound to a CustomerCreateDto object.</param>
        /// <returns>
        ///     On successful creation, redirects to the Index action (customer list).
        ///     If the submitted data is invalid, it redisplays the creation form with validation errors.
        /// </returns>
        /// <example>
        ///     POST: /Customers/Create
        /// With form data for a new customer.
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var created = await _customerService.CreateAsync(dto);
            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        ///  Renders the view for editing an existing customer's details.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to be edited.</param>
        /// <returns>
        ///     An <see cref="IActionResult"/> that displays the edit form populated with the customer's data.
        ///  (HTTP 404) if the customer is not found.
        /// </returns>
        /// <example>
        ///  GET: /Customers/Edit/5
        /// </example>
        public async Task<IActionResult> Edit(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) return NotFound();

            var updateDto = new CustomerUpdateDto
            {
               
            };

            return View(updateDto);
        }

        /// <summary>
        /// Handles the HTTP POST request to update an existing customer with the submitted form data.
        /// </summary>
        /// <param name="id">The unique identifier of the customer being updated, passed from the URL.</param>
        /// <param name="dto">The updated customer data submitted from the edit form.</param>
        /// <returns>
        /// Index page on successful update.
        /// The Edit view with validation errors if the submitted data is invalid.
        ///(HTTP 404) if the customer to update is not found.
        /// </returns>
        /// <example>
        /// POST: /Customers/Edit/5
        /// With updated form data for the customer.
        /// </example>
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerUpdateDto dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            var updated = await _customerService.UpdateAsync(id, dto);
            if (!updated) return NotFound();

            return RedirectToAction(nameof(Index));
        }

        /// <summary>
        ///     Renders a confirmation view for deleting a specific customer.
        /// </summary>
        ///  <param name="id">The unique identifier of the customer for whom the delete confirmation is sought.</param>
        /// <returns>
        ///     Displays the delete confirmation page with the customer's details.
        ///     Returns a (HTTP 404) if the customer is not found.
        /// </returns>
        /// <example>
        ///     GET: /Customers/Delete/5
        /// </example>
        public async Task<IActionResult> Delete(int id)
        {
            var customer = await _customerService.GetByIdAsync(id);
            if (customer == null) return NotFound();

            return View(customer);
        }

        /// <summary>
        ///     Handles the HTTP POST request to permanently delete a customer after confirmation.
        /// </summary>
        /// <param name="id">The unique identifier of the customer to be deleted.</param>
        /// <returns>
        ///     Index page upon successful deletion.
        ///     Returns a (HTTP 404) if the customer to delete is not found.
        /// </returns>
        /// <example>
        ///     POST: /Customers/Delete/5
        /// </example>
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var deleted = await _customerService.DeleteAsync(id);
            if (!deleted) return NotFound();

            return RedirectToAction(nameof(Index));
        }

    }
}
