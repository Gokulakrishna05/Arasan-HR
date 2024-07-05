using Arasan.Models;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface ICommission
    {
        DataTable GetUnit();
        DataTable GetItem();
        DataTable GetEditComm(String id);
        DataTable GetEditCommission(string id);

        string CommissionCRUD(Commission cp);

        string StatusDelete(string tag, string id);
        string RemoveChange(string tag, string id);


        DataTable GetAllCommissions(string strStatus);

    }
}
