using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Automation.BDaq;
using LogRecord;
using System.Collections;

namespace LeoMotion
{
    public class ExAdvanTech : ExCard
    {

        ErrorCode errorCode = ErrorCode.Success;
        InstantDiCtrl instantDiCtrl = new InstantDiCtrl();
        InstantDoCtrl instantDoCtrl = new InstantDoCtrl();

        public bool InitExIOBoard(string deviceDescription, string profilePath)
        {
            try
            {
                instantDiCtrl.SelectedDevice = new DeviceInformation(deviceDescription);
                //errorCode = instantDiCtrl.LoadProfile(profilePath);//Loads a profile to initialize the device.
                instantDoCtrl.SelectedDevice = new DeviceInformation(deviceDescription);
                //errorCode = instantDoCtrl.LoadProfile(profilePath);//Loads a profile to initialize the device.
                b_Connected = true;
            }
            catch (Exception ex)
            {
                LogHelper.Exception(this.GetType(), ex);
                b_Connected = false;
            }
            return b_Connected;
        }


        public override bool GetAllInput()
        {
            for (int i = 0; i < InputList.Length; i++)
            {
                byte data = 0;
                errorCode = instantDiCtrl.ReadBit(i / 8, i % 8, out data);
                if (errorCode == ErrorCode.Success)
                {
                    if (data == 1)
                    {
                        InputList[i] = true;
                    }
                    else
                    {
                        InputList[i] = false;
                    }
                }
                else
                {
                    return false;
                }
            }
            return true;
        }


        public override bool InitCard(ushort CardNum, ushort NodeNum)
        {
            throw new NotImplementedException();
        }

        public override bool WriteOutput(ushort IoNum, ushort Value)
        {
            errorCode = instantDoCtrl.WriteBit(IoNum / 8, IoNum % 8, (byte)Value);
            if (errorCode == ErrorCode.Success)
            {
                return true;
            }
            else
            {
                LogHelper.Error(this.GetType(), errorCode.ToString());
                return false;
            }
        }
    }
}
