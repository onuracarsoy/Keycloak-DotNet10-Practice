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
    public class RoleController(KeycloakService keycloakService, IOptions<KeycloakConfiguration> options) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetAllRolesForClient(CancellationToken cancellationToken = default)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/clients/{options.Value.ClientUUID}/roles";
        
            
            var response = await keycloakService.GetAsync<List<RoleDto>>(endpoint, true, cancellationToken);
            return Ok(response);
        }

        [HttpGet]
        public async Task<IActionResult> GetRoleByNameForClient(string roleName,CancellationToken cancellationToken = default)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/clients/{options.Value.ClientUUID}/roles/{roleName}";


            var response = await keycloakService.GetAsync<RoleDto>(endpoint, true, cancellationToken);
            return Ok(response);
        }

        [HttpPost]
        public async Task<IActionResult> CreateRoleForClient(CreateRoleDto createRoleDto, CancellationToken cancellationToken = default)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/clients/{options.Value.ClientUUID}/roles";


            var response = await keycloakService.PostAsync<string>(endpoint, createRoleDto, true, cancellationToken);
            return Ok(response);
        }

        [HttpDelete]
        public async Task<IActionResult> DeleteRoleForClient(string roleName, CancellationToken cancellationToken = default)
        {
            string endpoint = $"{options.Value.HostName}/admin/realms/{options.Value.Realm}/clients/{options.Value.ClientUUID}/roles/{roleName}";


            var response = await keycloakService.DeleteAsync<string>(endpoint, true, cancellationToken);
            return Ok(response);
        }
    }
}
