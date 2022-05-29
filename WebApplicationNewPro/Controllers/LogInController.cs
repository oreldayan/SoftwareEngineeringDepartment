using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplicationNewPro.Dal;
using WebApplicationNewPro.Models;

namespace WebApplicationNewPro.Controllers
{
    public class LogInController : Controller
    {
        // GET: LogIn
        
        public ActionResult Enter()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Enter(Users user)
        {
            
            UsersDal logins = new UsersDal();

            Users db_user = logins.LogIns.Where(x => x.Username == user.Username && x.Password == user.Password).FirstOrDefault();
            
            if (db_user==null)
            {
                Session["message"] = "The password or the user name is incorrect";
                return View("Enter");
            }
            
            else
            {
                Session["Permission"] = db_user.Permission;
                Session["Password"] = db_user.Password;
                Session["Username"] = db_user.Username.ToString();
                Session["Id"] = db_user.Id.ToString();
                if (Session["Permission"].ToString() == "1")
                {
                    return RedirectToAction("FacultyAdministrator", "FacultyAdministrator");
                }
                if (Session["Permission"].ToString() == "2")
                {
                    return RedirectToAction("Lecturer", "Lecturer");
                }
                else
                {
                    return RedirectToAction("Student", "Student");
                }
            }
            

            
        }


    }
}