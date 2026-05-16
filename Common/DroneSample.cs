using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace Common
{
    [DataContract]
    public class DroneSample
    {
        double linearAccelerationX;
        double linearAccelerationY;
        double linearAccelerationZ;
        double windSpeed;
        double windAngle;
        double time;

        public DroneSample() : this(0, 0, 0, 0, 0, 0) { }

        public DroneSample(double linearAccelerationX, double linearAccelerationY, double linearAccelerationZ, double windSpeed, double windAngle, double time)
        {
            this.linearAccelerationX = linearAccelerationX;
            this.linearAccelerationY = linearAccelerationY;
            this.linearAccelerationZ = linearAccelerationZ;
            this.windSpeed = windSpeed;
            this.windAngle = windAngle;
            this.time = time;
        }

        [DataMember]
        public double LinearAccelerationX { get => linearAccelerationX; set => linearAccelerationX = value; }
        [DataMember]
        public double LinearAccelerationY { get => linearAccelerationY; set => linearAccelerationY = value; }
        [DataMember]
        public double LinearAccelerationZ { get => linearAccelerationZ; set => linearAccelerationZ = value; }
        [DataMember]
        public double WindSpeed { get => windSpeed; set => windSpeed = value; }
        [DataMember]
        public double WindAngle { get => windAngle; set => windAngle = value; }
        [DataMember]
        public double Time { get => time; set => time = value; }

        public override string ToString()
        {
            return $"DroneSample: LinearAccelerationX={linearAccelerationX}, LinearAccelerationY={linearAccelerationY}, LinearAccelerationZ={linearAccelerationZ}, WindSpeed={windSpeed}, WindAngle={windAngle}, Time={time}";
        }

    }
}
