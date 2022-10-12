using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master
{
    public interface IItemSubGroupService
    {
        string ItemSubGroupCRUD(ItemSubGroup by);
        IEnumerable<ItemSubGroup> GetAllItemSubGroup();
        ItemSubGroup GetItemSubGroupById(string id);
    }
}
