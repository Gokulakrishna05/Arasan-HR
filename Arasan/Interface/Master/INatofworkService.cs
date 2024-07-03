using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface INatofworkService
    {
        DataTable GetAllNatofwork(string strStatus);
        DataTable GetEditNatofwork(string id);
        string NatofworkCRUD(Natofwork cy);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
    }
}
