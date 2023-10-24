using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Qualitycontrol
{
    public interface IORSAT
    {
        DataTable GetBranch();

        //string ORSATCRUD(ORSAT cy);

        //IEnumerable<QCFinalValueEntry> GetAllQCFinalValueEntry(string st, string ed);
        //DataTable DrumDeatils();
        //DataTable BatchDeatils(string id);
    }
}
