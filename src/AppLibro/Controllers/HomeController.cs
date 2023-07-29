using AppLibro.Repositories.Abstract;
using Microsoft.AspNetCore.Mvc;

namespace AppLibro.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ILibroService _libroService;

        public HomeController(ILogger<HomeController> logger, ILibroService libroService)
        {
            _logger = logger;
            _libroService = libroService;
        }

        public IActionResult Index(string term="", int currentPage = 1)
        {
            _logger.LogInformation("Tester");
            var libros = _libroService.List(term, true, currentPage);

            return View(libros);
        }

        public IActionResult LibroDetail(int libroId){
            var libro = _libroService.GetById(libroId);

            return View(libro);
        }

        public IActionResult About()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View("Error!");
        }
    }
}