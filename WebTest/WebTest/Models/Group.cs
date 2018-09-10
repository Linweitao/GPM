using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTest.Models
{
    public class Group
    {
        public string GroupID { get; set; }
        public string AdminTeaID { get; set; }
        public DateTime GroupTime { get; set; }
        public string GroupAddress { get; set; }
    }
}