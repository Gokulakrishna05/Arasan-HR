using Arasan.Interface;
using Arasan.Models;
using DocumentFormat.OpenXml.Spreadsheet;
using DocumentFormat.OpenXml.VariantTypes;
using Intuit.Ipp.DataService;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;
using Microsoft.VisualBasic;
using Nest;
using Oracle.ManagedDataAccess.Client;
using System.Data;
namespace Arasan.Services
{
    public class StockStatementService : IStockStatement
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StockStatementService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "Select B.BRANCHMASTID,B.BRANCHID From BRANCHMAST B Where B.IS_ACTIVE='Y' Union Select 1,'ALL' From Dual Order By 1";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = @"Select W.Wcbasicid, W.Wcid From Wcbasic W , Locdetails L Where W.Wctype = 'INTERNAL' And W.Ilocation = L.Locdetailsid
            And L.Locationtype = 'CURING'
            Union
            Select 1,'ALL' From Dual    
            Order By 2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetType()
        {
            string SvSql = string.Empty;
            SvSql = "Select 1 Id ,'Shed wise' Shed_Item From Dual Union Select 2 Id,'Item wise' Shed_Item From Dual";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable Getswis(string type)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT BINBASICID,BINID List,'' SNCATEGORY From Binbasic B,Locdetails L Where L.Locdetailsid = B.Locdetailsid And L.Locationtype = 'CURING' and 'Shed wise' = '" + type + "' Union select unique i.ITEMMASTERID,i.ITEMID,i.SNCATEGORY from CurInpdetail d,itemmaster i where d.ITEMID = i.ITEMMASTERID And 'Item wise' = '" + type + "' order by 3,2";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllStockStatementReport(string type, string loc, string branch, string asdate)
        {

            try
            {
                string SvSql = "";


                SvSql = @"Select C.Drumno,I.Itemid,C.Lotno, C.Qty,b.Binid 
                            From Curinpbasic Cp,curinpdetail Cd, binbasic B,Itemmaster I,
                            (
                            Select Drumno, Ls.Itemid, ls.Lotno, round(Sum(Plusqty) - sum(Minusqty), 0) Qty
                            From Lstockvalue Ls, locdetails L, branchmast Bm
                            Where Bm.Branchmastid = L.Branchid
                            And L.Locdetailsid = Ls.Locid
                            And(L.Locid = '" + loc + "' Or  'ALL' = '" + loc + "')";
                SvSql += "And Ls.Docdate <= '" + asdate + "' And(Bm.Branchid = '" + branch + "' Or 'ALL' = '" + branch + "')";

                SvSql += @"Group By Drumno,ls.Lotno,Ls.Itemid
                            Having Sum(Plusqty) - sum(Minusqty) > 0
                            ) C
                            Where Cp.Curinpbasicid = Cd.Curinpbasicid
                            And Cd.Batchno = C.Lotno(+)
                            And B.Binbasicid = Cd.Binmasterid
                            And I.Itemmasterid = C.Itemid
                            And(('Shed wise' = '" + type + "') Or('Item wise' = '" + type + "') )";
                SvSql += "Order By B.Binid,I.Itemid,C.Drumno";


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
