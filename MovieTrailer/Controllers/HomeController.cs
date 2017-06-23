using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using TestTamTam.Services;

namespace TestTamTam.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMoviesRepository _moviesRepository;

        public HomeController(IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
        }

        public async Task<IActionResult> Index()
        {
            var result = await _moviesRepository.GetMoviesByTitle("love");

            return View(result);
        }
    }
}