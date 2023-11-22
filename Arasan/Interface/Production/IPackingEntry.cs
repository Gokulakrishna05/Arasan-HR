using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface 
{
    public interface IPackingEntry
    {
        DataTable  GetPackNote();
        DataTable GetItem();
        DataTable GetMachine();
        DataTable GetNoteDetail(string Note);
        DataTable GetPackDetails(string Note);
        DataTable GetPackOutDetails(string Note);
        //DataTable GetConstkqty(string Note);

        string PackingEntryCRUD(PackingEntry cy);
        DataTable GetAllPackingentry(string strStatus);

        string StatusChange(string tag, string id);
    }
}
