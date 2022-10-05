using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master
{
    public interface ICountryService
    {
        string CountryCRUD(Country cy);
        IEnumerable<Country> GetAllCountry();
        Country GetCountryById(string id);
    }
}
