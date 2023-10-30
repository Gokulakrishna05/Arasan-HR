using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using System.Xml.Linq;

namespace Arasan.Controllers
{
    public class QcDashboardController : Controller
    {
        IQcDashboardService QcDashboardService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public QcDashboardController(IQcDashboardService _QcDashboardService, IConfiguration _configuratio)
        {
            QcDashboardService = _QcDashboardService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult QcDashboard()
        {
            QcDashboard H = new QcDashboard();
            //ViewBag.Name = Request.Cookies["UserName"];
            List<QcNotify> TData = new List<QcNotify>();
            QcNotify tda = new QcNotify();
            List<Notify> TData1 = new List<Notify>();
            Notify tda1 = new Notify();
            List<APOut> TDatao1 = new List<APOut>();
            APOut tdao1 = new APOut();
            DataTable dt2 = new DataTable();
            dt2 = QcDashboardService.IsQCNotify();
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new QcNotify();
                    tda.GateDate = dt2.Rows[i]["GATE_IN_DATE"].ToString() + " & " + dt2.Rows[i]["GATE_IN_TIME"].ToString();
                    tda.PartyName = dt2.Rows[i]["PARTYNAME"].ToString();
                    tda.ItemName = dt2.Rows[i]["ITEMID"].ToString();
                    tda.TotalQty = dt2.Rows[i]["TOTAL_QTY"].ToString();
                    tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                    tda.id = dt2.Rows[i]["POBASICID"].ToString();
                    TData.Add(tda);
                }
            }
            DataTable dt3 = new DataTable();
            dt3 = QcDashboardService.GetQCNotify();
            if (dt3.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new Notify();
                    tda1.ID = dt3.Rows[i]["QCNOTIFICATIONID"].ToString();

                    tda1.Doc = dt3.Rows[i]["DOCID"].ToString();
                    tda1.Date = dt3.Rows[i]["CREATED_ON"].ToString();
                    tda1.Drum = dt3.Rows[i]["DRUMNO"].ToString();
                    tda1.Item = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.Type = dt3.Rows[i]["TYPE"].ToString();
                    TData1.Add(tda1);
                }
            }
            DataTable dt4 = new DataTable();
            dt4 = QcDashboardService.GetMaterialnot();

