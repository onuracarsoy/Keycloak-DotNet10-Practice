using System.Text.Json.Serialization;

namespace LearnToKeycloak.Dtos
{
    public record RoleDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }  

        [JsonPropertyName("description")]
        public  string Description { get; set; } = default!;
    }

    public record CreateRoleDto
    {
        [JsonPropertyName("name")]
        public required string Name { get; set; } 
        [JsonPropertyName("description")]
        public string Description { get; set; } = default!;


    }

    public record RoleUserDto
    {
        [JsonPropertyName("id")]
        public Guid Id { get; set; }

        [JsonPropertyName("name")]
        public required string Name { get; set; }



    }

}
