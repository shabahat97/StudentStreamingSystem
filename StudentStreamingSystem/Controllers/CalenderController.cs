using StudentStreamingSystem.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace StudentStreamingSystem.Controllers
{
    public class CalenderController : Controller
    {
        StudentStreamingDBEntities objDb = new StudentStreamingDBEntities();


        public ActionResult Index()
        {
            return View();
        }

        public ActionResult IndexTest()
        {
            return View();
        }




        [HttpPost]
        public JsonResult SaveEvent(Course e)
        {
            //using (StudentStreamingDBEntities dc = new StudentStreamingDBEntities())
            //{
            //    if (e.CourseID > 0)
            //    {
            //        //Update the event
            //        var v = dc.TimeTables.Where(a => a. == e.CourseID).FirstOrDefault();
            //        if (v != null)
            //        {
            //            v.CourseName = e.CourseName;
            //            v.BeginsAt = e.BeginsAt;
            //            v.ClassType = e.ClassType;
            //            v.Credits = e.Credits;


            //            v.ThemeColor = e.ThemeColor;
            //        }
            //    }
            //    else
            //    {
            //        dc.Courses.Add(e);
            //    }
            TimeTable t = new TimeTable();

            int apno = Convert.ToInt32(Session["UserId"]);
            var status = false;

            var userWithSameApNo = objDb.TimeTables.Where(m => m.StudentId == apno && m.CourseName==e.CourseName).SingleOrDefault();
            if (userWithSameApNo == null)
            {

                if (e.ThemeColor == "red")
                {
                    e.ThemeColor = "Timeclash";

                }
                else if (e.ThemeColor == "green")
                {
                    e.ThemeColor = "Compulsory";
                }
                else if (e.ThemeColor == "orange")
                {
                    e.ThemeColor = "Selected Optional Course";
                }
                t.ClassType = e.ClassType;
                t.Campus = e.Campus;
                t.RoomName = e.RoomName;
                t.CourseType = e.CourseType;
                t.CourseName = e.CourseName;
                t.StudentId = apno;
                t.Credits = e.Credits;
                t.BeginsAt = e.BeginsAt;
                t.ThemeColor = e.ThemeColor;
                t.EndsAt = e.EndsAt;


                objDb.TimeTables.Add(t);
                objDb.SaveChanges();
                status = true;
            }
            else
            {
                status = false;

            }

            return new JsonResult { Data = new { status = status } };
        }

        //[HttpPost]
        //public JsonResult DeleteEvent(int eventID)
        //{
        //    var status = false;
        //    using (MyDatabaseEntities dc = new MyDatabaseEntities())
        //    {
        //        var v = dc.Events.Where(a => a.EventID == eventID).FirstOrDefault();
        //        if (v != null)
        //        {
        //            dc.Events.Remove(v);
        //            dc.SaveChanges();
        //            status = true;
        //        }
        //    }
        //    return new JsonResult { Data = new { status = status } };
        //}
        // GET: Calender
        string[] ids;
       
        public ActionResult Filter(FormCollection form)
        {


             string coursename = "";

                int courseid,userid;
            var courses = new List<Course>();
            ids = form["ID"].Split(new char[] { ',' });

            
            userid= Convert.ToInt32(Session["UserId"]);


            courses = objDb.Courses.Where(x => ids.Contains(x.CourseID.ToString())).ToList();
            foreach (var item in courses)
            {
                StudentCourseSelection result = new StudentCourseSelection();
                if (result.CourseName == item.CourseName)
                {
                    break;
                }
                else
                {


                    result.CourseName = item.CourseName;
                    result.StudentId = userid;
                }


                objDb.StudentCourseSelections.Add(result);
                objDb.SaveChanges();
            }


            TempData["mydata"] = courses;
        






           return RedirectToAction("Index");
        }

        public void SaveData(string Coursename,int userid)
        {
            StudentCourseSelection s = new StudentCourseSelection();
            s.CourseName = Coursename;
            s.StudentId = userid;

            objDb.StudentCourseSelections.Add(s);
            objDb.SaveChanges();


        }
     

        public JsonResult GetEvents()
        {
            using (StudentStreamingDBEntities dc=new StudentStreamingDBEntities())
            {
                var courses = TempData["mydata"];
                return new JsonResult { Data = courses, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }
    }
}