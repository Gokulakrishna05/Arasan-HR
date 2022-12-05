using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface 
{
    public interface IPurchaseReturn
    {
        DataTable GetPO();
        //DataTable GetItem();
        DataTable GetPODetails(string ItemId);
    }
}
