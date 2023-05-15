using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IDrumMaster
    {

        string DrumMasterCRUD(DrumMaster ss);
        IEnumerable<DrumMaster> GetAllDrumMaster();

        DataTable GetCategory();
        DataTable GetDrumType();
        DataTable GetDrumMaster(string id);



    }
}
