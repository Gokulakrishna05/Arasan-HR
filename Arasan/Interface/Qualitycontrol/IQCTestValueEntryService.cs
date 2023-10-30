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
        DataTable GetItem();
        string QCTestValueEntryCRUD(QCTestValueEntry cy);
        IEnumerable<QCTestValueEntry> GetAllQCTestValueEntry(string st, string ed);
        DataTable GetQCTestValueEntryDetails(string id);
        DataTable GetQCTestDetails(string id);
        DataTable GetAPOutDetails(string id);
        DataTable GetItemDetail(string id);
        DataTable GetAPOutItemDetails(string id);
        DataTable GetViewQCTestValueEntry(string id);
        DataTable GetViewQCTestDetails(string id);

        string StatusChange(string tag, string id);
        DataTable GetAPout(string id);
        DataTable GetAPout1(string id);
        DataTable GetDis(string id);
        DataTable GetResultItem(string id);
        DataTable GetResultItemDeatils(string id);
    }
}
