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
    public interface ITrailerRepository
    {
        Task<ServiceResult<IEnumerable<Trailer>>> GetAsync(string title);
    }

    public class TrailerRepository : ITrailerRepository
    {
        private const string RESOURCE_LOCATION = "https://www.googleapis.com/youtube/v3/search";
        private const string RESOURCE_PARAMS = "?q={0}%20trailer&part=snippet&type=video&key=AIzaSyD2Xu1qtdQMuxlCxy1X79FHhJi2oh4VlPY";
        private const string DATA_TYPE = "application/json";

        private readonly ICacheProvider<Trailer> _cacheProvider;

        public TrailerRepository(ICacheProvider<Trailer> cacheProvider)
        {
            _cacheProvider = cacheProvider;
        }

        public async Task<ServiceResult<IEnumerable<Trailer>>> GetAsync(string title)
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
                        result = JsonConvert.DeserializeObject<IEnumerable<Trailer>>(data, new JsonTrailerConverter());
                    }
                }
                _cacheProvider.Add(title, result);
            }
            return new ServiceResult<IEnumerable<Trailer>>(result);
        }

    }
}