using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;


namespace Arasan.Interface
{
    public interface ICustomerComplaint
    {
        string CustomerComplaintCRUD(CustomerComplaint cy);
        DataTable GetAllListCustomerComplaint(string strStatus);
        string StatusChange(string tag, string id);
        string ActStatusChange(string tag, string id);
    }
}
