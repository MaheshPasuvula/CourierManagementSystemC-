using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Exception
{
    internal class TrackingNumberNotFoundException : ApplicationException
    {
        public string message;

        public TrackingNumberNotFoundException() 
        {
            message = "TrackingNumber Not Found Exception ";

        }
        public TrackingNumberNotFoundException(string message):base(message)
        {
            
        }
    }
}
