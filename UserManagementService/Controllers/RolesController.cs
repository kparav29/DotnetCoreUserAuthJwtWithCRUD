using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Identity;

namespace UserManagementService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // [Authorize(Roles = "SuperAdmin")]
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

        [HttpPost("UpdateRole{rolename}")]
        public async Task<IActionResult> UpdateRole(string rolename)
        {
            if (string.IsNullOrEmpty(rolename))
                return new NotFoundResult();

            // Check if the role already exists
            var roleExists = await roleManager.FindByNameAsync(rolename);
            if (roleExists !=null)
            {
                roleExists.Name = rolename;
                await roleManager.UpdateAsync(roleExists);             
               
            }

            return new OkResult();

        }



    }
}
