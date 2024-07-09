using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface
{
    public interface IAccTreeView
    {
        DataTable GetParent();
        DataTable Getchild(string parentid);
        DataTable GetAccClass();
        DataTable GetAccType(string id);
        DataTable GetAccGroup(string id);
        DataTable GetAccLedger(string id);
        string NodeCreation(Accounttree cy);
        string NodeDelete(string id);
    }
}
