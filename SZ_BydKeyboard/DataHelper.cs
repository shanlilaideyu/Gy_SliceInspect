using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HalconDotNet;
using System.IO;
using System.Net;

namespace SZ_BydKeyboard
{
    public static class DataHelper
    {

        //所有输出项
        public static string[] Names = new string[] { "SN", "Station_id", "Test_time","Fixture", "MLB jumper flex B2B_P1", "MLB jumper flex B2B_P2", "MLB jumper flex B2B_P3", "MLB jumper flex B2B_Average"
            , "MLB jumper flex B2B", "Mesa flex_P1","Mesa flex_P2", "Mesa flex_(P1 P2)", "Mesa flex", "E75 flex_P1", "E75 flex_P2", "E75 flex_(P1 P2)",
            "E75 flex", "MLB flex_P1", "MLB flex_P2", "MLB flex_(P1 P2)","MLB flex", "Power flex_P1", "Power flex_P2", "Power flex_(P1 P2)", "Power flex", "Antenna shrapnel_P1",
            "Antenna shrapnel_P2", "Antenna shrapnel_(P1 P2)","Antenna shrapnel", "Power switch_P1","Power switch", "Mesa Connector buckle", "E75 Connector buckle", "MLB Connector buckle",
            "Power Connector buckle","Mesa jumper flex cosmetic 1","Mesa jumper flex cosmetic 2","Mesa jumper flex cosmetic 3","Mesa jumper flex cosmetic 4",
            "Switch screw 1", "Switch screw 2", "Main board screw 1","Main board screw 2", "Main board screw 3", "Main board screw 4","Main board screw 5", "Result"};

        //测量项
        public static string[] MeasureNames = new string[] {"MLB jumper flex B2B_P1", "MLB jumper flex B2B_P2", "MLB jumper flex B2B_P3", "MLB jumper flex B2B_Average"
            ,"Mesa flex_P1","Mesa flex_P2", "Mesa flex_(P1 P2)", "E75 flex_P1", "E75 flex_P2","E75 flex_(P1 P2)", "MLB flex_P1", "MLB flex_P2",
            "MLB flex_(P1 P2)", "Power flex_P1", "Power flex_P2", "Power flex_(P1 P2)", "Antenna shrapnel_P1", "Antenna shrapnel_P2",
            "Antenna shrapnel_(P1 P2)","Power switch_P1" };


        [NonSerialized]
        //工站1的值
        public static string[] Values1 = new string[Names.Length];
        //工站2的值
        public static string[] Values2 = new string[Names.Length];



        public static string Get(string url)
        {
            string result = "";
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(url);
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容  
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }

