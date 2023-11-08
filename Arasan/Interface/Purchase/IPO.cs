using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IPO
    {
        IEnumerable<PO> GetAllPO(string st, string ed);
        DataTable GetAllGateInward(string fromdate, string todate);
        IEnumerable<POItem> GetAllPOItem(string Poid);
        IEnumerable<POItem> GetAllGateInwardItem(string GateInwardId);
        DataTable GetPObyID(string Poid);
        DataTable GetPOrderID(string Poid);
        DataTable GetPOItembyID(string Poid);
        DataTable GetPOItem(string Poid);
        DataTable EditPObyID(string Poid);
        DataTable GetPOItemDetails(string Poid);
        string PurOrderCRUD(PO Cy);
        string GateInwardCRUD(GateInward Cy);
        string POtoGRN(string POID);
        DataTable GetPObySuppID(string supid);

        DataTable GetViewGateInward(string Poid);
        DataTable GetViewGateItems(string Poid);

        string StatusChange(string tag, int id);

        DataTable GetPOItemrep(string id);
        Task<IEnumerable<POItemDetail>> GetPOItemss (string supid, string s);

        Task<IEnumerable<POItemDetail>> GetPOItem(string id,string s);


    }
}
