using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Sales
{
    public interface IWorkOrderService
    {
        IEnumerable<WorkOrder> GetAllWorkOrder();
        DataTable GetQuo();
        string WorkOrderCRUD(WorkOrder cy);
        DataTable GetQuoDetails(string id);
        DataTable GetSatesQuoDetails(string id);
        DataTable GetWorkOrder(string id);
        DataTable GetWorkOrderDetails(string id);
        DataTable GetTax();
    }
}
