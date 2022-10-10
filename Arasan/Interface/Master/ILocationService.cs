using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface
{
    public interface ILocationService
    {

        string LocationsCRUD(Location cy);
        IEnumerable<Location> GetAllLocations();
        Location GetLocationsById(string id);

    }
}
