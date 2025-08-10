using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    public class CatalogController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
