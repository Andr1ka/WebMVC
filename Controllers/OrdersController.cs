using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
            var orders = _context.Orders.ToList();
            return View(orders);

        }

        // Другие действия для управления заказами (Create, Edit, Delete и т.д.)
    }
}
