using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface IDrumCategory
    {

        string DrumCategoryCRUD(DrumCategory ss);
        IEnumerable<DrumCategory> GetAllDrumCategory(string status);

        DataTable GetDrumCategory(string id);

        string StatusChange(string tag, int id);


    }
}
