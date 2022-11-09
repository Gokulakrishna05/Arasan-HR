using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IDirectPurchase
    {
        DataTable GetBranch();
        DataTable GetSupplier();
        DataTable GetCurency();
        DataTable GetLocation();
        DataTable GetItem(string value);
       //DataTable GetItemSubGrp();
       // DataTable GetItemSubGroup(string id);

        DataTable GetItemGrp();
    }
}
