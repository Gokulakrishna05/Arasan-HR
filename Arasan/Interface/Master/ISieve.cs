using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
namespace Arasan.Interface
{
    public interface ISieve
    {
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);

        string SieveCRUD(Sieve ss);
        DataTable GetSieve(string id);

        DataTable GetviewSieve(string id);

        DataTable GetAllSieve(string strStatus);
    }

}