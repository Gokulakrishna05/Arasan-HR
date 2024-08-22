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
using Microsoft.CodeAnalysis.Differencing;

namespace Arasan.Services
{
    public class OpeningSqlService : IOpeningSql
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public OpeningSqlService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllOpeningSql(string dtFrom, string Branch)
        {
            try
            {
                string SvSql = "";
                //SvSql = " select Decode(SIGN(SUM(DEBIT)-SUM(CREDIT)),1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) DB ,Decode(SIGN(SUM(DEBIT)-SUM(CREDIT)),-1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) CR from( SELECT M.MNAME GROUPNAME, M1.MNAME, Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), 1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) DEBIT , Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), -1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) CREDIT,m1.MASTERID,M1.mstatus,M.MALIE,M.MASTERID MID FROM DAILYTRANS D, MASTER M, MASTER M1, BRANCHMAST B, companymast c WHERE D.MID = M1.MASTERID And b.companyID = c.companymastid And c.companyid = 'TAAI' AND M.MASTERID = M1.MPARENT AND B.BRANCHMASTID = D.BRANCHIDAND(B.BRANCHID = '"+ Branch + "' OR '"+ Branch + "' = 'ALL')AND D.T2VCHDT < to_date('"+ dtFrom + "', 'dd/mm/yyyy')GROUP BY M.MNAME ,M1.MNAME, M.MALIE, M.MTREEID,m1.masterid,M1.mstatus,M.MASTERID)";
                SvSql = "SELECT Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), 1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) DB, Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), -1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) CR FROM ( SELECT  M.MNAME GROUPNAME, M1.MNAME,  Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), 1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) DEBIT,  Decode(SIGN(SUM(DEBIT) - SUM(CREDIT)), -1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) CREDIT,  M1.MASTERID,  M1.mstatus,  M.MALIE,  M.MASTERID MID  FROM DAILYTRANS D,  MASTER M,  MASTER M1,   BRANCHMAST B,   companymast c   WHERE    D.MID = M1.MASTERID  AND B.companyID = c.companymastid   AND c.companyid = 'TAAI'   AND M.MASTERID = M1.MPARENT    AND B.BRANCHMASTID = D.BRANCHID  AND D.T2VCHDT < TO_DATE('01/01/2024', 'dd/mm/yyyy')  GROUP BY  M.MNAME,   M1.MNAME,   M.MALIE,   M.MTREEID,   M1.MASTERID,   M1.mstatus,  M.MASTERID ) ";

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
