using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTest.Models
{
    public class Grade
    {
        public string UserID { get; set; }
        public int Score { get; set; }
        public int QualityControlGrade { get; set; }
        public int SoftwareGrade { get; set; }
        public int DefenseScore { get; set; }
        public string DefenseComment { get; set; }
    }
}