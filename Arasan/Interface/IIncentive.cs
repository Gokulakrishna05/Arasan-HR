
using System.Data;
using System.Diagnostics.Contracts;
using Arasan.Models;


namespace Arasan
{
    public interface IIncentive
    {
        DataTable GetEmpId();
        DataTable GetIncentiveEdit(string id);
        string IncentiveCRUD(Incentive ic);
        DataTable GetAllIncentive(string strStatus);
        string StatusChange(string tag, string id);

    }
}
