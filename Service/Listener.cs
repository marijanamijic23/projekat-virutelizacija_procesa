using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class Listener
    {
        public void OnTransferStarted()
        {
            Console.WriteLine("TRANSFER STARTED!");
            Console.WriteLine();
        }

        public void OnSampleReceived(DroneSample sample)
        {
            Console.WriteLine("SAMPLE RECEIVED!");
            Console.WriteLine($"Az={sample.LinearAccelerationZ}, WindSpeed={sample.WindSpeed}");
            Console.WriteLine();
        }

        public void OnTransferCompleted()
        {
            Console.WriteLine("TRANSFER COMPLETED!");
            Console.WriteLine();
        }

        public void OnWarningRaised(string warningType)
        {
            Console.WriteLine("WARNING RAISED!");
            Console.WriteLine($"Warning type: {warningType}");
            Console.WriteLine();
        }
    }
}
