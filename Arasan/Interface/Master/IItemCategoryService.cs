using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master

{
    public interface IItemCategoryService
    {
        string CategoryCRUD(ItemCategory iy);
        IEnumerable<ItemCategory> GetAllItemCategory(string status);
        ItemCategory GetCategoryById(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
    }
}
