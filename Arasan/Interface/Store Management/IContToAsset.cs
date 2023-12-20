using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface 
{
    public interface IContToAsset
    {
        string ConstoassetCRUD(ContToAsset Cy);

        DataTable GetAllConAsset(string id);
        DataTable ViewAsscon(string id);
        DataTable ViewAssconDet(string id);
    }
}
