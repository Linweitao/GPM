using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTest.Models
{
    public class TopicInformation
    {
        public string TopicID { get; set; }
        public string TeacherID { get; set; }
        public string TopicName { get; set; }
        public string TopicInfo { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public int ProIfPassed { get; set; }
    }
}