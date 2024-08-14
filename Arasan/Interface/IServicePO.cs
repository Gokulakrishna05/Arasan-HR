namespace Arasan.Interface;
using System.Data;
using Arasan.Models;

    public interface IServicePO
    {
    DataTable GetCurrency();
    DataTable GetTemp();
    DataTable GetPartyID();
    DataTable GetPrepared();
    DataTable GetTAN();
    DataTable GetUnit();
    DataTable GetAllServicePo(string strStatus);

    string GetInsService(ServicePOModel S);
    string StatusChange(string tag, string id);
    DataTable GetSPOBasicEdit(string id);
    DataTable GetSPOOrganizerEdit(string id);
    DataTable GetSPODetailEdit(string id);
    DataTable GetSPOTandcEdit(string id);



}

