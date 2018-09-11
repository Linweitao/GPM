using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebTest.Models;

namespace WebTest.Controllers
{
    public class PageController : Controller
    {
        RskllDB rsklldb = new RskllDB();
        public static string UserID;
        public static CheckGroup CGObj;
        public static Group GObj;
        // GET: Page
        public ActionResult Login()
        {
            return View("Login");
        }
        public ActionResult Main()
        {
            return View("Main");
        }
        public ActionResult Nav()
        {
            return View("Nav");
        }
        public ActionResult Nav_director()
        {
            return View("Nav_director");
        }
        public ActionResult Nav_manager()
        {
            return View("Nav_manager");
        }
        public ActionResult Nav_professor()
        {
            return View("Nav_professor");
        }
        public ActionResult Nav_teacher()
        {
            return View("Nav_teacher");
        }
        //登录功能
        public ActionResult LoginToMain(string userid,string password)
        {
            rsklldb.OpenConnection();
            string sql = "select * from [User] where UserID = '"+userid+"'";
            List<User> list = rsklldb.Detail<User>(sql);

            if (list.Count == 0 || list[0].UserPassWord != password)
            {
                Response.Write("<script>alert('账号或密码错误!')</script>");
                return Login();
            }
            else
            {
                Session["UserID"] = list[0].UserID;
                Session["UserName"] = list[0].UserName;
                //根据用户type选择对应用户界面
                switch (list[0].Type)
                {
                    case 0:@ViewData["type"] = "Nav"; break;
                    case 1:@ViewData["type"] = "Nav_teacher"; break;
                    case 2:@ViewData["type"] = "Nav_professor"; break;
                    case 3:@ViewData["type"] = "Nav_director"; break;
                    case 4:@ViewData["type"] = "Nav_manager"; break;
                }
                return View("Main");
            }

        }


        public ActionResult test()
        {
            ViewBag.yes = "look";
            ViewBag.id = Session["UserID"];
            return View("test");
        }


        //林伟涛2018.9.5写教师功能
        //提交选题功能
        public ActionResult TeaTopicInsertPage()
        {
            return View("TeaTopicInsertPage");
        }
        public ActionResult TeaDoTopicInsert(string topicname,string topicid,string starttime,string endtime,string topicinfo,string teacherid)
        {
            try
            {
                if(topicname==""||topicid== "" || starttime== "" || endtime== "")
                {
                    Response.Write("<script>alert('输入信息不完整!')</script>");
                    return View("TeaTopicInsertPage");
                }

                rsklldb.OpenConnection();
                string sql0 = "SELECT * FROM [TopicInformation] WHERE TopicID = '" + topicid + "'";
                List<TopicInformation> list = rsklldb.Detail<TopicInformation>(sql0);
                if (list.Count == 0)
                {
                    string sql = @"INSERT INTO [dbo].[TopicInformation]([TopicID] ,[TeacherID],[TopicName] ,[TopicInfo] ,[StartTime],[EndTime],[ProIfPassed])
                    VALUES('" + topicid + "','" + teacherid + "','" + topicname + "','" + topicinfo + "','" + starttime + "','" + endtime + "',0)";
                    rsklldb.InsertData(sql);
                    rsklldb.CloseConnection();
                    Response.Write("<script>alert('插入选题成功')</script>");
                    return View("TeaTopicInsertPage");
                }
                else
                {
                    Response.Write("<script>alert('选题编号重复!')</script>");
                    return View("TeaTopicInsertPage");
                }

            }
            catch(Exception ex)
            {
                Response.Write("<script>alert('有错误!')</script>");
                return View("TeaTopicInsertPage");
            }

        }

        //查看选题审核功能
        public ActionResult TeaCheckTopic()
        {
            rsklldb.OpenConnection();
            string sql = "SELECT * FROM [TopicInformation] WHERE TeacherID = '"+ Session["UserID"] + "'";
            ViewBag.dt = rsklldb.Detail<TopicInformation>(sql);
            rsklldb.CloseConnection();
            return View("TeaCheckTopic");
        }
        public ActionResult TeaCheckTopicDelete(string topicid)
        {
            rsklldb.OpenConnection();
            string sql = "delete FROM [TopicInformation] where TopicID = '"+topicid+"'";
            ViewBag.dt = rsklldb.Delete(sql);
            rsklldb.CloseConnection();
            return TeaCheckTopic();
        }
        public ActionResult TeaCheckTopicUpdatePage(string topicid)
        {
            rsklldb.OpenConnection();
            string sql = "SELECT * FROM [TopicInformation] where TopicID = '" + topicid + "'";
            ViewBag.dt = rsklldb.Detail<TopicInformation> (sql)[0];
            rsklldb.CloseConnection();
            return View("TeaCheckTopicUpdatePage");
        }
        public ActionResult TeaDoTopicUpdate(string topicname, string topicid, string starttime, string endtime, string topicinfo, string teacherid)
        {
            try
            {
                if (topicname == "" || topicid == "" || starttime == "" || endtime == "")
                {
                    Response.Write("<script>alert('更新信息不足!')</script>");
                    return TeaCheckTopicUpdatePage(topicid);
                }
                rsklldb.OpenConnection();
                string sql = @"UPDATE [dbo].[TopicInformation] SET [TeacherID] = '"+teacherid+ "' ,[TopicName] = '"+topicname+"' ,[TopicInfo] = '"+topicinfo+"' ,[StartTime] = '"+starttime+"',[EndTime] = '"+endtime+"' ,[ProIfPassed] = 0 WHERE TopicID ="+topicid;
                rsklldb.InsertData(sql);
                rsklldb.CloseConnection();
                return TeaCheckTopic();

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('有错误!')</script>");
                return TeaCheckTopicUpdatePage(topicid);
            }

        }
        public ActionResult TeaCheckTopicSearch(string flag)
        {
            rsklldb.OpenConnection();
            if(flag == null || flag.Equals("6"))
            {
                return TeaCheckTopic();
            }
            else
            {
                string sql = "SELECT * FROM [TopicInformation] WHERE TeacherID = '" + Session["UserID"] + "' AND ProIfPassed = " + flag;
                ViewBag.dt = rsklldb.Detail<TopicInformation>(sql);
                rsklldb.CloseConnection();
                return View("TeaCheckTopic");
            }
        }

