using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Text.RegularExpressions;
using MiniProjectMVC.Models;

namespace MiniProjectMVC.Controllers
{
    public class ForgotController : Controller
    {
        //
        // GET: /Forgot/
        dbLinQtoSQLDataContext DB = new dbLinQtoSQLDataContext();
        tblDemo D = new tblDemo();
        public  DataTable dt = new DataTable();

        #region Action For Get Index\Forgot

        public ActionResult Index()
        {
            return View();
        }
        #endregion


        #region Action For Post Index\Forgot
        [HttpPost]
        public ActionResult Index(ModelMy m)
        {
            try
            {
                //==================Check All Details Are Filled By User============
                if (string.IsNullOrEmpty(m.Email))
                {
                    ModelState.AddModelError("Email", "Please Enter Email Address !");
                }
                else
                {
                    Regex rw = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                    Match mt = rw.Match(m.Email);
                    if (!mt.Success)
                    {
                        ModelState.AddModelError("Email", "Invalid Email Address !");
                    }
                    else
                    {
                        var FindPassword = from x in DB.tblDemos where x.email == m.Email select x;

                        if (FindPassword != null)
                        {
                            //====================logic for remove columns from datatable=================
                            for (int i = 0; i < dt.Columns.Count; i++)
                            {
                                dt.Columns.RemoveAt(i);
                            }
                            dt.Columns.Add("Name");
                            dt.Columns.Add("Password");
                            dt.Columns.Add("Email");
                            dt.Columns.Add("Id");
                            //===================fill datatable from var =================================
                            foreach (var item in FindPassword)
                            {
                                DataRow dr = dt.NewRow();
                                dr["Id"] = item.id;
                                dr["Name"] = item.name;
                                dr["Email"] = item.email;
                                dr["Password"] = item.password;
                                dt.Rows.Add(dr);
                            }
                            if (dt.Rows.Count > 0)
                            {
                                string Pswd="Your Password : " + dt.Rows[0]["Password"].ToString();
                                ModelState.AddModelError("Error",Pswd );
                            }
                        }
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
