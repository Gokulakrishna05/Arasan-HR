using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Sales
{
    public interface IWorkOrderService
    {
        //IEnumerable<WorkOrder> GetAllWorkOrder();
        DataTable GetBranch();
        string WorkOrderCRUD(WorkOrder cy);
    }
}
