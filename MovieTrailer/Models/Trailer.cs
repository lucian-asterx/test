using System;

namespace MovieTraier.Models
{
    public class Trailer
    {
        public string Description { get; set; }
        public string VideoId { get; set; }
        public DateTime PublishedAt { get; set; }
        public string ThumbnailUrl { get; set; }
    }
}
