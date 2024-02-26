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
        //IEnumerable<CuringOutward> GetAllCuringOutward(string st,string ed);
        string  CuringOutwardCRUD(CuringOutward Cy);
        DataTable Getcuringoutward(string id);
        DataTable GetCuringDetail(string id);
        DataTable GetCuringOutwardByName(string name); 
        DataTable CuringOutwardDetail(string name);
        string StatusChange(string tag, int id);
        DataTable GetAllCuringOutwardDetails(string strStatus, string st, string ed);
    }
}
