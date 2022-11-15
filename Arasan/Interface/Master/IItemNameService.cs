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
        ItemName GetItemNameById(string id);

        DataTable GetItemGroup();
        DataTable GetItemCategory();
        DataTable GetItemSubGroup();
        DataTable GetHSNcode();
        DataTable GetSupplier();


    }
}
