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
    public class TrialBalanceReportService : ITrialBalanceReport
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public TrialBalanceReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllTrialBalanceReport(string dtFrom, string Branch,string Master)
        {
            try
            {
                string SvSql = "";
                SvSql = " SELECT M.MNAME GROUPNAME, M1.MNAME, Decode(SIGN(SUM(DEBIT)-SUM(CREDIT)),1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) DEBIT , Decode(SIGN(SUM(DEBIT)-SUM(CREDIT)),-1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) CREDIT,m1.MASTERID,M1.mstatus,M.MALIE FROM DAILYTRANS D, MASTER M, MASTER M1, BRANCHMAST B , companymast c WHERE D.MID = M1.MASTERID AND M.MASTERID='"+Master+"' And b.companyID = c.companymastid And c.companyid ='TAAI' AND M.MASTERID = M1.MPARENT AND B.BRANCHMASTID = D.BRANCHID AND (B.BRANCHID = '"+Branch+"' OR '"+Branch+"' = 'ALL' ) AND D.T2VCHDT <= to_date('"+dtFrom+"','dd/mm/yyyy') GROUP BY M.MNAME ,M1.MNAME, M.MALIE, M.MTREEID,m1.masterid,M1.mstatus,M.MALIE HAVING (SUM(DEBIT)-SUM(CREDIT)) <> 0 ORDER BY DECODE(M.MALIE, 'l',1,'a',2,'i',3,'e', 4),  M.MTREEID ,M1.MNAME";
 
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
         
        public DataTable GetMaster(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select MASTERID,MNAME FROM MASTER ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
