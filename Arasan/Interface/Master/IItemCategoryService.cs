using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface.Master

{
    public interface IItemCategoryService
    {
        string CategoryCRUD(ItemCategory iy);
        //IEnumerable<ItemCategory> GetAllItemCategory(string status);
        ItemCategory GetCategoryById(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);

        DataTable GetAllItemCategory(string strStatus);
    }
}
