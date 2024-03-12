using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Services;

namespace Arasan.Interface 
{
    public interface IStockReconcilation
    {

        DataTable GetItem(string id);
        DataTable GetDrum(string id,string loc);
    }
}
