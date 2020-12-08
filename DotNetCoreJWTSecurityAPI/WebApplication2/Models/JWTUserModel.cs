using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace WebApplication2.Models
{
    public class JWTUserModel
    {
        public string userid { get; set; }
        public string password { get; set; }

        [JsonProperty("email",NullValueHandling =NullValueHandling.Ignore)]
        public string? email { get; set; }

        [JsonProperty("phoneno", NullValueHandling = NullValueHandling.Ignore)]
        public string? phoneno { get; set; }

        [JsonProperty("created_date", NullValueHandling = NullValueHandling.Ignore)]
        public DateTime created_date { get; set; }

        [JsonProperty("login_timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public string? login_timestamp { get; set; }

        [JsonProperty("logout_timestamp", NullValueHandling = NullValueHandling.Ignore)]
        public string? logout_timestamp { get; set; }
        public string role { get; set; }
    }
}
