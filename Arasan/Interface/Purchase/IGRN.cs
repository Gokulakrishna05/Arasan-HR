using Arasan.Models;
using System.Data;
namespace Arasan.Interface
{
    public interface IGRN
    {
        IEnumerable<GRN> GetAllGRN(string st, string ed);
        IEnumerable<POItem> GetAllGRNItem(string Poid);
        DataTable GetGRNbyID(string Poid);
        DataTable GetGRNItembyID(string Poid);
        string GRNCRUD(GRN cy);
        string GRNACCOUNT(GRN cy);
        DataTable EditGRNbyID(string name);
        DataTable FetchAccountRec(string GRNId);
        DataTable LedgerList();
        string StatusChange(string tag, string id);
        string StatusActChange(string tag, string id);
        DataTable GetViewGRN(string id);
        DataTable GetViewGRNDetail(string id);
        DataTable GetAllListGRNItem(string strStatus);
        DataTable GetAllListDamageGRNItem(string strStatus);
        //DataTable GetAllListDamageGRNItemDetail(string strStatus);
        DataTable AccconfigLst();

        DataTable GetconfigItem(string ConId);
    }
}
