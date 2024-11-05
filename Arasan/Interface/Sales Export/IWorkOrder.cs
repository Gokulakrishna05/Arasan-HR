using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IWorkOrder
    {
        DataTable GetItem();
        DataTable GetSupplier();
        DataTable GetWorkCenter();
        DataTable GetItemDetails(string id);
        DataTable GetAllListWorkOrder(string strStatus);
        string StatusChange(string tag, string id);
        string ActStatusChange(string tag, string id);
        string WorkOrderCRUD(WorkOrder cy);
        DataTable GetWorkOrder(string id);
        DataTable GetWorkOrderItem(string id);
    }
}
