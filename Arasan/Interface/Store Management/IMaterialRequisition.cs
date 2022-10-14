using System.Data;
using Arasan.Models;
using Arasan.Models.Store_Management;

namespace Arasan.Interface
{
    public interface IMaterialRequisition
    {
        DataTable GetLocation();
        DataTable GetBranch();
        DataTable GetWorkCenter();
        DataTable GetItem(string value);
        DataTable GetItemGrp();

        string MaterialRequestCRUD(MaterialRequisition mr);
    }
}
