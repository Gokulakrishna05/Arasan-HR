using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Models;
using System.Data;

namespace Arasan.Interface.Qualitycontrol
{
    public interface IQCFinalValueEntryService
    {
        DataTable GetWorkCenter();
        DataTable GetProcess();
        string QCFinalValueEntryCRUD(QCFinalValueEntry cy);
    }
}
