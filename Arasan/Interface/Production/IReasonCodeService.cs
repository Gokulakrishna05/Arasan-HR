using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Production
{
    public interface IReasonCodeService
    {
        //IEnumerable<ReasonCode> GetAllReasonCode();
        DataTable GetReasonCode(string id);
        DataTable GetReasonItem(string id);

        DataTable Getstop();
        DataTable Getprocess();
        string ReasonCodeCRUD(ReasonCode cy);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
        DataTable GetAllReason(string strStatus);
    }
}
