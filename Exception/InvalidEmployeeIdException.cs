using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Exception
{
    internal class InvalidEmployeeIdException:ApplicationException
    {
        public string message;
        public InvalidEmployeeIdException()
        {
            message = "Invalid EmployeeID Exception";

        }
      public InvalidEmployeeIdException(string message):base(message)
        {

        }
    }
}
