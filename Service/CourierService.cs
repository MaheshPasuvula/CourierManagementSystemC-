using Assignment.Model;
using Assignment.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Service;
using Assignment.Exception;

namespace Assignment.Service
{
    internal class CourierService : ICourierService
    {
        ICourierUserRepository courierRepository = new CourierRepository();
        public void PlaceOrder()
        {

            Console.WriteLine("Enter your Courier Details");
            Console.WriteLine("Enter SenderName");
            string userSenderName = Console.ReadLine();
            Console.WriteLine("Enter SenderAddress");
            string userSenderAddress = Console.ReadLine();
            Console.WriteLine("Enter ReceiverName");
            string userReceiverName = Console.ReadLine();
            Console.WriteLine("Enter ReceiverAddress ");
            string userReceiverAddress = Console.ReadLine();
            Console.WriteLine("Enter Weight");
            decimal weight = Decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter UserID");
            int userID = int.Parse(Console.ReadLine());
            Courier usercourier = new Courier();

            usercourier.senderName = userSenderName;
            usercourier.senderAddress = userSenderAddress;
            usercourier.receiverName = userReceiverName;
            usercourier.receiverAddress = userReceiverAddress;
            usercourier.weight = weight;
            usercourier.userID = userID;
            ICourierService userCourierService = new CourierService();
            Console.WriteLine($"Unique Tracking Number = {courierRepository.PlaceOrder(usercourier)}");
        }
        public void GetOrderStatus()
        {

            Console.WriteLine("enter tracking number");
            int userTrackingNumber = int.Parse(Console.ReadLine());
            string orderstatus = courierRepository.GetOrderStatus(userTrackingNumber);
            if (orderstatus == "")
            {
                Console.WriteLine($"You Entered wrong TracKingNumber Please Check it");
            }
            else
            {
                Console.WriteLine($"Status of courier with Tracking Number {userTrackingNumber} is {courierRepository.GetOrderStatus(userTrackingNumber)}");
            }




        }
        public void CancelOrder()
        {
            Console.WriteLine("enter tracking number");
            int userEnteredTrackingNumber = int.Parse(Console.ReadLine());
            Console.WriteLine("enter tracking number");

            bool recordremoved = courierRepository.CancelOrder(userEnteredTrackingNumber);
            if (recordremoved)
            {
                Console.WriteLine($"Courier with TrackingID {userEnteredTrackingNumber} is Deleted");
            }
            else
            {
                Console.WriteLine($"Courier with TrackingID is Not Found");
            }
        }
        public void GetAssigned()
        {
            Console.WriteLine("enter staffid");
            int userStaffID = int.Parse(Console.ReadLine());
            List<Courier> assignedcourier = courierRepository.GetAssigned(userStaffID);
            foreach (var courier in assignedcourier)
            {
                Console.WriteLine($"courierid: {courier.courierID}, staffid: {courier.employeeID}");
            }
        }

