using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Store_Management
{
    public interface ISubContractingDC
    {
        DataTable GetAllListSubContractingDCItems(string strStatus);
        string GetDrumStock(string itemId);
        DataTable GetSupplier( );
        DataTable GetLocation( );
        DataTable GetPackItem( );
        DataTable GetItem(string value);
        DataTable GetPartyDetails(string itemId);
        DataTable GetPartyItem(string itemId);
        DataTable GetItemDetails(string ItemId);
        DataTable GetSubContractDrumDetails(string itemId ,string loc);
        string StatusChange(string tag, string id);
        string SubContractingDCCRUD(SubContractingDC ss);
        string ApproveSubContractingDCCRUD(SubContractingDC ss);
        string PackMatSubConDCCRUD(SubContractingDC ss);

        DataTable GetItemCF(string ItemId, string unitid);
        DataTable GetSubContractingDCDeatils(string id);
        DataTable ViewSubContractDrumDetails(string id);
        DataTable GetEditItemDetails(string id);
        DataTable GetEditReceiptDetailItem(string id);
        DataTable GetSubViewDeatils(string id);
        DataTable GetSubContractViewDetails(string id);
        DataTable GetReceiptViewDetail(string id);
        //DataTable GetPartyItem(string ItemId);

        Task<IEnumerable<Subcondcdet>> GetSubcondc(string id );
        Task<IEnumerable<Subcondcdetet>> GetSubcondcdet(string id );
    }
}
