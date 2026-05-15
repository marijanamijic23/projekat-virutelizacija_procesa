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
    
    public class DroneService : IDrone
    {
        SessionStatus status = SessionStatus.IDLE;
        public void startSession(string meta)
        {
            if (meta == null || meta == "")
            {
                throw new FaultException<DataFormatFault>(
                    new DataFormatFault { message = "Naziv fajla ne postoji!"}
                );
            }
            if(status == SessionStatus.IN_PROGRESS)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Sesija je vec pokrenuta!"}
                );
            }

            status = SessionStatus.IN_PROGRESS;
            Console.WriteLine($"Sesija se pokrece, {status}");
        }

        public void pushSample(DroneSample sample)
        {
            if(status != SessionStatus.IN_PROGRESS)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Status mora biti IN_PROGRESS!"}
                );
            }

            if(sample.WindSpeed <= 0)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Wind speed mora biti vrednost veca od 0!" }
                );
            }

            if(sample.WindAngle < 0 || sample.WindAngle > 360)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Vrednost za wind angel izlazi iz dozvoljenog opsega!" }
                );
            }

            if(sample.Time == null)
            {
                throw new FaultException<DataFormatFault>(
                    new DataFormatFault { message = "Vrednost za datum mora postojati!" }
                );
            }

            Console.WriteLine($"Primljen uzorak: LinearAccelerationX={sample.LinearAccelerationX}, LinearAccelerationY={sample.LinearAccelerationY}, LinearAccelerationZ={sample.LinearAccelerationZ}, WindSpeed={sample.WindSpeed}, WindAngle={sample.WindAngle}, Time={sample.Time}");
        }

        public void endSession()
        {
            if(status != SessionStatus.IN_PROGRESS)
            {
                throw new FaultException<ValidationFault>(
                    new ValidationFault { message = "Sesija nije pokrenuta!"}
                );
            }

            status = SessionStatus.COMPLETED;
            Console.WriteLine($"Sesija zavrsava {status}");
        }


    }
}
