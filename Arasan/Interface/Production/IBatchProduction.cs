using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{
    public interface IBatchProduction
    {
        DataTable ShiftDeatils();
        DataTable DrumDeatils();
        DataTable Getstkqty(string branch, string loc, string ItemId);
        DataTable BindProcess();
        DataTable SeacrhItem(string terms);
    }
}
