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
        //ItemGroup GetItemGroupById(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);

        DataTable GetCategory();
        DataTable GetGroup(string id);
    }
}
