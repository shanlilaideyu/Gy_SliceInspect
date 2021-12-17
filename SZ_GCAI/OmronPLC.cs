using HslCommunication;
using HslCommunication.ModBus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace SZ_BydKeyboard
{
    class OmronPLC
    {
        private static OmronPLC m_instance;
        private static readonly object m_objLock = new object();
        public static OmronPLC GetInstance()//单例模式
        {
            if (m_instance == null)
            {
                lock (m_objLock)
                {
                    if (m_instance == null)
                    {
                        m_instance = new OmronPLC();
                    }
                }
            }
            return m_instance;
        }

        private ModbusTcpNet PLC;
        public void Init_TCP(string ipAddress, int port)
        {
            PLC = new ModbusTcpNet(ipAddress, port, 1);
            PLC.DataFormat = HslCommunication.Core.DataFormat.CDAB;
            Thread.Sleep(100);
            OperateResult zz = PLC.ConnectServer();
            if (zz.IsSuccess)
                Common.ShowMsgEvent($"Tips:----{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ms")}---初始化PLC成功!");
            else
                Common.ShowMsgEvent($"Error:----{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ms")}---初始化PLC失败!");
            
        }

        public void Send(string _address, int _data)
        {
            if (PLC != null)
                PLC.Write(_address, (ushort)_data);
        }

        public int GetAddressValue(string _address)
        {
            if (PLC != null)
                return PLC.ReadInt16(_address).Content;
            else
                return -1;
        }

        public float GetAddressFloatValue(string _address)
        {
            if (PLC != null)
            {
                return PLC.ReadFloat(_address).Content;
            }
            return -1;
        }
        public void SendFloatValue(string _address, float _data)
        {
            if (PLC != null)
                PLC.Write(_address, (float)_data);
        }
    }
}
