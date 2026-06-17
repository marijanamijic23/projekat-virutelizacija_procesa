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

            ChannelFactory<IDroneService> drone = new ChannelFactory<IDroneService>("DroneTcpBinding");
            IDroneService sam = drone.CreateChannel();
            CsvManipulation csv = new CsvManipulation("3.csv");
            CsvManipulation log = new CsvManipulation("log.txt");

            string text = csv.ReadAllText();
            string[] array = text.Split('\n');
            DroneSample sample = new DroneSample();
            try
            {
                string meta = array[0];
                //string meta = "";
                //string meta = null;
                sam.StartSession(meta);
                Console.WriteLine("Session started, sending data...");
                for (int i = 1; i <= 110; i++)
                {
                    string line = array[i];
                    DroneSample droneSample = new DroneSample();
                    try
                    {


                        string[] words = line.Split(',');
                        if (!string.IsNullOrEmpty(words[18])) droneSample.LinearAccelerationX = double.Parse(words[18], CultureInfo.InvariantCulture);
                        if (!string.IsNullOrEmpty(words[19])) droneSample.LinearAccelerationY = double.Parse(words[19], CultureInfo.InvariantCulture);
                        if (!string.IsNullOrEmpty(words[20])) droneSample.LinearAccelerationZ = double.Parse(words[20], CultureInfo.InvariantCulture);
                        if (!string.IsNullOrEmpty(words[1])) droneSample.WindSpeed = double.Parse(words[1], CultureInfo.InvariantCulture);
                        if (!string.IsNullOrEmpty(words[2])) droneSample.WindAngle = double.Parse(words[2], CultureInfo.InvariantCulture);
                        if (!string.IsNullOrEmpty(words[0])) droneSample.Time = double.Parse(words[0], CultureInfo.InvariantCulture);
                        sam.PushSample(droneSample);
                    }
                    catch (FaultException<ValidationFault> ex)
                    {
                        log.AddTextToFile($"ERROR -> X:{droneSample.LinearAccelerationX}, Y:{droneSample.LinearAccelerationY}, Z:{droneSample.LinearAccelerationZ}, WindSpeed:{droneSample.WindSpeed}, WindAngle:{droneSample.WindAngle}, Time:{droneSample.Time} | {ex.Detail.message}");
                    }

                }

                sam.EndSession();
                Console.ReadKey();
            }
            catch (FaultException<DataFormatFault> ex)
            {
                log.AddTextToFile(ex.Detail.message);
            }
        }
    }
}

