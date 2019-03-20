using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    public interface ISeparationCondition
    {
        event EventHandler<SepCondEventArgs> WarningEvent;
        event EventHandler<RendEventArgs> RendererWarning; 

        void CheckForCondition(Track t1, Track t2);
    }

}
