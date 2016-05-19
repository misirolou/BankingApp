using System.Collections.Generic;
using Newtonsoft.Json;

namespace App1.REST
{
    public class AccountInfo
    {
        public class Owner
        {
            [JsonProperty(PropertyName = "id")]
            public string id { get; set; }
            [JsonProperty(PropertyName = "provider")]
            public string provider { get; set; }
            [JsonProperty(PropertyName = "display_name")]
            public string display_name { get; set; }
        }

        public class Balance
        {
            [JsonProperty(PropertyName = "currency")]
            public string currency { get; set; }
            [JsonProperty(PropertyName = "amount")]
            public string amount { get; set; }
        }

        public class AccountInfoDetailed
        {
            [JsonProperty(PropertyName = "id")]
            public string id { get; set; }
            [JsonProperty(PropertyName = "label")]
            public object label { get; set; }
            [JsonProperty(PropertyName = "number")]
            public string number { get; set; }
            public List<Owner> owners { get; set; }
            [JsonProperty(PropertyName = "type")]
            public object type { get; set; }
            public Balance balance { get; set; }
            [JsonProperty(PropertyName = "IBAN")]
            public object IBAN { get; set; }
            [JsonProperty(PropertyName = "swift_bic")]
            public object swift_bic { get; set; }
            [JsonProperty(PropertyName = "bank_id")]
            public string bank_id { get; set; }
        }
    }
}