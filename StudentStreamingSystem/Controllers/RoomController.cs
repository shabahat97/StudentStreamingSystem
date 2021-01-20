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
    public class RoomController : Controller
    {
        StudentStreamingDBEntities objDb = new StudentStreamingDBEntities();
        // GET: Room
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public ActionResult Create(Room room)
        {
            if (ModelState.IsValid)
            {
                Room obj = new Room();
                obj.RoomID = room.RoomID;
                obj.Name = room.Name;
                obj.Capacity = room.Capacity;
                objDb.Rooms.Add(obj);
                objDb.SaveChanges();
                return RedirectToAction("ViewRooms");
            }
            return View();
        }
        public ActionResult ViewRooms()
        {
            var res = objDb.Rooms.ToList();
            return View(res);
        }
        public ActionResult DeleteRooms(int id)
        {
            var res = objDb.Rooms.Where(x => x.RoomID == id).First();
            objDb.Rooms.Remove(res);
            objDb.SaveChanges();
            var list = objDb.Rooms.ToList();
            return View("ViewRooms",list);
        }
        public ActionResult EditRooms(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Room room = objDb.Rooms.Find(id);
            if (room == null)
            {
                return HttpNotFound();
            }
            return View(room);
        }
        [HttpPost]
        public ActionResult EditRooms(Room room)
        {
            if (ModelState.IsValid)
            {
                Room obj = new Room();
                obj.RoomID = room.RoomID;
                obj.Name = room.Name;
                obj.Capacity = room.Capacity;
                objDb.Entry(obj).State = EntityState.Modified;
                objDb.SaveChanges();
                return RedirectToAction("ViewRooms");
            }
            return View(room);
        }

    }
}