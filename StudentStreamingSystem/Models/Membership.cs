using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace StudentStreamingSystem.Models
{
    public class Membership
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string password { get; set; }
        public int APNo_StudentID { get; set; }
    }
}