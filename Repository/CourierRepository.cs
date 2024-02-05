using Assignment.Model;
using Assignment.Utility;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using ConsoleTables;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Exception;


namespace Assignment.Repository
{
    internal class CourierRepository : ICourierUserRepository
    {
        private static int uniqueTrackingNumber = 7000;//have to change this
        private static int uniqueCourierId = 2000;

        public string connectionString;
        SqlConnection sqlconnection = null;
        SqlCommand cmd = null;
        public CourierRepository()
        {
            sqlconnection = new SqlConnection(DBConnection.GetConnectionString());
            connectionString = DBConnection.GetConnectionString();
            cmd = new SqlCommand();
        }
        private bool OrderExists(int userOrderId)
        {
            // Check if the employee exists in your database using a SELECT query
            // Return true if exists, false otherwise
            // You should replace this with your actual database logic

            cmd.CommandText = "SELECT COUNT(*) FROM Courier WHERE COURIERID=@COURIERID";
            cmd.Parameters.Clear();
            cmd.Parameters.AddWithValue("@COURIERID", userOrderId);
            cmd.Connection = sqlconnection;
            sqlconnection.Open();


            int count = (int)cmd.ExecuteScalar();
            sqlconnection.Close();
            if (count > 0)
            {
                return true;
            }
            return false;


        }

