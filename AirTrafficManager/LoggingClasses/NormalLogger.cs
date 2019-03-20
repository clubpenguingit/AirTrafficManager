using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;
using AirTrafficManager.LoggingClasses;

namespace AirTrafficManager
{
    public class NormalLogger : ILogger
    {
        protected ISeparationCondition SepCond;
        protected string FileName;
        private IInputOutput inputoutputtype;


        public NormalLogger(string name, ISeparationCondition sepCond, IInputOutput inputOutput)
        {
            FileName = name;

            this.SepCond = sepCond;

            this.inputoutputtype = inputOutput;


            this.SepCond.WarningEvent += SepConditionOccured;
        }

        public void SepConditionOccured(object sender, SepCondEventArgs e)
        {
            inputoutputtype.Write(e, FileName);
        }
    }
}
