using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    public class NormalLogger : Logger
    {
        private IInputOutput inputoutputtype;
        public NormalLogger(string name, ISeparationCondition sepCond, IInputOutput inputOutput) : base(name, sepCond)
        {
            inputoutputtype = inputOutput;
        }

        public override void SepConditionOccured(object sender, SepCondEventArgs e)
        {
            inputoutputtype.Write(e, FileName);
        }
    }
}
