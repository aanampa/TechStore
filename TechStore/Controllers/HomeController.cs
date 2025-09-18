using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using TechStore.Application.Interfaces;
using TechStore.Models;
using TechStore.Web.Models;

namespace TechStore.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly IProductService _productService;

        public HomeController(ILogger<HomeController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        public async Task<IActionResult> Catalogo()
        {
            ProductoViewModel model = new ProductoViewModel();
            model.ProductoLista = await _productService.GetAllProductsAsync();
            return View(model);
        }

        public async Task<IActionResult> Index()
        {
            ProductoViewModel model = new ProductoViewModel();

            model.ProductoLista = await _productService.GetAllProductsAsync();
            //var products = await _productService.GetAllProductsAsync();

            //ViewBag.Productos = products;

            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
