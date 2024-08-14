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

        public DataTable GetAllReport(string dtFrom, string dtTo, string Branch, string Customer, string Item)
        {
            try
            {

                string SvSql = "SELECT A.DOCID,to_char(A.DOCDATE,'dd-MON-yyyy')DOCDATE,D.DOCID AS doc,C.BRANCHID,L.LOCID,P.PARTYID,I.ITEMID,U.UNITID,B.QTY,B.RATE,B.AMOUNT FROM GRNBLBASIC A,GRNBLDETAIL B,BRANCHMAST C,LOCDETAILS L,PARTYMAST P,POBASIC D,ITEMMASTER I,UNITMAST U ,ITEMGROUP P,ITEMSUBGROUP B WHERE A.CANCEL <> 'T' AND A.GRNBLBASICID=B.GRNBLBASICID AND C.BRANCHMASTID=A.BRANCHID AND L.LOCDETAILSID =A.LOCID AND P.PARTYMASTID=A.PARTYID AND I.ITEMMASTERID=B.ITEMID AND P.ITEMGROUPID=B.ITEMGROUPID AND I.SUBGROUPCODE=B.ITEMSUBGROUPID AND U.UNITMASTID=B.UNIT";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and A.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                }
                
               
                if (Branch != null)
                {
                    SvSql += " and C.BRANCHID='" + Branch + "'";
                }
                
               
                if (Customer != null)
                {
                    SvSql += " and P.PARTYID='" + Customer + "'";
                }
               
                if (Item != null)
                {
                    SvSql += " and I.ITEMID='" + Item + "'";
                }
                
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
        //public DataTable GetBranch()
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "Select BRANCHMASTID,BRANCHID from BRANCHMAST where STATUS = 'ACTIVE' and BRANCHID <> 'ALL' ORDER BY BRANCHID ASC";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}
        //public long GetMregion(string regionid, string id)
        //{
        //    string SvSql = "SELECT ITEMID,SUBGROUPCODE from ITEMMASTER where SUBGROUPCODE=" + regionid + " and ITEMMASTERID=" + id + "";
        //    DataTable dtCity = new DataTable();
        //    long user_id = datatrans.GetDataIdlong(SvSql);
        //    return user_id;
        //}

    }
}
