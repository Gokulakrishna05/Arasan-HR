using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Arasan.Interface
{
    public interface IContract
    {
        string ContractCRUD(Contract cy);
        IEnumerable<Contract> GetAllContract(string status);

        DataTable GetAllContracts(string strStatus);
        DataTable GetEditContract(string id);

        string StatusDelete(string tag, int id);



    }
}
