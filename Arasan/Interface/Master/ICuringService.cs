using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface ICuringService
    {
        string CuringCRUD(Curing cy);
        IEnumerable<Curing> GetAllCuring();
        DataTable GetCuring();
        Curing GetCuringById(string id);
        DataTable GetCuringDeatil(string id);
        DataTable GetSubgroup();
    }
}
