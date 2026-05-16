using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class TestSimulation
    {
        public void Test(string path)
        {
            using (CsvManipulation csv = new CsvManipulation(path))
            {
                try
                {
                    DroneService droneService = new DroneService();
                    DroneSample sample = new DroneSample();
                    sample.LinearAccelerationX = 13;
                    sample.LinearAccelerationY = 12;
                    sample.LinearAccelerationZ = 15;
                    sample.WindAngle = 90;
                    sample.WindSpeed = -77;
                    sample.Time = 1.5;
                    droneService.StartSession(path);
                    droneService.PushSample(sample);
                }
                catch
                {
                    Console.WriteLine("Nagli prekid!");
                }
            }
        }
    }
}
