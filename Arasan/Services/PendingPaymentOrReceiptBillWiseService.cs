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
    public class PendingPaymentOrReceiptBillWiseService : IPendingPaymentOrReceiptBillWise
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PendingPaymentOrReceiptBillWiseService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllPendingPaymentOrReceiptBillWise(string dtFrom, string Branch)
        {
            try
            {
                string SvSql = "";
                SvSql = "SELECT a.grouporder, Decode(A.GROUPORDER ,1,Decode(sign(A.AMOUNT),1,'Bills','Rec/Adv/Unadj') , 'Rec/Adv/Unadj')  AS  slno, A.DOCID, A.DOCDATE, A.GROUPNO, DECODE(C.CURRENCYID , 1, M.MNAME, M.MNAME||'       [ in '||c.maincurr||' ]') AS  MNAME, A.MATCHDATE, A.DUEDATE,  A.AMOUNT AS AMOUNT,  DECODE ( a.grouporder , 1, ROUND(R.AMT,2), 0)  AS PENDING,A.UserID FROM RPDETAILS A , ( select partyname, groupno, sum(amount) as amt from rpdetails where docdate <= '"+dtFrom+"' group by partyname, groupno having sum(amount) <> 0 ) r, master m, \r\n currency c,BRANCHMAST B1,COMPANYMAST CM\r\nwhere a.docdate <=  '"+dtFrom+"' and m.masterid = a.partyname and m.masterid = r.partyname and a.groupno = r.groupno and c.currencyid = a.maincurr    \r\nAND B1.BRANCHMASTID=A.BRANCHID  \r\nAND B1.COMPANYID=CM.COMPANYMASTID\r\nAND (B1.BRANCHID='"+Branch+"' OR    'ALL BRANCH' ='"+Branch+"')\r\norder by 6, A.MATCHDATE , 5, a.grouporder";
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
