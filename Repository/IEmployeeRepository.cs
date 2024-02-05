using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Repository
{
    internal interface IEmployeeRepository
    {
        public int AddCourierStaff(string userStaffName, long userStaffContactNumber);
        public bool CheckingEmployeeID(int userEmployeeID);
    }
}
