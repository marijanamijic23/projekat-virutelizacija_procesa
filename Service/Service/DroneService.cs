using Common;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.ServiceModel;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    
    public class DroneService : IDroneService
    {
        SessionStatus status = SessionStatus.IDLE;
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

            status = SessionStatus.IN_PROGRESS;
            Console.WriteLine($"Sesija se pokrece, {status}\n");
            string[] words = meta.Split(',');
            Console.WriteLine($"{words[18]} {words[19]} {words[20]} {words[1]} {words[2]} {words[0]}\n");
        }

        public void PushSample(DroneSample sample)
        {
            if (status != SessionStatus.IN_PROGRESS)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Status mora biti IN_PROGRESS!\n" },
                    new FaultReason("Validation fault")
                );
            }

            if(sample.WindSpeed <= 0)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Wind speed mora biti vrednost veca od 0!\n" },
                    new FaultReason("Validation fault")
                );
            }

            if(sample.WindAngle < 0 || sample.WindAngle > 360)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Vrednost za wind angel izlazi iz dozvoljenog opsega!\n" },
                    new FaultReason("Validation fault")
                );
            }

            Console.WriteLine($"Primljen uzorak: LinearAccelerationX={sample.LinearAccelerationX}, LinearAccelerationY={sample.LinearAccelerationY}, LinearAccelerationZ={sample.LinearAccelerationZ}, WindSpeed={sample.WindSpeed}, WindAngle={sample.WindAngle}, Time={sample.Time}\n");
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

            status = SessionStatus.COMPLETED;
            Console.WriteLine($"Sesija zavrsava {status}\n");
        }


    }
}
