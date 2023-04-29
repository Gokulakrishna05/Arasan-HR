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
        public DataTable IsQCNotify()
        {
            string SvSql = string.Empty;
            SvSql = "select to_char(GATE_IN_DATE,'dd-MON-yyyy') GATE_IN_DATE,GATE_IN_TIME,TOTAL_QTY,PARTYRCODE.PARTY,ITEMMASTER.ITEMID,UNITMAST.UNITID from GATE_INWARD_DETAILS LEFT OUTER JOIN GATE_INWARD on GATE_INWARD.GATE_IN_ID=GATE_INWARD_DETAILS.GATE_IN_ID LEFT OUTER JOIN  PARTYMAST on GATE_INWARD.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=GATE_INWARD_DETAILS.ITEM_ID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT WHERE GATE_INWARD.STATUS='Waiting' AND GATE_INWARD_DETAILS.QCFLAG='YES' AND PARTYMAST.TYPE IN ('Supplier','BOTH')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetQCNotify()
        {
            string SvSql = string.Empty;
            SvSql = "select to_char(CREATED_ON,'dd-MON-yyyy')CREATED_ON,DOCID,TYPE,DRUMMAST.DRUMNO,ITEMMASTER.ITEMID,QCNOTIFICATIONID from QCNOTIFICATION  LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=QCNOTIFICATION.ITEMID LEFT OUTER JOIN DRUMMAST ON DRUMMAST.DRUMMASTID=QCNOTIFICATION.DRUMNO WHERE QCNOTIFICATION.IS_COMPLETED='NO'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
