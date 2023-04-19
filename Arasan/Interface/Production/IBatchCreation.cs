using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IBatchCreation
    {
        DataTable GetWorkCenter();
       
        DataTable GetProcess();
        DataTable GetProcessid();
        IEnumerable<BatchCreation> GetAllBatchCreation();
        //DataTable GetProcessid();
        DataTable GetItem();
        DataTable GetBatchCreation(string id);
        DataTable GetBatchCreationDetail(string id);
        string BatchCRUD(BatchCreation id);
        DataTable GetBatchCreationInputDetail(string id);
        DataTable GetBatchCreationOutputDetail(string id);
        DataTable GetBatchCreationOtherDetail(string id);
        DataTable GetBatchCreationParmDetail(string id);
        DataTable GetBatchCreationByName(string name);
        DataTable BatchDetail(string name);
        DataTable BatchInDetail(string name);
        DataTable BatchOutDetail(string name);
        DataTable BatchOtherDetail(string name);
        DataTable BatchParemItemDetail(string name);
        DataTable GetProd();
    }
}
