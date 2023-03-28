using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IBatchProduction
    {
        DataTable ShiftDeatils();
        DataTable DrumDeatils();
        DataTable Getstkqty(string branch, string loc, string ItemId);
        DataTable BindProcess();
        DataTable SeacrhItem(string terms);
        string BatchProductionCRUD(BatchProduction cy);
        IEnumerable<BatchProduction>  GetAllBatchProduction();
        DataTable GetWorkCenter();
        DataTable GetBatchProduction(string id);
        DataTable GetBatchProInpDet(string id);
        DataTable GetBatchProConsDet(string id);
        DataTable GetBatchProOutDet(string id);
        DataTable GetBatchProWasteDet(string id);
        DataTable EditProEntry(string PROID);
        DataTable ProIndetail(string PROID);
        DataTable ProOutDetail(string PROID);
        DataTable ProConsDetail(string PROID);
        DataTable ProwasteDetail(string PROID);
    }
}
