using System.Collections.Generic;
using System.Data;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;

namespace Arasan.Interface.Qualitycontrol
{
    public interface IQCTestValueEntryService
    {
        DataTable ShiftDeatils();
        DataTable GetBranch();
        DataTable GetWorkCenter();
        string QCTestValueEntryCRUD(QCTestValueEntry cy);
        IEnumerable<QCTestValueEntry> GetAllQCTestValueEntry();
        DataTable GetQCTestValueEntryDetails(string id);
    }
}
