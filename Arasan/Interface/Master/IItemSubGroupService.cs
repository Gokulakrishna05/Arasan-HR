using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master
{
    public interface IItemSubGroupService
    {
        string ItemSubGroupCRUD(ItemSubGroup by);
        IEnumerable<ItemSubGroup> GetAllItemSubGroup(string status);
        ItemSubGroup GetItemSubGroupById(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
    }
}
