using System.Text.Json.Serialization;

namespace LearnToKeycloak.Dtos
{
    public record UserDto
    {
        //init kullanmadim cunku update edilebilir propertyler var
        
        [JsonPropertyName("id")]
        public Guid Id { get; set; }
        [JsonPropertyName("username")]
        public string Username { get; set; }
        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("emailVerified")]
        public bool EmailVerified { get; set; }
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }


    }

    public record UpdateUserDto
    {

        [JsonPropertyName("firstName")]
        public string FirstName { get; set; }
        [JsonPropertyName("lastName")]
        public string LastName { get; set; }
        [JsonPropertyName("email")]
        public string Email { get; set; }
        [JsonPropertyName("emailVerified")]
        public bool EmailVerified { get; set; }
        [JsonPropertyName("enabled")]
        public bool Enabled { get; set; }
    }
}
