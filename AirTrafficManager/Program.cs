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
            var receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();

            TransponderReceiverClient client = new TransponderReceiverClient(receiver, new TrackCalculator());
            AirTrafficManagementSystem atms = new AirTrafficManagementSystem(
                new MonitoredAirspace(95000, 5000, 95000, 5000, 20000, 500),
                client, new TrackCalculator());

            var sepcond = new SeparationCondition(atms);
            IRenderer rend = new Renderer(new Writer(),sepcond , 
                atms);
            var logger = new NormalLogger("loger_file.txt",sepcond, new FileLogger());

            while (true)
                System.Threading.Thread.Sleep(1000);

       
        }
    }
}
