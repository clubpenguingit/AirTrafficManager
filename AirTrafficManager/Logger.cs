using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Services;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager.Log
{
    abstract class Logger
    {
        protected ISeparationCondition SepCond;
        protected string FileName;

        protected Logger(string name, ISeparationCondition sepCond)
        {
            FileName = name;

            this.SepCond = sepCond;

            this.SepCond.WarningEvent += SepConditionOccured;
        }

        public abstract void SepConditionOccured(object sender, SepCondEventArgs e);
    }
}
