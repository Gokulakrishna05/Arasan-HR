using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using System.Data;

namespace Arasan.Interface
{
    public interface ICuringService
    {
        string CuringCRUD(Curing cy);

        DataTable GetCuring();
        //Curing GetCuringById(string id);
        DataTable GetCuringDeatil(string id);
        DataTable GetCuringDetails(string id);

        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetAllCuring(string strStatus);

     }
}