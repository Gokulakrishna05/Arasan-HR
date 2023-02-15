using Arasan.Interface.Master;
using Arasan.Interface.Sales;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;

namespace Arasan.Services.Sales
{
    public class SalesQuotationService : ISalesQuotationService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public SalesQuotationService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        public IEnumerable<SalesQuotation> GetAllSalesQuotation()
        {
            List<SalesQuotation> cmpList = new List<SalesQuotation>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {

                using (OracleCommand cmd = con.CreateCommand())
                {
                    con.Open();
                    cmd.CommandText = "Select  BRANCHMAST.BRANCHID, DOCID,to_char(DOCDATE,'dd-MON-yyyy') DOCDATE ,PARTYRCODE.PARTY,PURENQ.ENQNO,PURENQ.ENQDATE,MAINCURRENCY,EXRATE,PURQUOTBASICID,PURQUOTBASIC.STATUS from PURQUOTBASIC LEFT OUTER JOIN PURENQ on PURQUOTBASIC.ENQID=PURENQ.PURENQID LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PURQUOTBASIC.BRANCHID LEFT OUTER JOIN  PARTYMAST on PURQUOTBASIC.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN PARTYRCODE ON PARTYMAST.PARTYID=PARTYRCODE.ID Where PARTYMAST.TYPE IN ('Supplier','BOTH') ORDER BY PURQUOTBASICID DESC";
                    OracleDataReader rdr = cmd.ExecuteReader();
                    while (rdr.Read())
                    {
                        SalesQuotation cmp = new SalesQuotation
                        {
                            ID = rdr["PURQUOTBASICID"].ToString(),
                            Currency = rdr["MAINCURRENCY"].ToString(),




                        };
                        cmpList.Add(cmp);
                    }
                }
            }
            return cmpList;
        }
       
        public DataTable GetItemCF(string ItemId, string unitid)
        {
            string SvSql = string.Empty;
            SvSql = "Select CF from itemmasterpunit where ITEMMASTERID='" + ItemId + "' AND UNIT='" + unitid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST order by BRANCHMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getcountry()
        {
            string SvSql = string.Empty;
            SvSql = "select COUNTRYNAME,COUNTRYMASTID from CONMAST order by COUNTRYMASTID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string SalesQuotationCRUD(SalesQuotation cy)
        {
            throw new NotImplementedException();
        }

        public DataTable GetSalesQuotation(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PURQUOTDETAIL.QTY,PURQUOTDETAIL.PURQUOTDETAILID,PURQUOTDETAIL.ITEMID,UNITMAST.UNITID,PURQUOTDETAIL.RATE  from PURQUOTDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PURQUOTDETAIL.UNIT  where PURQUOTDETAIL.PURQUOTBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetSalesQuotationItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select PURQUOTDETAIL.QTY,PURQUOTDETAIL.PURQUOTDETAILID,PURQUOTDETAIL.ITEMID,UNITMAST.UNITID,PURQUOTDETAIL.RATE  from PURQUOTDETAIL LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=PURQUOTDETAIL.UNIT  where PURQUOTDETAIL.PURQUOTBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
