using Arasan.Models;
using System.Data;
//using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Arasan.Interface 
{
    public interface IAccConfig
    {
        
        string  ConfigCRUD(AccConfig Cy);
        DataTable GetConfig();
        DataTable Getledger();

        DataTable GetAccConfigItem(string id);
        DataTable GetAccConfig(string id);
        DataTable Getschemebyid(string id);
        IEnumerable<AccConfig> GetAllAccConfig(string Active);

        DataTable GetSchemeDetails(string itemId);

        string StatusChange(string tag, int id);

        string RemoveChange(string tag, int id);
    }
}

