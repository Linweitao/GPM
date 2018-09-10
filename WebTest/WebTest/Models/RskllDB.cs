namespace WebTest.Models
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;
    using System.Data.SqlClient;
    using System.Configuration;
    using System.Data;
    using System.Collections.Generic;
    using System.Reflection;

    public partial class RskllDB : DbContext
    {

        public RskllDB()
            : base("name=RskllDB")
        {
        }

        protected SqlConnection conn;

        public void OpenConnection()
        {
            conn = new SqlConnection(ConfigurationManager.ConnectionStrings["RskllDB"].ConnectionString);
            try
            {
                if (conn.State.ToString() != "Open")
                {
                    conn.Open();
                }
            }
            catch (SqlException ex)
            {

                throw ex;
            }
        }
        public void CloseConnection()
        {
            try
            {
                conn.Close();
            }
            catch (SqlException ex)
            {
                throw ex;
            }
        }
        //插入数据
        public int InsertData(string sql)
        {
            int i = 0;
            try
            {
                if (conn.State.ToString() == "Open")
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    i = cmd.ExecuteNonQuery();
                }
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public int Delete(string sql)
        {
            int i = 0;
            try
            {
                if (conn.State.ToString() == "Open")
                {
                    SqlCommand cmd = new SqlCommand(sql, conn);
                    i = cmd.ExecuteNonQuery();
                }
                return i;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //返回select数据项
        public List<T> Detail<T>(string sql)
        {
            try
            {
                SqlDataAdapter da = new SqlDataAdapter(sql, conn);
                DataTable dt = new DataTable();
                da.Fill(dt);
                return DataTableToList<T>(dt);
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        //将DataTable类型数据转为数列对象
        public static List<T> DataTableToList<T>(DataTable dt)
        {
            var list = new List<T>();
            Type t = typeof(T);
            var plist = new List<PropertyInfo>(typeof(T).GetProperties());

            foreach (DataRow item in dt.Rows)
            {
                T s = System.Activator.CreateInstance<T>();
                for (int i = 0; i < dt.Columns.Count; i++)
                {
                    PropertyInfo info = plist.Find(p => p.Name == dt.Columns[i].ColumnName);
                    if (info != null)
                    {
                        if (!Convert.IsDBNull(item[i]))
                        {
                            info.SetValue(s, item[i], null);
                        }
                    }
                }
                list.Add(s);
            }
            return list;
        }
    }
}
