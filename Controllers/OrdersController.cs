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
        [HttpGet("View")]
        public IActionResult ViewOrder()
        {
            return View();
        }
        [HttpGet("Edit")]
        public IActionResult EditOrder()
        {
            return View();
        }
        [HttpGet("Delete")]
        public IActionResult DeleteOrder()
        {
            return View();
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateOrder([FromBody] Order order)
        {
           
                try
                {
                    // Проверка на дублирование продуктов
                    var productIds = order.OrderProducts.Select(op => op.ProductId).ToList();
                    if (productIds.Count != productIds.Distinct().Count())
                    {
                        ModelState.AddModelError("", "Duplicate products are not allowed.");
                        return BadRequest(ModelState);
                    }

                // Добавление заказа
                var orderToSave = new Order { CustomerId = order.CustomerId };
                _context.Orders.Add(orderToSave);
                    await _context.SaveChangesAsync();

                    // Добавление продуктов в заказ
                    foreach (var orderProduct in order.OrderProducts)
                    {
                        orderProduct.OrderId = orderToSave.OrderId;
                        var product = await _context.Products.FindAsync(orderProduct.ProductId);
                        if (product != null)
                        {
                            orderProduct.Product = product;
                            _context.OrderProducts.Add(orderProduct);
                        }
                    }
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

        [HttpGet("GetOrders")]
        public async Task<IActionResult> GetOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderProducts)
                    .ThenInclude(op => op.Product)
                .ToListAsync();

            var orderDetails = orders.Select(order => new
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                CustomerName = order.Customer.Name,
                Products = order.OrderProducts.Select(op => new
                {
                    ProductId = op.ProductId,
                    ProductName = op.Product.Name
                }).ToList()
            }).ToList();

            return Json(orderDetails);
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditOrder([FromBody] Order order)
        {
           
                try
                {
                    var existingOrder = await _context.Orders
                        .Include(o => o.OrderProducts)
                        .FirstOrDefaultAsync(o => o.OrderId == order.OrderId);

                    if (existingOrder == null)
                    {
                        return NotFound();
                    }

                // Проверка на существование CustomerId
                var customerExists = await _context.Customers.AnyAsync(c => c.CustomerId == order.CustomerId);
                if (!customerExists)
                {
                    ModelState.AddModelError("CustomerId", "CustomerId does not exist.");
                    return BadRequest(ModelState);
                }

                // Обновление покупателя
                existingOrder.CustomerId = order.CustomerId;

                    // Удаление существующих продуктов из заказа
                    _context.OrderProducts.RemoveRange(existingOrder.OrderProducts);

                    // Добавление новых продуктов в заказ
                    foreach (var orderProduct in order.OrderProducts)
                    {
                        var product = await _context.Products.FindAsync(orderProduct.ProductId);
                        if (product != null)
                        {
                            var newOrderProduct = new OrderProduct { OrderId = existingOrder.OrderId, ProductId = orderProduct.ProductId };
                            _context.OrderProducts.Add(newOrderProduct);
                        }
                    }

                    await _context.SaveChangesAsync();
                    return Ok();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
        }

        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Order deleted successfully" });
        }



        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.OrderId == id);
        }

    }
}
