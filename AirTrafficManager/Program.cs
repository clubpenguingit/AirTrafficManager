using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NUnit.Framework.Internal.Execution;
using TransponderReceiver;

namespace AirTrafficManager
{
    class Program
    {
        static void Main(string[] args)
        {

            Console.WriteLine("Du kom til at køre Main, og den kaster snart en exception :)");
            var receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            var system = new TransponderReceiverClient(receiver, new TrackCalculator());
           // system.DataReceivedEvent += test;
            while (true)
                System.Threading.Thread.Sleep(1000);
        }
    }
}
