using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace LeoLink
{
    public class TcpServer
    {
        public string ReceivedMessage = null;

        byte[] buffer = new byte[1024 * 3];

        public bool b_Link = false;

        public delegate void DelegateRecive(string msg);
        public DelegateRecive ReciveEvent;
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
                //CommonData.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("收到客户端{0}消息:{1}--", socket.RemoteEndPoint.ToString(), message), CommonData.fm.loger);
                if (message != null)
                {
                    ReciveEvent(message);
                }
                //接收下一个消息(因为这是一个递归的调用，所以这样就可以一直接收消息了）
                socket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), socket);
            }
            catch (Exception ex)
            {
                var socket = ar.AsyncState as Socket;
                //移除已断开的客户端
                dictSocket.Remove(socket.RemoteEndPoint.ToString());
                count--;
                //CommonData.ShowMsgEvent.Invoke("Error:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("接收信息异常，原因为:{0}--", ex.Message), CommonData.fm.loger);
                //CommonData.ShowMsgEvent.Invoke("Error:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("客户端:{0}已断开--", socket.RemoteEndPoint.ToString()), CommonData.fm.loger);
            }
        }

        /// <summary>
        /// 发送消息
        /// </summary>
        /// <param name="msg"></param>
        public void SendMessage(string socketName, string msg)
        {
            var outputBuffer = Encoding.UTF8.GetBytes(msg);
            dictSocket[socketName].BeginSend(outputBuffer, 0, outputBuffer.Length, SocketFlags.None, null, null);
        }

        private int count = 0;

        //保存了服务器端所有负责和客户端通信发套接字
        public Dictionary<string, Socket> dictSocket = new Dictionary<string, Socket>();

        /// <summary>
        /// 客户端连接
        /// </summary>
        /// <param name="ar"></param>
        public void ClientAccepted(IAsyncResult ar)
        {
            #region
            //设置计数器
            count++;
            var socket = ar.AsyncState as Socket;
            //这就是客户端的Socket实例，我们后续可以将其保存起来
            var client = socket.EndAccept(ar);
            //存储客户端
            dictSocket.Add(client.RemoteEndPoint.ToString(), client);
            //客户端IP地址和端口信息
            IPEndPoint clientipe = (IPEndPoint)client.RemoteEndPoint;
            //CommonData.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("客户端:{0}连入成功，目前客户端个数{1}--", clientipe, count), CommonData.fm.loger);
            //接收客户端的消息(这个和在客户端实现的方式是一样的）异步
            client.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(Receive), client);
            //准备接受下一个客户端请求(异步)
            socket.BeginAccept(new AsyncCallback(ClientAccepted), socket);
            #endregion
        }

        public Socket Socket;

        public bool InitServer(string ServerIP, int ServerPort)
        {
            byte[] buffer = new byte[1024 * 3];
            //设定服务器IP地址
            IPAddress ip = IPAddress.Parse(ServerIP);
            Socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            try
            {
                Socket.Bind(new IPEndPoint(ip, ServerPort));
                Socket.Listen(10);
                //注册接收事件
                Socket.BeginAccept(new AsyncCallback(ClientAccepted), Socket);
                //ServerSocket.BeginReceive(buffer, 0, buffer.Length, SocketFlags.None, new AsyncCallback(ReceiveMessage), ServerSocket);
                //CommonData.ShowMsgEvent.Invoke("Tips:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("设定服务器:{0}({1})成功！--", CommonData.ServerIP, CommonData.ServerPort), CommonData.fm.loger);
                return true;
            }
            catch (Exception ex)
            {
                //CommonData.ShowMsgEvent.Invoke("Warning:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("设定服务器:{0}({1})失败！--", CommonData.ServerIP, CommonData.ServerPort), CommonData.fm.loger);
                //CommonData.ShowMsgEvent.Invoke("Warning:" + DateTime.Now.ToString("----yyyy/MM/dd hh:mm:ss:ms -->") + string.Format("失败原因:{0}--", ex.Message), CommonData.fm.loger);
                return false;
            }
        }

    }
}
