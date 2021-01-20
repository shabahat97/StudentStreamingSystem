using LinqToExcel;
using StudentStreamingSystem.Context;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace StudentStreamingSystem.Controllers
{
    public class CourseController : Controller
    {
        StudentStreamingDBEntities objDb = new StudentStreamingDBEntities();

        //public ActionResult CreateTimeTable()
        //{
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult CreateTimeTable(TimeTable t)
        //{
        //    if (ModelState.IsValid)
        //    {
               
        //        objDb.TimeTables.Add(t);
        //        objDb.SaveChanges();
        //    }
        //    return View();
        //}
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {

            var room = objDb.Rooms.ToList();


            ViewBag.roomname = new SelectList(room, "RoomID", "Name");
            return View();
        }
        [HttpPost]
        public ActionResult Create(Course course)
        {
            if(ModelState.IsValid)
            {
                var room = objDb.Rooms.ToList();

                var model = objDb.Rooms.FirstOrDefault(_ => _.RoomID == course.RoomId);







                ViewBag.roomname = new SelectList(room, "RoomID", "Name");

                Course obj = new Course();
                obj.CourseNo = course.CourseNo;
                obj.CourseName = course.CourseName;
                obj.RoomId = course.RoomId;
                obj.Programme = course.Programme;
                obj.Campus = course.Campus;
                obj.Credits = course.Credits;
                obj.RoomName = model.Name;
                obj.Streams = course.Streams;
                obj.ClassType = course.ClassType;
                if (course.CourseType == "Compulsory")
                {
                    obj.ThemeColor = "green";
                }
                if (course.CourseType=="Optional")
                {
                    obj.ThemeColor = "orange";
                }
                obj.CourseType = course.CourseType;
                obj.BeginsAt = course.BeginsAt;
                obj.EndsAt = course.EndsAt;
                objDb.Courses.Add(obj);
                objDb.SaveChanges();
                return RedirectToAction("ViewCourses");
            }
            return View();
            
   
        }
        public ActionResult ViewCourses()
        {
            var res = objDb.Courses.ToList();
            return View(res);
        }
        public ActionResult DeleteCourses(int id)
        {
            var res = objDb.Courses.Where(x => x.CourseID == id).First();
            objDb.Courses.Remove(res);
            objDb.SaveChanges();
            var list = objDb.Courses.ToList();
            return View("ViewCourses", list);
        }
        public ActionResult EditCourses(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Course course = objDb.Courses.Find(id);
            if (course == null)
            {
                return HttpNotFound();
            }
            return View(course);
        }
        [HttpPost]
        public ActionResult EditCourses(Course course)
        {
            if (ModelState.IsValid)
            {
                Course obj = new Course();
                obj.CourseNo = course.CourseNo;
                obj.CourseName = course.CourseName;
                obj.Programme = course.Programme;
                obj.Campus = course.Campus;
                obj.CourseID = course.CourseID;
                obj.CourseType = course.CourseType;

                obj.Credits = course.Credits;
                obj.Streams = course.Streams;
                obj.ClassType = course.ClassType;
                obj.RoomName = course.RoomName;

                if (course.CourseType == "Compulsory")
                {
                    obj.ThemeColor = "green";
                }
                if (course.CourseType == "Optional")
                {
                    obj.ThemeColor = "orange";
                }

                obj.BeginsAt = course.BeginsAt;
                obj.EndsAt = course.EndsAt;
                objDb.Entry(obj).State = EntityState.Modified;
                objDb.SaveChanges();
                return RedirectToAction("ViewCourses");
            }
            return View(course);
        }
    }
}