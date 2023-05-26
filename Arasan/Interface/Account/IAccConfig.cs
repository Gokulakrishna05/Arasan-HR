using Arasan.Models;
using System.Data;
using static Org.BouncyCastle.Math.EC.ECCurve;

namespace Arasan.Interface 
{
    public interface IAccConfig
    {
        DataTable GetLedger();
        string  ConfigCRUD(AccConfig Cy);

        DataTable GetConfig();
    }
}
