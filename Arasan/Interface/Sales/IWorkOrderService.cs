using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Services.Sales;

namespace Arasan.Interface.Sales
{
    public interface IWorkOrderService
    {
        IEnumerable<WorkOrder> GetAllWorkOrder(string status);
        DataTable GetQuo();
        DataTable GetLocation(string id);
        string WorkOrderCRUD(WorkOrder cy);
        string DrumAllocationCRUD(WDrumAllocation cy);
        DataTable GetQuoDetails(string id);
        DataTable GetSatesQuoDetails(string id);
        DataTable GetWorkOrder(string id);
        DataTable GetWorkOrderDetails(string id);
        DataTable GetTax(string id);
        DataTable GetParty();
        DataTable GetWorkOrderByID(string id);
        DataTable GetItem(string id);
        DataTable GetDrumAllByID(string id);
        DataTable GetDrumAllDetails(string id);
        DataTable GetAllocationDrumDetails(string id);
        DataTable GetDrumDetails(string Itemid, string locid);

        IEnumerable<WDrumAllocation> GetAllWDrumAll();
        string StatusDeleteMR(string tag, string id);
        string StatusStockRelease(string id, string jid,string bid);
        DataTable GetAllListWorkOrderItems(string strStatus);
        DataTable GetAllListWDrumAllocationItems();
        DataTable GetAllListWDrumAlloItems();
        Task<IEnumerable<OrderItemDetail>> GetOrderItem(string id );
        Task<IEnumerable<OrderDetail>> GetOrderItemDetail(string id );
    }
}
