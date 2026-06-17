using Common;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Service
{
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single)]
    public class DroneService : IDroneService
    {
        SessionStatus status = SessionStatus.IDLE;
        SessionData sessionData;
        CsvManipulation csvSession;
        CsvManipulation csvRejects;
        private double prevAz = double.NaN;
        private double azSum = 0;
        private int azCount = 0;

        public delegate void DroneHandler();
        public delegate void DroneSampleHandler(DroneSample sample);
        public delegate void DroneWarningHandler(string warningType);

        public event DroneHandler transferStartedEvent;
        public event DroneSampleHandler SampleReceivedEvent;
        public event DroneHandler transferCompletedEvent;
        public event DroneWarningHandler WarningRaisedEvent;

        public void StartSession(string meta)
        {
            if (meta == null || meta == "")
            {
                throw new FaultException<DataFormatFault>(
                    new DataFormatFault { message = "Naziv fajla ne postoji!\n"},
                    new FaultReason("Data Format Fault")
                );
            }
            if(status == SessionStatus.IN_PROGRESS)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Sesija je vec pokrenuta!\n"},
                    new FaultReason("Validation fault")
                );
            }

            sessionData = new SessionData();
            sessionData.SessionFilePath = "measurements_session.csv";
            sessionData.RejectsFilePath = "rejected.csv";

            csvSession = new CsvManipulation(sessionData.SessionFilePath);
            csvRejects = new CsvManipulation(sessionData.RejectsFilePath);

            if(transferStartedEvent != null)
            {
                transferStartedEvent();
            }

            status = SessionStatus.IN_PROGRESS;
            Console.WriteLine($"Sesija se pokrece, {status}\n");
            string[] words = meta.Split(',');
            Console.WriteLine($"{words[18]} {words[19]} {words[20]} {words[1]} {words[2]} {words[0]}\n");
        }

        public void PushSample(DroneSample sample)
        {
            Console.WriteLine("prenos u toku...");

            if (status != SessionStatus.IN_PROGRESS)
            { 
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Status mora biti IN_PROGRESS!\n" },
                    new FaultReason("Validation fault")
                );
            }

            if(sample.WindSpeed <= 0)
            {
                csvRejects.AddTextToFile(sample.LinearAccelerationX.ToString() + "," + sample.LinearAccelerationY.ToString() + "," + sample.LinearAccelerationZ.ToString() + "," + sample.WindSpeed.ToString() + "," + sample.WindAngle.ToString() + "," + sample.Time.ToString());
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Wind speed mora biti vrednost veca od 0!\n" },
                    new FaultReason("Validation fault")
                );
            }

            if(sample.WindAngle < 0 || sample.WindAngle > 360)
            {
                csvRejects.AddTextToFile(sample.LinearAccelerationX.ToString() + "," + sample.LinearAccelerationY.ToString() + "," + sample.LinearAccelerationZ.ToString() + "," + sample.WindSpeed.ToString() + "," + sample.WindAngle.ToString() + "," + sample.Time.ToString());
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Vrednost za wind angel izlazi iz dozvoljenog opsega!\n" },
                    new FaultReason("Validation fault")
                );
            }

            if(SampleReceivedEvent != null)
            {
                SampleReceivedEvent(sample);
            }
            csvSession.AddTextToFile(sample.LinearAccelerationX.ToString() + "," + sample.LinearAccelerationY.ToString() + "," + sample.LinearAccelerationZ.ToString() + "," + sample.WindSpeed.ToString() + "," + sample.WindAngle.ToString() + "," + sample.Time.ToString());
            Console.WriteLine($"Primljen uzorak: LinearAccelerationX={sample.LinearAccelerationX}, LinearAccelerationY={sample.LinearAccelerationY}, LinearAccelerationZ={sample.LinearAccelerationZ}, WindSpeed={sample.WindSpeed}, WindAngle={sample.WindAngle}, Time={sample.Time}\n");

            if (!double.IsNaN(prevAz))
            {
                double deltaAz = sample.LinearAccelerationZ - prevAz;
                if (Math.Abs(deltaAz) > double.Parse(ConfigurationManager.AppSettings["Az_threshold"], CultureInfo.InvariantCulture))
                {
                    if (WarningRaisedEvent != null)
                    {
                        if (deltaAz > 0)
                        {
                            WarningRaisedEvent($"AltitudeDropSpike - nagli skok");
                        }
                        else
                        {
                            WarningRaisedEvent("AltitudeDropSpike - nagli pad");
                        }
                    }
                }
            }

            azSum += sample.LinearAccelerationZ;
            azCount++;
            double azMean = azSum / azCount;

            if(sample.LinearAccelerationZ < 0.75 * azMean)
            {
                if (WarningRaisedEvent != null)
                {
                    WarningRaisedEvent("OutOfBandWarning - ispod ocekivane vrednosti");
                }
            }
            else if(sample.LinearAccelerationZ > 1.25 * azMean)
            {
                if (WarningRaisedEvent != null)
                {
                    WarningRaisedEvent("OutOfBandWarning - iznad ocekivane vrednosti");
                }
            }

            double Wkinetic = 0.5 * sample.WindSpeed*sample.WindSpeed;

            if(Wkinetic > double.Parse(ConfigurationManager.AppSettings["W_threshold"], CultureInfo.InvariantCulture))
            {
                if (WarningRaisedEvent != null)
                {
                    WarningRaisedEvent("WindEnergySpike - iznad očekivanog");
                }
            }

            prevAz = sample.LinearAccelerationZ;
        }

        public void EndSession()
        {
            if(status != SessionStatus.IN_PROGRESS)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Sesija nije pokrenuta!\n" },
                    new FaultReason("Validation fault")
                );
            }

            if(transferCompletedEvent != null)
            {
                transferCompletedEvent();
            }

            status = SessionStatus.COMPLETED;
            Console.WriteLine("zavrsen prenos");
            Console.WriteLine($"Sesija zavrsava {status}\n");
        }


    }
}
