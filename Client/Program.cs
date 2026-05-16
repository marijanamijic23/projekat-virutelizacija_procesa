using Common;
using Service;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

namespace Client
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ChannelFactory<IDrone> drone = new ChannelFactory<IDrone>("DroneTcpBinding");
            IDrone sam = drone.CreateChannel();
            CsvManipulation csv = new CsvManipulation("3.csv");
            CsvManipulation log = new CsvManipulation("log.txt");

            string tekst = csv.ReadAllText();
            string[] niz = tekst.Split('\n');
            sam.startSession("3.csv");
            for (int i = 1; i <= 110; i++)
            {
                try
                {
                    string red = niz[i];
                    string[] reci = red.Split(',');
                    DroneSample droneSample = new DroneSample();
                    droneSample.LinearAccelerationX = double.Parse(reci[18], CultureInfo.InvariantCulture);
                    droneSample.LinearAccelerationY = double.Parse(reci[19], CultureInfo.InvariantCulture);
                    droneSample.LinearAccelerationZ = double.Parse(reci[20], CultureInfo.InvariantCulture);
                    droneSample.WindSpeed = double.Parse(reci[1], CultureInfo.InvariantCulture);
                    droneSample.WindAngle = double.Parse(reci[2], CultureInfo.InvariantCulture);
                    droneSample.Time = DateTime.Parse(reci[0]);
                    sam.pushSample(droneSample);
                }
                catch
                {
                    log.AddTextToFile("Greska u redu:" + i);
                }
            }

            sam.endSession();
            Console.ReadKey();
        }
    }
}

