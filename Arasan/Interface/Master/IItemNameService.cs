using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IItemNameService
    {
        string ItemNameCRUD(ItemName by, List<IFormFile> file, List<IFormFile> file1);
        IEnumerable<ItemName> GetAllItemName();
        DataTable GetAllItems(string status);
        //ItemName GetSupplierDetailById(string id);
        DataTable GetItemGroup();
        DataTable GetItemCategory();
        DataTable GetItemSubGroup(string id);
        DataTable GetQCTemp();
        DataTable GetHSNcode();
        DataTable GetSupplier();
        DataTable BindBinID();
        string SupplierCRUD(ItemName pf);
        ItemName GetSupplierById(string id);
        DataTable GetAllSupplier(string id);
        DataTable GetAllUnit(string id);
        DataTable GetItemNameDetails(string id);
        DataTable GetBinDeatils(string id);
        DataTable GetSupplierName(string id);
        DataTable GetLedger();
        DataTable GetItem();
        DataTable GetUnit();
        DataTable GetAllAttachment(string id);
        string StatusChange(string tag, int id);
        string deletedoc(string tag, string id);
        string NewItemCRUD(ItemName ss, List<IFormFile> file, List<IFormFile> file1);


        //DataTable GetItem();
    }
}
