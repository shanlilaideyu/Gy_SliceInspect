using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LeoBase
{
    public interface IDisplay<T>
    {
        void DisplaySourceImage(T img);
    }
}
