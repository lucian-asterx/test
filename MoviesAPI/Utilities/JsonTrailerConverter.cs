using MoviesAPI.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MoviesAPI.Utilities
{
    public class JsonTrailerConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(IEnumerable<Trailer>));
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var result = new List<Trailer>();
            JObject root = JObject.Load(reader);

            var trailers = root["items"].Children<JObject>();

            foreach (var item in trailers)
            {
                var snippets = item["snippet"].ToArray();

                Trailer trailer = new Trailer();
                trailer.Description = snippets[2].Children().First().ToString();
                trailer.PublishedAt = DateTime.Parse(snippets[0].Children().First().ToString());

                var thumbnails = snippets[4].Children<JObject>();
                var mediumThumbnail = thumbnails["medium"].ToArray().First()["url"];

                trailer.ThumbnailUrl = mediumThumbnail.ToString();
                trailer.VideoId = item["id"].ToArray().Children().Skip(1).First().ToString();

                result.Add(trailer);
            }
            return result;
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
