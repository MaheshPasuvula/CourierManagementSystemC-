using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Model;
using Assignment.Repository;


namespace Assignment.Service
{
    internal class CourierCompanyService : ICourierCompanyService
    {
        ICourierCompanyCollectionRepository courierCompanyCollectionRepository = new CourierCompanyCollectionRepository();
        public void RemoveCourierCompany()
        {
            Console.WriteLine("Enter your CompanyName");
            string userCompanyName = Console.ReadLine();
            courierCompanyCollectionRepository.RemoveCourierCompany(userCompanyName);

        }
        public void GetCourierCompanies()
        {
            List<CourierCompany> courierCompanies;
            courierCompanies = courierCompanyCollectionRepository.GetCourierCompanies();
            Console.WriteLine("\nExisting Courier Companies:");
            foreach (var company in courierCompanies)
            {
                Console.WriteLine($"Company: {company.companyName}");
                Console.WriteLine("Couriers:");
                foreach (var courier in company.CourierDetails)
                {
                    Console.WriteLine($"- Sender: {courier.senderName}, Receiver: {courier.receiverName}, Weight: {courier.weight} gms, Status: {courier.status}, DeliveryDate: {courier.DeliveryDate}");
                }

                Console.WriteLine("Employees:");
                foreach (var employee in company.EmployeeDetails)
                {
                    Console.WriteLine($"- Employee: {employee.employeeName}, Email: {employee.email}");
                }

                Console.WriteLine("Locations:");
                foreach (var location in company.LocationDetails)
                {
                    Console.WriteLine($"- Location: {location.LocationName}, Address: {location.Address}");
                }

                Console.WriteLine();
            }
        }
        public void AddCourierCompany()
        {
            Console.WriteLine("enter your companyname");
            string usercompanyname = Console.ReadLine();
            Console.WriteLine("Enter Courier Details");
            Console.Write("Enter Courier ID: ");
            int courierId = int.Parse(Console.ReadLine());

            Console.Write("Enter Sender Name: ");
            string senderName = Console.ReadLine();

            Console.Write("Enter Sender Address: ");
            string senderAddress = Console.ReadLine();

            Console.Write("Enter Receiver Name: ");
            string receiverName = Console.ReadLine();

            Console.Write("Enter Receiver Address: ");
            string receiverAddress = Console.ReadLine();

            Console.Write("Enter Weight: ");
            decimal weight = decimal.Parse(Console.ReadLine());

            Console.Write("Enter Status: ");
            string status = Console.ReadLine();

            Console.Write("Enter Tracking Number: ");
            int trackingNumber = int.Parse(Console.ReadLine());

            Console.Write("Enter Delivery Date (yyyy-MM-dd): ");
            DateTime deliveryDate = DateTime.ParseExact(Console.ReadLine(), "yyyy-MM-dd", null);

            Console.Write("Enter User ID: ");
            int userId = int.Parse(Console.ReadLine());

            Console.Write("Enter Employee ID: ");
            int employeeId = int.Parse(Console.ReadLine());

            Console.WriteLine("Enter EmployeeDetails");
            Console.Write("Enter Employee ID: ");
            int employeeIdInput = int.Parse(Console.ReadLine());

            Console.Write("Enter Employee Name: ");
            string employeeName = Console.ReadLine();

            Console.Write("Enter Email: ");
            string email = Console.ReadLine();

            Console.Write("Enter Contact Number: ");
            long contactNumber = long.Parse(Console.ReadLine());

            Console.Write("Enter Role: ");
            string role = Console.ReadLine();

            Console.Write("Enter Salary: ");
            decimal salary = decimal.Parse(Console.ReadLine());
            Console.WriteLine("Enter Location Details");
            Console.Write("Enter Location ID: ");
            int locationId = int.Parse(Console.ReadLine());

            Console.Write("Enter Location Name: ");
            string locationName = Console.ReadLine();

            Console.Write("Enter Address: ");
            string address = Console.ReadLine();


            courierCompanyCollectionRepository.AddCourierCompany(usercompanyname, courierId, senderName, senderAddress, receiverName, receiverAddress, weight, status, trackingNumber, deliveryDate, userId, employeeId,
                 employeeIdInput, employeeName, email, contactNumber, role, salary, locationId, locationName, address);
        }
    }
}