        //查看学生选题情况
        public ActionResult TeaStuTopicState()
        {
            rsklldb.OpenConnection();
            string sql = "SELECT * FROM [TeaStuChoose] WHERE TeacherID = '" + Session["UserID"]+"'";
            ViewBag.dt = rsklldb.Detail<TeaStuChoose>(sql);
            rsklldb.CloseConnection();
            return View("TeaStuTopicState");
        }
        public ActionResult TeaStuTopicPass(string topicid,string topicname,string teacherid,string studentid,int pass)
        {
            rsklldb.OpenConnection();
            string sql1 = "UPDATE [TopicState] SET [StuIsPassed] = "+pass+"WHERE StudentID ='"+studentid + "'";
            rsklldb.InsertData(sql1);
            if (pass == 1)
            {
                string sql2 = "SELECT * FROM CheckGroup WHERE CheckGroupName = '" + topicname + "'";
                CheckGroup cg = rsklldb.Detail<CheckGroup>(sql2)[0];
                string sql3 = "UPDATE [Student] SET [TeaID] = '" + teacherid +"',TopicID = '"+topicid+"',GroupID= '"+cg.CheckGroupID+"',CheckGroupID = '"+cg.CheckGroupID+ "' WHERE UserID = '" + studentid + "'";
                rsklldb.InsertData(sql3);
            }
            rsklldb.CloseConnection();
            return TeaStuTopicState();
        }
        public ActionResult TeaStuTopicSearch(string flag)
        {
            rsklldb.OpenConnection();
            if (flag==null||flag.Equals("6"))
            {
                return TeaStuTopicState();
            }
            else
            {
                string sql = "SELECT * FROM [TeaStuChoose] WHERE TeacherID = '" + Session["UserID"] + "' AND StuIsPassed = " + flag;
                ViewBag.dt = rsklldb.Detail<TeaStuChoose>(sql);
                rsklldb.CloseConnection();
                return View("TeaStuTopicState");
            }
        }
        //学生列表（即教师界面的固定页）
        public ActionResult TeaStuList()
        {
            rsklldb.OpenConnection();
            string sql = "SELECT * FROM TeaStuInfo WHERE TeacherID = '" + Session["UserID"] + "'";
            ViewBag.dt = rsklldb.Detail<TeaStuInfo>(sql);
            rsklldb.CloseConnection();
            return View("TeaStuList");
        }
        //开题报告页面
        public ActionResult TeaKTpasspage()
        {
            rsklldb.OpenConnection();
            string sql = "SELECT * FROM TeaStuInfo WHERE TeacherID = '" + Session["UserID"] + "'";
            ViewBag.dt = rsklldb.Detail<TeaStuInfo>(sql);
            rsklldb.CloseConnection();
            return View("TeaKTpasspage");
        }
        //开题通过操作
        public ActionResult TeaKTpass(int pass,string studentid)
        {
            rsklldb.OpenConnection();
            string sql = "UPDATE Student SET IsPassed = "+pass+" WHERE UserID = '" + studentid + "'";
            ViewBag.dt = rsklldb.InsertData(sql);
            rsklldb.CloseConnection();
            return TeaKTpasspage();
        }
        //所属学生开题通过情况查询
        public ActionResult TeaKTpassSearch(string flag)
        {
            rsklldb.OpenConnection();
            if (flag == null || flag.Equals("6"))
            {
                return TeaKTpasspage();
            }
            else
            {
                string sql = "SELECT * FROM [TeaStuInfo] WHERE TeacherID = '" + Session["UserID"] + "' AND IsPassed = " + flag;
                ViewBag.dt = rsklldb.Detail<TeaStuInfo>(sql);
                rsklldb.CloseConnection();
                return View("TeaKTpasspage");
            }
        }
        //答辩组信息
        public ActionResult TeaDefenceGroup()
        {
            rsklldb.OpenConnection();
            string sql = "SELECT * FROM [DefenseGroup] WHERE TeacherID = '" + Session["UserID"]+"'";
            ViewBag.dt = rsklldb.Detail<DefenseGroup>(sql);
            ViewBag.IfInGroup = rsklldb.Detail<DefenseGroup>(sql).Count;
            rsklldb.CloseConnection();
            return View("TeaDefenceGroup");
        }
        //答辩评审结果录入
        public ActionResult TeaDefenceResult()
        {
            rsklldb.OpenConnection();
            string sql1 = "SELECT * FROM [Teacher] WHERE UserID = '" + Session["UserID"]+"'";
            Teacher tea = rsklldb.Detail<Teacher>(sql1)[0];
            if (tea.GroupID == null|| tea.GroupID.Equals(""))
            {
                return TeaDefenceGroup();
            }
            else
            {
                string groupid = tea.GroupID;
                string sql2 = "SELECT * FROM [TeaStuInfo] WHERE GroupID = '" + groupid + "'";
                ViewBag.dt = rsklldb.Detail<TeaStuInfo>(sql2);
            }
            rsklldb.CloseConnection();
            return View("TeaDefenceResult");
        }
        //答辩评审结果查询
        public ActionResult TeaDefenceResultSearch(string studentid)
        {
            rsklldb.OpenConnection();
            string sql1 = "SELECT * FROM [Teacher] WHERE UserID = '" + Session["UserID"] + "'";
            Teacher tea = rsklldb.Detail<Teacher>(sql1)[0];
            string groupid = tea.GroupID;
            string sql2 = "SELECT * FROM [TeaStuInfo] WHERE GroupID = '" + groupid +"' AND StudentID = '"+ studentid + "'";
            ViewBag.dt = rsklldb.Detail<TeaStuInfo>(sql2);
            rsklldb.CloseConnection();
            return View("TeaDefenceResult");
        }
        //成绩录入页面
        public ActionResult TeaInsertGradePage(string studentid)
        {
            rsklldb.OpenConnection();
            string sql = "SELECT * FROM [Grade] WHERE UserID = '" + studentid + "'";
            ViewBag.dt = rsklldb.Detail<Grade>(sql)[0];
            rsklldb.CloseConnection();
            return View("TeaInsertGradePage");
        }
        //成绩录入操作
        public ActionResult TeaInsertGrade(string studentid,string qg,string sg,string ds,string co)
        {
            rsklldb.OpenConnection();
            string sql = "UPDATE Grade SET [QualityControlGrade] = " + qg+ ",[SoftwareGrade] = "+sg+",[DefenseScore] = "+ds+ ",[DefenseComment] = '" + co + "' WHERE UserID = '" + studentid + "'";
            rsklldb.InsertData(sql);
            rsklldb.CloseConnection();
            return TeaInsertGradePage(studentid);
        }
        public ActionResult TeaCreateGroup()
        {
            rsklldb.OpenConnection();
            string sql = "SELECT * FROM [Teacher] WHERE UserID = '" + Session["UserID"] + "'";
            Teacher tea = rsklldb.Detail<Teacher>(sql)[0];
            if (tea.IsCreateRight == 1)
            {
                return Prof_CreateGroup();
            }
            else return View("TeaCreateGroup");
        }
        //材料
        public ActionResult TeaMaterialList()
        {
            rsklldb.OpenConnection();
            string sql = "SELECT * FROM [Teacher] WHERE UserID = '" + Session["UserID"] + "'";
            Teacher tea = rsklldb.Detail<Teacher>(sql)[0];
            string checkgroupid = tea.CheckGroupID;
            if (checkgroupid == null || checkgroupid == "")
            {
                rsklldb.CloseConnection();
                ViewBag.flag = 0;
                return View("TeaMaterialList");
            }
            else
            {
                ViewBag.flag = 1;
                string sql2 = "SELECT * FROM [TeaStuInfo] WHERE CheckGroupID = '" + checkgroupid + "'";
                ViewBag.dt = rsklldb.Detail<TeaStuInfo>(sql2);
                rsklldb.CloseConnection();
                return View("TeaMaterialList");
            }
        }
        //指定学生材料搜寻
        public ActionResult TeaMaterialSearch(string studentid)
        {
            rsklldb.OpenConnection();
            string sql = "SELECT * FROM [TeaStuInfo] WHERE StudentID = '" + studentid + "'";
            ViewBag.dt = rsklldb.Detail<TeaStuInfo>(sql);
            rsklldb.CloseConnection();
            return View("TeaMaterialList");
        }
        //材料分类列表
        public ActionResult TeaMateriallook(string studentid)
        {
            rsklldb.OpenConnection();
            string sql1 = "SELECT * FROM [StuMaterial] WHERE StudentID = '" + studentid + "' AND TaskID = 1";
            ViewBag.dt1 = rsklldb.Detail<StuMaterial>(sql1);
            string sql2 = "SELECT * FROM [StuMaterial] WHERE StudentID = '" + studentid + "' AND TaskID = 2";
            ViewBag.dt2 = rsklldb.Detail<StuMaterial>(sql2);
            string sql3 = "SELECT * FROM [StuMaterial] WHERE StudentID = '" + studentid + "' AND TaskID = 3";
            ViewBag.dt3 = rsklldb.Detail<StuMaterial>(sql3);
            rsklldb.CloseConnection();
            return View("TeaMateriallook");
        }
        //下载文件
        public ActionResult TeaDownload(string filename)
        {
            string filePath = Server.MapPath("~/Content/file/"+ filename);
            FileStream fs = new FileStream(filePath, FileMode.Open);
            return File(fs, "text/plain", filename);
        }
        //材料审核
        public ActionResult TeaMaterialReview(string filename)
        {
            rsklldb.OpenConnection();
            string sq = "SELECT * FROM [StuMaterial] WHERE MaterialFile = '" + filename+"'";
            ViewBag.dt = rsklldb.Detail<StuMaterial>(sq)[0];
            rsklldb.CloseConnection();
            return View("TeaMaterialReview");
        }
        public ActionResult TeaMaterialAdvice(string filename,string advice)
        {
            rsklldb.OpenConnection();
            string sql = "UPDATE Material SET Advice = '" + advice+ "' WHERE MaterialFile = '" + filename + "'";
            ViewBag.dt = rsklldb.InsertData(sql);
            rsklldb.CloseConnection();
            return TeaMaterialReview(filename);
        }



