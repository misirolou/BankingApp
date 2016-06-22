using Newtonsoft.Json;

namespace App1.Models
{
    public class Token
    {
        [JsonProperty(PropertyName = "token")]
        public string token { get; set; }

        public Token(string token)
        {
            this.token = token;
        }
    }

}