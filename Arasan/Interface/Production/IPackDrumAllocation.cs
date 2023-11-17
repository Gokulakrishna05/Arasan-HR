using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IPackDrumAllocation
    {
       
        DataTable GetLoc();
        DataTable GetDetails(string id);
    }
}
