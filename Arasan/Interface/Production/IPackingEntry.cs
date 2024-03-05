using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface 
{
    public interface IPackingEntry
    {
        DataTable  GetPackNote(string wcid);
        DataTable GetItem();
        DataTable GetMachine();
        DataTable GetNoteDetail(string Note);
        DataTable GetPackDetails(string Note);
        DataTable GetPackOutDetails(string Note);
        DataTable GetPacking(string Note);
        DataTable GetPackinp(string Note);
        DataTable GetPackMat(string Note);
        DataTable GetPackEmp(string Note);
        DataTable GetPackCons(string Note);
        DataTable GetPackMac(string Note);
        DataTable GetPackingDetail(string Note);
        //DataTable GetConstkqty(string Note);
        DataTable GETWC();
        string PackingEntryCRUD(PackingEntry cy);
        DataTable GetAllPackingentry(string strStatus);

        string StatusChange(string tag, string id);
    }
}
