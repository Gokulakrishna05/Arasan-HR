using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Sales
{
    public interface ISalesTargetService
    {
        DataTable GetAllListSalesTargetItem(string strStatus);
        DataTable GetItem();
        DataTable GetItemDetails(string itemId);
        DataTable GetSalesTarget(string id);
        DataTable GetSalesTargetItem(string id);
        string SalesTargetCRUD(SalesTarget cy);
        string StatusChange(string tag, string id);
    }
}
