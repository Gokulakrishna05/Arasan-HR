using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IHSNcodeService
    {
        string HSNcodeCRUD(HSNcode by);
        IEnumerable<HSNcode> GetAllHSNcode(string status);
        //HSNcode GetHSNcodeById(string id);

        //DataTable GetCGst();

        //DataTable GetSGst();
        //DataTable GetIGst();

        DataTable GetAllhsncode(string strStatus);
        DataTable GettariffItem(string id);

        DataTable GetHSNcode(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);
        DataTable Gethsnitem(string PRID);
        DataTable Gettariff();

    }
}
