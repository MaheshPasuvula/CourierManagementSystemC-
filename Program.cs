using Microsoft.VisualBasic;
using System;
using System.Buffers.Text;
using System.ComponentModel;
using System.Diagnostics.Metrics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Runtime.Intrinsics.X86;
using System.Xml;
using System.Xml.Linq;
using static System.Formats.Asn1.AsnWriter;
using Assignment.Model;
using Assignment.Repository;
using Assignment.Service;
using System.Runtime.CompilerServices;


namespace Assignment
{
    class CourierDetails
    {
        public int courierId { get; set; }
        public int orderId { get; set; }
        public string ProductName { get; set; }
        public string status { get; set; }
        public decimal weight { get; set; }
        public int Customerid { get; set; }



    }
    class EmployeeDetails
    {
        public int id { get; set; }
        public string name { get; set; }
        public string emailId { get; set; }
        public string phoneNumber { get; set; }
        public string department { get; set; }
        public decimal salary { get; set; }
        public string Address { get; set; }
    }
    class CustomerDetails
    {
        public int id { get; set; }
        public string name { get; set; }
        public string emailId { get; set; }
        public string phoneNumber { get; set; }
        public string Address { get; set; }
    }
    class TrackingHistory
    {
        public int trackingid { get; set; }
        public int courierid { get; set; }
        public string location { get; set; }
        public DateTime Trackingdate { get; set; }
    }
    class CourierCompanies
    {
        public string couriercompany { get; set; }
        public string locationcompany { get; set; }
        public int zipcode { get; set; }
    }
    class Customer
    {
        public string CustomerName { get; set; }
        public string Address { get; set; }
    }
    class CustomerValidator
    {
        public static bool ValidateCustomerData(string data, string detail)
        {
            switch (detail.ToLower())
            {
                case "name":
                    return ValidateName(data);
                case "address":
                    return ValidateAddress(data);
                case "phone":
                    return ValidatePhoneNumber(data);
                default:
                    Console.WriteLine("Invalid detail provided for validation");
                    return false;
            }
        }

        private static bool ValidateName(string name)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(name, "^[A-Z][a-z]+$");
        }

        private static bool ValidateAddress(string address)
        {
            return !System.Text.RegularExpressions.Regex.IsMatch(address, "[^a-zA-Z0-9]");
        }

