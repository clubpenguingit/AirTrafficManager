using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Er opgaven færdig nu?");
            var r = new Renderer(new Writer());
            var t1 = new Track("123456", 1, 1, 1, 1, 1);
            var t2 = new Track("234567", 2, 2, 2, 2, 2);
            r.RenderAirCraft(t1);

            r.RenderCondition(t1,t2,DateTime.Now);
        }
    }
}
