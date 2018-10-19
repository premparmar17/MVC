using ClaimBasedAuthentication.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace ClaimBasedAuthentication.Controllers
{
    [Authorize(ClaimType = "ssf", ClaimValue = "")]
    public class BaseController : Controller
    {
        protected internal Appuser CurrrentUser { get; private set; }

        protected override  void Initialize(RequestContext requestContext)
        {
            base.Initialize(requestContext);
            var user = this.User as ClaimsPrincipal;
            if (user != null && user.Identity.IsAuthenticated)
            {
                CurrrentUser = new Appuser(user);
            }
        }
        
    }
}