        /// <summary>
        /// 南伽2018.9.8合并
        /// </summary>
        /// <returns></returns>
        public ActionResult SelectManager() //页面加载
        {
            rsklldb.OpenConnection();
            string sql = "select * from [User] where Type = 4";
            List<User> list = rsklldb.Detail<User>(sql);
            @ViewBag.dt = list;
            return View("SelectManager");
        }
        public ActionResult SelectUser()
        {
            rsklldb.OpenConnection();
            string sql = "select * from [User] where  Type<>4 ORDER BY Type DESC";
            List<User> list = rsklldb.Detail<User>(sql);
            @ViewBag.dt = list;
            return View("SelectUser");
        }
        public ActionResult AddUser()
        {
            @ViewBag.deal = "确认添加";
            return View("AddUser");
        }
        public ActionResult AddManager()
        {
            @ViewBag.deal = "确认添加";
            return View("AddManager");
        }
        public ActionResult DeleteUser(string ID) //删除各种用户
        {
            rsklldb.OpenConnection();
            string sql1 = "select * from [User] where UserID= '" + ID + "'";
            List<User> list = rsklldb.Detail<User>(sql1);
            string sql = "delete from [User] where UserID = '" + ID + "'";
            rsklldb.InsertData(sql);
            if (list[0].Type == 4) return SelectManager();
            else return SelectUser();
        }
        public ActionResult SelectU(string userid, string all) //查找管理员
        {
            if (all == "刷新/显示全部") userid = "";
            rsklldb.OpenConnection();
            string sql = "select * from [User] where UserID like '%" + userid + "%'and Type=4";
            List<User> list = rsklldb.Detail<User>(sql);
            @ViewBag.dt = list;
            return View("SelectManager");
        }
        public ActionResult SelectU1(string userid, string all) //查找普通用户
        {
            if (all == "刷新/显示全部") userid = "";
            rsklldb.OpenConnection();
            string sql = "select * from [User] where UserID like '%" + userid + "%'and Type<>4 ORDER BY Type DESC";
            List<User> list = rsklldb.Detail<User>(sql);
            @ViewBag.dt = list;
            return View("SelectUser");
        }
        public ActionResult InsertUser(User Obj) //添加各种用户
        {
            if (Obj.UserID == null || Obj.UserName == null || Obj.UserPassWord == null)
            {
                Response.Write("<script>alert('信息填写不全!')</script>");
                return AddUser();
            }

            rsklldb.OpenConnection();
            string sql2 = "select * from [User] where UserID = '" + Obj.UserID + "'";
            List<User> list = rsklldb.Detail<User>(sql2);
            string sql1 = "delete from [User] where UserID = '" + Obj.UserID + "'";
            rsklldb.InsertData(sql1);
            string sql = "insert into [User]  (UserID,UserPassWord,UserName,UserSex,UserEmail,UserMobile,[Type]) values('" + Obj.UserID + "','" + Obj.UserPassWord + "','" + Obj.UserName + "','" + Obj.UserSex + "','" + Obj.UserEmail + "','" + Obj.UserMobile + "','" + Obj.Type + "')";
            if (Obj.Type == 0)
            {
                string sql4 = "insert into [Student]  (UserID) values('" + Obj.UserID + "')";
                rsklldb.InsertData(sql4);
                string sql5 = "insert into [Grade]  (UserID) values('" + Obj.UserID + "')";
                rsklldb.InsertData(sql5);
            }
            else if (Obj.Type == 1)
            {
                string sql3 = "insert into [Teacher]  (UserID,IsCreateRight,GroupID,CheckGroupID) values('" + Obj.UserID + "',0,'','')";
                rsklldb.InsertData(sql3);
            }
            rsklldb.InsertData(sql);
            if (list.Count != 0)
            {
                if (list[0].Type == 4) return SelectManager();
                else return SelectUser();
            }
            else
            {
                if (Obj.Type == 4) return AddManager();
                else return AddUser();
            }
        }

