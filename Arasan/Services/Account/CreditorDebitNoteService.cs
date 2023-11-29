using Arasan.Interface;
using Arasan.Interface.Account;
using Arasan.Interface.Master;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services
{ 
    public class CreditorDebitNoteService : ICreditorDebitNote
    { 
        private readonly string _connectionString;
        DataTransactions datatrans;


        //int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'ICNF' AND ACTIVESEQUENCE = 'T'  ");
        //string DocId = string.Format("{0}{1}", "OSA-", (idc + 1).ToString());

        public CreditorDebitNoteService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetGroup()
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUPID,ACCOUNTGROUP from ACCGROUP ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }  
        
        public DataTable GetGrp()
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUPID,ACCOUNTGROUP from ACCGROUP ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } 
        public DataTable GetLedger()
        {
            string SvSql = string.Empty;
            SvSql = "select LEDGERID,LEDNAME from ACCLEDGER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }  
        
        public DataTable GetLed()
        {
            string SvSql = string.Empty;
            SvSql = "select LEDGERID,LEDNAME from ACCLEDGER";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetGrpDetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUP.ACCOUNTGROUP,ACCLEDGER.LEDNAME from ACCLEDGER left outer join ACCGROUP on ACCGROUPID=ACCLEDGER.ACCGROUP WHERE ACCLEDGER.ACCGROUP = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLedbyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select LEDNAME,LEDGERID from ACCLEDGER where ACCGROUP = '" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        } public DataTable GetGRPbyId(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUP.ACCGROUPID,ACCGROUP.ACCOUNTGROUP,ACCLEDGER.ACCGROUP FROM ACCGROUP LEFT OUTER JOIN ACCLEDGER  ON ACCLEDGER.ACCGROUP = ACCGROUP.ACCGROUPID WHERE ACCGROUP.ACCGROUPID ='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSeq(string id)
        {
            string SvSql = string.Empty;
            if (id == "CreditNote")
            {
                SvSql = "select PREFIX,LASTNO FROM SEQUENCE WHERE PREFIX ='CNBF'";
            }
            else
            {
                SvSql = "select PREFIX,LASTNO FROM SEQUENCE WHERE PREFIX ='DNBF'";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

    }
}
