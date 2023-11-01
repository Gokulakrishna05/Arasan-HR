using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IORSAT
    {
        DataTable GetBranch();
        DataTable Getshift();
        DataTable Getwork();

        string ORSATCRUD(ORSAT cy);

        IEnumerable<ORSAT> GetAllORSAT(string st, string ed);
        
        DataTable GetViewORSAT(string id);
        DataTable GetViewORSATDetail(string id);

        string StatusChange(string tag, int id);
    }
}
