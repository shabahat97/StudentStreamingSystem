
using StudentStreamingSystem.Context;
using StudentStreamingSystem.Models;
using StudentStreamingSystem.MyClasses;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using System.Web.Mvc;
namespace StudentStreamingSystem.Controllers
{
    public class StudentController : Controller
    {
        StudentStreamingDBEntities objDb = new StudentStreamingDBEntities();
        // GET: Student
        [HttpGet]
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
        public ActionResult Login(Student student)
        {
            using (StudentStreamingDBEntities objDb = new StudentStreamingDBEntities())
            {
                var userdetails = objDb.Students.Where(x => x.Email == student.Email && x.APNo == student.APNo).FirstOrDefault();
                if (userdetails == null)
                {


                    ViewBag.ErrorMessage = "Invalid Email or APNo.";
                    return View();
                }
                else
                {
                    Session["UserId"] = userdetails.APNo;

                }
            }

            return RedirectToAction("ViewListTest");
        }
        public  ActionResult ConvertAptoId(int id)
        {
           

            var item = objDb.Students.Single(x => x.StudentID == id);
            item.APNo = id;


            objDb.Students.Attach(item);
            objDb.Entry(item).Property(X => X.APNo).IsModified = true;
                        objDb.SaveChanges();
            

            var list = objDb.Students.ToList();
            return View("ViewStudents", list);
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Student student)
        {
              if (ModelState.IsValid)
                {
                    Student obj = new Student();
                    obj.Email = student.Email;
                    obj.APNo = student.APNo;
                    objDb.Students.Add(obj);
                    objDb.SaveChanges();
                    return RedirectToAction("ViewStudents");
                }
                return View();
        }
        public ActionResult ViewStudents()
        {
            var res = objDb.Students.ToList();
            return View(res);
        }
        public ActionResult DeleteStudents(int id)
        {
            var res = objDb.Students.Where(x => x.StudentID == id).First();
            objDb.Students.Remove(res);
            objDb.SaveChanges();
            var list = objDb.Students.ToList();
            return View("ViewStudents", list);
        }
        public ActionResult EditStudents(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Student student = objDb.Students.Find(id);
            if (student == null)
            {
                return HttpNotFound();
            }
            return View(student);
        }
        [HttpPost]
        public ActionResult EditStudents(Student student)
        {
            if (ModelState.IsValid)
            {
                Student obj = new Student();
                obj.StudentID = student.StudentID;
                obj.Email = student.Email;
                obj.APNo = student.APNo;
                objDb.Entry(obj).State = EntityState.Modified;
                objDb.SaveChanges();
                return RedirectToAction("ViewStudents");
            }
            return View(student);
        }
        //public ActionResult SelectCourse()
        //{
        //    var getcourses = objDb.Courses.ToList();
        //    SelectList list = new SelectList(getcourses, "CourseID", "CourseName");
        //    ViewBag.courseList = list;
        //    return View();
        //}
        public ActionResult CourseInfo(int courseid)
        {
            try
            {
                var courses = objDb.Courses.Where(t => t.CourseID == courseid).Select(x => new { x.CourseID, x.CourseName, x.BeginsAt, x.EndsAt }).ToList();
                return Json(courses, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                ViewBag.Message = "Please Try later";
                return View();
            }
        }

        //public ActionResult SelectCourse()
        //{
          
        //    CourseDBHandle gc = new CourseDBHandle();

        //    var list = objDb.Courses.ToList();

        //    //List<Course> list = gc.GetClass(schoolid);
        //    ViewData["class_name"] = new SelectList(list, "CourseID", "Name");
        //    return View();
        //}
        //[HttpPost]
        //public ActionResult SelectCourse(Course c)
        //{

        //    CourseDBHandle gc = new CourseDBHandle();

        //    var list = objDb.Courses.ToList();

        //    //List<Course> list = gc.GetClass(schoolid);
        //    ViewData["class_name"] = new SelectList(list, "CourseID", "Name");
        //    return View();
        //}
        public ActionResult LoginUpload()
        {
            var res = objDb.Courses.ToList();
            return View(res);
        }
        [HttpPost]
        public ActionResult LoginUpload(FormCollection form )
        {
            var courses = new List<Course>();
            string[] ids = form["CourseID"].Split(new char[] { ',' });
                courses = objDb.Courses.Where(x => ids.Contains(x.CourseID.ToString())).ToList();
            return View(courses);
        }
        public ActionResult ChooseDropDown()
        {
          




                var course = objDb.Courses.ToList();


                ViewBag.courseselection = new SelectList(course, "CourseID", "CourseName");

            

            return View();
        }
        [HttpPost]
        public ActionResult ChooseDropDown(Course course)
        {

            return RedirectToAction("ViewCourses", new { id = course.CourseID });

        }

        public ActionResult ViewCourses(int  id)
        {

            var list = objDb.Courses.Where(s => s.CourseID == id).ToList();

            return View(list);
        }

        public ActionResult ViewList()
        {
            var res = objDb.TimeTables.ToList();
            return View(res);
        }
        public ActionResult ViewListTest()
        {
            var res = objDb.Courses.ToList();
            return View(res);
        }
        [HttpPost]
        public ActionResult ViewListTest(string searchTxt)
        {
            var res = objDb.Courses.ToList();

            if(searchTxt!=null)
            {
                res = objDb.Courses.Where(x => x.CourseName.Contains(searchTxt)).ToList();
            }
            return View(res);
        }
        [HttpGet]
        public ActionResult SendEmail()
        {
            return View();
        }
      [HttpPost]
        public ActionResult SendEmail(TimeTable t)
        {
            try
            {

                if (ModelState.IsValid)
                {
  //                  string textBody = "Hi<strong>" + list. + "</strong><br />" + t.Campus + " $ are credited to your account for March payout" +
  //"<br > you can find breakup below <table><tr><td>BLAH BLAH BLAH</td></tr></table>";
  //                  var senderEmail = new MailAddress("shabahatalichishti@gmail.com", "Jamil");
  //                  var receiverEmail = new MailAddress("shabahatalichishti@gmail.com", "Receiver");
  //                  var password = "Your Email Password here";
  //                  var sub = textBody;
  //                  var body = textBody;
  //                  var smtp = new SmtpClient
  //                  {
  //                      Host = "smtp.gmail.com",
  //                      Port = 587,
  //                      EnableSsl = true,
  //                      DeliveryMethod = SmtpDeliveryMethod.Network,
  //                      UseDefaultCredentials = false,
  //                      Credentials = new NetworkCredential(senderEmail.Address, password)
  //                  };
  //                  using (var mess = new MailMessage(senderEmail, receiverEmail)
  //                  {
  //                      Subject = textBody,
  //                      Body = body
  //                  })
  //                  {
  //                      smtp.Send(mess);
  //                  }
                    return View();
                }
            }
            catch (Exception)
            {
                ViewBag.Error = "Some Error";
            }
            return View();
        }
    
        public ActionResult ViewMyTimeTable()
        {
            int apno = Convert.ToInt32(Session["UserId"]);
            var list = objDb.TimeTables.Where(s => s.StudentId == apno).ToList();

            return View(list);
        }
        [HttpPost]
        public ActionResult ViewMyTimeTable(FormCollection form)
        {
            int apno = Convert.ToInt32(Session["UserId"]);
            var list = objDb.TimeTables.Where(s => s.StudentId == apno).ToList();
            string[] ids;
            //StringBuilder YourTable = new StringBuilder();
            var obj = objDb.Students.Find(apno);
            string studentemail = obj.Email;
            var textBody = "";
            var timetable = new List<TimeTable>();

            ids = form["item.ID"].Split(new char[] { ',' });
            timetable = objDb.TimeTables.Where(x => ids.Contains(x.ID.ToString())).ToList();

            var senderEmail = new MailAddress("acmgroupweltec@gmail.com", "AcmGroup");
            var receiverEmail = new MailAddress(studentemail, "Receiver");
            var password = "admin2020";

            foreach (var item in timetable)
            {

                textBody += "<table border=1>" + "<tr>" +
                 "<th>CourseName</th>" +
                 "<th>Credits</th>" +
                 "<th>ClassType</th>" +
                 "<th>CourseType</th>" +
                 "<th>BeginsAt</th>" +
                 "<th>EndsAt</th>" +
                 "<th>Warning</th>" +
                 "<th>RoomName</th>" +
                 "<th>Campus</th>" +
              "<tr/><tr>" +
                 "<td>" + item.CourseName + "</td>" +
                 "<td>" + item.Credits + "</td>" +
                 "<td>" + item.ClassType + "</td>" +
                 "<td>" + item.CourseType + "</td>" +
                 "<td>" + item.BeginsAt + "</td>" +
                 "<td>" + item.EndsAt + "</td>" +
                 "<td>" + item.ThemeColor + "</td>" +
                 "<td>" + item.RoomName + "</td>" +
                  "<td>" + item.Campus + "</td>" +

              "</tr>" +
              "</table>";
            }
            //YourTable.Append(textBody);
            string sub = "timetable";
            var body = textBody;
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(senderEmail.Address, password)
            };
            using (var mess = new MailMessage(senderEmail, receiverEmail)
            {

                IsBodyHtml = true,
                Subject = "Your Selected TimeTable is",
                Body = body
            })
            {
                smtp.Send(mess);
            }


          

            return View(list);
        }
        public ActionResult ShowCourseSelection()
        {


            var student = objDb.Students.ToList();

            
            ViewBag.course = new SelectList(student, "StudentID", "APNo");
            return View();
        }
        [HttpPost]
        public ActionResult ShowCourseSelection(Student s)
        {

            
            return RedirectToAction("ShowCourseLists", new { id = s.StudentID });
        }

