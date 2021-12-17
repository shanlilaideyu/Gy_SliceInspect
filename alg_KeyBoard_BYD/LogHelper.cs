using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace alg_KeyBoard_BYD
{
    public class LogHelper
    {
        public static string LogPath = "./AlgLog/Log.csv";
        public static string operaterLogPath = "./Log/OperaterLog.csv";

        static Action<string, string> ShowToUi;


        /// <summary>
        /// 锁，防止文件冲突
        /// </summary>
        private static object objData = new object();
        public static void RecordRunLog(string errBelong ,string logInfo)
        {
            Console.WriteLine(errBelong);
            lock (objData)
            {
                DateTime now = DateTime.Now;
                string filePath = string.Format("{0}Log\\{1}\\", LogPath, now.ToString("yyyyMMdd"));
                string fileName = now.ToString("DD日") + ".txt";         //每天创建一个txt，防止通讯数据交换频率高，数据量大，文本文件过大
                if (!Directory.Exists(filePath))
                    Directory.CreateDirectory(filePath);
                if (!File.Exists(filePath + fileName))
                    File.Create(filePath + fileName).Close();
                FileHelper.AppendText(filePath + fileName, string.Format("{0},{1},{2}", DateTime.Now.ToString(), logInfo, errBelong));
            }
           

        }

        public static void RecordRunLog(LogType errType, string errBelong, string logInfo)
        {
            //if (ShowToUi!=null)
            //{
            //    ShowToUi(errBelong,logInfo);
            //}
            FileHelper.AppendText(operaterLogPath, string.Format("{0},{1},{2},({3})", errType, DateTime.Now.ToString(), logInfo, errBelong));
        }
        #region 记录错误日志到文件方法
        /// <summary>  
        /// 记录错误日志到文件方法  
        /// </summary>  
        /// <param name="exMessage"></param>  
        /// <param name="exMethod"></param>  
        /// <param name="userID"></param>  
        public static void RecordErrorLog(string exMessage, string exMethod = "")
        {
            try
            {
                //string errVir = "./Log/Error/ErrorLog.txt";
                //string errPath = System.Web.HttpContext.Current.Server.MapPath(errVir);
                //string userID = Convert.ToString(global.LoginUser.ID);
                FileHelper.AppendText(operaterLogPath, string.Format("{时间: {0}   exMedthod:{1}     exMessage:{2}}", DateTime.Now.ToString(), exMethod, exMessage));
            }
            catch (Exception ex)
            {
                RecordErrorLog(ex.Message, "LogHelper——RecordErrorLog");
            }
        }
        #endregion

    }

    public enum LogType
    {
        err=0,
    }
}
