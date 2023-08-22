using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;
namespace Arasan.Interface 
{
	public interface IAPProductionEntry
	{


		DataTable ShiftDeatils();
		string APProductionEntryCRUD(APProductionentry Cy);

        string APProductionEntryDetCRUD(APProductionentryDet Cy );

        string APProEntryCRUD(APProductionentryDet Cy);
        
        IEnumerable<APProductionentry>  GetAllAPProductionentry();
		DataTable GetItemDetails(string id);

        DataTable SaveOutDetails(string id, string item,string drum, string time, string qty);

        DataTable GetOutItemDetails(string id);
        DataTable GetConItemDetails(string id);
		DataTable GetItem();

        DataTable GetOutItem();
        DataTable GetAPProd(string id);

        DataTable GetInput(string id);
        DataTable GetOutput(string id);

        DataTable GetResult(string id);
        DataTable GetCons(string id);
        DataTable GetEmpdet(string id);
        DataTable GetBreak(string id);
        DataTable GetItemCon();

        DataTable GetDrum();

        DataTable GetAPWorkCenter();
		DataTable GetMachineDetails(string id);
		DataTable GetEmployeeDetails(string id);




        Task<IEnumerable<APItemDetail>> GetAPItem( string aid);
        Task<IEnumerable<APItemDetails>> GetAPItems(string bid );
        Task<IEnumerable<APItemDetailsc>> GetAPItemsc(string cid);

    }
}
