using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;
namespace Arasan.Services
{
    public class HomeService : IHomeService
    {
        private readonly string _connectionString;
        DataTransactions _dtransactions;
        public HomeService(IConfiguration _configuratio)
        {
        _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable CuringGroup()
        {
            string SvSql = string.Empty;
            SvSql = "select SUBGROUP from CURINGMASTER GROUP BY SUBGROUP  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable curingsubgroup(string curingset)
        {
            string SvSql = string.Empty;
            SvSql = "select SHEDNUMBER,CAPACITY,STATUS,OCCUPIED from CURINGMASTER where SUBGROUP='" + curingset + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

       
        public DataTable GetquoteFollowupnextReport()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT QUO_ID, FOLLOWED_BY,FOLLOW_DATE ,SYSDATE,SYSDATE + 2,TO_CHAR(NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE  from quotation_follow_up  where NEXT_FOLLOW_DATE  between SYSDATE  and  SYSDATE + 2  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable GetEnqFollowupnextReport()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ENQ_ID,FOLLOWED_BY,FOLLOW_DATE ,SYSDATE,SYSDATE + 2,TO_CHAR(NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE  from ENQUIRY_FOLLOW_UP  where NEXT_FOLLOW_DATE  between SYSDATE  and  SYSDATE + 2  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesQuoteFollowupnextReport()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SALESQUOFOLLOWID,QUOTE_NO,FOLLOW_BY,FOLLOW_DATE,FOLLOW_STATUS ,SYSDATE,SYSDATE + 2,TO_CHAR(NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE  from SALESQUOFOLLOWUP  where NEXT_FOLLOW_DATE  between SYSDATE  and  SYSDATE + 2  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCurInward()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT CURINPBASICID,CURINPDETAILID,DRUMNO,ITEMMASTER.ITEMID ,SYSDATE,SYSDATE + 2,TO_CHAR(DUEDATE,'dd-MON-yyyy')DUEDATE  from CURINPDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=CURINPDETAIL.ITEMID  where DUEDATE  between SYSDATE  and  SYSDATE + 2  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCurInwardDoc(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT CURINPBASICID,DOCID  from CURINPBASIC  where CURINPBASICID='"+id+"'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