        public void CheckingTrackingNumber()
        {
            Console.WriteLine("Enter your TrackingNumber");
            int userTrackingNumber = int.Parse(Console.ReadLine());
            try
            {
                bool checkTrackingNumber = courierRepository.CheckingTrackingNumber(userTrackingNumber);
                if (checkTrackingNumber)
                {

                    Console.WriteLine($"Courier with TrackingNumber {userTrackingNumber} is Found");
                }
                else
                {
                    throw new TrackingNumberNotFoundException();
                }
            }
            catch (TrackingNumberNotFoundException ex)
            {

                Console.WriteLine(ex.message);
            }

        }
        public void AllOrders()
        {
            courierRepository.AllOrders();

        }
        public void OrderStatus()
        {
            Console.WriteLine("Enter your OrderID");
            int userOrderId = int.Parse(Console.ReadLine());

            courierRepository.OrderStatus(userOrderId);
        }
        public void Order()
        {

            Console.WriteLine("Enter details");
            Console.WriteLine("Enter your ID");
            int userSenderId = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter your Name");
            string userSenderName = Console.ReadLine();
            Console.WriteLine("Enter your Address");
            string userSenderAddress = Console.ReadLine();
            Console.WriteLine("Enter Receiver Name");
            string userReceiverName = Console.ReadLine();
            Console.WriteLine("Enter ReceiverAddress");
            string userReceiverAddress = Console.ReadLine();
            Console.WriteLine("Enter ServiceID");
            int userServiceID = int.Parse(Console.ReadLine());
            Console.WriteLine("Enter Weight");
            decimal userweight = decimal.Parse(Console.ReadLine());

            courierRepository.Order(userSenderId, userSenderName, userSenderAddress, userReceiverName, userReceiverAddress, userServiceID, userweight);


        }
        public void UpdateOrder()
        {
            Console.WriteLine("Enter Your OrderID");
            int userOrderId = int.Parse(Console.ReadLine());
            if (courierRepository.CheckingOrder(userOrderId))
            {

                Console.WriteLine("what Do you want to update");
                Console.WriteLine("1.SenderName");
                Console.WriteLine("2.SenderAddress");
                Console.WriteLine("3.ReceiverName");
                Console.WriteLine("4.ReceiverAddress");
                Console.WriteLine("5.ServiceId");
                Console.WriteLine("6.Weight");
                Console.WriteLine("7.Status");
                Console.WriteLine("8.DeliveryDate");
                Console.WriteLine("9.Delivered EmployeeID");
                Console.WriteLine("Select option");
                int userchoice = int.Parse(Console.ReadLine());
                switch (userchoice)
                {
                    case 1:
                        Console.WriteLine("Enter your new SenderName");
                        string userupdateSenderName = Console.ReadLine();
                        courierRepository.UpdateOrderSenderName(userOrderId, userupdateSenderName);
                        break;
                    case 2:
                        Console.WriteLine("Enter your new SenderAddress");
                        string userupdateSenderAddress = Console.ReadLine();
                        courierRepository.UpdateOrderSenderAddress(userOrderId, userupdateSenderAddress);

                        break;
                    case 3:
                        Console.WriteLine("Enter your new ReceiverName");
                        string userupdateReceiverName = Console.ReadLine();
                        courierRepository.UpdateOrderReceiverName(userOrderId, userupdateReceiverName);
                        break;
                    case 4:
                        Console.WriteLine("Enter your new ReceiverAddress");
                        string userupdateReceiverAddress = Console.ReadLine();
                        courierRepository.UpdateOrderReceiverAddress(userOrderId, userupdateReceiverAddress);
                        break;
                    case 5:
                        Console.WriteLine("Enter your new ServiceId");
                        int userupdateServiceId = int.Parse(Console.ReadLine());
                        courierRepository.UpdateOrderServiceId(userOrderId, userupdateServiceId);
                        break;
                    case 6:
                        Console.WriteLine("Enter your new Weight");
                        decimal userupdatedweight = decimal.Parse(Console.ReadLine());
                        courierRepository.UpdateOrderWeight(userOrderId, userupdatedweight);
                        break;
                    case 7:
                        Console.WriteLine("Enter your new Status");
                        string userupdatestatus = Console.ReadLine();
                        courierRepository.UpdateOrderStatus(userOrderId, userupdatestatus);
                        break;
                    case 8:
                        Console.WriteLine("Enter your new Deliverydate");
                        DateTime userupdateDeliverydate = DateTime.Parse(Console.ReadLine());
                        courierRepository.UpdateOrderDeliveryDate(userOrderId, userupdateDeliverydate);
                        break;
                    case 9:
                        Console.WriteLine("Enter your new DeliveryEmployeeID");
                        int userupdateDeliveryEmployeeID = int.Parse(Console.ReadLine());
                        courierRepository.UpdateOrderDeliveryEmployeeID(userOrderId, userupdateDeliveryEmployeeID);
                        break;





                }
            }
            else
            {
                Console.WriteLine("you Entered Wrong OrderId Please Check it");
            }
        }
        public void DeleteOrder()
        {
            Console.WriteLine("Enter your OrderID");
            int userOrderId = int.Parse(Console.ReadLine());
            string courierdeletemsg=courierRepository.DeleteOrder(userOrderId);
            Console.WriteLine(courierdeletemsg);
        }

    }
}