        public ActionResult UpdateUser(string ID) //修改各种用户
        {
            @ViewData["UserID"] = ID;
            rsklldb.OpenConnection();
            string sql = "select * from [User] where UserID = '" + ID + "'";
            List<User> list = rsklldb.Detail<User>(sql);
            @ViewData["UserName"] = list[0].UserName;
            @ViewData["UserEmail"] = list[0].UserEmail;
            @ViewData["UserMobile"] = list[0].UserMobile;
            @ViewData["UserPassWord"] = list[0].UserPassWord;
            @ViewData["Type"] = list[0].Type;
            @ViewBag.deal = "确认修改";
            return View("AddUser");
        }


        //教学干事功能
        public ActionResult DirectorScore() //教学干事查找分数列表
        {
            rsklldb.OpenConnection();
            string sql = "select * from [Grade]";
            List<Grade> list = rsklldb.Detail<Grade>(sql);
            @ViewBag.sc = list;
            return View("DirectorScore");
        }

        public ActionResult Dir_SelectScore(string userid, string all) //教学干事查找某学生分数
        {
            if (all == "刷新/显示全部") userid = "";
            rsklldb.OpenConnection();
            string sql = "select * from [Grade] where UserID like '%" + userid + "%'";
            List<Grade> list = rsklldb.Detail<Grade>(sql);
            @ViewBag.sc = list;
            return View("DirectorScore");
        }
        public ActionResult CountWay()
        { return View("CountWay"); }

