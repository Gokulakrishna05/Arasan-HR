using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
    public interface IETariff
    {
        DataTable GetETariff(string id);
        //string RemoveChange(string tag, int id);
        string StatusChange(string tag, int id);

        string ETariffCRUD(ETariff cy);

        DataTable GetAllETariff();

    }
}
