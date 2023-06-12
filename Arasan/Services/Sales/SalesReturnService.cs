using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Sales
{
    public class SalesReturnService : ISalesReturn
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalesReturnService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetInvoice()
        {
            string SvSql = string.Empty;
            SvSql = "select DOCID,PINVBASICID from PINVBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetInvoiceDetails(string invoiceid)
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYNAME,to_char(PINVBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,GROSS,NET from PINVBASIC WHERE PINVBASICId='" + invoiceid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetInvoiceItem(string invoiceid)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT PINVDETAIL.PINVDETAILID,PINVDETAIL.ITEMID as itemi,UNITMAST.UNITID,QTY,RATE,AMOUNT,ITEMMASTER.ITEMID,DISCOUNT,DISCPER,FREIGHT,CGSTP,CGST,SGSTP,SGST,IGSTP,IGST,TOTAMT,PINVDETAIL.UNIT,PINVDETAIL.EXCISETYPE,PINVDETAIL.TARIFFID FROM PINVDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=PINVDETAIL.ITEMID  LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PINVDETAIL.UNIT WHERE PINVBASICId ='" + invoiceid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
