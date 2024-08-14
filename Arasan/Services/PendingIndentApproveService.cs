//using Arasan.Interface.Report;
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
using System.Diagnostics;

namespace Arasan.Services
{
    public class PendingIndentApproveService : IPendingIndentApprove
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PendingIndentApproveService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllPendingIndentApprove(string dtFrom)
        {


            try
            {
                string SvSql = "";
                  SvSql = "SELECT PB.DOCID,PB.DOCDATE,IM.ITEMID,UM.UNITID,QTY ORD_QTY,POQTY PUR_QTY,((QTY+RETQTY)-(POQTY+SHCLQTY))  PEND_QTY,PD.DUEDATE,L.Locid,PD.Narration , Pd.App1Dt , Pd.App2Dt , Pb.EntryDate EntDt FROM PINDBASIC PB,PINDDETAIL PD,ITEMMASTER IM,UNITMAST UM,Locdetails L WHERE PB.PINDBASICID = PD.PINDBASICID AND IM.PRIUNIT = UM.UNITMASTID AND IM.ITEMMASTERID = PD.ITEMID  And L.Locdetailsid(+) = PD.Department and pb.docdate <='"+dtFrom+"' And ((QTY+RETQTY)-(POQTY+SHCLQTY)) >0 ORDER BY  PB.DOCDATE , PB.DOCID,IM.ITEMID";
               
                //else
                //{
                //    SvSql = "SELECT PB.DOCID,PB.DOCDATE,IM.ITEMID,UM.UNITID,QTY ORD_QTY,POQTY PUR_QTY,((QTY+RETQTY)-(POQTY+SHCLQTY))  PEND_QTY,PD.DUEDATE,L.Locid,PD.Narration , Pd.App1Dt , Pd.App2Dt , Pb.EntryDate EntDt FROM PINDBASIC PB,PINDDETAIL PD,ITEMMASTER IM,UNITMAST UM,Locdetails L WHERE PB.PINDBASICID = PD.PINDBASICID AND IM.PRIUNIT = UM.UNITMASTID AND IM.ITEMMASTERID = PD.ITEMID  And L.Locdetailsid(+) = PD.Department and pb.docdate <=:dtFrom And ((QTY+RETQTY)-(POQTY+SHCLQTY)) >0 ORDER BY  PB.DOCDATE , PB.DOCID,IM.ITEMID";


                //}
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