        public ActionResult Dir_CountGrade(string QualityControlGrade, string SoftwareGrade, string DefenseScore) //修改成绩计算方式
        {
            try
            {
                int Q = int.Parse(QualityControlGrade);
                int S = int.Parse(SoftwareGrade);
                int D = int.Parse(DefenseScore);
                rsklldb.OpenConnection();
                string sql = "select * from [Grade]";
                List<Grade> list = rsklldb.Detail<Grade>(sql);
                for (int i = 0; i < list.Count; i++)
                {
                    int q = list[i].QualityControlGrade;
                    int s = list[i].SoftwareGrade;
                    int d = list[i].DefenseScore;
                    list[i].Score = (q * Q + s * S + d * D) / (Q + S + D);
                    string sql1 = "update [Grade] set Score=" + list[i].Score + "where UserID='" + list[i].UserID + "'";
                    rsklldb.InsertData(sql1);
                }
                @ViewData["error"] = "更新成功!";
                return CountWay();
            }
            catch (Exception ex)
            {
                @ViewData["error"] = "输入数据有误，请重新输入";
                return CountWay();
            }


        }

        //教授答辩管理—教师授权
        public ActionResult Prof_permission() //责任教授查看所有指导教师
        {
            rsklldb.OpenConnection();
            string sql = "select * from [Teacher]";
            List<Teacher> list = rsklldb.Detail<Teacher>(sql);
            @ViewBag.tea = list;
            return View("Prof_permission");
        }

        public ActionResult Prof_SelectTeacher(string userid, string all) //责任教授查找指导教师
        {
            if (all == "刷新/显示全部") userid = "";
            rsklldb.OpenConnection();
            string sql = "select * from [Teacher] where UserID like '%" + userid + "%'";
            List<Teacher> list = rsklldb.Detail<Teacher>(sql);
            @ViewBag.tea = list;
            return View("Prof_permission");
        }
        public ActionResult Prof_CreateRight(string ID) //授权
        {
            rsklldb.OpenConnection();
            string sql1 = "update [Teacher] set IsCreateRight=1 where UserID='" + ID + "'";
            rsklldb.InsertData(sql1);
            return Prof_permission();
        }

        public ActionResult Prof_SaveRight(string ID) //收回权限
        {
            rsklldb.OpenConnection();
            string sql1 = "update [Teacher] set IsCreateRight=0 where UserID='" + ID + "'";
            rsklldb.InsertData(sql1);
            return Prof_permission();
        }

        //选题管理
        public ActionResult Prof_TopicInsertPage()
        {
            return View("Prof_TopicInsertPage");
        }
        //教授选题录入
        public ActionResult Prof_DoTopicInsert(string topicname, string topicid, string starttime, string endtime, string topicinfo, string teacherid)
        {
            try
            {
                if (topicname == "" || topicid == "" || starttime == "" || endtime == "")
                {
                    Response.Write("<script>alert('输入信息不完整!')</script>");
                    return View("Prof_DoTopicInsert");
                }

                rsklldb.OpenConnection();
                string sql0 = "SELECT * FROM [TopicInformation] WHERE TopicID = '" + topicid + "'";
                List<TopicInformation> list = rsklldb.Detail<TopicInformation>(sql0);
                if (list.Count == 0)
                {
                    string sql = @"INSERT INTO [dbo].[TopicInformation]([TopicID] ,[TeacherID],[TopicName] ,[TopicInfo] ,[StartTime],[EndTime],[ProIfPassed])
                    VALUES('" + topicid + "','" + teacherid + "','" + topicname + "','" + topicinfo + "','" + starttime + "','" + endtime + "',1)";
                    rsklldb.InsertData(sql);
                    rsklldb.CloseConnection();
                    Response.Write("<script>alert('插入选题成功!')</script>");
                    return View("Prof_TopicInsertPage");
                }
                else
                {
                    Response.Write("<script>alert('选题ID重复!')</script>");
                    return View("Prof_DoTopicInsert");
                }

            }
            catch (Exception ex)
            {
                Response.Write("<script>alert('出现错误!')</script>");
                return View("Prof_TopicInsertPage");
            }

        }

        public ActionResult Prof_TopicCheck()
        {
            rsklldb.OpenConnection();
            string sql = "select * from [TopicInformation]";
            List<TopicInformation> list = rsklldb.Detail<TopicInformation>(sql);
            @ViewBag.top = list;
            return View("Prof_TopicCheck");
        }

        public ActionResult Prof_SelectTopic(string userid, string all)//责任教授查找主题
        {
            if (all == "刷新/显示全部") userid = "";
            rsklldb.OpenConnection();
            string sql = "select * from [TopicInformation] where TopicID like '%" + userid + "%'";
            List<TopicInformation> list = rsklldb.Detail<TopicInformation>(sql);
            @ViewBag.top = list;
            return View("Prof_TopicCheck");
        }
        public ActionResult Prof_TopicCreateRight(string ID) //选题通过
        {
            rsklldb.OpenConnection();
            string sql1 = "update [TopicInformation] set ProIfPassed=1 where TopicID='" + ID + "'";
            rsklldb.InsertData(sql1);
            return Prof_TopicCheck();
        }

