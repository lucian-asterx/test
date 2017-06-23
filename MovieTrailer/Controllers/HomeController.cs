using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using MovieTraier.Models;
using MovieTraier.Services;

namespace MovieTraier.Controllers
{
    public class HomeController : Controller
    {
        private readonly IMoviesRepository _moviesRepository;

        public HomeController(IMoviesRepository moviesRepository)
        {
            _moviesRepository = moviesRepository;
        }

        public IActionResult Index()
        {
            return View(Enumerable.Empty<Movie>());
        }

        [HttpPost]
        public async Task<IActionResult> Index(string title)
        {
            var result = await _moviesRepository.GetMoviesByTitle(title);

            return View(result);
        }
    }
}