        List<Courier> courier = new List<Courier>()
       {
            new Courier{courierID=1,senderName="Mahesh",senderAddress="Hyderabad",receiverName="Umesh",receiverAddress="Rangareddy",weight=235.90m,status="delivered",trackingNumber=1234,DeliveryDate=new DateTime(2022,8,2),userID=5,employeeID=2 },
            new Courier{courierID=2,senderName="Umesh",senderAddress="Vijayawada",receiverName="Surya",receiverAddress="KarimNagar",weight=400.00m,status="Yet to Transit",trackingNumber=1235,DeliveryDate=new DateTime(2023,8,22),userID=3,employeeID=3 },
            new Courier{courierID=3,senderName="Suresh",senderAddress="Khammam",receiverName="Nagesh",receiverAddress="Guntur",weight=567.89m,status="In Transit",trackingNumber=1236,DeliveryDate=new DateTime(2024,2,12),userID=4,employeeID=2 },
            new Courier{courierID=4,senderName="Suresh",senderAddress="Khammam",receiverName="Nagesh",receiverAddress="Guntur",weight=345.89m,status="delivered",trackingNumber=1237,DeliveryDate=new DateTime(2024,1,12),userID=4,employeeID=5 },
            new Courier{courierID=5,senderName="Govind",senderAddress="KarimNagar",receiverName="Jagadessh",receiverAddress="Warangal",weight=567.89m,status="Yet to Transit",trackingNumber=1238,DeliveryDate=new DateTime(2024,2,12),userID=7,employeeID=2 }


       };
        public int PlaceOrder(Courier usercourier)
        {
            usercourier.trackingNumber = uniqueTrackingNumber++;
            usercourier.courierID = uniqueCourierId++;
            usercourier.status = "Yet to Transit";
            usercourier.DeliveryDate = DateTime.Now.AddDays(7);

            return usercourier.trackingNumber;
        }
        public string GetOrderStatus(int userTrackingNumber)
        {
            string orderstatus = "";
            foreach (var couriersrecords in courier)
            {
                if (couriersrecords.trackingNumber == userTrackingNumber)
                {
                    orderstatus = couriersrecords.status;
                }
            }
            return orderstatus;


        }
        public bool CancelOrder(int userTrackingNumber)
        {
            foreach (var couriersrecords in courier)
            {
                courier.RemoveAll(courier => courier.trackingNumber == userTrackingNumber);
                return true;
            }
            return false;
        }
        public List<Courier> GetAssigned(int userStaffID)
        {
            List<Courier> matchingCouriers = courier.Where(courier => courier.employeeID == userStaffID).ToList();

            return matchingCouriers;
        }
        public bool CheckingTrackingNumber(int userTrackingNumber)
        {

            Courier trackingNumberFound = courier.Find(courier => courier.trackingNumber == userTrackingNumber);
            if (trackingNumberFound != null)
            {
                return true;
            }
            return false;


        }
        public bool CheckingOrder(int userOrderId)
        {
            bool OrderFound = false;

            cmd.CommandText = "SELECT * FROM Courier";
            cmd.Connection = sqlconnection;
            sqlconnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
            {


                if ((int)reader["CourierID"] == userOrderId)
                {

                    OrderFound = true;
                    break;
                }
            }

            sqlconnection.Close();
            return OrderFound;
        }
        public void OrderStatus(int userOrderId)
        {
            cmd.CommandText = $"select * from Courier where CourierID={userOrderId}";
            cmd.Connection = sqlconnection;
            sqlconnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();




            int flag = 0;
            // Display data
            while (reader.Read())
            {


                if ((int)reader["CourierID"] == userOrderId)
                {

                    flag = 1;

                    string emailConformationmsg = $"Dear user,\n\n"
                                   + $"Thank you for your order! Your order status follows:\n\n"
                                   + $"Order Number: {userOrderId}\n"
                                   + $"Order Status: {reader["Status"]}\n"
                                   + "We appreciate your business.\n\n"
                                   + "Sincerely,\nFlipKart\nflipkart@gmail.com";

                    Console.WriteLine(emailConformationmsg);

                }
            }
            if (flag == 0)
            {
                Console.WriteLine($"The status Of Order with {userOrderId} is Not Found");

            }

            sqlconnection.Close();

        }
        public int UniqueTrackingNumberGenerator()
        {
            object trackingNumber = null;
            try
            {
                cmd.CommandText = "SELECT MAX(TrackingNumber) FROM Courier";
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                trackingNumber = cmd.ExecuteScalar();

                // Check for DBNull in case there are no rows in the table
                if (trackingNumber == DBNull.Value)
                {
                    trackingNumber = 0; // or any default value
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }

            finally
            {
                sqlconnection.Close();
            }
            return Convert.ToInt32(trackingNumber);
        }

        public int GetMaxCourierID()
        {
            object courierId = null;

            try
            {
                // Initialize a new SqlCommand object
                cmd.CommandText = "SELECT MAX(CourierID) FROM COURIER";
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                courierId = cmd.ExecuteScalar();
                // Check for DBNull in case there are no rows in the table
                if (courierId == DBNull.Value)
                {
                    courierId = 0; // or any default value
                }
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            finally
            {

                sqlconnection.Close();

            }
            Console.WriteLine(courierId);
            return Convert.ToInt32(courierId);


        }
        public void AllOrders()
        {
            cmd.CommandText = $"select * from Courier";
            cmd.Connection = sqlconnection;
            sqlconnection.Open();
            SqlDataReader reader = cmd.ExecuteReader();
            if (reader.HasRows)
            {
                var table = new ConsoleTable("COURIERID", "SENDERID", "SENDERNAME", "SENDERADDRESS", "RECEIVERNAME", "RECEIVERADDRESS", "SERVICEID", "WEIGHT", "STATUS", "TRACKINGNUMBER", "DELIVERYDATEDATE", "DELIVEREDEMPLOYEEID");

                while (reader.Read())
                {
                    // Add rows to the ConsoleTable
                    table.AddRow(
                        reader["COURIERID"],
                        reader["SENDERID"],
                        reader["SENDERNAME"],
                        reader["SENDERADDRESS"],
                        reader["RECEIVERNAME"],
                        reader["RECEIVERADDRESS"],
                        reader["SERVICEID"],
                        reader["WEIGHT"],
                        reader["STATUS"],
                        reader["TRACKINGNUMBER"],
                        reader["DELIVERYDATE"],
                        reader["DELIVEREDEMPLOYEEID"]
                    );
                }
                string tableContent = table.ToString();

                // Return the table content as a string
                Console.WriteLine(tableContent); ;
            }
            sqlconnection.Close();

        }
        public void Order(int userSenderId, string userSenderName, string userSenderAddress, string userReceiverName, string userReceiverAddress, int userServiceID, decimal userweight)
        {
            string emailConformationmsg = null;
            try
            {
                using (SqlCommand cmd = new SqlCommand())
                {
                    cmd.Connection = sqlconnection;

                    cmd.CommandText = "insert into Courier(SenderID,SenderName,SenderAddress,ReceiverName,ReceiverAddress,ServiceID,weight,status,TrackingNumber,DeliveryDate,DeliveredEmployeeID)" +
                     "values(@SenderID, @SenderName, @SenderAddress, @ReceiverName, @ReceiverAddress, @ServiceID, @Weight, @status, @TrackingNumber, @DeliveryDate, 9)";
                    cmd.Parameters.AddWithValue("@SenderID", userSenderId);
                    cmd.Parameters.AddWithValue("@SenderName", userSenderName);
                    cmd.Parameters.AddWithValue("@SenderAddress", userSenderAddress);
                    cmd.Parameters.AddWithValue("@ReceiverName", userReceiverName);
                    cmd.Parameters.AddWithValue("@ReceiverAddress", userReceiverAddress);
                    cmd.Parameters.AddWithValue("@ServiceID", userServiceID);
                    cmd.Parameters.AddWithValue("@Weight", userweight);
                    cmd.Parameters.AddWithValue("@status", "Processing");

                    int trackingNumbergen = UniqueTrackingNumberGenerator();
                    cmd.Parameters.AddWithValue("@TrackingNumber", ++trackingNumbergen);
                    cmd.Parameters.AddWithValue("@DeliveryDate", DateTime.Now.AddDays(7));


                    sqlconnection.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    if (rowsAffected > 0)
                    {
                        emailConformationmsg = $"Dear {userSenderName},\n\n"
                                           + $"Thank you for your order! Your order details are as follows:\n\n"
                                           + $"ReceiverName:{userReceiverName}\n"
                                           + $"Weight:{userweight}\n"
                                           + $"Delivery Address: {userReceiverAddress}\n"
                                           + $"Expected Delivery Date: {DateTime.Now.AddDays(7).ToString("MMMM dd, yyyy")}\n\n"
                                           + "We appreciate your business.\n\n"
                                           + "Sincerely,\nFlipKart\nflipkart@gmail.com";
                    }
                    else
                    {
                        Console.WriteLine("No rows were affected. Check your query and parameters.");
                    }
                }
            }
            catch (SqlException ex)
            {
                if (ex.Number == 547)
                {
                    Console.WriteLine("Foreign key violation detected. Make sure the referenced data exists.");
                    // Handle the foreign key violation as needed
                    // You can log the error, inform the user, or take appropriate actions
                }
                else
                {
                    Console.WriteLine("Error code: " + ex.Number + ", Message: " + ex.Message);
                }
            }
            finally
            {

                sqlconnection.Close();
            }


            Console.WriteLine(emailConformationmsg);

        }
        public void UpdateOrderSenderName(int userOrderId, string userupdateSenderName)
        {
            try
            {
                cmd.CommandText = $"update Courier set SenderName=@SenderName  where CourierId=@CourierID";

                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                // Execute the update command
                cmd.ExecuteNonQuery();
                string emailConformationmsg = $"Dear user,\n\n"

                                        + $"SenderName is Updated to: {userupdateSenderName}\n"

                                        + "Sincerely,\nFlipKart\nflipkart@gmail.com";

                Console.WriteLine(emailConformationmsg);
            }
            catch (System.Exception ex)
            {
                // Handle exceptions (log, display an error message, etc.)
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Close the connection in the finally block to ensure it is closed even in case of an exception
                sqlconnection.Close();
            }

        }
        public void UpdateOrderSenderAddress(int userOrderId, string userupdateSenderAddress)
        {
            try
            {
                cmd.CommandText = $"update Courier set SenderAddress=@SenderAddress  where CourierId=@CourierId";
                // Add parameters to the command
                cmd.Parameters.AddWithValue("@SenderAddress", userupdateSenderAddress);
                cmd.Parameters.AddWithValue("@CourierId", userOrderId);

                cmd.Connection = sqlconnection;
                sqlconnection.Open();

                // Execute the update command
                cmd.ExecuteNonQuery();

                string emailConformationmsg = $"Dear user,\n\n"
                                             + $"SenderAddress is Updated to: {userupdateSenderAddress}\n"
                                             + "Sincerely,\nFlipKart\nflipkart@gmail.com";

                Console.WriteLine(emailConformationmsg);
            }
            catch (System.Exception ex)
            {
                // Handle exceptions (log, display an error message, etc.)
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Close the connection in the finally block to ensure it is closed even in case of an exception
                sqlconnection.Close();
            }

        }
        public void UpdateOrderReceiverName(int userOrderId, string userupdateReceiverName)
        {
            try
            {
                cmd.CommandText = $"update Courier set ReceiverName=@ReceiverName  where CourierId=@CourierID";
                cmd.Parameters.AddWithValue("@ReceiverName", userupdateReceiverName);
                cmd.Parameters.AddWithValue("@CourierId", userOrderId);

                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                // Execute the update command
                cmd.ExecuteNonQuery();
                string emailConformationmsg = $"Dear user,\n\n"

                                        + $"ReceiverName is Updated to: {userupdateReceiverName}\n"

                                        + "Sincerely,\nFlipKart\nflipkart@gmail.com";

                Console.WriteLine(emailConformationmsg);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine(ex);
            }
            finally
            {
                // Close the connection in the finally block to ensure it is closed even in case of an exception
                sqlconnection.Close();
            }


        }
        public void UpdateOrderReceiverAddress(int userOrderId, string userupdateReceiverAddress)
        {
            try
            {
                cmd.CommandText = "UPDATE Courier SET ReceiverAddress = @ReceiverAddress WHERE CourierId = @CourierId";

                // Add parameters to the command
                cmd.Parameters.AddWithValue("@ReceiverAddress", userupdateReceiverAddress);
                cmd.Parameters.AddWithValue("@CourierId", userOrderId);

                cmd.Connection = sqlconnection;
                sqlconnection.Open();

                // Execute the update command
                cmd.ExecuteNonQuery();

                string emailConformationmsg = $"Dear user,\n\n"
                                             + $"ReceiverAddress is Updated to: {userupdateReceiverAddress}\n"
                                             + "Sincerely,\nFlipKart\nflipkart@gmail.com";

                Console.WriteLine(emailConformationmsg);
            }
            catch (System.Exception ex)
            {
                // Handle exceptions (log, display an error message, etc.)
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Close the connection in the finally block to ensure it is closed even in case of an exception
                sqlconnection.Close();
            }

        }
        public void UpdateOrderServiceId(int userOrderId, int userupdateServiceId)
        {
            try
            {
                cmd.CommandText = $"update Courier set ServiceID={userupdateServiceId}  where CourierId={userOrderId}";

                cmd.Parameters.AddWithValue("@ServiceID", userupdateServiceId);
                cmd.Parameters.AddWithValue("@CourierID", userOrderId);

                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                // Execute the update command
                cmd.ExecuteNonQuery();

                string emailConformationmsg = $"Dear user,\n\n"

                                        + $"ServiceID is Updated to: {userupdateServiceId}\n"

                                        + "Sincerely,\nFlipKart\nflipkart@gmail.com";

                Console.WriteLine(emailConformationmsg);
            }
            catch (System.Exception ex)
            {
                // Handle exceptions (log, display an error message, etc.)
                Console.WriteLine($"Error: {ex.Message}");
            }
            finally
            {
                // Close the connection in the finally block to ensure it is closed even in case of an exception
                sqlconnection.Close();
            }


        }
        public void UpdateOrderWeight(int userOrderId, decimal userupdateweight)
        {
            try
            {
                cmd.CommandText = $"update Courier set Weight=@Weight  where CourierId=@CourierID";
                cmd.Parameters.AddWithValue("@Weight", userupdateweight);
                cmd.Parameters.AddWithValue("@CourierID", userOrderId);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                cmd.ExecuteNonQuery();

                string emailConformationmsg = $"Dear user,\n\n"

                                        + $"Weight is Updated to: {userupdateweight}\n"

                                        + "Sincerely,\nFlipKart\nflipkart@gmail.com";

                Console.WriteLine(emailConformationmsg);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

            }
            finally
            {
                // Close the connection in the finally block to ensure it is closed even in case of an exception
                sqlconnection.Close();
            }

        }
        public void UpdateOrderStatus(int userOrderId, string userupdatestatus)
        {
            try
            {
                cmd.CommandText = $"update Courier set Status=@Status where CourierId=@CourierID";
                cmd.Parameters.AddWithValue("@Status", userupdatestatus);
                cmd.Parameters.AddWithValue("@CourierID", userOrderId);
                cmd.Connection = sqlconnection;
                cmd.ExecuteNonQuery();

                sqlconnection.Open();
                string emailConformationmsg = $"Dear user,\n\n"

                                        + $"Status is Updated to: {userupdatestatus}\n"

                                        + "Sincerely,\nFlipKart\nflipkart@gmail.com";

                Console.WriteLine(emailConformationmsg);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

            }
            finally
            {
                // Close the connection in the finally block to ensure it is closed even in case of an exception
                sqlconnection.Close();
            }

        }
        public void UpdateOrderDeliveryDate(int userOrderId, DateTime userupdateDeliverydate)
        {
            try
            {
                cmd.CommandText = $"update Courier set DeliveyDate=@DeliveryDate  where CourierId=@CourierID";
                cmd.Parameters.AddWithValue("@DeliveryDate", userupdateDeliverydate);
                cmd.Parameters.AddWithValue("@CourierID", userOrderId);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                cmd.ExecuteNonQuery();

                string emailConformationmsg = $"Dear user,\n\n"

                                        + $"Delivery is Updated to: {userupdateDeliverydate}\n"

                                        + "Sincerely,\nFlipKart\nflipkart@gmail.com";

                Console.WriteLine(emailConformationmsg);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

            }
            finally
            {
                // Close the connection in the finally block to ensure it is closed even in case of an exception
                sqlconnection.Close();
            }


        }
        public void UpdateOrderDeliveryEmployeeID(int userOrderId, int userupdateDeliveryEmployeeID)
        {
            try
            {
                cmd.CommandText = $"update Courier set DeliveredEmployeeID=@DeliveredEmployeeID where CourierId=@CourierID";
                cmd.Parameters.AddWithValue("@DeliveredEmployeeID", userupdateDeliveryEmployeeID);
                cmd.Parameters.AddWithValue("@CourierID", userOrderId);
                cmd.Connection = sqlconnection;
                sqlconnection.Open();
                cmd.ExecuteNonQuery();

                string emailConformationmsg = $"Dear user,\n\n"

                                        + $"DeliverdEmployeeID is Updated to: {userupdateDeliveryEmployeeID}\n"

                                        + "Sincerely,\nFlipKart\nflipkart@gmail.com";

                Console.WriteLine(emailConformationmsg);
            }
            catch (System.Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

            }
            finally
            {
                // Close the connection in the finally block to ensure it is closed even in case of an exception
                sqlconnection.Close();
            }


        }


        public string DeleteOrder(int userOrderId)
        {
            String Conformationmessage = null;
            try
            {
                if (OrderExists(userOrderId))
                {
                    cmd.CommandText = "DELETE FROM Courier  where COURIERID=@COURIERID";
                    cmd.Connection = sqlconnection;
                    cmd.Parameters.Clear();
                    cmd.Parameters.AddWithValue("@COURIERID", userOrderId);
                    sqlconnection.Open();
                    SqlDataReader reader = cmd.ExecuteReader();

                    Conformationmessage = $"Dear User,\n\n"
                                                + $" Courier with CourierID {userOrderId}is Deleted \n"
                                                + $"Thank you\n"
                                               + "Sincerely,\nFlipKart\nflipkart@gmail.com";
                }
                else
                {
                    throw new OrderNotFoundException();
                }

            }
            catch (OrderNotFoundException ex)
            {
                Console.WriteLine($"Error:{ex.Message}");

            }
            finally
            {
                sqlconnection.Close();
            }
            return Conformationmessage;
        }
    }
}

