using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface ICityService
    {
        string CityCRUD(City by);
        IEnumerable<City> GetAllCity();
        City GetCityById(string id);

        DataTable GetState();
        DataTable Getcountry();
    }
}