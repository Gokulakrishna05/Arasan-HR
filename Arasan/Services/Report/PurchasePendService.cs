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

namespace Arasan.Services
{
    public class PurchasePendService : IPurchasePend
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchasePendService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        //public DataTable GetAllReport(string Branch, string Sdate, string Edate)
        //{
        //    string SvSql = string.Empty;
        //    //SvSql = "select ((QTY+RETQTY)-(nvl(GRNQTY,0)+SHCLQTY+PINDDETAIL.POQTY)) PEND_QTY,to_char(PINDDETAIL.DUEDATE,'dd-MON-yyyy')DUEDATE ,PINDDETAIL.GRNQTY ,PINDBASIC.PINDBASICID,PINDBASIC.DOCID,to_char(PINDBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,ITEMMASTER.ITEMID,PINDDETAIL.QTY  from PINDBASIC INNER JOIN PINDDETAIL ON PINDDETAIL.PINDDETAILID = PINDBASIC.PINDBASICID LEFT OUTER JOIN BRANCHMAST  ON BRANCHMAST.BRANCHMASTID=PINDBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = PINDBASIC.LOCID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID = PINDDETAIL.ITEMID Where PINDBASIC.BRANCHID ='" + Branch + "' AND PINDBASIC.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "'  ";
        //    SvSql = "select ((QTY+RETQTY)-(nvl(GRNQTY,0)+SHCLQTY+PINDDETAIL.POQTY)) PEND_QTY,to_char(PINDDETAIL.DUEDATE,'dd-MON-yyyy')DUEDATE ,PINDDETAIL.GRNQTY ,PINDBASIC.PINDBASICID,PINDBASIC.DOCID,to_char(PINDBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,ITEMMASTER.ITEMID,PINDDETAIL.QTY  from PINDBASIC INNER JOIN PINDDETAIL ON PINDDETAIL.PINDDETAILID = PINDBASIC.PINDBASICID LEFT OUTER JOIN BRANCHMAST  ON BRANCHMAST.BRANCHMASTID=PINDBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID = PINDBASIC.LOCID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID = PINDDETAIL.ITEMID Where  PINDBASIC.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "'  ";

        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}


        public DataTable GetAllReport(string Branch, string Sdate, string Edate)
        {

            try
            {
                string SvSql = "SELECT Br.BranchID,PB.DOCID,PB.DOCDATE,IM.ITEMID,UM.UNITID,QTY ORD_QTY,PD.GRNQTY PUR_QTY,((QTY+RETQTY)-(nvl(GRNQTY,0)+SHCLQTY+PD.POQTY)) PEND_QTY,PD.DUEDATE,L.Locid,PD.Narration  , Pd.App2Dt ,Decode(sign(pd.qty-pd.POQTY),1,'DIRECT PURCHASE','PURCHASE ORDER') TransType,  Pb.EntryDate EntDt,Round((Sysdate-PD.DueDate),0) Pdays FROM PINDBASIC PB,PINDDETAIL PD,ITEMMASTER IM,UNITMAST UM,Locdetails L ,BranchMast Br WHERE PB.PINDBASICID = PD.PINDBASICID AND IM.PRIUNIT = UM.UNITMASTID AND IM.ITEMMASTERID = PD.ITEMID AND PB.BranchID = Br.BranchMastID AND PD.APPROVED2='YES' And L.Locdetailsid(+) = PD.Department AND (PD.POQTY=0 or PD.POQTY is null or (Pd.qty-Pd.POQTY)>0) ";
                if (Sdate != null && Edate != null)
                {
                    SvSql += " and PB.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Branch != null)
                {
                    SvSql += " and Br.BRANCHID='" + Branch + "' ";
                }
                else
                {
                    SvSql += "";
                }

                SvSql += "Union All SELECT Br.BranchID,(B.DOCID||'--'||pb.DOCID) docid,B.DOCDATE,IM.ITEMID,UM.UNITID,Po.QTY ORD_QTY,Po.GRNQTY PUR_QTY,((Po.QTY+Po.REJQTY)-(nvl(Po.GRNQTY,0)+Po.SHCLQTY)) PEND_QTY,PD.DUEDATE,L.Locid,PD.Narration ,  Pd.App2Dt ,Decode((pd.POQTY),0,'DIRECT PURCHASE','PURCHASE ORDER') T,  Pb.EntryDate EntDt,Round((Sysdate-PD.DueDate),0) Pdays FROM PINDBASIC PB,PINDDETAIL PD,ITEMMASTER IM,UNITMAST UM,Locdetails L,PoDetail PO,PoBasic B,BranchMast Br WHERE PB.PINDBASICID = PD.PINDBASICID AND IM.PRIUNIT = UM.UNITMASTID AND IM.ITEMMASTERID = PD.ITEMID AND PO.PINDDETAILID=PD.PINDDETAILID And B.POBASICID=Po.POBASICID ANd B.Active=0 AND PD.APPROVED2='YES' And L.Locdetailsid(+) = PD.Department AND PB.BranchID = Br.BranchMastID AND (PD.POQTY)>0  ";
                if (Sdate != null && Edate != null)
                {
                    SvSql += " and PB.DOCDATE BETWEEN '" + Sdate + "' AND '" + Edate + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Branch != null)
                {
                    SvSql += " and Br.BRANCHID='" + Branch + "' ";
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


    }
}
