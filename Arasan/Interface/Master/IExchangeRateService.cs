using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IExchangeRateService
    {
        string ExchangeRateCRUD(ExchangeRate cy);

        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetAllExchangeGRID(string strStatus);
        DataTable GetEditExchangeDetail(string id);
        DataTable GetSym();

    }
}
