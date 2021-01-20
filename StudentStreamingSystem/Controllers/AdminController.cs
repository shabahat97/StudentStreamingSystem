using StudentStreamingSystem.Context;
using StudentStreamingSystem.MyClasses;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentStreamingSystem.Controllers
{
    public class AdminController : Controller
    {
        StudentStreamingDBEntities objDb = new StudentStreamingDBEntities();
        // GET: Admin
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Login()
        {
            return View();
        }
        [HttpPost]
       [ValidateGoogleCaptcha]
        public ActionResult Login(Admin admin)
        {
            using (StudentStreamingDBEntities objDb = new StudentStreamingDBEntities())
            {
                var admindetails = objDb.Admins.Where(x => x.Email == admin.Email && x.Password == admin.Password).FirstOrDefault();
                if (admindetails == null)
                {
                    ViewBag.ErrorMessage = "Email or Password is not correct";
                    return View();
                }
            }
            return RedirectToAction("AdminUpload");
        }
        public ActionResult AdminUpload()
        {
            return View();
        }
    }
}