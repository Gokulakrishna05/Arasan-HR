using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IPackDrumAllocation
    {
       
        DataTable GetLoc();
        DataTable GetAllPackDerum(string strStatus);
        DataTable GetDetails(string id);
        DataTable GetPackDrum(string id);
        DataTable EditDrumDetail(string id);

        string PackDrumCRUD(PackDrumAllocation Cy);
        string StatusChange(string tag, string id);

    }
}
