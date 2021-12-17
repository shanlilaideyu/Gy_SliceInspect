using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using LogRecord;
using System.Windows;

namespace LeoUserManger
{
    public static class DBManager
    {
        private static string connStr = "./PwdManager.db";
        //创建数据库
        public static void CreateDB()
        {
            SQLiteConnection.CreateFile("./PwdManager.db");
        }

        public static string getConnStr()
        {
            if (!File.Exists("./PwdManager.db"))
            {
                CreateDB();
                initDB();
            }
            SQLiteConnectionStringBuilder connstr = new SQLiteConnectionStringBuilder();
            connstr.DataSource = connStr;
            return connstr.ToString();
        }

        //数据库初始化
        public static void initDB()
        {
            ExecuteSql("create table records(username varchar(20) primary key, JobNumber varchar(100), CardNumber varchar(100), LimitName varchar(100), picpath varchar(100));");
        }

        //执行Sql语句
        public static int ExecuteSql(string sqlStr)
        {
            using (SQLiteConnection conn = new SQLiteConnection(getConnStr()))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand comm = conn.CreateCommand();
                    comm.CommandText = sqlStr;
                    comm.CommandType = System.Data.CommandType.Text;
                    return comm.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    //MessageBox.Show(ex.Message.ToString());
                    return -1;
                }
            }
        }

        public static bool InsetRecord(string username, string JobNumber, string CardNumber, string LimitName)//插入记录
        {
            bool IsInSys = false;
            string strt = "";
            VerifyLoginUser(CardNumber, out IsInSys, out strt);
            if (IsInSys)
            {
                MessageBox.Show("添加用户权限失败,用户卡号已存在！");
                return false;
            }

            string str = "insert into records values('" + username + "', '" + JobNumber + "', '" + CardNumber + "', '" + LimitName + "', '');";
            if (ExecuteSql(str) != 1)
            {
                MessageBox.Show("添加用户权限失败,用户姓名已存在！");
                return false;
            }
            else return true;
        }

        public static bool AddLoginUser(string username, string JobNumber, string CardNumber, string LimitName)//添加用户权限
        {
            if (username == string.Empty || JobNumber == string.Empty || CardNumber == string.Empty || LimitName == string.Empty)
            {
                MessageBox.Show("姓名或工号或者卡号不能为空！");
                return false;
            }

            if (InsetRecord(username, JobNumber, CardNumber, LimitName))
            {
                MessageBox.Show("添加用户权限成功！");
                return true;
            }
            return false;
        }

        public static void LoadLoginUserName(List<string> usernameList)//加载用户信息
        {
            using (SQLiteConnection conn = new SQLiteConnection(DBManager.getConnStr()))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();

                    //获取数据函数
                    cmd.CommandText = "SELECT count(*) FROM records";
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    sr.Read();
                    int num = sr.GetInt32(0);
                    sr.Close();
                    if (num <= 0)
                    {
                        MessageBox.Show("当前没有用户权限记录!");
                        return;
                    }

                    //遍历username里面的值
                    cmd.CommandText = "select * from records";
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        string _username = "";
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            if (i == 3)
                            {
                                _username += reader[i].ToString();
                                break;
                            }
                            else
                            {
                                if (reader[i].ToString() == "Admin" && i == 0)
                                {
                                    _username = reader[i].ToString();
                                    break;
                                }
                                _username += reader[i].ToString() + ",";
                            }
                        }
                        usernameList.Add(_username);
                    }
                    reader.Close();


                }
                catch (Exception)
                {
                }
            }
        }

        public static bool DelRecord(string name)//删除记录
        {
            string[] sArray = name.Split(',');
            if (sArray.Length <= 0)
            {
                MessageBox.Show("分割名称失败！", "提示");
                return false;
            }

            if (sArray[3] == "开发者")
            {
                MessageBox.Show("开发者不可删除！", "提示");
                return false;
            }

            string str = "delete from records where username = '" + sArray[0] + "';";
            if (ExecuteSql(str) != 1)
            {
                MessageBox.Show("删除用户权限记录失败！", "提示");
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool VerifyLoginUser(string IdCard, out bool IsInSystem, out string UserInfo)//用户卡号输入，验证是否存在，读出管理员信息
        {
            IsInSystem = false;
            UserInfo = "";

            using (SQLiteConnection conn = new SQLiteConnection(DBManager.getConnStr()))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();

                    //获取数据函数
                    cmd.CommandText = "SELECT count(*) FROM records";
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    sr.Read();
                    int num = sr.GetInt32(0);
                    sr.Close();
                    if (num <= 0)
                    {
                        MessageBox.Show("当前没有用户权限记录!");
                        return false;
                    }

                    //遍历username里面的值
                    cmd.CommandText = "select * from records";
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            string s = reader[i].ToString();
                        }
                        if (IdCard == reader[2].ToString())
                        {
                            IsInSystem = true;
                            UserInfo = $"{reader[0].ToString()},{reader[1].ToString()},{reader[2].ToString()},{reader[3].ToString()}";
                            break;
                        }
                    }
                    reader.Close();
                }
                catch (Exception)
                {
                }
            }

            return false;
        }

        public static bool VerifyLoginAdmin(string JobNumber, out bool IsInSystem, out string StrUserInfo)//管理员验证密码是否存在
        {
            IsInSystem = false;
            StrUserInfo = "";

            using (SQLiteConnection conn = new SQLiteConnection(DBManager.getConnStr()))
            {
                try
                {
                    conn.Open();
                    SQLiteCommand cmd = conn.CreateCommand();

                    //获取数据函数
                    cmd.CommandText = "SELECT count(*) FROM records";
                    SQLiteDataReader sr = cmd.ExecuteReader();
                    sr.Read();
                    int num = sr.GetInt32(0);
                    sr.Close();
                    if (num <= 0)
                    {
                        MessageBox.Show("当前没有用户权限记录!");
                        return false;
                    }

                    //遍历username里面的值
                    cmd.CommandText = "select * from records";
                    SQLiteDataReader reader = cmd.ExecuteReader();
                    while (reader.Read())
                    {
                        if ("开发者" == reader[3].ToString())
                        {
                            if (JobNumber == reader[2].ToString())
                            {
                                IsInSystem = true;
                                StrUserInfo = $"{reader[0].ToString()},{reader[1].ToString()},{reader[2].ToString()},{reader[3].ToString()}";
                                break;
                            }
                        }
                    }
                    reader.Close();
                }
                catch (Exception)
                {

                }
            }
            return false;
        }
    }
}
