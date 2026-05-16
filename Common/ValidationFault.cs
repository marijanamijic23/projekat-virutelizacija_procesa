using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    [DataContract]
    public class ValidationFault
    {
        [DataMember]
        public string message { get; set; } 
    }

}
