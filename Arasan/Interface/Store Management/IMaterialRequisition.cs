using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IMaterialRequisition
    {
         // DataTable GetLocation();
         // DataTable GetBranch();
       //DataTable GetWorkCenter(string LocationId);
        //   DataTable GetItem(string value);
        //  DataTable GetItemGrp();
        DataTable GetItem();
        DataTable GetItemDetails(string ItemId);
       //  string MaterialRequestCRUD(MaterialRequisition mr);
        string MaterialCRUD(MaterialRequisition cy);
        IEnumerable<MaterialRequisition> GetAllMaterial();
       // MaterialRequisition GetMaterialById(string id);
        DataTable GetmaterialReqDetails(string id);
        DataTable GetmaterialReqItemDetails(string id);
        DataTable Getstkqty(string ItemId,string locid, string brid);
        DataTable GetMatbyID(string MatId);
        DataTable GetMatItemByID(string MatId);
        DataTable GetLocation( );
        DataTable GetWorkCenter();
        DataTable BindProcess(string id);
        string IssuetoIndent(MaterialRequisition cy);
        string ApproveMaterial(MaterialRequisition cy);


    }
}
