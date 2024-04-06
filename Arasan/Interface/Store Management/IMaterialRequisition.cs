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
        DataTable GetItemGrp();
        DataTable GetLoc();
        DataTable GetItemGroup(string id);
        DataTable GetItemDetails(string ItemId);
        //  string MaterialRequestCRUD(MaterialRequisition mr);
        string MaterialCRUD(MaterialRequisition cy);
        //IEnumerable<MaterialRequisition> GetAllMaterial(string status, string st, string ed);
        // MaterialRequisition GetMaterialById(string id);
        DataTable GetmaterialReqDetails(string id);
        DataTable GetmaterialReqItemDetails(string id);
        DataTable Getstkqty(string ItemId, string locid );
        DataTable GetItemLot(string ItemId, string locid );
        DataTable GetMatbyID(string MatId);
        DataTable GetMatItemByID(string MatId);
        DataTable GetIndMatItemByID(string MatId);
        DataTable GetLocation(string id);
        DataTable GetWorkCenter(string id);
        DataTable BindProcess(string id);
        DataTable GetReqMatItemByID(string id);
        string IssuetoIndent(MaterialRequisition cy);
        string ApproveMaterial(MaterialRequisition cy);

        string MaterialStatus(MaterialRequisition cy);
        string WholeStockGURD(MaterialRequisition cy);
        string MaterialReqGURD(MaterialReq cy);
        string ApproveReqGURD(MaterialReq cy);
        DataTable GetMatStabyID(string MatId);
        DataTable GetMatStaItemByID(string MatId);
        string StatusChange(string tag, string id);
        DataTable GetAllMaterialRequItems(string strStatus);
        DataTable GetMRItem(string id);
        DataTable GetAllInventoryReq(string strStatus);
        DataTable GetAllMaterialDetailRequItems(string strStatus);

    }
}
