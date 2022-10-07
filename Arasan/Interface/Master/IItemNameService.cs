using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IItemNameService
    {
        DataTable GetItemGroup();
        DataTable GetItemCategory();
        DataTable GetItemSubGroup();
    }
}
