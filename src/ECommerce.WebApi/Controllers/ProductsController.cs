using Microsoft.AspNetCore.Mvc;

namespace ECommerce.WebApi.Controllers
{
    public class ProductsController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
