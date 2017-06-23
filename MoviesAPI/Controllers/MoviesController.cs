using Microsoft.AspNetCore.Mvc;
using MoviesAPI.Entities;
using MoviesAPI.Repositories;
using MoviesAPI.Utilities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MoviesAPI.Controllers
{
    [Route("movies")]
    public class MoviesController : Controller
    {
        private const string TITLE_PARAM = "{title}";

        private readonly IMovieRepository _movieRepository;
        private readonly ITrailerRepository _trailerRepository;

        public MoviesController(IMovieRepository movieRepository, ITrailerRepository trailerRepository)
        {
            _movieRepository = movieRepository;
            _trailerRepository = trailerRepository;
        }

        [HttpGet(TITLE_PARAM)]
        public async Task<IEnumerable<Movie>> GetAsync(string title)
        {
            var movies = await _movieRepository.GetAsync(title);

            foreach (var movie in movies.Result)
            {
                var trailers = await _trailerRepository.GetAsync(movie.Title);

                movie.Trailers = trailers.Result;
            }

            return movies.Result;
        }
    }
}