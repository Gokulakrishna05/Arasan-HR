using Arasan.Models;
using System.Data;

namespace Arasan.Interface.Master
{
    public interface IMchine
    {
        DataTable GetUnit();
        DataTable GetAllMachine(string strStatus);
        DataTable GetAllMach(string strStatus);
        string Homereturn(Machine Mac);
        string StatusChange(string tag,string id);

        DataTable GetMachineEdit(string id);
        DataTable GetItem();
        DataTable GetMajor();
        //DataTable GetComp(string id); 
        //DataTable GetMajor(string id);
        DataTable GetCheck();
        string AddPurchaseCRUD(string id);
        string AddMadeCRUD(string id);




    }
   
}
