using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Services ;

namespace Arasan.Interface 
{
    public interface IDrumChange
    {

        DataTable GetItem(string id);
        DataTable GetDrum(string id,string loc);
        DataTable GetBatch(string id, string item, string loc);
        DataTable Getpackeddrum();
        DataTable GetreuseItem();
        DataTable GetEmployee();

        DataTable GetAllDrumChangeDeatils(string strStatus, string strfrom, string strTo);
    }
}
