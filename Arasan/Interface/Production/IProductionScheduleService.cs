using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Production
{
    public interface IProductionScheduleService
    {
        DataTable GetProductionSchedule(string id);
        //DataTable GetProductionScheduleServiceDetail(string id);
        string ProductionScheduleCRUD(ProductionSchedule cy);
        DataTable GetProcess(string id);
        DataTable GetWorkCenter();
        DataTable GetItem(string value);

        DataTable GetItemDetails(string ItemId);
        DataTable GetItemSubGroup(string id);
        DataTable GetItemSubGrp();
        IEnumerable<ProductionSchedule> GetProductionSchedule();
        DataTable GetProductionScheduleDetail(string id);
    }
}
