using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Exception
{
    internal class OrderNotFoundException:ApplicationException
    {
        public string Message;

        public OrderNotFoundException()
        {
            Message = "OrderNumber  Not Found Exception ";

        }
        public OrderNotFoundException(string message) : base(message)
        {

        }
    }
}
