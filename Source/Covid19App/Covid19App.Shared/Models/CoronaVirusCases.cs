using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Covid19App.Shared.Models
{
    public class CoronaVirusCases
    {
        [JsonProperty("latest")]
        public Latest Latest { get; set; }

        [JsonProperty("locations")]
        public List<Location> Locations { get; set; }
    }

    public class Latest
    {
        [JsonProperty("confirmed")]
        public long Confirmed { get; set; }

        [JsonProperty("deaths")]
        public long Deaths { get; set; }

        [JsonProperty("recovered")]
        public long Recovered { get; set; }
    }

    public class Location
    {
        [JsonProperty("coordinates")]
        public Coordinates Coordinates { get; set; }

        [JsonProperty("country")]
        public string Country { get; set; }

        [JsonProperty("country_code")]
        public string CountryCode { get; set; }

        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }

        [JsonProperty("latest")]
        public Latest Latest { get; set; }

        [JsonProperty("province")]
        public string Province { get; set; }
    }

    public class Coordinates
    {
        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }
    }
}