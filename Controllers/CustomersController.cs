using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using WebMVC.Model;
using WebMVC.Model.EntityFramework;

namespace WebMVC.Controllers
{
    [Route("[controller]")]
    public class CustomersController : Controller
    {
        private readonly ApplicationContext _context;

        public CustomersController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Create")]
        public IActionResult CreateCustomer()
        {
            return View();
        }

        [HttpGet("View")]
        public IActionResult ViewCustomer()
        {
            return View();
        }
        [HttpGet("Edit")]
        public IActionResult EditCustomer()
        {
            return View();
        }
        [HttpGet("Delete")]
        public IActionResult DeleteCustomer()
        {
            return View();
        }

        [HttpGet("GetCustomers")]
        public async Task<IActionResult> GetCustomers()
        {
            var customers = await _context.Customers.ToListAsync();
            return Json(customers);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateCustomer([FromBody] Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Customer created successfully", customerId = customer.CustomerId });
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetCustomer/{id}")]
        public async Task<IActionResult> GetCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return Json(customer);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditCustomer([FromBody] Customer customer)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.CustomerId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return BadRequest(ModelState);
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteCustomer(int id)
        {
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }

            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private bool CustomerExists(int id)
        {
            return _context.Customers.Any(e => e.CustomerId == id);
        }
    }
}
