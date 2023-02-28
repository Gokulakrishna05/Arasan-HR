using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IBatchCreation
    {
        DataTable GetWorkCenter();
        DataTable GetWorkCenterGr(string id);
        DataTable GetProcess(string id);
        DataTable GetProcessid(string id);
        IEnumerable<BatchCreation> GetAllBatchCreation();
        DataTable GetProcessid();
        DataTable GetItem();
        DataTable GetBatchCreation(string id);
        DataTable GetBatchCreationDetail(string id);
        string BatchCRUD(BatchCreation id);
    }
}
