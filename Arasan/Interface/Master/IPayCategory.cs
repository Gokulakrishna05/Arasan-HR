using System.Data;
using Arasan.Models;

namespace Arasan.Interface.Master
{
    public interface IPayCategory
    {
        DataTable getPayCat();
        DataTable getPayCatId(string id);
        string PayCategoryCRUD(PayCategory cy);
        string StatusChange(string tag, string id);
        string RemoveChange(string tag, string id);
        DataTable GetAllPayCategory(string strStatus);

        DataTable GetEditPayCat(string id);
        DataTable GetEditPayCode(string id);



    }
}
