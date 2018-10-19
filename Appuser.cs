using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Security.Principal;
using System.Web;

namespace ClaimBasedAuthentication.Models
{
    public class Appuser : IPrincipal
    {
        public string UserName { get { return _claims?.Claims?.FirstOrDefault(x => x.Type == "UserName")?.Value; } }
        private readonly ClaimsPrincipal _claims;
        public Appuser(ClaimsPrincipal claims)
        {
            _claims = claims;
        }
        public IIdentity Identity { get { return _claims.Identity; } }
       
        public bool IsInRole(string role)
        {
            return _claims.IsInRole(role);
        }
    }
}