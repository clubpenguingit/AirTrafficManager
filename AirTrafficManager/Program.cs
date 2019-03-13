using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TransponderReceiver;

namespace AirTrafficManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Du kom til at køre Main");
            var receiver = TransponderReceiverFactory.CreateTransponderDataReceiver();
            var system = new TransponderReceiverClient(receiver);

            while (true)
                System.Threading.Thread.Sleep(1000);
        }
    }
}
