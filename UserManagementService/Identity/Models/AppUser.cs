using Microsoft.AspNetCore.Identity;

namespace UserManagementService.Identity.Models
{
    public class AppUser:IdentityUser
    {
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
