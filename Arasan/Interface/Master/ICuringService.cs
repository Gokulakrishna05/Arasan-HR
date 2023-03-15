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
        Curing GetCuringById(string id);

        DataTable GetCuring();
        //DataTable GetSubgroup();
    }
}
