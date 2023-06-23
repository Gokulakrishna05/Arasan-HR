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
        IEnumerable<Location> GetAllLocations(string status);
        Location GetLocationsById(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
        DataTable GetEditLocation(string id);
    }
}
