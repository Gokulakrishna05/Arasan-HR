using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IPurchaseIndent
    {
        DataTable GetBranch();
        DataTable GetLocation();
        DataTable GetSLocation();
        DataTable GetItemSubGrp();
        DataTable GetItem(string value);
        DataTable GetItemDetails(string ItemId);
        DataTable GetEmp();
        string IndentCRUD(PurchaseIndent cy);
        DataTable GetIndent();
        DataTable GetIndentItem(string PRID);
        DataTable GetIndentItemApprove();

    }
}
