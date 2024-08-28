using Arasan.Interface.Report;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using ClosedXML.Excel;
using Microsoft.CodeAnalysis.Operations;

namespace Arasan.Services
{
    public class FGStockSivakasiDepotService : IFGStockSivakasiDepot
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public FGStockSivakasiDepotService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllFGStockSivakasiDepot(string dtFrom)
        {
            try
            {
                string SvSql = "";
                SvSql = "Select itemcat,sncategory,Itemid Grade,Unitid Unit,Count(Itemid) Noofbags,Qty AvgPer,(Count(Itemid)*Qty) Totqty,To_date(sysdate,'DD/MM/YYYY')-To_Date(min(dt1),'DD/MM/YYYY') Laydays,subcategory From \r\n (\r\nSelect I.itemcat,i.SNCATEGORY,I.Itemid,U.Unitid,M.Lotno,Sum(Plusqty-Minusqty) Qty,M.Docdate Dt1,i.SUBCATEGORY\r\n From PLstockvalue S,Itemmaster I,Locdetails L,Unitmast U,LotmastA M\r\nWhere S.Docdate <= '"+dtFrom+"'\r\nAnd I.Itemmasterid = S.Itemid\r\nAnd I.Priunit = U.Unitmastid\r\nAnd S.Cancel='F' and S.DRUMNO not like '%E%'\r\nAnd L.Locdetailsid = S.Locid\r\nAnd S.Lotno=M.Lotno(+)\r\nAnd L.LocID = 'FG GODOWN-SVKS DEPOT'\r\nGroup By I.itemcat,SNCATEGORY,I.Itemid,Unitid,M.Lotno,i.SUBCATEGORY,M.Docdate\r\nHaving Sum(Plusqty-Minusqty) > 0\r\nUnion\r\nSelect I.itemcat,i.SNCATEGORY,I.Itemid||'-Export',U.Unitid,M.Lotno,Sum(Plusqty-Minusqty) Qty,M.Docdate Dt1,i.SUBCATEGORY\r\n From PLstockvalue S,Itemmaster I,Locdetails L,Unitmast U,LotmastA M\r\nWhere S.Docdate <= '"+dtFrom+"'\r\nAnd I.Itemmasterid = S.Itemid\r\nAnd I.Priunit = U.Unitmastid\r\nAnd S.Cancel='F' and S.DRUMNO like '%E%'\r\nAnd L.Locdetailsid = S.Locid\r\nAnd S.Lotno=M.Lotno(+)\r\nAnd L.LocID = 'FG GODOWN-SVKS DEPOT'\r\nGroup By I.itemcat,SNCATEGORY,I.Itemid,Unitid,M.Lotno,i.SUBCATEGORY,M.Docdate\r\nHaving Sum(Plusqty-Minusqty) > 0\r\n)\r\nGroup by itemcat,sncategory,Itemid,Unitid,Qty,subcategory\r\nOrder by 2,9 ,1,Itemid";
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
