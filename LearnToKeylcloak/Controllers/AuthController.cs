using LearnToKeycloak.Dtos;
using LearnToKeylcloak.Dtos;
using LearnToKeylcloak.Options;
using LearnToKeylcloak.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Scalar.AspNetCore;
using System.Net;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace LearnToKeycloak.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AuthController(KeycloakService keycloakService, IOptions<KeycloakConfiguration> options) : ControllerBase
    {

        //açıklama düzeltilebilir
        [HttpPost]
        public async Task<IActionResult> Register(RegisterDto registerDto, CancellationToken cancellationToken = default)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/users";
            object data = new
            {
                username = registerDto.UserName,
                firstName = registerDto.FirstName,
                lastName = registerDto.LastName,
                email = registerDto.Email,
                enabled = true,
                emailVerified = true,
                credentials = new List<object>
                {
                    new
                    {
                            type = "password",
                            value = registerDto.Password,
                            temporary = false
                    }

                }
            };

            //Burada generic olarak string kullandık çünkü Keycloak'ın kullanıcı oluşturma endpointi başarılı olduğunda boş bir cevap döner.
            //Veri tipi olarak string kullanmak, boş cevabı düzgün bir şekilde işlemenizi sağlar.
            //Veya invalid bir istek yapıldığında hata mesajını yakalayabilirsiniz.
            var response = await keycloakService.PostAsync<object>(endpoint, data, true, cancellationToken);
            return Ok(response); 
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginDto loginDto, CancellationToken cancellationToken = default)
        {
            string endpoint = $"{options.Value.HostName}/realms/{options.Value.Realm}/protocol/openid-connect/token";

            var data = new List<KeyValuePair<string, string>>
            {
                new KeyValuePair<string, string>("client_id", options.Value.ClientId),
                new KeyValuePair<string, string>("client_secret", options.Value.ClientSecret),
                new KeyValuePair<string, string>("grant_type", "password"),
                new KeyValuePair<string, string>("username",loginDto.UserName),
                new KeyValuePair<string, string>("password", loginDto.Password)

            };
            var response = await keycloakService.PostUrlEncodedFormAsync<object>(endpoint, data, false, cancellationToken);
            return Ok(response);
        }
    }
}