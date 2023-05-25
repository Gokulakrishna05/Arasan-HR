using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master
{
    public interface ICompanyService
    {
        string CompanyCRUD(Company cy);
        IEnumerable<Company> GetAllCompany();
        Company GetCompanyById(string id);
        string StatusChange(string tag, int id);

    }
}