        //public ActionResult  ShowCourseList(int id)
        //{
        //    var list = objDb.TimeTables.Where(s => s.StudentId == id).ToList();

        //    return View(list);
        //}

        public ActionResult ShowCourseLists(int id)
        {
            var list = objDb.TimeTables.Where(s => s.StudentId == id).ToList();

            return View(list);
        }

        //[HttpPost]
        //public ActionResult ShowCourseSelection(tblEnrollStudentInCourseValidation testing)
        //{
        //    try
        //    {
        //        int studentid = Convert.ToInt32(Session["Student"]);
        //        int tempclassid;
        //        int originalclassid;
        //        string Regno;
        //        var getstudentid = db.Students.Find(studentid);
        //        tempclassid = getstudentid.Class_Id;
        //        Regno = getstudentid.RegNo;
        //        var classid = db.Tbl_Class.Where(x => x.Class_Id == tempclassid).SingleOrDefault();
        //        originalclassid = classid.Class_Id;
        //        //AB enroll course wala dropdown ajayega 
        //        CourseDBHandle gc = new CourseDBHandle();

        //        List<tblEnrollStudentInCourseValidation> list = gc.GetSudentEnrollCourse(Regno, originalclassid);
        //        ViewBag.course = new SelectList(list, "courseId", "courseName");


        //        //var data = db.SchoolAssignments.ToList();
        //        return RedirectToAction("viewHomeWork", new { courseid = testing.courseId });
        //    }
        //    catch (Exception ex)
        //    {
        //        ViewBag.Message = "Please try later";
        //        return View();
        //    }


    }
}