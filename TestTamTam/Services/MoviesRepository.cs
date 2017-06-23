using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using TestTamTam.Models;

namespace TestTamTam.Services
{
    public interface IMoviesRepository
    {
        Task<IEnumerable<Movie>> GetMoviesByTitle(string title);
    }

    public class MoviesRepository : IMoviesRepository
    {
        private const string RESOURCE_LOCATION = "http://localhost:60921/movies/";
        private const string TITLE_PARAM = "{title}";
        private const string DATA_TYPE = "application/json";

        public async Task<IEnumerable<Movie>> GetMoviesByTitle(string title)
        {
            var result = Enumerable.Empty<Movie>();

            using (var client = new HttpClient())
            {
                client.BaseAddress = new Uri(RESOURCE_LOCATION);
                client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.GetAsync(title);
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    result = JsonConvert.DeserializeObject<IEnumerable<Movie>>(data);
                }
            }

            return result;
        }
    }
}