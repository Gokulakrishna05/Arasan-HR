using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IBonusMaster
    {

        DataTable GetDesignation();

        DataTable GetBonusMasterEdit(string id);
        string GetInsBonusM(BonusMaster Em);

        DataTable GetAllBonusMaster(string strStatus);

        string StatusChange(string tag, string id);


    }
}
