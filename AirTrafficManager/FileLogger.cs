using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AirTrafficManager
{
    class FileLogger : IInputOutput
    {
        public void Write(SepCondEventArgs e, string fileName)
        {
            using (StreamWriter logfile = new StreamWriter(fileName, true))
            {
                logfile.WriteLine($"{e.TimeOfOccurrence} - Separation Condition Error between tags: " +
                                  $"{e.Track1.Tag} and {e.Track2.Tag}");
            }
        }
    }
}
