using StudentStreamingSystem.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentStreamingSystem.Controllers
{
    public class ViewTimeTableController : Controller
    {
        StudentStreamingDBEntities objDb = new StudentStreamingDBEntities();
        // GET: ViewTimeTable
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ViewList()
        {
            var res = objDb.TimeTables.ToList();
            return View(res);
        }
    }
}