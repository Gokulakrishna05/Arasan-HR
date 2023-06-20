using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master
{
    public interface ICurrencyService
    {
        string CurrencyCRUD(Currency cy);
        IEnumerable<Currency> GetAllCurrency(string status);
        Currency GetCurrencyById(string id);

        string StatusChange(string tag, int id);
    }
}
