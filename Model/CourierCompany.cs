using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Model
{
    internal class CourierCompany
    {
        public string companyName { get; set; }
        public List<Courier>CourierDetails { get; set; }
        public List<Employee>EmployeeDetails { get; set; }
        public List<Location>LocationDetails { get; set; }



    }
}
