using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebTest.Models
{
    //教师在查看学生选题情况时的视图
    public class TeaStuChoose
    {
        public string TopicID { get; set; }
        public string TopicName { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public int StuIsPassed { get; set; }
        public string TeacherID { get; set; }
    }
    //学生情况
    public class TeaStuInfo
    {
        public string TopicID { get; set; }
        public string TopicName { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string TeacherID { get; set; }
        public string TeacherName { get; set; }
        public int StudentSex { get; set; }
        public string StudentEmail { get; set; }
        public string StudentMobile { get; set; }
        public int IsPassed { get; set; }
        public string GroupID { get; set; }
        public string CheckGroupID { get; set; }
    }
    //答辩组信息及教师成员信息
    public class DefenseGroup
    {
        public string TeacherID { get; set; }
        public string TeacherName { get; set; }
        public string TeacherEmail { get; set; }
        public string TeacherMobile { get; set; }
        public string AdminTeaID { get; set; }
        public DateTime GroupTime { get; set; }
        public string GroupAddress { get; set; }
        public string GroupID { get; set; }
    }

    public class StuMaterial
    {
        public int TaskID { get; set; }
        public string CheckGroupID { get; set; }
        public string StudentID { get; set; }
        public string StudentName { get; set; }
        public string TopicID { get; set; }
        public string TopicName { get; set; }
        public string MaterialFile { get; set; }
        public string Advice { get; set; }
        public int Week { get; set; }
    }
    public class Stu_CGIsPassed
    {
        public string UserID { get; set; }
        public string CheckGroupID { get; set; }

        public int IsPassed { get; set; }
    }

    public class Stu_InGroup
    {
        public string UserID { get; set; }
        public string GroupID { get; set; }
        public string AdminTeaID { get; set; }

        public DateTime GroupTime { get; set; }
        public string GroupAddress { get; set; }

        public int DefenseScore { get; set; }
        public string DefenseComment { get; set; }
    }
}