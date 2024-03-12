using Arasan.Interface;
using Arasan.Models;
using Dapper;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
namespace Arasan.Services 
{
    public class DrumChangeService : IDrumChange
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DrumChangeService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select I.ITEMID,L.ITEMID as item from LSTOCKVALUE L,ITEMMASTER I WHERE L.ITEMID=I.ITEMMASTERID AND L.LOCID='" + id +"' HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0  GROUP BY  I.ITEMID,L.ITEMID";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDrum(string id,string loc)
        {
            string SvSql = string.Empty;
            SvSql = "Select L.DRUMNO from LSTOCKVALUE L ,LOTMAST LT WHERE LT.INSFLAG='1' AND L.DRUMNO=LT.DRUMNO AND L.ITEMID='"+ id +"' AND L.LOCID='" + loc + "' HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0  GROUP BY  L.DRUMNO";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatch(string id, string item, string loc)
        {
            string SvSql = string.Empty;
            SvSql = "Select L.LOTNO from LSTOCKVALUE L ,LOTMAST LT WHERE LT.INSFLAG='1' AND L.LOTNO=LT.LOTNO AND L.ITEMID='" + item + "' AND L.LOCID='" + loc + "' AND L.DRUMNO='"+ id + "' HAVING SUM(L.PLUSQTY-L.MINUSQTY) > 0  GROUP BY  L.LOTNO";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable Getpackeddrum()
        {
            string SvSql = string.Empty;
            SvSql = "Select  DRUMNO,DRUMMASTID from DRUMMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetreuseItem()
        {
            string SvSql = string.Empty;
            SvSql = "Select  ITEMID,ITEMMASTERID from ITEMMASTER WHERE IGROUP='PACKING MATERIALS'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmployee()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPNAME,EMPMASTID from EMPMAST ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllDrumChangeDeatils(string strStatus, string strfrom, string strTo)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "Select  DOCID, to_char(UNPACKBASIC.DOCDATE,'dd-MM-yyyy')DOCDATE ,UNPACKBASICID,ETYPE,LOCDETAILS.LOCID ,ITEMMASTER.ITEMID  from UNPACKBASIC  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=UNPACKBASIC.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=UNPACKBASIC.LOCATION   WHERE UNPACKBASIC.IS_ACTIVE='Y' ORDER BY  UNPACKBASICID DESC";
            }
            else
            {
                SvSql = "Select  DOCID, to_char(UNPACKBASIC.DOCDATE,'dd-MM-yyyy')DOCDATE ,UNPACKBASICID,ETYPE,LOCDETAILS.LOCID ,ITEMMASTER.ITEMID  from UNPACKBASIC  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=UNPACKBASIC.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=UNPACKBASIC.LOCATION   WHERE UNPACKBASIC.IS_ACTIVE='N' ORDER BY  UNPACKBASICID DESC";

            }
            if (strfrom == null && strTo == null)
            {
                SvSql = "Select  DOCID, to_char(UNPACKBASIC.DOCDATE,'dd-MM-yyyy')DOCDATE ,UNPACKBASICID,ETYPE,LOCDETAILS.LOCID ,ITEMMASTER.ITEMID  from UNPACKBASIC  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=UNPACKBASIC.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=UNPACKBASIC.LOCATION   WHERE UNPACKBASIC.IS_ACTIVE='Y' ORDER BY  UNPACKBASICID DESC";
            }
            else
            {
                SvSql = "Select  DOCID, to_char(UNPACKBASIC.DOCDATE,'dd-MM-yyyy')DOCDATE ,UNPACKBASICID,ETYPE,LOCDETAILS.LOCID ,ITEMMASTER.ITEMID  from UNPACKBASIC  LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=UNPACKBASIC.OITEMID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=UNPACKBASIC.LOCATION  ";
                if (strfrom != null && strTo != null)
                {
                    SvSql += " WHERE UNPACKBASIC.DOCDATE BETWEEN '" + strfrom + "' AND '" + strTo + "'";
                }
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
