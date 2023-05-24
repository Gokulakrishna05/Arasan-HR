using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IAccountGroup
    {
        string AccountGroupCRUD(AccountGroup cy);
        DataTable GetAccountGroup(string id);
        DataTable GetAccType();
        IEnumerable<AccountGroup> GetAllAccountGroup();
        string StatusChange(string tag, int id);
    }
}
