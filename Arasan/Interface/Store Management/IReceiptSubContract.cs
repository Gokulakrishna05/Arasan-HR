using Arasan.Models;
using System.Data;

namespace Arasan.Interface 
{
    public interface IReceiptSubContract
    {

        DataTable GetSupplier(string id);
        DataTable GetDC();
        DataTable GetDrum();
        DataTable GetDCDetails(string id);
        DataTable GetRecvItemDetails(string id);
        DataTable GetDelivItemDetails(string id);
        DataTable GetReceiptSubContract(string id);
        DataTable GetRecemat(string id);
        DataTable GetDrimdetails(string id);
        DataTable GetDeliItem(string id);
        DataTable GetDrumItemDetails(string id);
        DataTable GetAllReceiptSubContractItem(string id);

        string ReceiptSubContractCRUD(ReceiptSubContract Cy);
    }
}
