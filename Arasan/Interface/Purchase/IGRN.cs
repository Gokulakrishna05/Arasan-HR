using Arasan.Models;
using System.Data;
namespace Arasan.Interface
{
    public interface IGRN
    {
        IEnumerable<GRN> GetAllGRN();
        IEnumerable<POItem> GetAllGRNItem(string Poid);
        DataTable GetGRNbyID(string Poid);
        DataTable GetGRNItembyID(string Poid);
        string GRNCRUD(GRN cy);
        DataTable EditGRNbyID(string name);
        DataTable FetchAccountRec(string GRNId);
        DataTable LedgerList();
    }
}
