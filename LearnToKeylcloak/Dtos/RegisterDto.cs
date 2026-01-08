namespace LearnToKeycloak.Dtos
{
   
    public record RegisterDto
    {
        //init kullanimiyla sadece nesne olusturulurken deger atanabilir
        //required ile deger atamasinin zorunlu oldugu belirtilir
        public required string UserName { get; init; }
        public required string FirstName { get; init; }
        public required string LastName { get; init; }
        public required string Password { get; init; }
        public required string Email { get; init; }
    }
}
