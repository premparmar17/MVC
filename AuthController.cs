using ClaimBasedAuthentication.Models;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;

namespace ClaimBasedAuthentication.Controllers
{
    public class AuthController : Controller
    {
        private IAuthenticationManager AuthenticationManager
        {
            get { return HttpContext.GetOwinContext().Authentication; }
        }
        // GET: Auth
        public ActionResult Login(string ReturnUrl)
        {
            ViewBag.ReturnUrl = ReturnUrl;
            return View(new User());
        }

        
        [HttpPost, ValidateAntiForgeryToken]
        public ActionResult Login(User user, string ReturnUrl)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    ModelState.AddModelError("Password", "Please Enter Details");
                    return View(user);
                }

                bool isAuthenticated = Authenticate(user);
                if (isAuthenticated)
                {
                    var identity = new ClaimsIdentity(
                        AuthenticationHelper.CreateClaim(user),
                        DefaultAuthenticationTypes.ApplicationCookie
                        );
                    AuthenticationManager.SignIn(new AuthenticationProperties()
                    {
                        AllowRefresh = false,
                        IsPersistent = true,
                        ExpiresUtc = DateTime.UtcNow.AddMinutes(5)
                    }, identity);

                    if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl)) return Redirect(ReturnUrl);

                    return RedirectToAction("Index", "Home");
                }
                return View(user);
            }
            catch (Exception)
            {
                throw;
            }
        }

        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut(DefaultAuthenticationTypes.ApplicationCookie);
            return Redirect("~/");
        }
        private bool Authenticate(User user)
        {
            using (ClaimAuthenticationDbEntities entity = new ClaimAuthenticationDbEntities())
                return entity.Users.Any(x => x.UserName == user.UserName && x.Password == user.Password);
        }
    }
}