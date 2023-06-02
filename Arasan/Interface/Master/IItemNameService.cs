using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IItemNameService
    {
        string ItemNameCRUD(ItemName by);
        IEnumerable<ItemName> GetAllItemName();
        DataTable GetAllItems();
        //ItemName GetSupplierDetailById(string id);
        DataTable GetItemGroup();
        DataTable GetItemCategory();
        DataTable GetItemSubGroup();
        DataTable GetHSNcode();
        DataTable GetSupplier();
        DataTable BindBinID();
        string SupplierCRUD(ItemName pf);
        ItemName GetSupplierById(string id);
        DataTable GetAllSupplier(string id);
        DataTable GetItemNameDetails(string id);
        DataTable GetBinDeatils(string id);
        DataTable GetSupplierName(string id);
        DataTable GetLedger();
        //string StatusChange(string tag, int id);
        //DataTable GetItem();
    }
}
