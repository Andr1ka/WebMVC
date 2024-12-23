using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
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
            var customers = _context.Customers.ToList();
            return View(customers);
        }

        // Другие действия для управления покупателями (Create, Edit, Delete и т.д.)
    }
}
