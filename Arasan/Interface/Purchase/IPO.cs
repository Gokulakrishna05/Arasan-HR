using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IPO
    {
        IEnumerable<PO> GetAllPO(string status);
        DataTable GetAllGateInward(string fromdate, string todate);
        IEnumerable<POItem> GetAllPOItem(string Poid);
        IEnumerable<POItem> GetAllGateInwardItem(string GateInwardId);
        DataTable GetPObyID(string Poid);
        DataTable GetPOItembyID(string Poid);
        DataTable EditPObyID(string Poid);
        DataTable GetPOItemDetails(string Poid);
        string PurOrderCRUD(PO Cy);
        string GateInwardCRUD(GateInward Cy);
        string POtoGRN(string POID);
        DataTable GetPObySuppID(string supid);

        string StatusChange(string tag, int id);




        Task<IEnumerable<POItemDetail>> GetPOItem(string id);


    }
}
