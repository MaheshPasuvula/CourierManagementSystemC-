using Assignment.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using Assignment.Service;


namespace Assignment.Repository
{
    internal class CourierCompanyCollectionRepository : ICourierCompanyCollectionRepository

    {



        List<CourierCompany> courierCompanies = new List<CourierCompany>
        {
            new CourierCompany {companyName = "DTDL", CourierDetails = new List<Courier>
            { new Courier{courierID=789, senderName = "Raju",senderAddress="Hyderabad", receiverName = "Vijay", receiverAddress="Vijayawada", weight = 275.5m, status = "Delivered",trackingNumber=9087,DeliveryDate= new DateTime(2019,12,23),userID=6,employeeID=7 },
              new Courier{courierID=789, senderName = "Somesh",senderAddress="Vijayawada", receiverName = "Ranjith", receiverAddress="RangaReddy", weight = 475.5m, status = "Delivered",trackingNumber=9088,DeliveryDate= new DateTime(2020,12,23),userID=3,employeeID=4 }, },
               EmployeeDetails = new List<Employee>
                {new Employee{ employeeID=5,employeeName = "Vishwas",email = "vishwas@gmail.com",contactNumber=9876564532,role="Driver",salary=23450},
                   new Employee{ employeeID=7,employeeName = "Vishal",email = "vishal@gmail.com",contactNumber=9876523432,role="DeliveryBoy",salary=33450},
               },
                LocationDetails = new List<Location>
                {
                    new Location{LocationID=3,LocationName = "Location1",Address = "Bangalore"},
                    new Location{LocationID=3,LocationName = "Location2",Address = "Vijayawada"},
                },
            },
             new CourierCompany {companyName = "BlueDart", CourierDetails = new List<Courier>
            { new Courier{courierID=989, senderName = "Ramesh",senderAddress="Mahabubnagar", receiverName = "Vijay", receiverAddress="Vijayawada", weight = 375.5m, status = "Delivered",trackingNumber=9087,DeliveryDate= new DateTime(2019,10,13),userID=6,employeeID=7 },
              new Courier{courierID=990, senderName = "Suresh",senderAddress="Vijayawada", receiverName = "Ranjith", receiverAddress="RangaReddy", weight = 775.5m, status = "Delivered",trackingNumber=9088,DeliveryDate= new DateTime(2020,09,12),userID=3,employeeID=4 }, },
               EmployeeDetails = new List<Employee>
                {new Employee{ employeeID=5,employeeName = "Vishwas",email = "vishwas@gmail.com",contactNumber=9876564532,role="Driver",salary=23450},
                   new Employee{ employeeID=7,employeeName = "Vishal",email = "vishal@gmail.com",contactNumber=9876523432,role="DeliveryBoy",salary=33450},
               },
                LocationDetails = new List<Location>
                {
                    new Location{LocationID=3,LocationName = "Location1",Address = "Bangalore"},
                    new Location{LocationID=3,LocationName = "Location2",Address = "Vijayawada"},
                },
            },

        };
        public List<CourierCompany> GetCourierCompanies()
        {

            return courierCompanies;
        }


        public void AddCourierCompany(string usercompanyname, int courierId, string senderName, string senderAddress, string receiverName, string receiverAddress, decimal weight, string status, int trackingNumber, DateTime deliveryDate, int userId, int employeeId,
                 int employeeIdInput, string employeeName, string email, long contactNumber, string role, decimal salary, int locationId, string locationName, string address)
        {




            Courier courier = new Courier
            {
                courierID = courierId,
                senderName = senderName,
                senderAddress = senderAddress,
                receiverName = receiverName,
                receiverAddress = receiverAddress,
                weight = weight,
                status = status,
                trackingNumber = trackingNumber,
                DeliveryDate = deliveryDate,
                userID = userId,
                employeeID = employeeId
            };



            Employee employee = new Employee
            {
                employeeID = employeeIdInput,
                employeeName = employeeName,
                email = email,
                contactNumber = contactNumber,
                role = role,
                salary = salary
            };


            Location location = new Location
            {
                LocationID = locationId,
                LocationName = locationName,
                Address = address
            };

            CourierCompany newCompany = new CourierCompany
            {
                companyName = usercompanyname,
                CourierDetails = new List<Courier> { courier },
                EmployeeDetails = new List<Employee> { employee },
                LocationDetails = new List<Location> { location }
            };
            courierCompanies.Add(newCompany);

            Console.WriteLine($"New Courier Company '{usercompanyname}' created successfully!");




        }


        public void RemoveCourierCompany(string userCompanyName)
        {
            CourierCompany companyToDelete = courierCompanies.FirstOrDefault(c => c.companyName.Equals(userCompanyName, StringComparison.OrdinalIgnoreCase));

            if (companyToDelete != null)
            {
                // Remove the company from the list
                courierCompanies.Remove(companyToDelete);
                Console.WriteLine($"Courier Company '{userCompanyName}' deleted successfully!");
            }
            else
            {
                Console.WriteLine($"No Courier Company found with the name '{userCompanyName}'. Deletion failed.");
            }

        }
    }

}
