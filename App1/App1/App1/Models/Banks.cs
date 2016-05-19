using Newtonsoft.Json;
using System.Collections.Generic;

namespace App1.Models
{
    public class bankslist
    {
        public List<Banks> banklist;
    }

    public class bankstuff
    {
        public Banks data;
    }

    public class Banks
    {
        [JsonProperty(PropertyName = "id")]
        public string id { get; set; }

        [JsonProperty(PropertyName = "short_name")]
        public string short_name { get; set; }

        [JsonProperty(PropertyName = "full_name")]
        public string full_name { get; set; }

        [JsonProperty(PropertyName = "logo")]
        public string logo { get; set; }

        [JsonProperty(PropertyName = "website")]
        public string website { get; set; }
    }
}