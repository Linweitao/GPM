using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTest.Models
{
    public class Student
    {
        public string UserID { get; set; }
        public string TeaID { get; set; }
        public string TopicID { get; set; }
        public int Year { get; set; }
        public int Grade { get; set; }
        public string GroupID { get; set; }
        public int IsPassed { get; set; }
        public string Academy { get; set; }
        public string CheckGroupID { get; set; }

    }
}