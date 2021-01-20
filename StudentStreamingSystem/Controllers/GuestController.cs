using StudentStreamingSystem.Context;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentStreamingSystem.Controllers
{
    public class GuestController : Controller
    {
        // GET: Guest
        private StudentStreamingDBEntities objDb = new StudentStreamingDBEntities();
        public ActionResult Index()
        {
            Session["UserId"] = 0;
            var res = objDb.Courses.ToList();
            return View(res);
        }
   }
}