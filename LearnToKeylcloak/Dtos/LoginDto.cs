namespace LearnToKeycloak.Dtos
{
    public record LoginDto
    {
        public required string UserName { get; init; }
        public required string Password { get; init; }
    }
   
}
