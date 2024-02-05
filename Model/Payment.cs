using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    internal class Payment
    {
        public long PaymentID { get; set; }
        public long CourierID { get; set; }
        public double Amount { get; set; }
        public DateTime PaymentDate { get; set; }

    }
}
