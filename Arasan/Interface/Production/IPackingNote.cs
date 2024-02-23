using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
using System.Reflection.Metadata;

namespace Arasan.Interface
{
    public interface IPackingNote
    {
        DataTable ShiftDeatils();
        DataTable GetWorkCenter();
        IEnumerable<PackingNote> GetAllPackingNote(string st, string ed);
        string PackingNoteCRUD(PackingNote Cy);
        DataTable GetItembyId(string id);
        DataTable GetItem(string id);
        DataTable GetDrumLocation();

        DataTable DrumDeatils(string id, string loc);
        DataTable GetDrumDetails(string id, string item);
        DataTable GetDrumLot(string id, string item, string drum);

        DataTable DrumDeatils(string id,string loc);
        DataTable GetDrumDetails(string id,string item);
        DataTable GetDrumDetailsdd(string id,string item);
        DataTable GetDrumLot(string id,string item,string drum);

        DataTable GetBatch(string id);
        DataTable GetPackingNote(string id);
        //DataTable GetItemDet(string id); 
        DataTable GetDrumItem(string id);
        DataTable GetSchedule();
        DataTable EditNote(string id);
        DataTable EditDrumDetail(string id);
        DataTable GetAllPackingDeatils(string strStatus, string strfrom, string strTo);

        string StatusChange(string tag, int id);
    }
}
