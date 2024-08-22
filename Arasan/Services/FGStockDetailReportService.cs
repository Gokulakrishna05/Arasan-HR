 using Arasan.Interface;
using Arasan.Models;
using DocumentFormat.OpenXml.VariantTypes;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;
using Nest;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Arasan.Services
{
    public class FGStockDetailReportService : IFGStockDetailReport
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public FGStockDetailReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllFGStockDetailReport(string dtFrom, string loc)
        {

            try
            {
                string SvSql = "";


                //SvSql = " Select itemcat, sncategory, Itemid Grade,Unitid Unit, Count(Itemid) Noofbags,Qty AvgPer,(Count(Itemid) * Qty) Totqty,To_date(sysdate, 'DD/MM/YYYY') - To_Date(min(dt1), 'DD/MM/YYYY') Laydays,subcategory From ( Select I.itemcat, i.SNCATEGORY, I.Itemid, U.Unitid, M.Lotno, Sum(Plusqty-Minusqty) Qty,M.Docdate Dt1, i.SUBCATEGORY From PLstockvalue S, Itemmaster I,Locdetails L, Unitmast U,LotmastA M Where S.Docdate <= '" + dtFrom + "' And I.Itemmasterid = S.Itemid And I.Priunit = U.Unitmastid And S.Cancel = 'F' and S.DRUMNO not like '%E%' And L.Locdetailsid = S.Locid And S.Lotno = M.Lotno(+) And(L.LocID = '" + loc + "' Or 'ALL' = '" + loc + "') Group By I.itemcat,SNCATEGORY,I.Itemid,Unitid,M.Lotno,i.SUBCATEGORY,M.Docdate Having Sum(Plusqty - Minusqty) > 0 Union Select I.itemcat,i.SNCATEGORY,I.Itemid || '-Export',U.Unitid,M.Lotno,Sum(Plusqty - Minusqty) Qty,M.Docdate Dt1, i.SUBCATEGORY From PLstockvalue S, Itemmaster I,Locdetails L, Unitmast U,LotmastA M Where S.Docdate <= '" + dtFrom + "' And I.Itemmasterid = S.Itemid And I.Priunit = U.Unitmastid And S.Cancel = 'F' and S.DRUMNO like '%E%' And L.Locdetailsid = S.Locid And S.Lotno = M.Lotno(+) And(L.LocID = '" + loc + "' Or 'ALL' = '" + loc + "') Group By I.itemcat,SNCATEGORY,I.Itemid,Unitid,M.Lotno,i.SUBCATEGORY,M.Docdate Having Sum(Plusqty - Minusqty) > 0 ) Group by itemcat,sncategory,Itemid,Unitid,Qty,subcategory Order by 2,9 ,1,Itemid";

                SvSql = "Select itemcat,sncategory,Itemid Grade,Unitid Unit,Count(Itemid) Noofbags,Qty AvgPer,(Count(Itemid)*Qty) Totqty,subcategory,dt1 as asdate From ( Select I.itemcat,i.SNCATEGORY,I.Itemid,U.Unitid,S.Lotno,Sum(Plusqty-Minusqty) Qty,(to_date(SysDate,'dd-mon-yy')-Min(M.Docdate)) Dt1,i.SUBCATEGORY  From PLstockvalue S,Itemmaster I,Locdetails L,Unitmast U,PLotmast M Where S.Docdate <= '" + dtFrom + "' And I.Itemmasterid = S.Itemid And S.Cancel='F' And I.Priunit = U.Unitmastid And S.LOTNO=M.LOTNO And L.Locdetailsid = S.Locid And ( L.LocID = '" + loc + "' Or 'ALL' = '" + loc + "' )  Group By I.itemcat,SNCATEGORY,I.Itemid,Unitid,S.Lotno,i.SUBCATEGORY,M.DOCDATE Having Sum(Plusqty-Minusqty) > 0 Order by 2 ) Group by itemcat,sncategory,Itemid,Unitid,Qty,subcategory,dt1  Order by 2,1,Itemid asc, 9 desc";
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

        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCDETAILSID,LOCID from LOCDETAILS  WHERE IS_ACTIVE='Y' Union Select 1,'ALL' From Dual Order By 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
