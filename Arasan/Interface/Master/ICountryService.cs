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
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetAllCountryGRID(string strStatus);
    }
}
