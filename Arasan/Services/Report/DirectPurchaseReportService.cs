using Arasan.Interface.Report;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;

using Microsoft.Reporting.Map.WebForms.BingMaps;

namespace Arasan.Services.Report
{
    public class DirectPurchaseReportService : IDirectPurchaseReportService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public DirectPurchaseReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetAllDPReport(string dtFrom, string dtTo, string Branch, string Customer, string Item)
        {
            try
            {
                string SvSql = "SELECT A.DOCID,to_char(A.DOCDATE,'dd-MON-yyyy')DOCDATE,C.BRANCHID,L.LOCID,P.PARTYID,I.ITEMID,U.UNITID,B.QTY,B.PRIQTY,B.RATE,B.AMOUNT,B.BRATE,B.BAMOUNT,IFREIGHTCH FREIGHT,IPKNFDCH PKNFD,IDELCH DELCH,ICSTCH ,ILRCH LORRY,-ISPLDISCF IsplD,IOTHERCH OTHER,B.COSTRATE , A.RefNo , A.RefDt,To_char(A.Docdate,'MON') Mon FROM DPBASIC A,DPDETAIL B,BRANCHMAST C,LOCDETAILS L,PARTYMAST P,ITEMMASTER I,UNITMAST U,ITEMGROUP P,ITEMSUBGROUP B WHERE A.CANCEL <> 'T'AND A.DPBASICID=B.DPBASICID AND C.BRANCHMASTID=A.BRANCHID AND  L.LOCDETAILSID =A.LOCID AND P.PARTYMASTID=A.PARTYID AND I.ITEMMASTERID=B.ITEMID  AND P.ITEMGROUPID=B.ITEMGROUPID AND I.SUBGROUPCODE=B.ITEMSUBGROUPID AND U.UNITMASTID=B.UNIT";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and A.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Branch != null)
                {
                    SvSql += " and C.BRANCHID='" + Branch + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Customer != null)
                {
                    SvSql += " and P.PARTYID='" + Customer + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Item != null)
                {
                    SvSql += " and I.ITEMID='" + Item + "'";
                }
                else
                {
                    SvSql += "";
                }
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
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER Where SUBGROUPCODE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
