using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface ITaxService
    {
        string TaxCRUD(Tax cy);
        IEnumerable<Tax> GetAllTax();

        DataTable GetTax(string id);
        string StatusChange(string tag, int id);
    }
}
