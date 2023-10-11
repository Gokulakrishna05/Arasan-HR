using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Qualitycontrol
{
    public interface IQCFinalValueEntryService
    {
        DataTable GetWorkCenter();
        DataTable GetProcess();
        string QCFinalValueEntryCRUD(QCFinalValueEntry cy);
        IEnumerable<QCFinalValueEntry> GetAllQCFinalValueEntry(string st,string ed);
        DataTable DrumDeatils();
        DataTable BatchDeatils(string id);
        DataTable GetItemDetail(string id);
        DataTable GetItem(string value);
        DataTable GetQCFVDeatil(string id);
        DataTable GetQCFVResultDetail(string id);
        DataTable GetQCFVGasDetail(string id);
        DataTable GetQC (string id);
        DataTable GetQCDetails(string id);
        DataTable GetQCOutDeatil(string id);

        string StatusChange(string tag, int id);
        DataTable GetAPOutDetails(string id);
        DataTable GetAPOutItemDetails(string id);
    }
}
