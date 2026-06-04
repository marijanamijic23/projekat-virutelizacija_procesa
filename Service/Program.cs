using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Service
{
    class Program
    {
        static void Main(string[] args)
        {
            DroneService droneService = new DroneService();
            Listener listener = new Listener();
            ServiceHost host = new ServiceHost(droneService);

            droneService.transferStartedEvent += listener.OnTransferStarted;
            droneService.SampleReceivedEvent += listener.OnSampleReceived;
            droneService.transferCompletedEvent += listener.OnTransferCompleted;
            droneService.WarningRaisedEvent += listener.OnWarningRaised;

            host.Open();

            Console.WriteLine("Service is running. Press any key to stop.");

            string file = "file";

            /*TestSimulation test = new TestSimulation();
            test.Test(file);*/

            Console.ReadKey();
            host.Close();
            Console.WriteLine("Service has stopped.");
            
        }
    }
}
