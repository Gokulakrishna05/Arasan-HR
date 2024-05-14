using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using System.Data;

namespace Arasan.Interface
{
    public interface IPrivilegesService
    {
        DataTable GetParent();
        DataTable Getchild(string parentid);
        string privilegesCRUD(PrivilegesModel Cy);
    }
}
