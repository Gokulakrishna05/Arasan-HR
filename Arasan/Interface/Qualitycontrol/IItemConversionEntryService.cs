using System.Collections.Generic;
using System.Data;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;

namespace Arasan.Interface.Qualitycontrol
{
    public interface IItemConversionEntryService
    {
        IEnumerable<ItemConversionEntry> GetAllItemConversionEntry(string st,string ed);
        DataTable GetBatchDetails(string id);
        DataTable GetBinDetails(string id);
        DataTable GetDrumStockDetail(string id, string item);
        DataTable GetEditDeatils(string id);
        DataTable GetItem(string id);
        DataTable GetItem();
        DataTable GetStockDetails(string id);
        DataTable GetUnitDeatils(string id);
        string ItemConversionEntryCRUD(ItemConversionEntry cy);
    }
}
