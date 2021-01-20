using StudentStreamingSystem.Context;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StudentStreamingSystem.Controllers
{
    public class TutorController : Controller
    {
        StudentStreamingDBEntities objDb = new StudentStreamingDBEntities();
        // GET: Tutor
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                Tutor obj = new Tutor();
                obj.Name = tutor.Name;
                obj.Email = tutor.Email;
                objDb.Tutors.Add(obj);
                objDb.SaveChanges();
                return RedirectToAction("ViewTutor");
            }
            return View();
        }
        public ActionResult DeleteTutors(int id)
        {
            var res = objDb.Tutors.Where(x => x.TutorID == id).First();
            objDb.Tutors.Remove(res);
            objDb.SaveChanges();
            var list = objDb.Tutors.ToList();
            return View("ViewTutor", list);
        }
        public ActionResult ViewTutor()
        {
            var res = objDb.Tutors.ToList();
            return View(res);
        }
        public ActionResult EditTutors(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Tutor tutor = objDb.Tutors.Find(id);
            if (tutor == null)
            {
                return HttpNotFound();
            }
            return View(tutor);
        }
        [HttpPost]
        public ActionResult EditTutors(Tutor tutor)
        {
            if (ModelState.IsValid)
            {
                Tutor obj = new Tutor();
                obj.TutorID = tutor.TutorID;
                obj.Name = tutor.Name;
                obj.Email = tutor.Email;
                objDb.Entry(obj).State = EntityState.Modified;
                objDb.SaveChanges();
                return RedirectToAction("ViewTutor");
            }
            return View(tutor);
        }
    }
}