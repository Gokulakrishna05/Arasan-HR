using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface 
{
    public interface IProductionLog
    {
        DataTable ShiftDeatils();
        DataTable BindProcess();
        DataTable GetWorkCenter();
        DataTable GetReason();
        IEnumerable<ProductionLog> GetAllProductionLog();
    }
}
