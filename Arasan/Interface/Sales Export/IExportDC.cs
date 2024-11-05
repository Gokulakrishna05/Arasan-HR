using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IExportDC
    {
        DataTable GetItem();
        DataTable GetSupplier();
        DataTable GetWorkCenter();
        string StatusChange(string tag, string id);
        string ActStatusChange(string tag, string id);
        DataTable GetItemDetails(string id);
        string ExportDCCRUD(ExportDC cy);
        DataTable GetAllListExportDC(string strStatus);
        DataTable GetExportDC(string id);
        DataTable GetExportDCItem(string id);
        DataTable GetExportDCScrap(string id);
        DataTable GetExportDCView(string id);
        DataTable GetExportDCItemview(string id);
        DataTable GetExportDCScrapview(string id);
    }
}
