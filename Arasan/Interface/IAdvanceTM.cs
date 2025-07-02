using Arasan.Models;
using System.Data;

namespace Arasan.Interface
{
    public interface IAdvanceTM
    {

       
        string GetAdvanceT(AdvanceTM Em);

        DataTable GetAdvanceTMEdit(string id);
        DataTable GetAllAdvanceTM(string strStatus);
        string StatusChange(string tag, string id);
    }
}
