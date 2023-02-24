using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IBatchCreation
    {
        DataTable GetWorkCenter();
        DataTable GetProcess(string id);
        IEnumerable<BatchCreation> GetAllBatchCreation();
        DataTable GetProcess();
        DataTable GetItem();
    }
}
