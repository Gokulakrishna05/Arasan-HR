using Arasan.Interface;
using Arasan.Models;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Arasan.Services
{
    public class SubContractingMonthwiseReportService : ISubContractingMonthwiseReport
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SubContractingMonthwiseReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllSubContMonWisReport(string dtFrom, string dtTo)
        {

            try
            {
                string SvSql = "";

                SvSql = " SELECT PartyID, ItemID, UnitID, round(SUM(apr), 0) apr,round(SUM(may), 0) may,round(Sum(jun), 0) jun,round(Sum(jul), 0) jul,round(Sum(aug), 0) aug,round(Sum(sep), 0) sep,round(Sum(oct), 0) oct,round(Sum(nov), 0) nov,round(Sum(decm), 0) decm,round(Sum(jan), 0) jan,round(Sum(feb), 0) feb,round(Sum(mar), 0) mar,round(Sum(apr + may + jun + jul + aug + sep + oct + nov + decm + jan + feb + mar), 0) tot FROM( SELECT PartyID, ItemID, UnitID, DECODE(m, 'OP', SUM(ff), 0) OP, DECODE(m, 'APR', SUM(ff), 0) apr, DECODE(m, 'MAY', SUM(ff), 0) MAY, DECODE(m, 'JUN', SUM(ff), 0) JUN, DECODE(m, 'JUL', SUM(ff), 0) JUL, DECODE(m, 'AUG', SUM(ff), 0) aug, DECODE(m, 'SEP', SUM(ff), 0) sep, DECODE(m, 'OCT', SUM(ff), 0) OCT, DECODE(m, 'NOV', SUM(ff), 0) NOV, DECODE(m, 'DEC', SUM(ff), 0) DEcm, DECODE(m, 'JAN', SUM(ff), 0) JAN, DECODE(m, 'FEB', SUM(ff), 0) FEB, DECODE(m, 'MAR', SUM(ff), 0) MAR from( Select P.PartyID, B.DocID DCNo, TO_CHAR(B.DOCDATE, 'MON') m, B.RefNo, B.RefDate, 'Item Detail' Type, I.ItemID, I.ItemDesc, U.UnitID, D.MrQty ff, D.MRRATE From SubMRBasic B, subactmrdet D, PartyMast P, ItemMaster  I, UnitMast U Where B.SubMRBasicID = D.SubMRBasicID And B.PartyID = P.PartyMastID And D.MItemID = I.ItemMasterID And B.DocDate Between '" + dtFrom + "' And '" + dtTo + "' And I.PriUnit = U.UnitMastID )Group by PartyID, ItemID, UnitID, m )Group by PartyID,ItemID,UnitID Order by 1,2";
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
