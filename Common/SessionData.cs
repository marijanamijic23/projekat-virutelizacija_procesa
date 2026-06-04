using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public class SessionData
    {
        public string SessionFilePath { get; set; }
        public string RejectsFilePath { get; set; }
        public SessionData(string sessionFilePath, string rejectsFilePath)
        {
            SessionFilePath = sessionFilePath;
            RejectsFilePath = rejectsFilePath;
        }

        public SessionData() { }
    }
}
