using System.Data.Entity;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;
using System.Collections.Generic;

namespace ClaimBasedAuthentication.Models
{
    public class ApplicatoinUser : IdentityUser
    {
        public string Name { get; set; }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicatoinUser> manager)
        {
            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);
            return userIdentity;
        }
    }

    internal class AuthenticationHelper
    {
        internal static List<Claim> CreateClaim(User user)
        {
            var claims = new List<Claim>()
            {
                new Claim("UserName",user.UserName)
            };
            return claims;
        }
    } 
}