        //查询排队状态
        public static bool QueueUp(string strDataCode)
        {
            try
            {


                string result = string.Empty;
                StringBuilder builder = new StringBuilder();
                builder.Append($"http://{Common.mes.Ip}:{Common.mes.Port}/manufacturing-web-ext/BobcatIntegerationServlet");
                builder.Append("?");
                builder.Append($"c=validate&test_station_name={Common.mes.test_station_name}&station_id={Common.mes.station_id}&sn={strDataCode}");

                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
                //添加参数  
                HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
                Stream stream = resp.GetResponseStream();
                try
                {
                    //获取内容  
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
                finally
                {
                    stream.Close();
                }
                if (result.Substring(0, result.IndexOf(" ")) == "0")
                {
                    Common.ShowMsgEvent.Invoke($"Tips:{DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff")}-->[{strDataCode}]已在排队状态！--");
                    return true;
                }
                else
                {
                    Common.ShowMsgEvent.Invoke($"Warming:{DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff")}-->[{strDataCode}]未在排队状态！返回信息为：[{result.Substring(result.IndexOf(',') + 1)}]--");
                    return false;
                }
            }
            catch (Exception ex)
            {
                Common.ShowMsgEvent.Invoke($"Warming:{DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff")}-->[{strDataCode}]查询排队状态失败，原因为{ex.Message}]--");
                return false;
            }
        }
        //发送测试数据（产品OK或NG时发送）
        public static bool SendRawData(string[] Values, string strRawData)
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            builder.Append($"http://{Common.mes.Ip}:{Common.mes.Port}/manufacturing-web-ext/BobcatIntegerationServlet");
            builder.Append("?");
            builder.Append($"c=RAW_DATA_COLLECT&test_station_name={Common.mes.test_station_name}&station_id={Common.mes.station_id}&sn={Values[0]}&result={Values[Values.Length - 1]}&project={Common.mes.project}&raw_data_result={Values[Values.Length - 1]}&raw_data={strRawData}");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            //添加参数  
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容  
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            if (result.Substring(0, result.IndexOf(" ")) == "0")
            {
                Common.ShowMsgEvent.Invoke($"Tips:{DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff")}-->[{Values[0]}]数据上传成功！--");
                return true;
            }
            else
            {
                Common.ShowMsgEvent.Invoke($"Warming:{DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff")}-->[{Values[0]}]数据上传失败！！返回信息为：[{result.Substring(result.IndexOf(',') + 1)}]--");
                return false;
            }
        }
        //过站（代表这个工站测试通过，需要产品OK）
        public static bool PassStation(string[] Values, string strDataCode)
        {
            string result = string.Empty;
            StringBuilder builder = new StringBuilder();
            builder.Append($"http://{Common.mes.Ip}:{Common.mes.Port}/manufacturing-web-ext/BobcatIntegerationServlet");
            builder.Append("?");
            builder.Append($"c=ADD_RECORD&test_station_name={Common.mes.test_station_name}&station_id={Common.mes.station_id}&sn={strDataCode}&result={Values[Values.Length - 1]}&product={Common.mes.product}&user_id={Common.mes.user_id}");
            builder.Append($"&mac_address={Common.mes.mac_address}&start_time={Common.mes.start_time}&stop_time={Common.mes.stop_time}");
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            //添加参数  
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容  
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            if (result.Substring(0, result.IndexOf(" ")) == "0")
            {
                Common.ShowMsgEvent.Invoke($"Tips:{DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff")}-->[{strDataCode}]过站成功！--");
                return true;
            }
            else
            {
                Common.ShowMsgEvent.Invoke($"Warming:{DateTime.Now.ToString("----yyyy/MM/dd HH:mm:ss:fff")}-->[{strDataCode}]过站失败！！返回信息为：[{result.Substring(result.IndexOf(',') + 1)}]--");
                return false;
            }
        }


        public static string Get(string url, Dictionary<string, string> dic)
        {
            string result = "";
            StringBuilder builder = new StringBuilder();
            builder.Append(url);
            if (dic.Count > 0)
            {
                builder.Append("?");
                int i = 0;
                foreach (var item in dic)
                {
                    if (i > 0)
                        builder.Append("&");
                    builder.AppendFormat("{0}={1}", item.Key, item.Value);
                    i++;
                }
            }
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(builder.ToString());
            //添加参数  
            HttpWebResponse resp = (HttpWebResponse)req.GetResponse();
            Stream stream = resp.GetResponseStream();
            try
            {
                //获取内容  
                using (StreamReader reader = new StreamReader(stream))
                {
                    result = reader.ReadToEnd();
                }
            }
            finally
            {
                stream.Close();
            }
            return result;
        }

        /// <summary>
        /// 获取限位表头
        /// </summary>
        /// <param name="LimitName"></param>
        /// <param name="LimitValue"></param>
        /// <returns></returns>
        public static string GetLimitHead(string LimitName, double[] LimitValue)
        {
            string[] values = new string[Names.Length];
            values[3] = LimitName;
            for (int i = 0; i < LimitValue.Length; i++)
            {
                values[Names.ToList<string>().IndexOf(MeasureNames[i])] = LimitValue[i].ToString();
            }
            return ArrayToString(values);
        }

        /// <summary>
        /// 转成MES需要的字符格式
        /// </summary>
        /// <param name="Names"></param>
        /// <param name="Values"></param>
        /// <returns></returns>
        public static string ValueToString(string[] Names, string[] Values)
        {
            string temp = string.Empty;
            //for (int i = 1; i < 3; i++)
            //{
            //    temp += $"&{Names[i]}={Values[i]}";
            //}
            //temp += $"&{Names[Names.Length - 1]}={Values[Names.Length - 1]}";
            //temp += "&raw_data=";
            for (int i = 4; i < Names.Length - 1; i++)
            {
                temp += Names[i] + @"/" + Values[i] + ";";
            }
            return temp;
        }

        /// <summary>
        /// 写入报表  
        /// </summary>
        /// <param name="fileName">文件名</param>
        public static void WriteToCsv(string[] Values, string fileName)
        {

            HTuple hv_FileHandle = null;
            if (!File.Exists(fileName))
            {
                HOperatorSet.OpenFile(fileName, "output", out hv_FileHandle);
                HOperatorSet.FwriteString(hv_FileHandle, ArrayToString(Names));
                HOperatorSet.FnewLine(hv_FileHandle);
                HOperatorSet.FwriteString(hv_FileHandle, GetLimitHead("USL", Common.Data.USL));
                HOperatorSet.FnewLine(hv_FileHandle);
                HOperatorSet.FwriteString(hv_FileHandle, GetLimitHead("LSL", Common.Data.LSL));
                HOperatorSet.CloseFile(hv_FileHandle);
            }
            //写入新的数据即可
            HOperatorSet.OpenFile(fileName, "append", out hv_FileHandle);
            HOperatorSet.FnewLine(hv_FileHandle);
            HOperatorSet.FwriteString(hv_FileHandle, ArrayToString(Values));
            HOperatorSet.CloseFile(hv_FileHandle);
        }

        private static string ArrayToString(string[] temp)
        {
            string str = temp[0];
            for (int i = 1; i < temp.Length; i++)
            {
                str += "," + temp[i];
            }
            return str;
        }

        private static string ArrayToString(double[] temp)
        {
            string str = temp[0].ToString();
            for (int i = 1; i < temp.Length; i++)
            {
                str += "," + temp[i];
            }
            return str;
        }

    }
}
