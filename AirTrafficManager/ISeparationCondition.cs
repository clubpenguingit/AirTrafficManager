using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    #region WarningEvent arguments

    

    public class SepCondEventArgs : EventArgs
    {
        public Track Track1 { get; set; }
        public Track Track2 { get; set; }
        public DateTime TimeOfOccurrence { get; set; }
    }
    #endregion

    public interface ISeparationCondition
    {
        event EventHandler<SepCondEventArgs> WarningEvent;
    }
}
