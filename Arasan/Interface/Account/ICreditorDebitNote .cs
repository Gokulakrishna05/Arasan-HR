using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
//using GrapeCity.DataVisualization.Chart;

namespace Arasan.Interface.Account
{
    public interface ICreditorDebitNote
    {
        DataTable GetGroup();
        DataTable GetLedger();

        DataTable GetGrp();

        DataTable GetLed();
        DataTable GetSeq(String ItemId);
        DataTable GetGrpDetail(string id);
        DataTable GetGRPbyId(string id);
        DataTable GetLedbyId(string id);
    }
}
