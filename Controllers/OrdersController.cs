using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebMVC.Model;
using WebMVC.Model.EntityFramework;

namespace WebMVC.Controllers
{
    [Route("[controller]")]
    public class OrdersController : Controller
    {
        private readonly ApplicationContext _context;

        public OrdersController(ApplicationContext context)
        {
            _context = context;
        }

        [HttpGet("Index")]
        public IActionResult Index()
        {
            return View();

        }
        [HttpGet("Create")]
        public IActionResult CreateOrder()
        {
            return View();

        }


        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    _context.Orders.Add(order);
                    await _context.SaveChangesAsync();
                    return Ok(new { message = "Order created successfully", orderId = order.OrderId });
                }
                catch (DbUpdateException ex)
                {
                    Console.WriteLine($"DbUpdateException: {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }

            // Логирование ошибок валидации
            foreach (var key in ModelState.Keys)
            {
                var errors = ModelState[key].Errors;
                foreach (var error in errors)
                {
                    Console.WriteLine($"Validation error for {key}: {error.ErrorMessage}");
                }
            }

            return BadRequest(ModelState);
        }

    }
}
