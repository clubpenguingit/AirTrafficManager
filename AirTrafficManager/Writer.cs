using System;

namespace AirTrafficManager
{
    public class Writer : IWriter
    {
        public void Write(string s)
        {
            Console.WriteLine(s);
        }
    }
}