using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LeoLink
{
    public class TcpClient
    {
        public string ReceivedMessage = null;

        public bool b_Link = false;

        private Socket Socket;

        static byte[] buffer = new byte[1024 * 3];
        /// <summary>
        /// 接收信息
        /// </summary>
        /// <param name="ar"></param>
        private void Receive(IAsyncResult ar)
        {
            try
            {
                var socket = ar.AsyncState as Socket;
                var length = socket.EndReceive(ar);
                //读取出来消息内容
                var message = Encoding.ASCII.GetString(buffer, 0, length);
                //显示消息
                // CommonData.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("收到服务器消息:{0}--", message), CommonData.fm.loger);
                if (message != null)
                {
                    ReceivedMessage = message.Substring(0,message.IndexOf("\r\n"));
                }
                //接收下一个消息(因为这是一个递归的调用，所以这样就可以一直接收消息了）
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), socket);
            }
            catch (Exception ex)
            {
                b_Link = false;
                //CommonData.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("接收信息异常，原因为:{0}--", ex.Message), CommonData.fm.loger);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string msg)
        {
            var outputBuffer = Encoding.UTF8.GetBytes(msg);
            Socket.BeginSend(outputBuffer, 0, outputBuffer.Length, SocketFlags.None, null, null);
        }

        public void ClearMsg()
        {
            ReceivedMessage = null;
        }

        public bool ConnectServer(string ServerIP, int ServerPort)
        {
            //设定服务器IP地址
            IPAddress ip = IPAddress.Parse(ServerIP);
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Socket.Connect(ip, ServerPort);
                //注册接收事件
                Socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), Socket);
                //CommonData.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("连接到服务器:{0}({1})成功！--", CommonData.ServerIP, CommonData.ServerPort), CommonData.fm.loger);
                b_Link = true;
            }
            catch(Exception E)
            {
                //CommonData.ShowMsgEvent.Invoke("Error:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("连接到服务器:{0}({1})失败！--", CommonData.ServerIP, CommonData.ServerPort), CommonData.fm.loger);
                b_Link = false;
            }
            return b_Link;
        }
    }
}
