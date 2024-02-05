using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Assignment.Model;
using Assignment.Repository;

namespace Assignment.Repository
{
    internal class EmployeeRepository : IEmployeeRepository
    {
        private static int employeeID = 100;
        private static int uniqueCourierId = 2000;


        List<Employee> employee = new List<Employee>()
        {
            new Employee{employeeID=1,employeeName="Vijay",email="vijay123@gmail.com",contactNumber=9346789876,role="Driver",salary=20000 },
            new Employee{employeeID=2,employeeName="Ajay",email="ajay123@gmail.com",contactNumber=9342539876,role="DeliveryBoy",salary=25000 },
            new Employee{employeeID=3,employeeName="Bhasker",email="bhasker123@gmail.com",contactNumber=9332789876,role="DeliveryBoy",salary=35000 },
            new Employee{employeeID=4,employeeName="Chenna",email="chenna123@gmail.com",contactNumber=9346789456,role="Manager",salary=50000 },
            new Employee{employeeID=5,employeeName="BalaKrishna",email="balakrishna123@gmail.com",contactNumber=9346669876,role="DeliveryBoy",salary=25000 },
            new Employee{employeeID=6,employeeName="Krishna",email="krishna123@gmail.com",contactNumber=9346775876,role="Manager",salary=45000 }

        };
        public int AddCourierStaff(string userStaffName, long userStaffContactNumber)
        {
            Employee employee = new Employee();

            employee.employeeID = employeeID++;
            employee.employeeName = userStaffName;
            employee.email = userStaffName + "123@gmail.com";
            employee.contactNumber = userStaffContactNumber;
            return employee.employeeID; ;
        }
        public bool CheckingEmployeeID(int userEmployeeID)
        {
            Employee employeeIdFound = employee.Find(employee => employee.employeeID == userEmployeeID);
            if (employeeIdFound != null)
            {
                return true;
            }

            return false;


        }
    }
}
