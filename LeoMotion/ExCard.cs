using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoMotion
{

    public abstract class ExCard
    {

        public bool[] InputList;

        public string[] InputNames;

        public string[] OutputNames;

        public bool b_Connected;

        public ushort n_CardNum, n_NodeNum;

        public abstract bool InitCard(ushort CardNum, ushort NodeNum);

        public abstract bool GetAllInput();
        public abstract bool WriteOutput(ushort IoNum, ushort Value);

    }

}