            List<MatNotify> TDatan = new List<MatNotify>();
            MatNotify tdan = new MatNotify();

            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    tdan = new MatNotify();
                    tdan.Date = dt4.Rows[i]["DOCDATE"].ToString();
                    tdan.LocationName = dt4.Rows[i]["LOCID"].ToString();
                    tdan.ItemName = dt4.Rows[i]["ITEMID"].ToString();
                    tdan.TotalQty = dt4.Rows[i]["QTY"].ToString();
                    tdan.Unit = dt4.Rows[i]["UNITID"].ToString();
                    tdan.stockQty = dt4.Rows[i]["STOCK"].ToString();
                    TDatan.Add(tdan);
                }
            }

            DataTable Outdt = new DataTable();
            Outdt = QcDashboardService.GetAPout();
            if (Outdt.Rows.Count > 0)
            {
                for (int i = 0; i < Outdt.Rows.Count; i++)
                {
                    tdao1 = new APOut();
                    tdao1.id = Outdt.Rows[i]["APPRODUCTIONBASICID"].ToString();
                    tdao1.ItemName = Outdt.Rows[i]["ITEMID"].ToString();
                    tdao1.Drum = Outdt.Rows[i]["DRUMNO"].ToString();
                    tdao1.Time = Outdt.Rows[i]["FROMTIME"].ToString();
                    tdao1.TotalQty = Outdt.Rows[i]["OUTQTY"].ToString();
                    DataTable Outdt1 = new DataTable();
                    Outdt1 = QcDashboardService.GetAPout1(tdao1.id);
                    if (Outdt1.Rows.Count > 0)
                    {
                        tdao1.ApId = Outdt1.Rows[0]["Ap"].ToString();
                    }
                    DataTable DIS = new DataTable();
                    DIS = QcDashboardService.GetDis(tdao1.id);
                    if (DIS.Rows.Count > 0)
                    {
                        for (int j = 0; j < DIS.Rows.Count; j++)
                        {

                            tdao1.dis = DIS.Rows[j]["APPROID"].ToString();

                        }
                    }
                    DataTable FIN = new DataTable();
                    FIN = QcDashboardService.GetFinal(tdao1.id);
                    if (FIN.Rows.Count > 0)
                    {
                        for (int k = 0; k < FIN.Rows.Count; k++)
                        {

                            tdao1.Fin = DIS.Rows[k]["APPROID"].ToString();

                        }
                    }
                    TDatao1.Add(tdao1);
                }
               
            }
            List<APOutItem> TDatak = new List<APOutItem>();
            APOutItem tda2 = new APOutItem();
            DataTable dt = new DataTable();
            dt = QcDashboardService.GetAPoutItem();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tda2 = new APOutItem();
                    tda2.id = dt.Rows[i]["APPRODUCTIONBASICID"].ToString();
                    tda2.ItemName = dt.Rows[i]["ITEMID"].ToString();
                    tda2.Drum = dt.Rows[i]["DRUMNO"].ToString();
                    tda2.Time = dt.Rows[i]["FROMTIME"].ToString();
                    tda2.TotalQty = dt.Rows[i]["OUTQTY"].ToString();
                    TDatak.Add(tda2);
                }
            }
            List<GRNItem> TDatak1 = new List<GRNItem>();
            GRNItem tda3 = new GRNItem();
            DataTable dt1 = new DataTable();
            dt1 = QcDashboardService.GetGRNItem();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tda3 = new GRNItem();
                    tda3.id = dt1.Rows[i]["GRNBLBASICID"].ToString();
                    tda3.Docid = dt1.Rows[i]["DOCID"].ToString();
                    tda3.Docdate = dt1.Rows[i]["DOCDATE"].ToString();
                    tda3.Party = dt1.Rows[i]["PARTYNAME"].ToString();
                    tda3.Cur = dt1.Rows[i]["MAINCURR"].ToString();
                    TDatak1.Add(tda3);
                }
            }

            //GridDisplay Reg = new GridDisplay();
            //List<GridDisplay> Data1 = new List<GridDisplay>();
            //DataTable dt = new DataTable();
            //dt = QcDashboardService.GetquoteFollowupnextReport();
            //for (int i = 0; i < dt.Rows.Count; i++)
            //{
            //    Reg = new GridDisplay();
            //    Reg.displaytext = dt.Rows[i]["QUO_ID"].ToString();
            //    Reg.followedby = dt.Rows[i]["FOLLOWED_BY"].ToString();
            //    Reg.status = dt.Rows[i]["NEXT_FOLLOW_DATE"].ToString();
            //    Data1.Add(Reg);
            //}
            //EnqDisplay tdas = new EnqDisplay();
            //List<EnqDisplay> Data2 = new List<EnqDisplay>();
            //DataTable dt1 = new DataTable();
            //dt1 = QcDashboardService.GetEnqFollowupnextReport();
            //for (int i = 0; i < dt1.Rows.Count; i++)
            //{
            //    tdas = new EnqDisplay();
            //    tdas.displaytext = dt1.Rows[i]["ENQ_ID"].ToString();
            //    tdas.followedby = dt1.Rows[i]["FOLLOWED_BY"].ToString();
            //    tdas.status = dt1.Rows[i]["NEXT_FOLLOW_DATE"].ToString();

            //    Data2.Add(tdas);

            //}

            //SalesQuoteDisplay sq = new SalesQuoteDisplay();
            //List<SalesQuoteDisplay> Data3 = new List<SalesQuoteDisplay>();
            //DataTable dt5 = new DataTable();
            //dt5 = QcDashboardService.GetSalesQuoteFollowupnextReport();
            //for (int i = 0; i < dt5.Rows.Count; i++)
            //{
            //    sq = new SalesQuoteDisplay();
            //    sq.displaytext = dt5.Rows[i]["QUOTE_NO"].ToString();
            //    sq.followby = dt5.Rows[i]["FOLLOW_BY"].ToString();
            //    sq.status = dt5.Rows[i]["NEXT_FOLLOW_DATE"].ToString();

            //    Data3.Add(sq);

            //}

            //H.Folllst = Data1;
            //H.Enqlllst = Data2;
            //H.SalesQuotelllst = Data3;
            H.qcNotifies = TData;
            H.Notifies = TData1;
            H.APOutlist = TDatao1;
            H.Materialnotification = TDatan;
            H.Aplast = TDatak;
            H.Grnplst = TDatak1;
            //H.Quotefollowcunt = dt.Rows.Count;
            //H.EnqFollowcunt = dt1.Rows.Count;
            //H.SalesQuoteFollowcunt = dt5.Rows.Count;
            return View(H);
        }
        
    }
}
