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
    //[Authorize]
    public class UserRoleController(KeycloakService keycloakService, IOptions<KeycloakConfiguration> options) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> AssignRoleToUserByUserIdForClient(Guid userId, List<RoleUserDto> roleUserDto,CancellationToken cancellationToken)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/users/{userId}/role-mappings/clients/{options.Value.ClientUUID}";


            var response = await keycloakService.PostAsync<string>(endpoint, roleUserDto, true, cancellationToken);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> UnassignRoleToUserByUserIdForClient(Guid userId, List<RoleUserDto> roleUserDto, CancellationToken cancellationToken)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/users/{userId}/role-mappings/clients/{options.Value.ClientUUID}";


            var response = await keycloakService.DeleteAsync<string>(endpoint, roleUserDto, true, cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRoleToUserByUserIdForClient(Guid userId, CancellationToken cancellationToken)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/users/{userId}/role-mappings/clients/{options.Value.ClientUUID}";


            var response = await keycloakService.GetAsync<List<RoleUserDto>>(endpoint, true, cancellationToken);
            return Ok(response);
        }
        
    }
}
