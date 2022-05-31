using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace GitHubAPIRequests
{
    public class Location
    {
        [JsonPropertyName("country")]
        public string country{ get; set; }

        [JsonPropertyName("post code")]
        public string postCode { get; set; }

        [JsonPropertyName("country abbreviation")]
        public string countryAbbreviation { get; set; }

        [JsonPropertyName("places")]
        public List<Place> Places { get; set; }

    }

    public class Place
    {
        [JsonPropertyName("place name")]
        public string PlaceName { get; set; }
        public string State { get; set; }
        public string StateAbreviation { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }


    }
}