using ClaimBasedAuthentication.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using t = ClaimBasedAuthentication.Models;
namespace ClaimBasedAuthentication.Controllers
{

    public class HomeController : BaseController
    {
        public ActionResult Index()
        {

            string controllerAction = "HelloTest";
            Func<string, string> IsSelected = x => x == controllerAction ? "active" : "";

            var isSelected = IsSelected("HelloTest");
            //using (TestService testService = new TestService())
            //{
            //   // List<t.User> users = testService.GetAllUsers();
            //}

            //using (TestService testService = new TestService())
            //{
            //  //  List<t.User> users = testService.GetAllUsers();
            //}
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";
            return View();
        }
    }
}