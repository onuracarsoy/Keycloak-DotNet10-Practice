using System.Text.Json.Serialization;

namespace LearnToKeylcloak.Dtos
{
   
    public sealed record GetAccesTokenResponseDto
    {
        
        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; } = default;
    }
}
