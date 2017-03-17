using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.ComponentModel.DataAnnotations;
using MiniProjectMVC.Models;
using System.Dynamic;
using System.Text.RegularExpressions;

namespace MiniProjectMVC.Controllers
{
    public class HomeController : Controller
    {
        
        dbLinQtoSQLDataContext db = new dbLinQtoSQLDataContext();
        tblDemo d = new tblDemo();

        #region Actions For Index/Home
        // GET: /Home/
        public ActionResult Index()
        {
            MainModel modelAll = new MainModel();
            modelAll.ModelMy = GetModelMy();
            modelAll.ModelDemo = GettlblDemo();
            

            if (Session["id"] != null)
            {
            }
            else
            {
                return RedirectToAction("Index", "LogIn");
            }
            return View(modelAll);
        }

//==================Post action for Index========================
        [HttpPost]
        public ActionResult Index(ModelMy m)
        {
            return View();
        }
        #endregion

        #region Actions For LogOutButton\Home
        //====================Post action for LogOut========================
        [HttpPost]
        public ActionResult LogOut()
        {
            Session.Abandon();
            return RedirectToAction("Index", "LogIn");
        }
        #endregion

        #region Action For link Edit 
        //===================Post action for Grid Edit=========================

        [HttpGet]
        public ActionResult GridEdit(int id)
        {
            Session["id"] = id;
            return RedirectToAction("EditForm", "Home");
        }
        #endregion


        #region Action For Link Delete
//===================Post action for Grid Delete=======================
        public ActionResult GridDelete(int id)
        {
            var uniq = db.tblDemos.Single(x => x.id == id);
            db.tblDemos.DeleteOnSubmit(uniq);
            db.SubmitChanges();
            return RedirectToAction("Index", "Home");
        }
        #endregion


        #region Functions For Get Models
        //==================Method for Get Models===============================

 //*********** 1 Method For tblDemo *************
        public List<tblDemo> GettlblDemo()
        {
            Modeldemo dm = new Modeldemo();
            MainModel mm = new MainModel();
          // dm.ld = db.tblDemos.ToList();
            mm.ModelDemo = db.tblDemos.ToList();
            return mm.ModelDemo;
        }


 //********** 2 Method For ModelMy **************
        public ModelMy GetModelMy()
        {
            ModelMy m = new ModelMy();
            return m;
        }
        #endregion


        #region Action For EditForm Get & Post EditForm\Home
        //============Get Action for EditForm =================
        [HttpGet]
        public ActionResult EditForm()
        {
             ModelMy m = new ModelMy();
                MainModel mm = new MainModel();

            if (Session["id"] != null)
            {
 //=============Filter Data ================
               
                    int id = Convert.ToInt32(Session["id"]);
                    var data = from x in db.tblDemos where x.id == id select x;

                    foreach (var item in data)
                    {
                        m.Name = item.name;
                        m.id = item.id;
                        m.Email = item.email;
                    }
                    mm.ModelMy = m;

                    return View("EditForm", mm);
            }
            else
            {
                return View("Index", "LogIn");
            }
            return View("Index", "Home");
        }

 //==========Post Action for EditForm ===================
        [HttpPost]
        public ActionResult EditForm(MainModel mm)
        {
            //============Filter user entered details=============
            if (string.IsNullOrEmpty(mm.ModelMy.Name))
            {
                ModelState.AddModelError("Name", "Please Enter Name !");
            }
            if (string.IsNullOrEmpty(mm.ModelMy.Email))
            {
                ModelState.AddModelError("Email", "Please Enter Email !");
            }
            else
            {
                Regex rg = new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$");
                Match mt = rg.Match(mm.ModelMy.Email);
                if (!mt.Success)
                {
                    ModelState.AddModelError("Email", "Invalid Email Address !");
                }
            }

            if (ModelState.IsValid)
            {
               int id= Convert.ToInt32(Session["id"]);
               var vid = db.tblDemos.Single(x => x.id == id);
              
               vid.name = mm.ModelMy.Name;
               vid.email = mm.ModelMy.Email;
               db.SubmitChanges();
               return RedirectToAction("Index", "Home");
                
            }
            return RedirectToAction("EditForm", "Home");

        }
        #endregion

        
    }
}
