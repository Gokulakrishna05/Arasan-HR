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
        IEnumerable<QCFinalValueEntry> GetAllQCFinalValueEntry();
        DataTable DrumDeatils();
        DataTable BatchDeatils();
        DataTable GetItem(string value);
        DataTable GetQCFVDeatil(string id);
        DataTable GetQCFVResultDetail(string id);
        DataTable GetQCFVGasDetail(string id);
    }
}
