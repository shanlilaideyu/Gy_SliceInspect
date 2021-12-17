using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoMotion
{
    public class ExGoogolTech : ExCard
    {
        public override bool GetAllInput()
        {
            try
            {
                short rtn;
                ushort value;
                //rtn = mc.GT_GetExtIoValue(0, out value);

                for (int i = 0; i < 16; i++)
                {
                    rtn= mc.GT_GetExtIoBit((short)0, (short)i, out value);
                    if (value != (ushort)0)
                        InputList[i] = false;
                    else
                        InputList[i] = true;
                }
                return true;
            }
            catch 
            {
                return false;
            }
        }

        public override bool InitCard(ushort CardNum, ushort NodeNum)
        {
            short rtn;
            rtn = mc.GT_Open((short)CardNum, (short)NodeNum);
            if (rtn != 0)
                return false;
            rtn = mc.GT_OpenExtMdl("gts.dll");
            rtn = mc.GT_LoadExtConfig("mdl.cfg");
            rtn = mc.GT_SetExtIoValue(0, 0xffff);
            b_Connected = true;
            return b_Connected;
        }
        public override bool WriteOutput(ushort IoNum, ushort Value)
        {
            short rtn;
            rtn = mc.GT_SetExtIoBit(0, (short)IoNum, Value);
            if (rtn == 0)
            {
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
