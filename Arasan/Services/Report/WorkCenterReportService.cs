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
    public class WorkCenterReportService : IWorkCenterReportService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public WorkCenterReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetWorkCenter()
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProcess()
        {
            string SvSql = string.Empty;
            SvSql = "Select PROCESSID ,PROCESSMASTID from PROCESSMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllWorkCenterReport(string dtFrom, string dtTo, string WorkCenter, string Process)
        {
            try
            {
                string SvSql = "";
                if (dtFrom == null && WorkCenter == null && Process == null)
                {
                    SvSql = "SELECT B.DOCID,to_char(B.DOCDATE,'dd-MON-yyyy')DOCDATE,W.WCID,P.PROCESSID FROM BCPRODBASIC B , WCBASIC W , PROCESSMAST P WHERE B.WCID = W.WCBASICID AND B.WPROCESSID = P.PROCESSMASTID";

                }
                else
                {
                    SvSql = "SELECT B.DOCID,to_char(B.DOCDATE,'dd-MON-yyyy')DOCDATE,W.WCID,P.PROCESSID FROM BCPRODBASIC B , WCBASIC W , PROCESSMAST P WHERE B.WCID = W.WCBASICID AND B.WPROCESSID = P.PROCESSMASTID";
                    if (dtFrom != null && dtTo != null)
                    {
                        SvSql += " and B.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                    }


                    if (WorkCenter != null)
                    {
                        SvSql += " and W.WCID='" + WorkCenter + "'";
                    }


                    if (Process != null)
                    {
                        SvSql += " and P.PROCESSID='" + Process + "'";
                    }
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
    }
}
