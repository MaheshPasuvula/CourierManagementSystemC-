using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service
{
    internal interface ICourierService
    {
        void PlaceOrder();
        void GetOrderStatus();
        void CancelOrder();
        void GetAssigned();
        void CheckingTrackingNumber();
        void AllOrders();
        void OrderStatus();
        void UpdateOrder();
        void DeleteOrder();
        void Order();
        

    }
}
