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
        IEnumerable<AccDescrip> GetAllAccDescrip(string Active);
        string AccDescripCRUD(AccDescrip cy);
        string StatusChange(string tag, int id);
        
        string RemoveChange(string tag, int id);
    }
}
