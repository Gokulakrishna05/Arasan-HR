using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface 
{
    public interface IWorkOrderShortClose
    {
        DataTable GetWorkOrder(string id);
        DataTable GetWorkOrderDetails(string id);
        string  WorkShortCRUD(WorkOrderShortClose Cy);
    }
}
