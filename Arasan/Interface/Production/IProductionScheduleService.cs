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
        DataTable GetWorkCenter();
        DataTable GetItem(string value);
        DataTable GetItem();
        DataTable GetItemDetails(string ItemId);
        DataTable GetItemSubGroup(string id);
        DataTable GetItemSubGrp();
        IEnumerable<ProductionSchedule> GetProductionSchedule();
        DataTable GetProductionScheduleDetail(string id);
        DataTable GetProductionScheduleOutputDetail(string id);
        DataTable GetProductionScheduleParametersDetail(string id);
        DataTable GetOutputDetailsDayWiseDetail(string id);
        DataTable GetPackDetail(string id);
        DataTable GetProcess();
        DataTable EditProSche(string id);
        DataTable ProIndetail(string id);
        DataTable ProOutDetail(string id);
        DataTable ProParmDetail(string id);
        DataTable ProDailyDatDetail(string id);
        DataTable ProPackDetail(string id);
          //string StatusChange(string id, int tag);


    }
}
