﻿using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Store_Management
{
    public interface IDirectAddition
    {
        string DirectAdditionCRUD(DirectAddition by);
        IEnumerable<DirectAddition> GetAllDirectAddition();
        DirectAddition GetDirectAdditionById(string id);
        DataTable GetLocation();
        DataTable GetBranch();
        DataTable GetItem(string value);
        DataTable GetDirectAdditionDetails(string id);
        DataTable GetDAItemDetails(string id);
        DataTable GetItemCF(string ItemId, string Unitid);
        IEnumerable<DirectItem> GetAllDirectAdditionItem(string id);
    }
}