        public ActionResult Prof_TopicSaveRight(string ID) //选题不通过
        {
            rsklldb.OpenConnection();
            string sql1 = "update [TopicInformation] set ProIfPassed=2 where TopicID='" + ID + "'";
            rsklldb.InsertData(sql1);
            return Prof_TopicCheck();
        }
        //教授开题管理
        public ActionResult Prof_CreateCheckGroup() //显示评审组列表
        {
            rsklldb.OpenConnection();
            string sql = "select * from [TopicInformation] ";
            List<TopicInformation> list = rsklldb.Detail<TopicInformation>(sql);
            for (int i = 0; i < list.Count; i++) //对于每个过教授审的选题，检查表中有无记录，若无则插入
            {
                if (list[i].ProIfPassed == 1)
                {
                    string sql3 = "select * from [CheckGroup] where CheckGroupName='" + list[i].TopicName + "'";
                    List<CheckGroup> list2 = rsklldb.Detail<CheckGroup>(sql3);
                    if (list2.Count == 0)  //判断是否是首次插入
                    {
                        string sql1 = "insert into[CheckGroup](CheckGroupName) values('" + list[i].TopicName + "')";
                        rsklldb.InsertData(sql1);
                    }
                }
                else
                {
                    string sql4 = "delete from [CheckGroup] where CheckGroupName='" + list[i].TopicName + "'";
                    rsklldb.InsertData(sql4);
                }

            }
            string sql2 = "select * from [CheckGroup]";
            List<CheckGroup> list1 = rsklldb.Detail<CheckGroup>(sql2);
            @ViewBag.ck = list1;
            return View("Prof_CreateCheckGroup");
        }
        public ActionResult Prof_CheckGroupSelect(string userid, string all)//搜索评审组
        {
            if (all == "刷新/显示全部") return Prof_CreateCheckGroup();
            rsklldb.OpenConnection();
            string sql = "select * from [CheckGroup] where CheckGroupName like '%" + userid + "%'";
            rsklldb.InsertData(sql);
            List<CheckGroup> list = rsklldb.Detail<CheckGroup>(sql);
            @ViewBag.ck = list;
            return View("Prof_CreateCheckGroup");
        }
        public ActionResult Prof_InsertCheckGroup(string ID) //评审组编辑信息提交之前
        {
            rsklldb.OpenConnection();
            string sql = "select * from [CheckGroup] where CheckGroupName like '%" + ID + "%'";
            List<CheckGroup> list = rsklldb.Detail<CheckGroup>(sql);
            @ViewData["CheckGroupName"] = list[0].CheckGroupName;
            @ViewData["CheckGroupID"] = list[0].CheckGroupID;
            @ViewData["CheckAdminTeaID"] = list[0].CheckAdminTeaID;
            CGObj = list[0];
            string sql1 = "select * from [Teacher]";
            List<Teacher> list1 = rsklldb.Detail<Teacher>(sql1);
            @ViewBag.tea = list1;
            return View("Prof_CheckGroupEditor");
        }
        public ActionResult Prof_CheckGroupEditor(CheckGroup Obj) //评审组编辑信息提交以后
        {
            rsklldb.OpenConnection();
            //更新数据项
            string sql3 = "delete from [CheckGroup] where CheckGroupName = '" + Obj.CheckGroupName + "'";
            rsklldb.InsertData(sql3);
            string sql = "insert into [CheckGroup](CheckGroupID,CheckAdminTeaID,CheckGroupName) values('" + Obj.CheckGroupID + "','" + Obj.CheckAdminTeaID + "','" + Obj.CheckGroupName + "')";
            rsklldb.InsertData(sql);
            string sql2 = "select * from [CheckGroup]";
            List<CheckGroup> list1 = rsklldb.Detail<CheckGroup>(sql2);
            @ViewBag.ck = list1;
            return View("Prof_CreateCheckGroup");
        }
        public ActionResult Prof_AddTeaInCG(string ID) //将教师添加进当前评审组
        {
            rsklldb.OpenConnection();
            string sql1 = "update [Teacher] set CheckGroupID='" + CGObj.CheckGroupID + "' where UserID='" + ID + "'";
            rsklldb.InsertData(sql1);
            return Prof_InsertCheckGroup(CGObj.CheckGroupName);
        }

        public ActionResult Prof_RemoveTeaInCG(string ID) //将教师移出当前评审组
        {
            rsklldb.OpenConnection();
            string sql1 = "update [Teacher] set CheckGroupID='' where UserID='" + ID + "'";
            rsklldb.InsertData(sql1);
            return Prof_InsertCheckGroup(CGObj.CheckGroupName);
        }

