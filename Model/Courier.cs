using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    internal class Courier
    {
        public Courier()
        {
            trackingNumber++;
        }
        public  int courierID{ get; set; }
        public string senderName { get; set; }
        public string senderAddress { get; set; }
        public string receiverName { get; set; }
        public string receiverAddress { get; set; }
         public decimal weight { get; set; }
        public string status { get; set; }
        public  int trackingNumber { get; set; }
        public DateTime DeliveryDate { get; set; }
         public int userID { get; set; }
        public int employeeID { get; set; }



    }
}
