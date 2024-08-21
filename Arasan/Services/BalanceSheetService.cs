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
    public class BalanceSheetService : IBalanceSheet
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public BalanceSheetService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllBalanceSheet(string dtFrom, string Branch)
        {
            try
            {
                string SvSql = "";
                SvSql = " SELECT GROUPNAME,DECODE(SIGN(SUM(DEBIT)-SUM(CREDIT)),1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) DB ,DECODE(SIGN(SUM(DEBIT)-SUM(CREDIT)),-1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) CR,MALIE,MID FROM(SELECT M.MNAME GROUPNAME, M1.MNAME, DECODE(SIGN(SUM(DEBIT) - SUM(CREDIT)), 1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) DEBIT , DECODE(SIGN(SUM(DEBIT) - SUM(CREDIT)), -1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) CREDIT,m1.MASTERID,M1.mstatus,M.MALIE,M.MASTERID MID FROM DAILYTRANS D, MASTER M, MASTER M1, BRANCHMAST B, companymast c WHERE D.MID = M1.MASTERID AND b.companyID = c.companymastid AND c.companyid = 'TAAI' AND M.MASTERID = M1.MPARENT AND B.BRANCHMASTID = D.BRANCHID AND M1.MPARENT1 IN(1,51)AND(B.BRANCHID = '" + Branch + "' OR '$branch' = 'ALL')AND D.T2VCHDT <= TO_DATE('" + dtFrom + "', 'dd/mm/yyyy')GROUP BY M.MNAME ,M1.MNAME, M.MALIE, M.MTREEID,m1.masterid,M1.mstatus,M.MALIE,M.MASTERID HAVING(SUM(DEBIT)-SUM(CREDIT)) <> 0)GROUP BY GROUPNAME,MALIE,MID ORDER BY DECODE(MALIE, 'l',1,'a',2,'i',3,'e', 4),GROUPNAME";

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
        public DataTable GetAllBalanceSheet1(string dtFrom, string Branchs)
        {
            try
            {
                string SvSql = "";
                SvSql = " SELECT GROUPNAME,DECODE(SIGN(SUM(DEBIT)-SUM(CREDIT)),1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) DB ,DECODE(SIGN(SUM(DEBIT)-SUM(CREDIT)),-1,ABS(SUM(DEBIT)-SUM(CREDIT)),0) CR,MALIE,MID FROM( SELECT M.MNAME GROUPNAME, M1.MNAME, DECODE(SIGN(SUM(DEBIT) - SUM(CREDIT)), 1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) DEBIT , DECODE(SIGN(SUM(DEBIT) - SUM(CREDIT)), -1, ABS(SUM(DEBIT) - SUM(CREDIT)), 0) CREDIT,m1.MASTERID,M1.mstatus,M.MALIE,M.MASTERID MID FROM DAILYTRANS D, MASTER M, MASTER M1, BRANCHMAST B, companymast c WHERE D.MID = M1.MASTERID AND b.companyID = c.companymastid AND c.companyid = 'TAAI' AND M.MASTERID = M1.MPARENT AND B.BRANCHMASTID = D.BRANCHID AND M1.MPARENT1 IN(101,151)AND(B.BRANCHID = '" + Branchs + "' OR '" + Branchs + "' = 'ALL')AND D.T2VCHDT <= TO_DATE('" + dtFrom + "', 'dd/mm/yyyy')GROUP BY M.MNAME ,M1.MNAME, M.MALIE, M.MTREEID,m1.masterid,M1.mstatus,M.MALIE,M.MASTERID HAVING(SUM(DEBIT)-SUM(CREDIT)) <> 0)GROUP BY GROUPNAME,MALIE,MID ORDER BY DECODE(MALIE, 'l',1,'a',2,'i',3,'e', 4),GROUPNAME";

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
