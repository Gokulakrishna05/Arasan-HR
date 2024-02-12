using System.Data;
using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;

namespace Arasan.Interface.Production
{
    public interface IDrumIssueEntryService
    {
        DataTable BindBinID();
        DataTable GetLocation();
        DataTable DrumDeatils(string id,string item);
        string DrumIssueEntryCRUD(DrumIssueEntry cy);
        IEnumerable<DrumIssueEntry> GetAllDrumIssueEntry(string st, string ed);
        DataTable GetBranch();
        DataTable GetAllDrumIssueItems(string strStatus);
        DataTable GetDIEDetail(string id);
        DataTable GetDrumIssuseDetails(string id);
        DataTable GetItem(string id);
        DataTable EditDrumIssue(string DRUM);
        DataTable EditDrumDetail(string DRUM);
        DataTable GetDrumDetails(string DRUM);
        //DataTable GetItemDetails(string id);
        DataTable GetStockDetails(string DRUM,string item);
        DataTable GetDrumStockDetail(string id,string item);
    }

}
