using System;
using System.Text.Json.Serialization;

namespace AdminSystem.API.Model{

    public class Login {
        [JsonPropertyName("email")]
        public string Email { get; set; }
        
        [JsonPropertyName("password")]
        public string Password { get; set; }
    }
}
