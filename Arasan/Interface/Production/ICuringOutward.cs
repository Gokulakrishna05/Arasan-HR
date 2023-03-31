using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface 
{
    public interface ICuringOutward
    {
        DataTable ShiftDeatils(string id);
        DataTable GetWorkCenter();
        DataTable GetWorkCenterID();
        DataTable GetDrumLocation();
        DataTable GetDrumNo(string id);
        DataTable GetBatch(string id);
        DataTable GetPackingNote();
        DataTable GetItembyId(string id);
        DataTable GetPackingDetail(string id);
        IEnumerable<CuringOutward> GetAllCuringOutward();
        string  CuringOutwardCRUD(CuringOutward Cy);
    }
}
