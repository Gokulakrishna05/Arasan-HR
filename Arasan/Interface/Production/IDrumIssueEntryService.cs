﻿using System.Data;
using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;

namespace Arasan.Interface.Production
{
    public interface IDrumIssueEntryService
    {
        DataTable BindBinID();
        DataTable DrumDeatils();
        string DrumIssueEntryCRUD(DrumIssueEntry cy);
        IEnumerable<DrumIssueEntry> GetAllDrumIssueEntry();
        DataTable GetBranch();
        DataTable GetDIEDetail(string id);
        DataTable GetDrumIssuseDetails(string id);
        DataTable GetItem();
    }
}