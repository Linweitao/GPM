using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTest.Models
{
    public class User
    {
        public string UserID { get; set; }
        public string UserPassWord  { get; set; }
        public string UserName { get; set; }
        public int UserSex { get; set; }
        public string UserEmail { get; set; }
        public string UserMobile { get; set; }
        public int Type { get; set; }

    }
}