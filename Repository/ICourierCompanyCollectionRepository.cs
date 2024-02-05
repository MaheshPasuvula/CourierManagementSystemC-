using Assignment.Model;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Assignment.Repository
{
    internal interface ICourierCompanyCollectionRepository
    {
        List<CourierCompany> GetCourierCompanies();
        void AddCourierCompany(string usercompanyname, int courierId, string senderName, string senderAddress, string receiverName, string receiverAddress, decimal weight, string status, int trackingNumber, DateTime deliveryDate, int userId, int employeeId,
                 int employeeIdInput, string employeeName, string email, long contactNumber, string role, decimal salary, int locationId, string locationName, string address);
        void RemoveCourierCompany(string userCompanyName);
    }
}
