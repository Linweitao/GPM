using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTest.Models
{
    public class TopicState
    {
        public string TopicID { get; set; }
        public string TeacherID { get; set; }
        public string StudentID { get; set; }
        public int StuIsPassed { get; set; }
    }
}