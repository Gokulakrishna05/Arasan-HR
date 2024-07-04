using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface ILocationService
    {
        DataTable GetBranch();
        string LocationsCRUD(Location cy);
        string LocCRUD(Location cy);
        Location GetLocationsById(string id);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        string CityCRUD(string id);
        DataTable GetEditLocation(string id);

        DataTable GetAllLocation(string strStatus);
        //DataTable GetSupplier();
        DataTable GetPartyDetails(string ItemId);
        DataTable GetSupplier();
        DataTable GetState();
        DataTable GetEditBinDeatils(string id);
        DataTable GetEditLocDeatils(string id);
        DataTable GetViewLocationDeatils(string id);
        DataTable GetViewEditLocDeatils(string id);
    }
}
