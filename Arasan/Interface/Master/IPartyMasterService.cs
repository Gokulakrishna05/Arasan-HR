using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IPartyMasterService
    {
        string PartyCRUD(PartyMaster emp);
        DataTable GetState();
        DataTable GetCity();
        DataTable GetCountry();
        //IEnumerable<PartyMaster> GetAllParty(string status);
       DataTable GetParty(string id);
        DataTable GetCountryDetails(string id);
        DataTable GetPartyContact(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
        string PartyTypeCRUD(string id);
        string CityCRUD(string id);
        string PartyGroup(string id);
        DataTable GetLedger();
        DataTable Getratecode();

        //DataTable GetEmpEduDeatils(string data);
        //DataTable GetEmpPersonalDeatils(string id);
        //DataTable GetEmpSkillDeatils(string id);

        DataTable GetAllParty(string strStatus);
    }
}
