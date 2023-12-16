using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface
{ 
    public interface IAssetAddDed
    {

        string  AssetAddDedCRUD(AssetAddDed Cy);

        DataTable GetAllAddition(string id);
        DataTable GetAllDeduction(string id);
    }
}
