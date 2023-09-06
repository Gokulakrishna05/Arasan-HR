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
        DataTable GetLotstkqty(string branch, string loc, string ItemId,string LotNo);
        DataTable BindProcess();
        DataTable GetItem();
        DataTable GetConItem();
        DataTable GetOutItem();
      
        DataTable SeacrhItem(string terms);
        string BatchProductionCRUD(BatchProduction cy);
        string BPRODStock(ProductionEntry cy);
        IEnumerable<BatchProduction>  GetAllBatchProduction(string st, string ed);
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
        double GetStockInQty(string Itemid, string barchid, string Locid);
        DataTable Getlot(string Itemid, string barchid, string Locid);

        string StatusChange(string tag, int id);


        DataTable GetConItemDetails(string id);
    }
}
