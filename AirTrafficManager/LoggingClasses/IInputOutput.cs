using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    public interface IInputOutput
    {
        void Write(SepCondEventArgs e, string fileName);

    }
}
