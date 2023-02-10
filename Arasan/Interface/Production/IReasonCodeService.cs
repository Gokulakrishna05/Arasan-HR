using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Production
{
    public interface IReasonCodeService
    {
        IEnumerable<ReasonCode> GetAllReasonCode();
        DataTable GetReasonCode(string id);
        string ReasonCodeCRUD(ReasonCode cy);
    }
}
