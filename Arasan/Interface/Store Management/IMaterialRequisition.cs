using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IMaterialRequisition
    {
      //  DataTable GetLocation();
       // DataTable GetBranch();
       DataTable GetWorkCenter(string LocationId);
     //   DataTable GetItem(string value);
      //  DataTable GetItemGrp();

       // string MaterialRequestCRUD(MaterialRequisition mr);
    }
}
