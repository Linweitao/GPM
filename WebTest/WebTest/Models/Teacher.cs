using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTest.Models
{
    public class Teacher
    {
        public string UserID { get; set; }
        public int IsCreateRight { get; set; }
        public string GroupID { get; set; }
        public string CheckGroupID { get; set; }

    }
}