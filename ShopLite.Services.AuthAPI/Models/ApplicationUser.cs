using Microsoft.AspNetCore.Identity;

namespace ShopLite.Services.AuthAPI.Models
{
    public class ApplicationUser: IdentityUser
    {
        public string Name { get; set; }
    }
}
