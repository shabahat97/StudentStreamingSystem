﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Data.OleDb;
using System.IO;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using LinqToExcel;
using StudentStreamingSystem.Context;

namespace StudentStreamingSystem.Controllers
{
    public class CoursesController : Controller
    {
        StudentStreamingDBEntities db = new StudentStreamingDBEntities();
        // GET: Course
        public ActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public JsonResult UploadExcelCourse(HttpPostedFileBase FileUpload)
        {

            List<string> data = new List<string>();
            if (FileUpload != null)
            {
                // tdata.ExecuteCommand("truncate table OtherCompanyAssets");  
                if (FileUpload.ContentType == "application/vnd.ms-excel" || FileUpload.ContentType == "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                {


                    string filename = FileUpload.FileName;
                    string targetpath = Server.MapPath("~/Doc/");
                    FileUpload.SaveAs(targetpath + filename);
                    string pathToExcelFile = targetpath + filename;
                    var connectionString = "";
                    if (filename.EndsWith(".xls"))
                    {
                        connectionString = string.Format("Provider=Microsoft.Jet.OLEDB.4.0; data source={0}; Extended Properties=Excel 8.0;", pathToExcelFile);
                    }
                    else if (filename.EndsWith(".xlsx"))
                    {
                        connectionString = string.Format("Provider=Microsoft.ACE.OLEDB.12.0;Data Source={0};Extended Properties=\"Excel 12.0 Xml;HDR=YES;IMEX=1\";", pathToExcelFile);
                    }

                    var adapter = new OleDbDataAdapter("SELECT * FROM [Sheet1$]", connectionString);
                    var ds = new DataSet();

                    adapter.Fill(ds, "ExcelTable");

                    DataTable dtable = ds.Tables["ExcelTable"];

                    string sheetName = "Sheet1";

                    var excelFile = new ExcelQueryFactory(pathToExcelFile);
                    var listCourses = from c in excelFile.Worksheet<Course>(sheetName) select c;

                  //  BEGIN - Clearing up the existing data in the table before Inserting the records.
                    StudentStreamingDBEntities cleanTableEntities = new StudentStreamingDBEntities();
                    var objContext = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)cleanTableEntities).ObjectContext;
                    objContext.ExecuteStoreCommand("Truncate table Course");
                    //  END
                    var list = db.Courses.ToList();
                    db.Courses.RemoveRange(list);
                    foreach (var c in listCourses)
                    {
                        try
                        {
                          

                            //if (a.Name != "" && a.Address != "" && a.ContactNo != "")
                            if (c.CourseName != "")
                            {

                                //User TU = new User();
                                Course TC = new Course();
                                TC.CourseNo = c.CourseNo;
                                TC.CourseName = c.CourseName;
                                TC.Programme = c.Programme;
                                TC.Campus = c.Campus;
                                TC.Credits = c.Credits;
                                TC.RoomName = c.RoomName;
                                TC.Streams = c.Streams;
                                TC.ClassType = c.ClassType;
                                TC.CourseType = c.CourseType;
                                if (c.CourseType == "Compulsory")
                                {
                                    TC.ThemeColor = "green";
                                }
                                if (c.CourseType == "Optional")
                                {
                                    TC.ThemeColor = "orange";
                                }

                                TC.BeginsAt = c.BeginsAt;
                                TC.EndsAt = c.EndsAt;
                                db.SaveChanges();
                                db.Courses.Add(TC);
                                db.SaveChanges();
                            }
                            else
                            {
                                return Json(data, JsonRequestBehavior.AllowGet);
                            }
                        }

                        catch (DbEntityValidationException ex)
                        {

                        }
                    }
                    //deleting excel file from folder  
                    if ((System.IO.File.Exists(pathToExcelFile)))
                    {
                        System.IO.File.Delete(pathToExcelFile);
                    }
                    return Json("success", JsonRequestBehavior.AllowGet);
                }
                else
                {
                    //alert message for invalid file format  
                    data.Add("<ul>");
                    data.Add("<li>Only Excel file format is allowed</li>");
                    data.Add("</ul>");
                    data.ToArray();
                    return Json(data, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                data.Add("<ul>");
                if (FileUpload == null) data.Add("<li>Please choose Excel file</li>");
                data.Add("</ul>");
                data.ToArray();
                return Json(data, JsonRequestBehavior.AllowGet);
            }
        }
        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
        public ActionResult GetList()
        {

            using (StudentStreamingDBEntities db = new StudentStreamingDBEntities())
            {

                var courselist = db.Courses.ToList<Course>();
                return Json(new { data = courselist }, JsonRequestBehavior.AllowGet);
            }

        }

    }
}