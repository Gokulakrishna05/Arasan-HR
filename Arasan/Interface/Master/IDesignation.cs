using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IDesignation
    {
        string DesignationCRUD(Designation ss);
        //IEnumerable<Designation> GetAllDesignation(string status);
        DataTable GetDeptName();
        DataTable GetDesignation(string id);

        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);

        DataTable GetAllDESIGNATION(string strStatus);
    }
}
