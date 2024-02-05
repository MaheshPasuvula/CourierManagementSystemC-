using Assignment.Exception;
using Assignment.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Service
{
    internal class EmployeeService : IEmployeeService
    {
        IEmployeeRepository employeeRepository = new EmployeeRepository();
        public void AddCourierStaff()
        {


            Console.WriteLine("Enter StaffName ");
            String userStaffName = Console.ReadLine();
            Console.WriteLine("Enter StaffContactNumber");
            long userStaffContactNumber = long.Parse(Console.ReadLine());

            int staffID = employeeRepository.AddCourierStaff(userStaffName, userStaffContactNumber);

            Console.WriteLine($" EmployeeID for New Employee {staffID}");
        }
        public void CheckingEmployeeID()
        {
            try
            {
                Console.WriteLine("Enter your EmployeeID");
                int userEmployeeID = int.Parse(Console.ReadLine());

                bool checkEmployee = employeeRepository.CheckingEmployeeID(userEmployeeID);
                if (checkEmployee == false)
                {
                    throw new InvalidEmployeeIdException();

                }
                else
                {
                    Console.WriteLine($"Employee with EmployeeID{userEmployeeID} is Found");
                }
            }
            catch (InvalidEmployeeIdException ex)
            {
                Console.WriteLine(ex.message);
            }
        }
    }
}
