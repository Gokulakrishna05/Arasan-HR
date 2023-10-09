using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IIssueToProduction
    {
        DataTable  GetWorkCenter();
        DataTable  GetItem();
        DataTable GetStockDetails(string loc, string item);
        DataTable GetStockDetail(string loc, string item);
        string IssueToProductionCRUD(IssueToProduction Cy);
    }
}
