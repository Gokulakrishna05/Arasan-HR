using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IItemGroupService
    {
        string ItemGroupCRUD(ItemGroup by);
        //IEnumerable<ItemGroup> GetAllItemGroup(string status);

        DataTable GetAllItemGroup(string strStatus);
        ItemGroup GetItemGroupById(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
    }
}
