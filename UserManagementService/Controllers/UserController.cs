using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagementService.Authentication.Model;
using UserManagementService.Helper;
using UserManagementService.Identity;
using UserManagementService.Identity.Models;
using UserManagementService.User;

namespace UserManagementService.Controllers
{
    [Authorize(AuthenticationSchemes = "Bearer")]
    [Route("api/[controller]")]
    [ApiController]   
    public class UserController : ControllerBase
    {
        private readonly UserManager<AppUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;

        public UserController(UserManager<AppUser> _userManager, RoleManager<IdentityRole> _roleManager)
        {
            userManager = _userManager;
            roleManager = _roleManager;           
        }

        [HttpPost("CreateUser")]
        public async Task<IActionResult> CreateUser([FromBody] UserModel model) {

            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User already exists!" });

            AppUser user = new()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User creation failed! Please check user details and try again." });

            if (!await roleManager.RoleExistsAsync(model.UserRole.Trim()))
                await roleManager.CreateAsync(new IdentityRole(model.UserRole.Trim()));

            if (await roleManager.RoleExistsAsync(model.UserRole.Trim()))
            {
                await userManager.AddToRoleAsync(user, model.UserRole.Trim());
            }

            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpDelete("DeleteUser{username}")]
        public async Task<IActionResult> DeleteUser(string username)
        {

            var userExists = await userManager.FindByNameAsync(username);
            if (userExists == null)
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User does not exists!" });

           
            var result = await userManager.DeleteAsync(userExists);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new Response { Status = "Error", Message = "User deletion failed! Please check user details and try again." });


            return Ok(new Response { Status = "Success", Message = "User created successfully!" });
        }

        [HttpGet("GetUser")]
        public  ActionResult<AppUser> GetUser() 
        {

            var userList = userManager.Users.ToList();
            if (userList == null)
                return StatusCode(StatusCodes.Status404NotFound, new Response { Status = "Error", Message = "User does not exists!" });

            return Ok(userList);
        }

    }
}
