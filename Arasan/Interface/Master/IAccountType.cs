﻿using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IAccountType
    {
        string AccountTypeCRUD(AccountType ss);
        IEnumerable<AccountType> GetAllAccountType();

        DataTable GetAccountType(string id);

        string StatusChange(string tag, int id);
    }
}