using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface ICuringService
    {
        string CuringCRUD(Curing cy);
        IEnumerable<Curing> GetAllCuring(string status);
        DataTable GetCuring();
        //Curing GetCuringById(string id);
        DataTable GetCuringDeatil(string id);
        DataTable GetCuringDetails(string id);
        DataTable GetSubgroup();

        string StatusChange(string tag, int id);
        string RemoveChange(string tag, int id);

    }
}
