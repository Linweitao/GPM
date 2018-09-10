using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTest.Models
{
    public class Material
    {
        public string StudentID { get; set; }
        public string TopicID { get; set; }
        public string MaterialFile { get; set; }
        public string CheckGroupID { get; set; }
        public string Advice { get; set; }
        public int TaskID { get; set; }
        public int Week { get; set; }
    }
}