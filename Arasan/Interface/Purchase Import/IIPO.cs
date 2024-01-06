using Arasan.Models;
using System.Data;
namespace Arasan.Interface 
{
    public interface IIPO
    {
      
        
        
      
        DataTable GetPObyID(string Poid);
        DataTable GetPOrderID(string Poid);
        DataTable GetPOItembyID(string Poid);
        DataTable GetPOItem(string Poid);
        DataTable EditPObyID(string Poid);
        DataTable GetItemCF(string item,string unit);
        DataTable GetPOItemDetails(string Poid);
        
        string PurOrderCRUD(ImportPO Cy);
        
        string POtoGRN(string POID);
        string CRUDPOAttachement(List<IFormFile> POID, ImportPO cy);
        DataTable GetPObySuppID(string supid);

        

        string StatusChange(string tag, string id);

        DataTable GetPOItemrep(string id);
         
        DataTable GetAllPoItems(string strStatus);
        DataTable GetAllAttachment(long id);
    }
}
