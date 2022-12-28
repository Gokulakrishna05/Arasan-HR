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
        IEnumerable<PartyMaster> GetAllParty();
       DataTable GetParty(string id);
        DataTable GetCountryDetails(string id);
        DataTable GetPartyContact(string id);
        //DataTable GetEmpEduDeatils(string data);
        //DataTable GetEmpPersonalDeatils(string id);
        //DataTable GetEmpSkillDeatils(string id);
    }
}
