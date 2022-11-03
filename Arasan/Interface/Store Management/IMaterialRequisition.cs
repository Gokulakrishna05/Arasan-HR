using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IMaterialRequisition
    {
      //  DataTable GetLocation();
       // DataTable GetBranch();
       DataTable GetWorkCenter(string LocationId);
        //   DataTable GetItem(string value);
        //  DataTable GetItemGrp();
        DataTable GetItem(string value);
        DataTable GetItemDetails(string ItemId);
       //  string MaterialRequestCRUD(MaterialRequisition mr);
        string MaterialCRUD(MaterialRequisition cy);
        IEnumerable<MaterialRequisition> GetAllMaterial();
        MaterialRequisition GetMaterialById(string id);
    }
}
