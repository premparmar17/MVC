using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using MiniProjectMVC.Models;
using System.Data;
using System.Text.RegularExpressions;

namespace MiniProjectMVC.Controllers
{
    public class LogInController : Controller
    {
        //
        // GET: /LogIn/
        dbLinQtoSQLDataContext db = new dbLinQtoSQLDataContext();
        tblDemo d = new tblDemo();
        ModelMy m = new ModelMy();
        public   DataTable dt = new DataTable();
        public static  IEnumerable<DataRow> t=null;


        #region Action For Get Index\LogIn
        public ActionResult Index()
        {
            return View();
        }
        #endregion

        #region Action For Post Index\logIn
        [HttpPost]
        public ActionResult Index(ModelMy d)
        {
            try
            {
 //-------------------------------Check User Entered Data OR Not-------------------//
                if (string.IsNullOrEmpty(d.Email))
                {
                    ModelState.AddModelError("Email", "Email is required");
                }
                else
                {
                    Regex rg = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match mt = rg.Match(d.Email);
                    if (!mt.Success)
                    {
                        ModelState.AddModelError("Email", "Invalid Email Address !");
                    }
                }
                if (string.IsNullOrEmpty(d.Password))
                {
                    ModelState.AddModelError("Password", "Password is required");
                }
                


  //---------------------------After Set Validation Resend To View----------------//
                if (ModelState.IsValid)
                {
//---------------------------Delete columns from datatable---------------------------------//

                    for (int j = 0; j < dt.Columns.Count; j++)
                    {
                        dt.Columns.RemoveAt(j);
                    }
 //-----------------------Create columns for datatable-----------------------------//
                    if (dt.Columns.Count == 0)
                    {
                        dt.Columns.Add("Id");
                        dt.Columns.Add("Name");
                        dt.Columns.Add("Email");
                        dt.Columns.Add("Password");
                    }


                    var b = from a in db.tblDemos where (a.email == d.Email) && (a.password == d.Password) select a;
//----------------------------------Fill value from var to datarow for bind datatable------------------------------//

                    foreach (var item in b)
                    {
                        DataRow dr = dt.NewRow();
                        dr["Id"] = item.id;
                        dr["Name"] = item.name;
                        dr["Email"] = item.email;
                        dr["Password"] = item.password;
                        dt.Rows.Add(dr);

                    }

 //=====Check Email And Password Exits Or Not=====//
                    if (dt.Rows.Count > 0)
                    {
                        Session["id"] = dt.Rows[0]["id"].ToString();
                        Session["name"] = dt.Rows[0]["name"].ToString();

                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        ModelState.AddModelError("Error", "Invalid UserName OR Password!");
                    }
                }


            }
            catch (Exception ex)
            {
                ModelState.AddModelError("Error", "Error: " + ex.Message.ToString());
            }
            return View();
        }
        #endregion

        #region Action for Get Test/LogiN

        public ActionResult Test()
        {
            return View();
        }
        #endregion
    }
}
