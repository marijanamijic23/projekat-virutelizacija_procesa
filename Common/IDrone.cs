using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ServiceModel;

namespace Common
{
    [ServiceContract]
    public interface IDrone
    {
        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(DataFormatFault))]
        void StartSession(string meta);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(DataFormatFault))]
        void PushSample(DroneSample sample);

        [OperationContract]
        [FaultContract(typeof(ValidationFault))]
        [FaultContract(typeof(DataFormatFault))]
        void EndSession();
    }
}
