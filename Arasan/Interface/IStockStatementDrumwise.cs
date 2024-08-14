
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
namespace Arasan.Interface
{
    public interface IStockStatementDrumwise
    {
        DataTable GetLocation(string LocID);
        DataTable GetSN(string SNID);
        DataTable GetAllStockStatementDrumwise(string dtFrom, string dtTo, string SN, string Location);



    }
}
