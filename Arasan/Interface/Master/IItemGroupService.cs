using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master
{
    public interface IItemGroupService
    {
        string ItemGroupCRUD(ItemGroup by);
        IEnumerable<ItemGroup> GetAllItemGroup(string status);
        ItemGroup GetItemGroupById(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
    }
}