        //教授答辩管理
        public ActionResult Prof_CreateGroup() //显示答辩组列表
        {
            rsklldb.OpenConnection();
            string sql = "select * from [CheckGroup] ";
            List<CheckGroup> list = rsklldb.Detail<CheckGroup>(sql);
            for (int i = 0; i < list.Count; i++) //对于每个已创建ID的评审组，检查表中有无记录，若无则插入
            {
                if (list[i].CheckGroupID != ""&& list[i].CheckGroupID != null)
                {
                    string sql3 = "select * from [Group] where GroupID='" + list[i].CheckGroupID + "'";
                    List<CheckGroup> list2 = rsklldb.Detail<CheckGroup>(sql3);
                    if (list2.Count == 0)  //判断是否是首次插入
                    {
                        string sql1 = "insert into[Group](GroupID) values('" + list[i].CheckGroupID + "')";
                        rsklldb.InsertData(sql1);
                    }
                }
                else
                {
                    string sql4 = "delete from [Group] where GroupID='" + list[i].CheckGroupID + "'";
                    rsklldb.InsertData(sql4);
                }

            }
            string sql2 = "select * from [Group]";
            List<Group> list1 = rsklldb.Detail<Group>(sql2);
            @ViewBag.g = list1;
            return View("Prof_CreateGroup");
        }

        public ActionResult Prof_GroupSelect(string userid, string all)//搜索答辩组
        {
            if (all == "刷新/显示全部") return Prof_CreateGroup();
            rsklldb.OpenConnection();
            string sql = "select * from [Group] where GroupID like '%" + userid + "%'";
            rsklldb.InsertData(sql);
            List<Group> list = rsklldb.Detail<Group>(sql);
            @ViewBag.g = list;
            return View("Prof_CreateGroup");
        }

        public ActionResult Prof_InsertGroup(string ID) //答辩组信息提交之前
        {
            rsklldb.OpenConnection();
            string sql = "select * from [Group] where GroupID like '%" + ID + "%'";
            List<Group> list = rsklldb.Detail<Group>(sql);
            @ViewData["GroupID"] = ID;
            @ViewData["AdminTeaID"] = list[0].AdminTeaID;
            @ViewData["GroupTime"] = list[0].GroupTime;
            @ViewData["GroupAddress"] = list[0].GroupAddress;
            GObj = list[0];
            string sql1 = "select * from [Teacher]";
            List<Teacher> list1 = rsklldb.Detail<Teacher>(sql1);
            @ViewBag.tea = list1;
            return View("Prof_GroupEditor");
        }
        public ActionResult Prof_GroupEditor(Group Obj) //答辩组编辑信息提交以后
        {
            rsklldb.OpenConnection();
            //更新数据项
            string sql3 = "delete from [Group] where GroupID = '" + Obj.GroupID + "'";
            rsklldb.InsertData(sql3);
            string sql = "insert into [Group](GroupID,AdminTeaID,GroupTime,GroupAddress) values('" + Obj.GroupID + "','" + Obj.AdminTeaID + "','" + Obj.GroupTime + "','" + Obj.GroupAddress + "')";
            rsklldb.InsertData(sql);
            string sql2 = "select * from [Group]";
            rsklldb.InsertData(sql2);
            List<Group> list1 = rsklldb.Detail<Group>(sql2);
            @ViewBag.g = list1;
            return View("Prof_CreateGroup");
        }

        public ActionResult Prof_AddTeaInG(string ID) //将教师添加进当前答辩组
        {
            rsklldb.OpenConnection();
            string sql1 = "update [Teacher] set GroupID='" + GObj.GroupID + "' where UserID='" + ID + "'";
            rsklldb.InsertData(sql1);
            return Prof_InsertGroup(GObj.GroupID);
        }

        public ActionResult Prof_RemoveTeaInG(string ID) //将教师移出当前答辩组
        {
            rsklldb.OpenConnection();
            string sql1 = "update [Teacher] set GroupID='' where UserID='" + ID + "'";
            rsklldb.InsertData(sql1);
            return Prof_InsertGroup(GObj.GroupID);
        }

        //学生功能
        public ActionResult Stu_SearchGrade() //学生查看成绩
        {
            rsklldb.OpenConnection();
            string sql = "select * from [Grade] where UserID='" + Session["UserID"] + "'";
            List<Grade> list = rsklldb.Detail<Grade>(sql);
            @ViewBag.sc = list;
            return View("Stu_SearchGrade");
        }

        public ActionResult Stu_SelectTopic() //学生选题
        {
            rsklldb.OpenConnection();
            string sql = "select * from [TopicState] where StudentID='" + Session["UserID"] + "'";
            List<TopicState> list = rsklldb.Detail<TopicState>(sql);
            @ViewBag.ts = list;
            string sql1 = "select * from [TopicInformation] where ProIfPassed=1"; //查找列表中所有已过审选题
            List<TopicInformation> list1 = rsklldb.Detail<TopicInformation>(sql1);
            @ViewBag.ti = list1;
            return View("Stu_SelectTopic");
        }

        public ActionResult Stu_TopicSelect(string ID) //学生查看选题结果及提交选题
        {
            rsklldb.OpenConnection();
            string sql = "select * from [TopicState] where StudentID= '" + Session["UserID"] + "'";
            List<TopicState> list = rsklldb.Detail<TopicState>(sql);
            if (list.Count == 0 || (list[0].StuIsPassed == 0 || list[0].StuIsPassed == 2))
            {
                //如果学生是首次提交选题或选题未过审，可以反复提交选题
                string sql1 = "select * from [TopicInformation] where TopicID='" + ID + "'"; //提取被选中选题的信息
                List<TopicInformation> list1 = rsklldb.Detail<TopicInformation>(sql1);
                string sql2 = "delete from [TopicState] where StudentID = '" + Session["UserID"] + "'"; //清除系统旧信息
                rsklldb.InsertData(sql2);
                string sql3 = "insert into [TopicState](TopicID,TeacherID,StudentID,StuIsPassed) values('" + list1[0].TopicID + "','" + list1[0].TeacherID + "','" + Session["UserID"] + "',0)"; //清除系统旧信息
                rsklldb.InsertData(sql3);
            }
            return Stu_SelectTopic();
        }

