using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Model;
using Assignment.Service;

namespace Assignment.Repository
{
    internal interface ICourierUserRepository
    {
        int PlaceOrder(Courier usercourier);
        string GetOrderStatus(int userTrackingNumber);
        bool CancelOrder(int userTrackingNumber);
        List<Courier> GetAssigned(int userStaffID);
        bool CheckingTrackingNumber(int userTrackingNumber);
        void AllOrders();
        void OrderStatus(int userOrderId);
        void Order(int userSenderId, string userSenderName, string userSenderAddress, string userReceiverName, string userReceiverAddress, int userServiceID, decimal userweight);
        string DeleteOrder(int userOrderId);

        bool CheckingOrder(int userOrderId);
        void UpdateOrderSenderName(int userOrderId, string userupdateSenderName);
        void UpdateOrderSenderAddress(int userOrderId, string userupdateSenderAddress);
        void UpdateOrderReceiverName(int userOrderId, string userupdateReceiverName);
        void UpdateOrderReceiverAddress(int userOrderId, string userupdateReceiverAddress);
        void UpdateOrderServiceId(int userOrderId, int userupdateServiceId);
        void UpdateOrderWeight(int userOrderId, decimal userupdatedweight);
        void UpdateOrderStatus(int userOrderId, string userupdatestatus);
        void UpdateOrderDeliveryDate(int userOrderId, DateTime userupdateDeliverydate);
        void UpdateOrderDeliveryEmployeeID(int userOrderId, int userupdateDeliveryEmployeeID);

    }
}
