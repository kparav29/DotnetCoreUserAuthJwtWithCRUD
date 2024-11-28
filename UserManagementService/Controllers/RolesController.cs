using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Identity;

namespace UserManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "Bearer")]
    public class RolesController : ControllerBase
    {

        private readonly RoleManager<IdentityRole> roleManager;
       

        public RolesController(RoleManager<IdentityRole> _roleManager)
        {
            roleManager = _roleManager;
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(string rolename) {

            if (string.IsNullOrEmpty(rolename))
                return new NotFoundResult();

            // Check if the role already exists
            bool roleExists = await roleManager.RoleExistsAsync(rolename);
            if (roleExists)
            {
                await roleManager.CreateAsync(new IdentityRole(Enum.Parse<UserRoles>(rolename.Trim()).ToString()));
            }

            return new OkResult();
        }

        [HttpPost("DeleteRole{rolename}")]
        public async Task<IActionResult> DeleteRole(string rolename) {
            if (string.IsNullOrEmpty(rolename))
                return new NotFoundResult();

            // Check if the role already exists
            var roleExists = await roleManager.FindByNameAsync(rolename);
            if (roleExists != null){
               
                await roleManager.DeleteAsync(roleExists);
            }
            return new OkResult();

        }

        [HttpPost("UpdateRole{originalRoleName}/{newRolename}")]
        public async Task<IActionResult> UpdateRole(string originalRoleName,string newRolename)
        {
            if (string.IsNullOrEmpty(originalRoleName))
                return new NotFoundResult();

            // Check if the role already exists
            var roleExists = await roleManager.FindByNameAsync(originalRoleName);
            if (roleExists !=null)
            {
                roleExists.Name = newRolename;
                await roleManager.UpdateAsync(roleExists);             
               
            }

            return new OkResult();

        }



    }
}
