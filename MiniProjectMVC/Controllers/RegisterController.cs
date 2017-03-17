using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using MiniProjectMVC.Models;
using System.Text.RegularExpressions;

namespace MiniProjectMVC.Controllers
{
    public class RegisterController : Controller
    {
        //
        // GET: /Register/

        dbLinQtoSQLDataContext db = new dbLinQtoSQLDataContext();
        tblDemo d = new tblDemo();
        public static DataTable dt = new DataTable();

        #region Action For Get Index\Register
        public ActionResult Index()
        {
            return View();
        }
        #endregion


        #region Action For Post Index\Register
        [HttpPost]
        public ActionResult Index(ModelMy m)
        {
            try
            {
                //--------------------Check All Details Fill By User------------------//
                if (string.IsNullOrEmpty(m.Name))
                {
                    ModelState.AddModelError("Name", "Please Enter Name!");
                }
                if (string.IsNullOrEmpty(m.Email))
                {
                    ModelState.AddModelError("Email", "Please Enter Email Address!");
                }
                else
                {
                    Regex rg = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match mt = rg.Match(m.Email);
                    if (!mt.Success)
                    {
                        ModelState.AddModelError("Email", "Invaid Email Address!");
                    }
                }
                if (string.IsNullOrEmpty(m.Password))
                {
                    ModelState.AddModelError("Password", "Please Enter Password!");
                }

                if (ModelState.IsValid)
                {
                    var FindEmail = from x in db.tblDemos where x.email == m.Email select x;

                    if (FindEmail != null)
                    {
                        //----------------Logic for Remove Datatable Columns-------------
                        for (int i = 0; i < dt.Columns.Count; i++)
                        {
                            dt.Columns.RemoveAt(i);
                        }
                        //----------------Logic For Add New Columns-----------------------
                        dt.Columns.Add("Email");
                        //----------------Logic for Add rows into datatable from var--------
                        foreach (var item in FindEmail)
                        {
                            DataRow dr = dt.NewRow();
                            dr["Email"] = item.email;
                            dt.Rows.Add(dr);
                        }
                        //------------------Check if Datatable have any data------------
                        if (dt.Rows.Count > 0)
                        {
                            ModelState.AddModelError("Error", "Sorry This Email Already In Use!");
                        }
                        else
                        {
                            d.name = m.Name;
                            d.email = m.Email;
                            d.password = m.Password;
                            db.tblDemos.InsertOnSubmit(d);
                            db.SubmitChanges();
                            ViewBag.Message = "Register Successfully!";
                        }
                        return RedirectToAction("Index", "Home");
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
    }
}
