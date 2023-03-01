using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface.Production
{
    public interface ICuringInwardService
    {
        DataTable ShiftDeatils();
        DataTable DrumDeatils();
        string CuringInwardCRUD(CuringInward cy);
        IEnumerable<CuringInward> GetAllCuringInward();
        DataTable GetBranch();
        DataTable GetCuringInward(string id);
        DataTable GetCuringInwardDetail(string id);
    }
}
