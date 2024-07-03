using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IDrumMaster
    {

        string DrumMasterCRUD(DrumMaster ss);
        DataTable GetAllDrummast(string strStatus);
        DataTable GetCategory();
        DataTable GetDrumType();
        DataTable GetDrumMaster(string id);

        string CategoryCRUD(string id);
        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);


    }
}
