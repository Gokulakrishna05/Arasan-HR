using Arasan.Models;
using System.Data;
namespace Arasan.Interface
{
    public interface IProductionEntry
    {
        DataTable ShiftDeatils();
        DataTable DrumDeatils();
        DataTable Getstkqty(string branch, string loc, string ItemId);
        DataTable BindProcess();
        IEnumerable<ProductionEntry> GetAllProductionEntry();
        string ProductionEntryCRUD(ProductionEntry id);
        DataTable GetWorkCenter();
        DataTable EditProEntry(string PROID);
        DataTable ProIndetail(string PROID);
        DataTable ProOutDetail(string PROID);
        DataTable ProConsDetail(string PROID);
        DataTable ProwasteDetail(string PROID);
        DataTable GetInwardEntry();
        DataTable ProOutInwardDetail(string PROID);
        string CuringInwardEntryCRUD(ProductionEntry id);
        DataTable GetInwardItem(string inid);
        DataTable GetInward();
        DataTable GetProduction(string id);
        DataTable GetProInpDet(string id);
        DataTable GetProConsDet(string id);
        DataTable GetProOutDet(string id);
        DataTable GetProWasteDet(string id);
        string PRODStock(ProductionEntry cy);
    }
}
