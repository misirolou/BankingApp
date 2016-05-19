using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace App1.Models
{
    class Accounts
    {
        public class Self
        {
            [JsonProperty(PropertyName = "href")]
            public string href { get; set; }
        }

        public class Detail
        {
            [JsonProperty(PropertyName = "href")]
            public string href { get; set; }
        }

        public class Links
        {
            public Self self { get; set; }
            public Detail detail { get; set; }
        }

        public class Account
        {
            [JsonProperty(PropertyName = "id")]
            public string id { get; set; }
            [JsonProperty(PropertyName = "label")]
            public object label { get; set; }
            [JsonProperty(PropertyName = "bank_id")]
            public string bank_id { get; set; }
            [JsonProperty(PropertyName = "_links")]
            public Links _links { get; set; }
        }

    }
}
