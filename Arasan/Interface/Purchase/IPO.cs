using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IPO
    {
        IEnumerable<PO> GetAllPO();
        IEnumerable<POItem> GetAllPOItem(string Poid);
        DataTable GetPObyID(string Poid);
        DataTable GetPOItembyID(string Poid);
        DataTable EditPObyID(string Poid);
        DataTable GetPOItemDetails(string Poid);
    }
}
