using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework;
using AirTrafficManager;
using AirTrafficManager.Log;
using NSubstitute;

namespace ATMSUnitTests
{
    class SeperationConditionTests
    {
        private AirTrafficManagementSystem _atms;
        [SetUp]
        public void Setup()
        {
            _atms = Substitute.For<AirTrafficManagementSystem>();

            var sepcond = new SeparationCondition(_atms);
        }
    }
}
