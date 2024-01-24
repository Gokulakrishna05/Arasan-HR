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
        DataTable GetProdSche(string id);
        DataTable GetAllProdSch(string id);
        DataTable GetPackItem(string id);
        DataTable GetProdScheInputDetail(string id);
        DataTable GetProdScheOutputDetail(string id);
        DataTable GetProductionScheduleDetail(string id);
        DataTable GetProductionScheduleOutputDetail(string id);
        DataTable GetProductionScheduleParametersDetail(string id);
        DataTable GetOutputDetailsDayWiseDetail(string id);
        DataTable GetPackDetail(string id);
        DataTable GetProdSchePolish(string id);
        DataTable GetPolishInputDetail(string id);
        DataTable GetPolishOutputDetail(string id);
        DataTable GetProdScheRVD(string id);
        DataTable GetRVDInputDetail(string id);
        DataTable GetRVDOutputDetail(string id);
        DataTable GetProdSchePaste(string id);
        DataTable GetPasteInputDetail(string id);
        DataTable GetPasteOutputDetail(string id);
        DataTable GetProcess();
        DataTable EditProSche(string id);
        DataTable ProIndetail(string id);
        DataTable ProOutDetail(string id);
        DataTable ProParmDetail(string id);
        DataTable ProDailyDatDetail(string id);
        DataTable ProPackDetail(string id);
        string StatusChange(string id );

        string StatusChange(string tag, int id);
    }
}
