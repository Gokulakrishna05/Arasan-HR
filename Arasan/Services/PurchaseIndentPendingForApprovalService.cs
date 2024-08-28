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
    public class PurchaseIndentPendingForApprovalService : IPurchaseIndentPendingForApproval
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchaseIndentPendingForApprovalService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllPurchaseIndentPendingForApproval(string dtFrom)
        {
            try
            {
                string SvSql = "";

                SvSql = "SELECT PB.DOCID,PB.DOCDATE,IM.ITEMID,UM.UNITID,QTY ORD_QTY,POQTY PUR_QTY,((QTY+RETQTY)-(POQTY+SHCLQTY)) \r\nPEND_QTY,PD.DUEDATE,L.Locid,PD.Narration , Pd.App1Dt , Pd.App2Dt , Pb.EntryDate EntDt\r\nFROM PINDBASIC PB,PINDDETAIL PD,ITEMMASTER IM,UNITMAST UM,Locdetails L\r\nWHERE PB.PINDBASICID = PD.PINDBASICID\r\nAND IM.PRIUNIT = UM.UNITMASTID\r\nAND IM.ITEMMASTERID = PD.ITEMID\r\nAND (PD.APPROVED1 is null or (PD.APPROVED2  is null and Pd.Approved1='YES')  )\r\nAnd L.Locdetailsid(+) = PD.Department\r\nand pb.docdate <='"+dtFrom+"'\r\nAnd ((QTY+RETQTY)-(POQTY+SHCLQTY)) >0\r\nORDER BY  PB.DOCDATE , PB.DOCID,IM.ITEMID";
                
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
