using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IHolidayMaster
    {
        DataTable GetHolidayMasterEdit(string id);
        string GetHMaster(HolidayMaster Em);

        DataTable GetAllHolidayMaster(string strStatus);
        string StatusChange(string tag, string id);

    }
}
