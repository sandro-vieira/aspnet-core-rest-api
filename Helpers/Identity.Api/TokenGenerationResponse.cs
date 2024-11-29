using System.Text.Json.Serialization;

namespace Identity.Api
{
    public class TokenGenerationResponse
    {
        [JsonPropertyName("token_type")]
        public required string TokenType { get; set; }
        [JsonPropertyName("expires_in")]
        public DateTime? ExpiresIn { get; set; }
        [JsonPropertyName("access_token")]
        public required string AccessToken { get; set; }
    }
}
