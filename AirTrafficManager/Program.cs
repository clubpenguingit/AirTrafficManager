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

            Console.WriteLine("Du kom til at køre Main");
            //var receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            //var system = new TransponderReceiverClient(receiver, new TrackCalculator());
           // system.DataReceivedEvent += test;
           //var r = new Renderer(new Writer());
           //var l = new List<Track>();
           //var t = new Track("123456", 10, 20, 4000, DateTime.Now, 1000, 80);
           //l.Add(t);
           //var t2 = new Track("098765", 100, 200, 5000, DateTime.Now, 2000, 100);
           //l.Add(t2);
           //r.RenderAirCrafts(l, true);
           //r.RenderCondition(t,t2, DateTime.Now);

            while (false)
                System.Threading.Thread.Sleep(1000);
        }
    }
}
