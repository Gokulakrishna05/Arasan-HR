using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master

{
    public interface IItemCategoryService
    {
        string CategoryCRUD(ItemCategory Ic);
        IEnumerable<ItemCategory> GetAllItemCategory();
        ItemCategory GetCategoryById(string id);
    }
}
