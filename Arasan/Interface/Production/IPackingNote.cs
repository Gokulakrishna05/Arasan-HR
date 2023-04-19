using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IPackingNote
    {
        DataTable ShiftDeatils();
        DataTable GetWorkCenter();
        IEnumerable<PackingNote>  GetAllPackingNote();
        string PackingNoteCRUD(PackingNote Cy);
        DataTable GetItembyId(string id);
        DataTable GetDrumLocation();
        DataTable GetDrumNo(string id);
        DataTable GetDrumDetails(string id);
        DataTable GetBatch(string id);
        DataTable GetPackingNote(string id);
        //DataTable GetItemDet(string id); 
        DataTable GetDrumItem(string id);
        DataTable GetSchedule( );

    }
}
