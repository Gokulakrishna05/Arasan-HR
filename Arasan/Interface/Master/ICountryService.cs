using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface ICountryService
    {
        string CountryCRUD(Country cy);
        //IEnumerable<Country> GetAllCountry(/*string status*/);
        Country GetCountryById(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
        DataTable GetAllCountryGRID();
    }
}
