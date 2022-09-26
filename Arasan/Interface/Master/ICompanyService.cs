using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
namespace Arasan.Interface.Master
{
    public interface ICompanyService
    {
        IEnumerable<Company> GetAllCompany();
        Company GetCompanyById(string id);
        string CompanyCRUD(Company cy);
    }
}
