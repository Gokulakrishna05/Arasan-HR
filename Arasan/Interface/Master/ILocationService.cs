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
        IEnumerable<Location> GetAllLocations();
        Location GetLocationsById(string id);
        string StatusChange(string tag, int id);
        DataTable GetEditLocation(string id);
    }
}
