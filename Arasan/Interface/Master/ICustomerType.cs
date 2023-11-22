using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface
{
    public interface ICustomerType
    {
        string CustomerCRUD(CustomerType by);
        DataTable GetCustomerType(string id);

        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
        DataTable GetAllCUSTOMERTYPE(string strStatus);
    }
}
