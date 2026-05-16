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
            string[] array = tekst.Split('\n');
            DroneSample sample = new DroneSample();
            string meta = array[0];
            sam.StartSession(meta);
            for (int i = 1; i <= 110; i++)
            {
                try
                {
                    string red = array[i];
                    string[] words = red.Split(',');
                    DroneSample droneSample = new DroneSample();
                    if (!string.IsNullOrEmpty(words[18]))droneSample.LinearAccelerationX = double.Parse(words[18], CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(words[19])) droneSample.LinearAccelerationY = double.Parse(words[19], CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(words[20])) droneSample.LinearAccelerationZ = double.Parse(words[20], CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(words[1])) droneSample.WindSpeed = double.Parse(words[1], CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(words[2])) droneSample.WindAngle = double.Parse(words[2], CultureInfo.InvariantCulture);
                    if (!string.IsNullOrEmpty(words[0])) droneSample.Time = double.Parse(words[0], CultureInfo.InvariantCulture);
                    sam.PushSample(droneSample);
                }
                catch (FaultException<ValidationFault> ex)
                {
                    log.AddTextToFile(ex.Detail.message);
                }
                catch (FaultException<DataFormatFault> ex)
                {
                    log.AddTextToFile(ex.Detail.message);
                }
                catch (FaultException ex)
                {
                    log.AddTextToFile(ex.Message);
                }
                catch (Exception ex)
                {
                    log.AddTextToFile(ex.Message);
                }
            }

            sam.EndSession();
            Console.ReadKey();
        }
    }
}

