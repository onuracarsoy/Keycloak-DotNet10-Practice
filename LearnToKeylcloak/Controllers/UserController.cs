using LearnToKeycloak.Dtos;
using LearnToKeylcloak.Options;
using LearnToKeylcloak.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace LearnToKeycloak.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    [Authorize("AdminPolicy")]
    public class UserController(KeycloakService keycloakService, IOptions<KeycloakConfiguration> options) : ControllerBase
    {
        [HttpGet] 
        public async Task<IActionResult> GetAllUsers(CancellationToken cancellationToken = default)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/users";

            List<UserDto> data = await keycloakService.GetAsync<List<UserDto>>(endpoint, true, cancellationToken);

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByEmail(string email,CancellationToken cancellationToken = default)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/users?email={email}";

            List<UserDto> data = await keycloakService.GetAsync<List<UserDto>>(endpoint, true, cancellationToken);

            return Ok(data);
        }

        [HttpGet]
        public async Task<IActionResult> GetUserByUsername(string username, CancellationToken cancellationToken = default)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/users?username={username}";

            List<UserDto> data = await keycloakService.GetAsync<List<UserDto>>(endpoint, true, cancellationToken);

            return Ok(data);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateUser(Guid id,UpdateUserDto updateUserDto, CancellationToken cancellationToken = default)
        {

            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/users/{id}";

          var response = await keycloakService.PutAsync<object>(endpoint,updateUserDto ,true, cancellationToken);

            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteUser(Guid id, CancellationToken cancellationToken = default)
        {

            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/users/{id}";

            var response = await keycloakService.DeleteAsync<object>(endpoint, true, cancellationToken);

            return Ok(response);
        }
    }
}
