using MoviesAPI.Entities;
using MoviesAPI.Utilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace MoviesAPI.Repositories
{
    public interface IMovieRepository
    {
        Task<ServiceResult<IEnumerable<Movie>>> GetAsync(string title);
    }

    public class MovieRepository : IMovieRepository
    {
        private const string RESOURCE_LOCATION = "https://moviesapi.com/m.php";
        private const string RESOURCE_PARAMS = "?t={0}&type=movie&r=json";
        private const string DATA_TYPE = "application/json";

        private readonly ICacheProvider<Movie> _cacheProvider;

        public MovieRepository(ICacheProvider<Movie> cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public async Task<ServiceResult<IEnumerable<Movie>>> GetAsync(string title)
        {
            var result = _cacheProvider.Get(title);

            if (!result.Any())
            {
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(RESOURCE_LOCATION);
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue(DATA_TYPE));

                    var path = String.Format(RESOURCE_PARAMS, title);
                    var response = await client.GetAsync(path);
                    if (response.IsSuccessStatusCode)
                    {
                        var data = await response.Content.ReadAsStringAsync();
                        result = JsonConvert.DeserializeObject<IEnumerable<Movie>>(data);
                    }
                }
                _cacheProvider.Add(title, result);
            }
            return new ServiceResult<IEnumerable<Movie>>(result);
        }
    }
}