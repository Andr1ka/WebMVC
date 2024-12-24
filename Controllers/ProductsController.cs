using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using WebMVC.Model;
using WebMVC.Model.EntityFramework;

namespace WebMVC.Controllers
{
    [Route("[controller]")]
    public class ProductsController : Controller
    {
        private readonly ApplicationContext _context;

        public ProductsController(ApplicationContext context)
        {
            _context = context;
        }

        [Route("Index")]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet("Create")]
        public IActionResult CreateProduct()
        {
            return View();
        }

        [HttpGet("View")]
        public IActionResult ViewProduct()
        {
            return View();
        }

        [HttpGet("Edit")]
        public IActionResult EditProduct()
        {
            return View();
        }

        [HttpGet("Delete")]
        public IActionResult DeleteProduct()
        {
            return View();
        }

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            var products = await _context.Products.ToListAsync();
            return Json(products);
        }

        [HttpPost("Create")]
        public async Task<IActionResult> CreateProduct([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(product);
                await _context.SaveChangesAsync();
                return Ok(new { message = "Product created successfully", productId = product.ProductId });
            }
            return BadRequest(ModelState);
        }



        [HttpDelete("Delete/{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPut("Edit")]
        public async Task<IActionResult> EditProduct([FromBody] Product product)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    Console.WriteLine($"Updating product with ID: {product.ProductId}");
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                    Console.WriteLine($"Product with ID: {product.ProductId} updated successfully");
                    return Ok();
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    Console.WriteLine($"DbUpdateConcurrencyException: {ex.Message}");
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Exception: {ex.Message}");
                    return StatusCode(500, "Internal server error");
                }
            }
            return BadRequest(ModelState);
        }

        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            return Json(product);
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
