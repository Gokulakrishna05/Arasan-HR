using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master
{
    public interface ICurrencyService
    {
        string CurrencyCRUD(Currency cy);
        IEnumerable<Currency> GetAllCurrency();
        Currency GetCurrencyById(string id);
    }
}
