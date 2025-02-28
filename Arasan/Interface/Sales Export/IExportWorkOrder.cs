﻿using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;


namespace Arasan.Interface
{
    public interface IExportWorkOrder
    {
        string ExportWorkOrderCRUD(ExportWorkOrder cy);
        DataTable GetSupplier();
        DataTable GetExRateDetails(string id);
        DataTable GetItem();
        DataTable Gettemplete();
        DataTable GetCondition();
        DataTable GetExportWorkOrderView(string id);
        DataTable GetItemDetails(string itemId);
        DataTable GetAllExportWorkOrder(string strStatus);

        string StatusChange(string tag, string id);
        string ActStatusChange(string tag, string id);
        IEnumerable<WorkOrderItem> GetAllExportWorkOrderItem(string id);
        DataTable GetExportWorkOrderItem(string id);
        DataTable GetExportWorkOrder(string id);
        DataTable GetExportWorkItem(string id);

        string DispDrumCRUD(ExportWorkOrder cy);
        DataTable GetAllListWorkScheduleItems();
        string StatusStockRelease(string id, string jid, string bid);
        DataTable GetDrumAllByID(string id);
        DataTable GetDrumAllDetails(string id);
        DataTable GetAllocationDrumDetails(string id);
        DataTable GetWorkOrderByID(string id);
        DataTable GetWorkOrderDetailsss(string id);
        DataTable GetDrumDetails(string Itemid, string locid);

        string DrumAllocationCRUD(EWDrumAllocation cy);
    }
}
