using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    public class RendEventArgs : EventArgs
    {
        public List<SepCondEventArgs> ListOfCurrentConditions;
        public DateTime TimeOfEvent { get; set; }
    }
}
