using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IProcessMastService
    {
        DataTable GetAllProcessMast(string strStatus);
        DataTable GetEditProcessMast(string id);
        string ProcessMastCRUD(ProcessMast cy);
        string StatusChange(string tag, string id);

    }
}
