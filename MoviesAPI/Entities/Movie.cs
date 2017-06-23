using System.Collections.Generic;

namespace MoviesAPI.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Year { get; set; }
        public string Poster { get; set; }
        public string Type { get; set; }
        public IEnumerable<Trailer> Trailers { get; set; }
    }
}