using Arasan.Interface.Report;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;

using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace Arasan.Services.Report
{
    public class GRNReportService : IGRNReportService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public GRNReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllReport(string Branch, string Customer,string Item,string dtFrom, string dtTo)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT  GRNBLDETAIL.GRNBLDETAILID,GRNBLBASIC.GRNBLBASICID,BRANCHMAST.BRANCHID,ITEMMASTER.ITEMID,GRNBLBASIC.DOCID,to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,CURRENCY.MAINCURR,PARTYMAST.PARTYID,POBASIC.DOCID AS doc  FROM GRNBLBASIC INNER JOIN  GRNBLDETAIL ON GRNBLBASIC.GRNBLBASICID=GRNBLDETAIL.GRNBLDETAILID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=GRNBLDETAIL.ITEMID LEFT OUTER JOIN POBASIC ON POBASIC.POBASICID=GRNBLBASIC.POBASICID LEFT OUTER JOIN BRANCHMAST  ON BRANCHMAST.BRANCHMASTID=GRNBLBASIC.BRANCHID LEFT OUTER JOIN CURRENCY ON CURRENCY.CURRENCYID=GRNBLBASIC.MAINCURRENCY LEFT OUTER JOIN PARTYMAST ON PARTYMAST.PARTYMASTID=GRNBLBASIC.PARTYID Where GRNBLBASIC.BRANCHID='" + Branch + "' AND GRNBLBASIC.PARTYID ='" + Customer + "' AND GRNBLDETAIL.ITEMID='" + Item + "' AND GRNBLBASIC.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            //using (DataTable dt = new DataTable())
            //{
            //    adapter.Fill(dt);

            //    DataView dv1 = dt.DefaultView;
            //    // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

            //    dtt = dv1.ToTable();
            //    using (GRNReport wb = new GRNReport())
            //    {
            //        wb.Worksheets.Add(dtt, "GRNReport");


            //        using (MemoryStream MyMemoryStream = new MemoryStream())
            //        {
            //            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //            Response.AddHeader("content-disposition", "attachment;  filename=CallsTenderReport.xlsx");
            //            wb.SaveAs(MyMemoryStream);
            //            MyMemoryStream.WriteTo(Response.OutputStream);
            //            Response.Flush();
            //            Response.End();
            //            //wb.SaveAs(MyMemoryStream);
            //            return File(MyMemoryStream.ToArray(), "application/ms-excel", "GRNReport.xlsx");
            //        }
            //    }
            //}
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;     
        }

        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER Where SUBGROUPCODE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString); 
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMASTID,BRANCHID from BRANCHMAST where STATUS = 'ACTIVE' and BRANCHID <> 'ALL' ORDER BY BRANCHID ASC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public long GetMregion(string regionid, string id)
        //{
        //    string SvSql = "SELECT ITEMID,SUBGROUPCODE from ITEMMASTER where SUBGROUPCODE=" + regionid + " and ITEMMASTERID=" + id + "";
        //    DataTable dtCity = new DataTable();
        //    long user_id = datatrans.GetDataIdlong(SvSql);
        //    return user_id;
        //}

    }
}
