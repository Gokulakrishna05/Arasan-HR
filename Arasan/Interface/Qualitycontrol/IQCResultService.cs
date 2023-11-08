using System.Collections.Generic;
using System.Data;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
namespace Arasan.Interface.Qualitycontrol
{
    public interface IQCResultService
    {


        //QCResult GetQCResultById(string id);
        string QCResultCRUD(QCResult cy);
        DataTable GetQCResult(string id);
        DataTable GetEmp();
        DataTable GetLocation();


        IEnumerable<QCResult> GetAllQCResult(string st,string ed);
       
        DataTable GetParty(string value);
        DataTable GetItembyId(string value);
        //DataTable GetPartybyId(string value);
        DataTable GetGRNDetails(string itemId);
        DataTable GetGRN();
        DataTable GetQCResultDetail(string id);
        DataTable GetViewQCResult(string id);
        DataTable GetViewQCResultDetail(string id);
        DataTable GetGRNItemDetails(string itemId);

        string StatusChange(string tag, int id);
    }
}