        public ActionResult Stu_CGIsPassed() //学生查看开题评审结果
        {
            rsklldb.OpenConnection();
            string sql = "select * from [Stu_CGIsPassed] where UserID='" + Session["UserID"] + "'";
            List<Stu_CGIsPassed> list = rsklldb.Detail<Stu_CGIsPassed>(sql);
            @ViewBag.cgi = list;
            return View("Stu_CGIsPassed");
        }

        public ActionResult Stu_InGroup() //学生查看答辩组
        {
            rsklldb.OpenConnection();
            string sql = "select * from [Stu_InGroup] where UserID='" + Session["UserID"] + "'";
            List<Stu_InGroup> list = rsklldb.Detail<Stu_InGroup>(sql);
            @ViewBag.ig = list;
            return View("Stu_InGroup");
        }
        
        /// <summary>
        /// 林伟涛2018.9.8合并的学生材料管理功能
        /// </summary>
        /// <returns></returns>
        public ActionResult Stu_MaterialList()
        {
            rsklldb.OpenConnection();
            string sql1 = "SELECT * FROM [StuMaterial] WHERE StudentID ='" + Session["UserID"] + "' AND TaskID = 1";
            ViewBag.dt1 = rsklldb.Detail<StuMaterial>(sql1);
            string sql2 = "SELECT * FROM [StuMaterial] WHERE StudentID ='" + Session["UserID"] + "' AND TaskID = 2";
            ViewBag.dt2 = rsklldb.Detail<StuMaterial>(sql2);
            string sql3 = "SELECT * FROM [StuMaterial] WHERE StudentID ='" + Session["UserID"] + "' AND TaskID = 3";
            ViewBag.dt3 = rsklldb.Detail<StuMaterial>(sql3);
            rsklldb.CloseConnection();
            return View("Stu_MaterialList");
        }
        //学生查看材料评审建议
        public ActionResult Stu_Materiallook(string filename)
        {
            rsklldb.OpenConnection();
            string sq = "SELECT * FROM [StuMaterial] WHERE MaterialFile = '" + filename + "'";
            ViewBag.dt = rsklldb.Detail<StuMaterial>(sq)[0];
            rsklldb.CloseConnection();
            return View("Stu_Materiallook");
        }

        public ActionResult Stu_MaterialUploadPage()
        {
            return View("Stu_MaterialUploadPage");
        }

        public ActionResult Stu_MaterialUpload(int taskid, int week, HttpPostedFileBase file)
        {
            var filename = file.FileName;
            var filePath = Server.MapPath(string.Format("~/Content/file", "File"));
            rsklldb.OpenConnection();
            string sq = "SELECT * FROM [Student] WHERE UserID = '" + Session["UserID"] + "'";
            Student s = rsklldb.Detail<Student>(sq)[0];
            if (taskid == 2)
            {

                string sql0 = "SELECT * FROM [Material] WHERE StudentID = '" + Session["UserID"] + "'";
                List<Material> listm = rsklldb.Detail<Material>(sql0);
                foreach(Material m in listm)
                {
                    if (m.TaskID == taskid && m.Week == week)
                    {
                        string sql1 = "DELETE FROM [Material] WHERE StudentID = '" + Session["UserID"] + "' AND TaskID = " + taskid + " AND Week = " + week;
                        rsklldb.Delete(sql1);
                    }
                }

                String FileName = Session["UserID"].ToString() + taskid + week + filename;
                string sql = @"INSERT INTO [dbo].[Material]([StudentID],[TopicID],[MaterialFile],[CheckGroupID],[TaskID],[Week])
                VALUES('" + s.UserID + "','" + s.TopicID + "','" + FileName + "','" + s.CheckGroupID + "'," + taskid + "," + week + ")";
                rsklldb.InsertData(sql);
                rsklldb.CloseConnection();
                file.SaveAs(Path.Combine(filePath, FileName));
                return View("Stu_MaterialUploadPage");
            }
            else
            {
                string sql0 = "SELECT * FROM [Material] WHERE StudentID = '" + Session["UserID"] + "'";
                List<Material> listm = rsklldb.Detail<Material>(sql0);
                foreach (Material m in listm)
                {
                    if (m.TaskID == taskid)
                    {
                        string sql1 = "DELETE FROM [Material] WHERE StudentID = '" + Session["UserID"] + "' AND TaskID = " + taskid;
                        rsklldb.Delete(sql1);
                    }
                }
                String FileName = Session["UserID"].ToString() + taskid + filename;
                string sql = @"INSERT INTO [dbo].[Material]([StudentID],[TopicID],[MaterialFile],[CheckGroupID],[TaskID])
                VALUES('" + s.UserID + "','" + s.TopicID + "','" + FileName + "','" + s.CheckGroupID + "'," + taskid + ")";
                rsklldb.InsertData(sql);
                rsklldb.CloseConnection();
                file.SaveAs(Path.Combine(filePath, FileName));
                return View("Stu_MaterialUploadPage");
            }

        }
    }
}