        private static bool ValidatePhoneNumber(string phoneNumber)
        {
            return System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\d{3}-\d{3}-\d{4}$");
        }
    }

    internal class Program
    {
        public static string FormatZipCode(string userZipcode)
        {
            if (userZipcode.Length == 6 && int.TryParse(userZipcode, out _))
            {
                return userZipcode;
            }
            else
            {
                return "000000";
            }
        }
        public static string FormatAddress(string userStreet, string userCity, string userState, string userZipcode)
        {
            userStreet = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userStreet.ToLower());
            userCity = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userCity.ToLower());
            userState = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(userState.ToLower());
            userZipcode = FormatZipCode(userZipcode);
            return $"{userStreet}, {userCity}, {userState},{userZipcode}";
        }
        public static string OrderConformationMailGenerator(string customerName, int orderNumber, string deliveryAddress, DateTime expectedDeliveryDate)
        {
            string emailConformationmsg = $"Dear {customerName},\n\n"
                            + $"Thank you for your order! Your order details are as follows:\n\n"
                            + $"Order Number: {orderNumber}\n"
                            + $"Delivery Address: {deliveryAddress}\n"
                            + $"Expected Delivery Date: {expectedDeliveryDate.ToString("MMMM dd, yyyy")}\n\n"
                            + "We appreciate your business.\n\n"
                            + "Sincerely,\nFlipKart\nflipkart@gmail.com";

            return emailConformationmsg;
        }
        public static decimal CalculateShippingCost(decimal distance, decimal weight)
        {
            decimal cost = 0;
            if (weight < 200.00m && distance < 100.00m)
            {
                cost = (0.05m) * weight;
            }
            else if ((weight > 200.00m && weight < 500.00m) && distance > 100.00m && distance < 250.00m)
            {
                cost = 100 + ((0.05m) * weight);
            }
            else
            {
                cost = 500 + ((0.05m) * weight);
            }
            return cost;
        }
        public static string GeneratePassword()
        {
            string capitalLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowerLetters = "abcdefghijklmnopqrstuvwxyz";
            string specialCharacters = "!@#$%^&*()_+-/.,';";
            string numbers = "0123456789";
            Random random = new Random();
            int randomnumber1 = random.Next(1, 23);
            int randomnumber2 = random.Next(1, 23);
            int randomnumber3 = random.Next(1, 15);
            int randomnumber4 = random.Next(1, 8);
            return $"{capitalLetters.Substring(randomnumber1, 2)}{lowerLetters.Substring(randomnumber2, 2)}{specialCharacters.Substring(randomnumber3, 2)}{numbers.Substring(randomnumber4, 2)}";

        }
        public static void NearestLocation(int zipCode, CourierCompanies[] courierCompanies)
        {
            CourierCompanies nearestCourier = null;
            double minDistance = double.MaxValue;


            foreach (var couriercmpny in courierCompanies)
            {
                double distance = Math.Abs(couriercmpny.zipcode - zipCode);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestCourier = couriercmpny;
                }
            }
            if (nearestCourier != null)
            {
                Console.WriteLine("The Nearest Available Courier company is:");
                Console.WriteLine($"Company: {nearestCourier.couriercompany}, Location: {nearestCourier.locationcompany}, Zipcode: {nearestCourier.zipcode}");
            }
            else
            {
                Console.WriteLine("No courier companies available.");
            }
        }
        public static void CheckAddress(string userAddress, Customer[] customerRecords)
        {
            Console.WriteLine("Matched records by user Entered Address");
            foreach (var customer in customerRecords)
            {
                if (customer.Address.Equals(userAddress))//string Method
                {
                    Console.WriteLine($"customerName={customer.CustomerName},customerAddress={customer.Address}");
                }
            }
        }



        static void Main(string[] args)
        {
            CourierDetails[] courierDetails = new CourierDetails[5]
            {
               new CourierDetails{courierId=1, orderId=1,ProductName="refrigetor",status="delivered", weight=1200,Customerid=2},
               new CourierDetails{courierId=2, orderId=2,ProductName="table fan",status="processing", weight=120,Customerid=3},
               new CourierDetails{courierId=3, orderId=3,ProductName="blender",status="delivered", weight=255,Customerid=2},
               new CourierDetails{courierId=4, orderId=4,ProductName="books",status="cancelled", weight=375.34m,Customerid=5},
               new CourierDetails{courierId=5, orderId=5,ProductName="table",status="processing", weight=573.45m,Customerid=2}
            };

            //foreach (var records in courierDetails)
            //{
            //    Console.WriteLine($"courierid ={records.courierId},orderid={records.orderId},Productname={records.ProductName},status={records.status},weight={records.weight},Customerid={records.Customerid}");
            //}
            #region Task 1: Control Flow Statements
            /* 1.Write a program that checks whether a given order is delivered or not based on its status(e.g.,
            "Processing," "Delivered," "Cancelled"). Use if-else statements for this. */
            //    while (true)
            //    {
            //        Console.WriteLine("Enter your order_id to check status of order");
            //        int userorderid = int.Parse(Console.ReadLine());
            //        int flag = 0;
            //        foreach (var records in courierDetails)
            //        {
            //            if (records.orderId == userorderid)
            //            {
            //                flag = 1;
            //                if (records.status == "delivered")
            //                {
            //                    Console.WriteLine($" order with orderid {records.orderId} is delivered");
            //                    break;
            //                }
            //                else if (records.status == "processing")
            //                {
            //                    Console.WriteLine($"order with orderid {records.orderId} in processing");
            //                    break;
            //                }
            //                else
            //                {
            //                    Console.WriteLine($"order with orderid {records.orderId} is Cancelled");
            //                    break;
            //                }
            //            }
            //        }
            //        if (flag == 0)
            //        {
            //            Console.WriteLine($"you entered orderid {userorderid} is not found  Please check again");
            //        }





            //    }
            //}

            /*2. Implement a switch-case statement to categorize parcels based on their weight into "Light," 
               "Medium," or "Heavy." */
            //string category = "";
            //foreach (var records in courierDetails)
            //{
            //    switch (records.weight)
            //    {
            //        case decimal when (records.weight < 300.00m):
            //            category = "Light";
            //            break;
            //        case decimal when (records.weight > 300.00m && records.weight < 500.00m):
            //            category = "Medium";
            //            break;
            //        default:
            //            category = "Heavy";
            //            break;

            //    }
            //    Console.WriteLine($"order with orderid {records.orderId} is categorized as {category}");
            //}

            /*3.Implement User Authentication 1.Create a login system for employees and customers using Java
                 control flow statements.*/
            //EmployeeDetails[] employeeDetails = new EmployeeDetails[5]
            //{
            //    new EmployeeDetails{id=1,name="Mahesh",emailId="maheshnlvp@gmail.com",phoneNumber="9390011724",department="Software Developer",salary=40000,Address="Hyderabad"},
            //    new EmployeeDetails{id=2,name="Umesh",emailId="umeshnlvp@gmail.com",phoneNumber="9390034172",department="Software Developer",salary=20000,Address="Hyderabad"},
            //    new EmployeeDetails{id=3,name="Ganesh",emailId="ganeshnlvp@gmail.com",phoneNumber="9324011724",department="HR",salary=25000,Address="Vijayawada"},
            //    new EmployeeDetails{id=4,name="Suresh",emailId="sureshnlvp@gmail.com",phoneNumber="9240011724",department="Full stack Developer",salary=35000,Address="Khammam"},
            //    new EmployeeDetails{id=5,name="Govind",emailId="govindnlvp@gmail.com",phoneNumber="9390045724",department="hr",salary=25000,Address="Karimnagar"}


            //};
            //Console.WriteLine("Employee Details");
            //Console.WriteLine("-----------------------------------------------------");
            //foreach (var empinfo in employeeDetails)
            //{
            //    Console.WriteLine($"employeeid = {empinfo.id},name={empinfo.name},emailid = {empinfo.emailId},phoneNumber={empinfo.phoneNumber},department = {empinfo.department},salary={empinfo.salary},Address={empinfo.Address}");
            //}

            //CustomerDetails[] customerDetails = new CustomerDetails[5]
            //{
            //    new CustomerDetails{id=1,name="Alice",emailId="alice@gmail.com",phoneNumber="8767834567",Address="RangaReddy"},
            //    new CustomerDetails{id=2,name="Alen",emailId="alen@gmail.com",phoneNumber="8762434567",Address="Vijayawada"},
            //    new CustomerDetails{id=3,name="Mark",emailId="mark@gmail.com",phoneNumber="8767834257",Address="VishakaPatnam"},
            //    new CustomerDetails{id=4,name="Antony",emailId="antony@gmail.com",phoneNumber="8767834237",Address="RangaReddy"},
            //    new CustomerDetails{id=5,name="Daniel",emailId="daniel@gmail.com",phoneNumber="8357834567",Address="Hyderabad"}




            //};
            //Console.WriteLine("Customer Details");
            //Console.WriteLine("------------------------------------------");
            //foreach (var customerdetails in customerDetails)
            //{
            //    Console.WriteLine($"id = {customerdetails.id}, name={customerdetails.name},emailid={customerdetails.emailId},phoneNumber={customerdetails.phoneNumber}");
            //}

            //while (true)
            //{
            //    Console.WriteLine("Enter your choice");
            //Console.WriteLine(" 1.Employee");
            //Console.WriteLine("2.Customer");
            //int userchoice = int.Parse(Console.ReadLine());

            //    switch (userchoice)
            //    {
            //        case 1:
            //            Console.WriteLine("you Selected Employee");
            //            Console.WriteLine("Enter Your Credentials for Login");
            //            Console.WriteLine("Enter your id");
            //            int userempid = int.Parse(Console.ReadLine());
            //            Console.WriteLine("Enter your emailId");
            //            string userempemailId = Console.ReadLine();
            //            int flag = 0;
            //            foreach (var empinfo in employeeDetails)
            //            {
            //                if (empinfo.id == userempid && empinfo.emailId == userempemailId)
            //                {
            //                    flag = 1;
            //                    Console.WriteLine("your Credentials are valid");
            //                    Console.WriteLine("Welcome to Our Website");
            //                    Console.WriteLine("Thankyou");
            //                }
            //            }
            //            if (flag == 0)
            //            {
            //                Console.WriteLine("you Entered Credentials are Invalid Please Check Again");
            //            }
            //            break;
            //        case 2:
            //            Console.WriteLine("you Selected Customer");
            //            Console.WriteLine("Enter your Credentials for Login");
            //            Console.WriteLine("Enter your Id");
            //            int userCustomerid = int.Parse(Console.ReadLine());
            //            Console.WriteLine("Enter your EmailId");
            //            string userCustomeremailId = Console.ReadLine();
            //            int flag2 = 0;
            //            foreach (var customerinfo in customerDetails)
            //            {
            //                if (customerinfo.id == userCustomerid && customerinfo.emailId == userCustomeremailId)
            //                {
            //                    flag2 = 1;
            //                    Console.WriteLine("your Credentials are valid");
            //                    Console.WriteLine("Welcome to Our Website");
            //                    Console.WriteLine("Thankyou");

            //                }
            //            }
            //            if (flag2 == 0)
            //            {
            //                Console.WriteLine("you Entered Credentials are Invalid Please Check Again");
            //            }

            //            break;
            //        default:
            //            Console.WriteLine("you selected wrong choice please check Again");
            //            break;


            //    }
            //}
            /*4.Implement Courier Assignment Logic 1.Develop a mechanism to assign couriers to shipments based
                  on predefined criteria(e.g., proximity, load capacity) using loops. */
            //foreach (var courierinfo in courierDetails)
            //{
            //    if (courierinfo.weight <= 350.00m)
            //    {
            //        Console.WriteLine($"Courier with courierid= {courierinfo.courierId} is Assigned to Shippment1");
            //    }
            //    else
            //    {
            //        Console.WriteLine($"Courier with courierid= {courierinfo.courierId} is Assigned to Shippment2");

            //    }
            //}

            #endregion
            #region Task 2:: Loops and Iteration
            /* 5.Write a Java program that uses a for loop to display all the orders for a specific customer.*/
            //foreach( var courierinfo in courierDetails )
            //{
            //    if (courierinfo.Customerid == 2)
            //    {
            //        Console.WriteLine($"orderid={courierinfo.orderId},ProductName={courierinfo.ProductName},status={courierinfo.status},weight={courierinfo.weight},coustmerid = {courierinfo.Customerid}");
            //    }
            //}
            /*6. Implement a while loop to track the real-time location of a courier until it reaches its destination.*/

            //string[,] trackingLocation = 
            //{
            //   {"1","Hyderabad"},
            //    {"1","Vijayawada"},
            //    {"1","Khammam"},
            //   {"2","Rangareddy"},
            //    {"2","Khammam"},
            //    {"2","Vijayawada"}


            //};
            //while (true)
            //{
            //    Console.WriteLine("Enter Your Courierid");
            //    string usercourierid = Console.ReadLine();
            //    int flag = 0;
            //    int i = 0;
            //    while (i < trackingLocation.GetLength(0))
            //    {
            //        if (trackingLocation[i, 0] == usercourierid)
            //        {
            //            flag = 1;
            //            Console.WriteLine($"your courier with courierid {trackingLocation[i, 0]}, {trackingLocation[i, 1]}");

            //        }
            //        i++;
            //    }
            //    if (flag == 0)
            //    {
            //        Console.WriteLine("You entered wrong courierid Please checkit");
            //    }
            //}
            #endregion
            #region Task  3: : Arrays and Data Structures 
            /* 7.Create an array to store the tracking history of a parcel, where each entry represents a location
                    update.*/
            //TrackingHistory[] trackingHistories = new TrackingHistory[10]
            //{
            //    new TrackingHistory{trackingid=1234,courierid=2,location="Hyderabad",Trackingdate=new DateTime(2023,01,01)},
            //   new TrackingHistory{trackingid=1234,courierid=2,location="Vijayawada",Trackingdate=new DateTime(2023,01,02)},
            //   new TrackingHistory{trackingid=1236,courierid=3,location="Rangareddy",Trackingdate=new DateTime(2023,01,10)},
            //   new TrackingHistory{trackingid=1236,courierid=3,location="KarimNagar",Trackingdate=new DateTime(2023,01,12)},
            //   new TrackingHistory{trackingid=1238,courierid=4,location="Vijayawada",Trackingdate=new DateTime(2023,02,01)},
            //   new TrackingHistory{trackingid=1238,courierid=4,location="Hyderabad",Trackingdate=new DateTime(2023,02,05)},
            //   new TrackingHistory{trackingid=1240,courierid=1,location="Srikakulam",Trackingdate=new DateTime(2023,04,05)},
            //   new TrackingHistory{trackingid=1240,courierid=1,location="Adilabad",Trackingdate=new DateTime(2023,04,10)},
            //   new TrackingHistory{trackingid=1242,courierid=5,location="Srikakulam",Trackingdate=new DateTime(2023,04,05)},
            //   new TrackingHistory{trackingid=1242,courierid=5,location="Nizamabad",Trackingdate=new DateTime(2023,04,08)}

            //};
            //foreach (var trackhistory in trackingHistories)
            //{
            //    if (trackhistory.courierid == 2)
            //    {


            //        Console.WriteLine($" trackingid={trackhistory.trackingid},courierid={trackhistory.courierid},location={trackhistory.location},Date={trackhistory.Trackingdate}");
            //    }
            //}
            /*8.Implement a method to find the nearest available courier for a new order using an array of couriers.*/
            //CourierCompanies[] courierCompanies = new CourierCompanies[5]
            //{
            //      new CourierCompanies{couriercompany="DHL",locationcompany="Hyderabad",zipcode=509100},
            //      new CourierCompanies{couriercompany="FastParcel",locationcompany="Vijayawada",zipcode=508200},
            //      new CourierCompanies{couriercompany="DTDC",locationcompany="Rangareddy",zipcode=507200},
            //      new CourierCompanies{couriercompany="ELBEX",locationcompany="Vishakapatnam",zipcode=508100},
            //     new CourierCompanies{couriercompany="HIGH EXPRESS",locationcompany="KarimNagar",zipcode=507100}

            //};


            //Console.WriteLine("Enter your zipCode for finding Nearest Courier company");
            //int userZipCode = int.Parse(Console.ReadLine());
            // NearestLocation(userZipCode,courierCompanies);

            #endregion
            #region Task  4:: Strings,2d Arrays, user defined functions,Hashmap 
            /* 9.Parcel Tracking: Create a program that allows users to input a parcel tracking number.Store the
           tracking number and Status in 2d String Array. Initialize the array with values.Then, simulate the
          tracking process by displaying messages like "Parcel in transit," "Parcel out for delivery," or "Parcel 
             delivered" based on the tracking number's status.*/
            //string[,] parcelTracking =
            //{
            //    {"1234","Parcel in Transit" },
            //    {"1235","Parcel out for Delivery" },
            //    {"1236","Parcel Delivered" },
            //    {"1237","Parcel in Transit" },
            //    {"1238","Parcel out for Delivery" },
            //    {"1239","Parcel Delivered" }
            //};
            //int flag = 0;
            //Console.WriteLine("Enter Parcel TrackingId");
            //string userParcelid= Console.ReadLine();

            //for(int i=0;i<parcelTracking.GetLength(0);i++)
            //{
            //    if (parcelTracking[i,0]==userParcelid)
            //    {
            //        flag = 1;
            //        Console.WriteLine($"your parcel with parcelid {parcelTracking[i, 0]}, {parcelTracking[i,1]}");

            //    }
            //}
            //if(flag==0)
            //{
            //    Console.WriteLine("you entered wrong ParcelId Please check it");
            //}

            /* 10.Customer Data Validation: Write a function which takes 2 parameters, data - denotes the data and
 detail - denotes if it is name addtress or phone number.Validate customer information based on
 following critirea. Ensure that names contain only letters and are properly capitalized, addresses do not
 contain special characters, and phone numbers follow a specific format(e.g., ###-###-####).*/
            //Console.WriteLine("Enter customer information:");

            //// Get name input
            //Console.Write("Name(Starts with capitalLetter): ");
            //string name = Console.ReadLine();
            //DisplayValidationResult("Name", CustomerValidator.ValidateCustomerData(name, "name"));

            //// Get address input
            //Console.Write("Address(doesnot contain special characters): ");
            //string address = Console.ReadLine();
            //DisplayValidationResult("Address", CustomerValidator.ValidateCustomerData(address, "address"));

            //// Get phone number input
            //Console.Write("Phone Number Format( ###-###-####): ");
            //string phoneNumber = Console.ReadLine();
            //DisplayValidationResult("Phone Number", CustomerValidator.ValidateCustomerData(phoneNumber, "phone"));


            //static void DisplayValidationResult(string detail, bool isValid)
            //{
            //    Console.WriteLine($"{detail}: {(isValid ? "Valid" : "Invalid")}");
            //}
            /*11.Address Formatting: Develop a function that takes an address as input (street, city, state, zip code)
    and formats it correctly, including capitalizing the first letter of each word and properly formatting the
    zip code. */
            //Console.WriteLine("Enter your Address");
            //Console.WriteLine("Enter your Street");
            //string userStreet= Console.ReadLine();
            //Console.WriteLine("Enter your City");
            //string userCity= Console.ReadLine();
            //Console.WriteLine("Enter your State");
            //string userState= Console.ReadLine();
            //Console.WriteLine("Enter Zip code");
            //string userZipcode= Console.ReadLine();
            //String FormatedAddress = FormatAddress(userStreet, userCity, userState, userZipcode);
            //Console.WriteLine(FormatedAddress);

            /* 12.Order Confirmation Email: Create a program that generates an order confirmation email.The email
    should include details such as the customer's name, order number, delivery address, and expected 
    delivery date.*/
            //string customerName = "Ganesh";
            //int orderNumer = 1234;
            //string deliveryAddress = "Hyderabad";
            //DateTime expectedDeliveryDate = DateTime.Now.AddDays(7);
            //String orderConformationMailMessage=OrderConformationMailGenerator(customerName,orderNumer, deliveryAddress, expectedDeliveryDate);
            //Console.WriteLine(orderConformationMailMessage);

            /*13.Calculate Shipping Costs: Develop a function that calculates the shipping cost based on the distance
    between two locations and the weight of the parcel.You can use string inputs for the source and
    destination addresses.*/
            //Console.WriteLine("Ente your Source Address");
            //string userSourceAddress= Console.ReadLine();
            //Console.WriteLine("Enter your Destination Address");
            //string userDestinationAddress= Console.ReadLine();
            //Console.WriteLine("Enter distance Between Source and Destination Address");
            //decimal distance=decimal.Parse(Console.ReadLine());
            //Console.WriteLine("Enter Weight of Parcel");
            //decimal weight = decimal.Parse(Console.ReadLine());
            //decimal shippingcost= CalculateShippingCost(distance, weight);
            //Console.WriteLine($"SourceAddress={userSourceAddress}\n"
            //    + $"DestinationAddress={userDestinationAddress}\n"
            //    + $"Distance between Source and Destination Address={distance}\n" +
            //    $"Weight of Parcel={weight}\n" +
            //    $"Total Shippingcost={shippingcost}");
            /*14.Password Generator: Create a function that generates secure passwords for courier system
    accounts.Ensure the passwords contain a mix of uppercase letters, lowercase letters, numbers, and
    special characters.*/
            //Console.WriteLine("Generated Password ::");
            //string generatedPassword = GeneratePassword();
            //Console.WriteLine(generatedPassword);
            /* 15.Find Similar Addresses: Implement a function that finds similar addresses in the system. This can be
    useful for identifying duplicate customer entries or optimizing delivery routes.Use string functions to
    implement this.*/
            // Customer[] customerRecords = new Customer[10]
            // {
            // new Customer { CustomerName = "John Doe", Address = "123 Main St, City A" },
            // new Customer { CustomerName = "Daniel", Address = "456 Main St, City D" },
            // new Customer { CustomerName = "John Wesly", Address = "786  St, City B" },
            // new Customer { CustomerName = "Daniel", Address = "456 Main St, City D" },
            // new Customer { CustomerName = "John Doe", Address = "123 Main St, City A" },
            // new Customer { CustomerName = "John Wesly", Address = "786  St, City B" },
            // new Customer { CustomerName = "Alice", Address = " Main St, City E" },
            // new Customer { CustomerName = "Aman", Address = "498 Main St, City R" },
            // new Customer { CustomerName = "John Doe", Address = "123 Main St, City A" },
            // new Customer { CustomerName = "Glen", Address = "763 Main St, City G" }


            //};
            // Console.WriteLine("enter Address you want to check");
            // String userAddress= Console.ReadLine();
            // CheckAddress(userAddress, customerRecords);

            #endregion

            ICourierService courierService = new CourierService();
            IEmployeeService employeeService = new EmployeeService();
            ICourierCompanyService courierCompanyService = new CourierCompanyService();
            #region  Task  6::Service Provider Interface
            //bool exit = true;
            //while (exit)
            //{

            //        Console.WriteLine("Enter Your Choice");
            //        Console.WriteLine("1.Place Order");
            //        Console.WriteLine("2.Get Order Status");
            //        Console.WriteLine("3.Cancel Order");
            //        Console.WriteLine("4.GetAssigned Order");
            //        Console.WriteLine("5.AddNewStaff");
            //        Console.WriteLine("6.Exit");
            //        int userChoice = int.Parse(Console.ReadLine());
            //        switch (userChoice)
            //        {
            //            case 1:
            //                courierService.PlaceOrder();
            //                break;
            //            case 2:
            //                courierService.GetOrderStatus();
            //                break;
            //            case 3:
            //                courierService.CancelOrder();

            //                break;
            //            case 4:
            //                courierService.GetAssigned();
            //                break;
            //            case 5:
            //                employeeService.AddCourierStaff();
            //                break;
            //                case 6:
            //                exit = false;
            //                break;
            //            default:
            //                Console.WriteLine("you Entered Wrong Choice Please Check it Again");
            //                break;
            //        }

            //}


            #endregion
            #region Task  7:  Exception Handling
            //bool exit = true;
            //while (exit)
            //{
            //    Console.WriteLine("Enter your Choice");
            //    Console.WriteLine("1.Check TrackingNumber");
            //    Console.WriteLine("2.Check Employee");
            //    Console.WriteLine("3.Exit");
            //    int userchoice = int.Parse(Console.ReadLine());
            //    switch (userchoice)
            //    {
            //        case 1:

            //            courierService.CheckingTrackingNumber();
            //            break;
            //        case 2:
            //            employeeService.CheckingEmployeeID();
            //            break;
            //        default:
            //            Console.WriteLine("You Entered Wrong Choice Please Check it");
            //            break;


            //    }
            //}
            #endregion
            #region Task 8: Collections
            //bool exit = true;
            //while (exit)
            //{
            //    Console.WriteLine("Select  your  Choice");
            //    Console.WriteLine("1.GetAll  Courier Companies");
            //    Console.WriteLine("2.Add Courier Company");
            //    Console.WriteLine("3.Remove Companies");
            //    Console.WriteLine("4.Exit");
            //    int userChoice = int.Parse(Console.ReadLine());
            //    switch (userChoice)
            //    {
            //        case 1:
            //            courierCompanyService.GetCourierCompanies();
            //            break;

            //        case 2:
            //            courierCompanyService.AddCourierCompany();
            //            break;
            //        case 3:
            //            courierCompanyService.RemoveCourierCompany();
            //            break;
            //        case 4:
            //            exit = false;
            //            break;

            //        default:
            //            Console.WriteLine("You Selected Wrong Option Please Check it");
            //            break;


            //    }

            //}
            #endregion
            #region Task 9: Database
            //bool exit = true;
            //while (exit)
            //{
            //    Console.WriteLine("Welcome To Mahesh Courier Management System");
            //    Console.WriteLine("----------------------------------------------");
            //    Console.WriteLine("Select Your Choice");
            //    Console.WriteLine("1.To All Couriers");
            //    Console.WriteLine("2.To Order Status");
            //    Console.WriteLine("3.To Place Order");
            //    Console.WriteLine("4.To Update Order Details");
            //    Console.WriteLine("5.To Delete Order");
            //    Console.WriteLine("6.Exit");
            //    Console.WriteLine("Enter your Choice");
            //    int userOption = int.Parse(Console.ReadLine());
            //    switch (userOption)
            //    {
            //        case 1:
            //            courierService.AllOrders();
            //            break;
            //        case 2:
            //            courierService.OrderStatus();
            //            break;
            //        case 3:
            //            courierService.Order();
            //            break;
            //        case 4:
            //            courierService.UpdateOrder();
            //            break;
            //        case 5:
            //            courierService.DeleteOrder();
            //            break;
            //        case 6:
            //            exit = false;
            //            break;
            //        default:
            //            Console.WriteLine("you Selected Wrong Choice Please Check it again");
            //            break;
            //    }
            //}

            #endregion

        }

    }
}


