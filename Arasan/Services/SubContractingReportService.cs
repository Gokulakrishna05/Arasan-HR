using Arasan.Interface;
using Arasan.Models;

using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Arasan.Services
{
    public class SubContractingReportService : ISubContractingReport
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SubContractingReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }


        public DataTable GetAllSubContReport(string dtFrom, string dtTo)
        {

            try
            {
                string SvSql = "";
                
                    SvSql = "Select P.PartyID, B.DocID DCNo, B.DocDate DCDate, B.RefNo, B.RefDate, 'Item Detail' Type, I.ItemID, I.ItemDesc, U.UnitID, D.MrQty,D.BRATE,I.SnCategory Icat,L.LOCID,Round((D.MrQty*D.BRATE)*1.18,2) Net From SubMRBasic B, subactmrdet D, PartyMast P, LocDetails L, ItemMaster  I, UnitMast U Where B.SubMRBasicID = D.SubMRBasicID and L.LOCDETAILSID = B.RLOCID And B.PartyID = P.PartyMastID And D.MItemID = I.ItemMasterID And B.DocDate Between '" + dtFrom + "' And '" + dtTo + "' And I.PriUnit = U.UnitMastID Order by PartyID,Locid,Icat,DcNo";
                
                OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
                DataTable dtReport = new DataTable();
                adapter.Fill(dtReport);
                return dtReport;

            }

            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
