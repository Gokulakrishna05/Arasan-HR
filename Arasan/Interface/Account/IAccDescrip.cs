using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IAccDescrip
    {
        DataTable GetAccDescrip(string id);
        IEnumerable<AccountType> GetAllAccDescrip(string status);
    }
}
