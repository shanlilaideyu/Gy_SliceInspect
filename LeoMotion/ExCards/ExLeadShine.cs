using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoMotion
{
    public class ExLeadShine : ExCard
    {

        public ExLeadShine(int Num)
        {
            InputList = new bool[Num];
        }

        public override bool GetAllInput()
        {
            ushort n_state = 0;
            //读取连接状态
            LTDMC.nmc_get_connect_state(n_CardNum, ref n_NodeNum, ref n_state);
            if (n_state != 1)
            {
                //打开连接
                LTDMC.nmc_set_connect_state(n_CardNum, n_NodeNum, 1, 0);
                LTDMC.nmc_get_connect_state(n_CardNum, ref n_NodeNum, ref n_state);
            }
            if (n_state == 1)
            {
                for (ushort i = 0; i < InputList.Length; i++)
                {
                    ushort nValue = 2;
                    short result = LTDMC.nmc_read_inbit(n_CardNum, n_NodeNum, i, ref nValue);
                    if (nValue == 0)
                    {
                        InputList[i] = true;
                    }
                    else if (nValue == 1)
                    {
                        InputList[i] = false;
                    }
                }
                return true;
            }
            else
            {
                return false;
            }
        }


        public override bool InitCard(ushort CardNum, ushort NodeNum)
        {
            n_CardNum = CardNum;
            n_NodeNum = NodeNum;
            return true;
        }

        public override bool WriteOutput(ushort IoNum, ushort Value)
        {
            ushort n_state = 0;
            //读取连接状态
            LTDMC.nmc_get_connect_state(n_CardNum, ref n_NodeNum, ref n_state);
            if (n_state != 1)
            {
                //打开连接
                LTDMC.nmc_set_connect_state(n_CardNum, n_NodeNum, 1, 0);
                LTDMC.nmc_get_connect_state(n_CardNum, ref n_NodeNum, ref n_state);
            }
            if (n_state == 1)
            {
                short result = LTDMC.nmc_write_outbit(n_CardNum, n_NodeNum, IoNum, Value);
                if (result == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }
        }
    }
}

