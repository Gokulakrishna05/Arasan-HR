using System.Collections.Generic;
using System.Data;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
namespace Arasan.Interface.Qualitycontrol
{
    public interface IQCResultService
    {
        DataTable GetGRN();
        //DataTable GetGRN(string id);
        DataTable GetGRNDetails(string id);
        DataTable GetItembyId(string id);
        string QCResultCRUD(QCResult cy);

        DataTable GetQCResult(string id);
        IEnumerable<QCResult> GetAllQCResult();
        DataTable GetLocation();
    }
}
