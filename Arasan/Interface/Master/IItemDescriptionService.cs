using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using System.Data;


namespace Arasan.Interface
{
    public interface IItemDescriptionService
    {
        IEnumerable<ItemDescription> GetAllItemDescription();
        DataTable GetEditItemDescription(string id);
        string ItemDescriptionCRUD(ItemDescription cy);
    }
}
