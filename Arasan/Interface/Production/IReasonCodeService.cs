using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Production
{
    public interface IReasonCodeService
    {
        IEnumerable<ReasonCode> GetAllReasonCode();
        string ReasonCodeCRUD(ReasonCode cy);
    }
}
