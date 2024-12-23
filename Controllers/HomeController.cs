using Microsoft.AspNetCore.Mvc;

namespace WebMVC.Controllers
{
    [Route("[controller]")]
    [Route("")]
    public class HomeController : Controller
    {

        [Route("Index")]
        [Route("")]
        public IActionResult Index()
        {
            return View();
        }
    }
}
