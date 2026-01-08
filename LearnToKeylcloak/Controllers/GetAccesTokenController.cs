using LearnToKeylcloak.Dtos;
using LearnToKeylcloak.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LearnToKeylcloak.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAccesTokenController : ControllerBase
    {

        private readonly KeycloakService _keycloakService;

        public GetAccesTokenController(KeycloakService keycloakService)
        {
            _keycloakService = keycloakService;
        }

        [HttpGet]
        public async Task<IActionResult> GetToken()
        {
            var response =await _keycloakService.GetAccessToken();
            GetAccesTokenResponseDto token = new GetAccesTokenResponseDto
            {
                AccessToken = response
            };
            return Ok(token);
        }
    }
}
