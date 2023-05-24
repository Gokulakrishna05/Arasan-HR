using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface
{
    public interface IAccTreeView
    {
        DataTable GetAccType();
        DataTable GetAccGroup(string id);
        DataTable GetAccLedger(string id);
    }
}
