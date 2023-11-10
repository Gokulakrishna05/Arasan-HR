using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IAccClass
    {
        IEnumerable<AccClass> GetAllAccClass(string active);
        string AccClassCRUD(AccClass ss);
        DataTable GetAccClass(string id);
        //DataTable GetType();
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);

        //string GetNumberwithPrefix();
    }
}
