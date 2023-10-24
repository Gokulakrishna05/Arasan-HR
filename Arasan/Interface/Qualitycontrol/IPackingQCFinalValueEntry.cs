using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Qualitycontrol
{
    public interface IPackingQCFinalValueEntry
    {

        string PackingQCFinalValueEntryCRUD(PackingQCFinalValueEntry cy);
        string StatusChange(string tag, int id);
        IEnumerable<PackingQCFinalValueEntry> GetAllPackingQCFinalValueEntry(string st, string ed);
        
        DataTable GetPackingQCFinal();
        DataTable GetViewPacking(string id);
        DataTable GetViewPackingItem(string id);
        DataTable GetViewPackingGas(string id);

        DataTable GetPackingEntryDetails(string itemId);
        DataTable GetPEntryItemgrpDetails(string id);
        DataTable GetPackingitemDetail(string id);
    }
}
