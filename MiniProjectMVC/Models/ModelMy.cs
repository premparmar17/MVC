using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;
using System.ComponentModel.DataAnnotations;
using System.Text;
using System.Collections;

namespace MiniProjectMVC.Models
{
    public class ModelMy
    {

        public int id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string Error { get; set; }
        public string Css { get; set; }
        public object list { get; set; }
    }

    public class Modeldemo
    {
       public List<tblDemo> ld = new List<tblDemo>();

    }

    public class MainModel
    {
        public ModelMy ModelMy { get; set; }
        public List<tblDemo> ModelDemo { get; set; }

    }
   
}