using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using NuGet.Protocol.Plugins;
using System.Xml.Linq;
using Org.BouncyCastle.Ocsp;

namespace Arasan.Controllers
{
    public class HomeController : Controller
    {
        IHomeService HomeService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        public HomeController(IHomeService _HomeService, IConfiguration _configuratio)
        {
            HomeService = _HomeService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public IActionResult PurchaseDash( )
        {
            Home H = new Home();
             
                int indent = datatrans.GetDataId("select count(*) as cunt from PINDBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
                int enq = datatrans.GetDataId("select count(*) as cunt from PURENQBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
                int quot = datatrans.GetDataId("select count(*) as cunt from PURQUOTBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
                int po = datatrans.GetDataId("select count(*) as cunt from POBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
                int gate = datatrans.GetDataId("select count(*) as cunt from GATE_INWARD   where TRUNC(GATE_IN_DATE) = TRUNC(SYSDATE) ");
                int grn = datatrans.GetDataId("select count(*) as cunt from GRNBLBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE) ");
                H.indent = indent;
                H.enqury = enq;
                H.qout = quot;
                H.po = po;
                H.gate = gate;
                H.grn = grn;
          
               
           
            PurchaseDash tad = new PurchaseDash();
            List<PurchaseDash> Data = new List<PurchaseDash>();
            IndentCreate tad1 = new IndentCreate();
            List<IndentCreate> Data1 = new List<IndentCreate>();
            IssuePen tad2 = new IssuePen();
            List<IssuePen> Data2 = new List<IssuePen>();
            DataTable dt2 = new DataTable();
           
            dt2 = HomeService.GetDamageGRNDetail();
            if (dt2.Rows.Count>0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tad = new PurchaseDash();
                    
                  
                   
                    tad.ItemName = dt2.Rows[i]["ITEMID"].ToString();
                    tad.qty = dt2.Rows[i]["DAMAGE_QTY"].ToString();
                    tad.grnbasicid = dt2.Rows[i]["GRNBLBASICID"].ToString();
                    DataTable dt1 = new DataTable();
                    dt1 = HomeService.GetGRN(tad.grnbasicid);
                    if (dt1.Rows.Count > 0)
                    {
                        tad.grn = dt1.Rows[0]["DOCID"].ToString();
                        tad.Date = dt1.Rows[0]["DOCDATE"].ToString();
                        tad.party = dt1.Rows[0]["PARTYNAME"].ToString();
                    }
                  
                    DateTime Current = DateTime.Parse(tad.Date);

                    TimeSpan difference = DateTime.Now - Current;
                    int daysAgo = (int)difference.TotalDays;
                     if(daysAgo==0)
                    {
                        tad.days = "Today";
                    }
                    else
                    {
                        tad.days = daysAgo + "days ago";
                       
                    }
                    Data.Add(tad);
                }
                }
            
            DataTable Idt2 = new DataTable();
            Idt2 = HomeService.GetMatDetail();
            if (Idt2.Rows.Count > 0)
            {
                for (int i = 0; i < Idt2.Rows.Count; i++)
                {
                    tad1 = new IndentCreate();
                  
                    tad1.ItemName = Idt2.Rows[i]["ITEMID"].ToString();
                    tad1.qty = Idt2.Rows[i]["QTY"].ToString();
                    tad1.basicid = Idt2.Rows[i]["STORESREQBASICID"].ToString();
                    DataTable dt1 = new DataTable();
                    dt1 = HomeService.GetMat(tad1.basicid);
                    if (dt1.Rows.Count > 0)
                    {
                        tad1.docid = dt1.Rows[0]["DOCID"].ToString();
                        tad1.Date = dt1.Rows[0]["DOCDATE"].ToString();
                        tad1.location = dt1.Rows[0]["LOCID"].ToString();
                    }
                    DateTime Current = DateTime.Parse(tad1.Date);

                    TimeSpan difference = DateTime.Now - Current;
                    int daysAgo = (int)difference.TotalDays;
                    if (daysAgo == 0)
                    {
                        tad1.days = "Today";
                    }
                    else
                    {
                        tad1.days = daysAgo + "days ago";
                    }
                    Data1.Add(tad1);
                }
            }
            DataTable Idt3 = new DataTable();
            Idt3 = HomeService.GetIssMatDetail();
            if (Idt3.Rows.Count > 0)
            {
                for (int i = 0; i < Idt3.Rows.Count; i++)
                {
                    tad2 = new IssuePen();

                    tad2.ItemName = Idt3.Rows[i]["ITEMID"].ToString();
                    tad2.qty = Idt3.Rows[i]["QTY"].ToString();
                    tad2.basicid = Idt3.Rows[i]["STORESREQBASICID"].ToString();
                    DataTable dt1 = new DataTable();
                    dt1 = HomeService.GetMat(tad2.basicid);
                    if (dt1.Rows.Count > 0)
                    {
                        tad2.docid = dt1.Rows[0]["DOCID"].ToString();
                        tad2.Date = dt1.Rows[0]["DOCDATE"].ToString();
                        tad2.location = dt1.Rows[0]["LOCID"].ToString();
                    }
                    DateTime Current = DateTime.Parse(tad2.Date);

                    TimeSpan difference = DateTime.Now - Current;
                    int daysAgo = (int)difference.TotalDays;
                    if (daysAgo == 0)
                    {
                        tad2.days = "Today";
                    }
                    else
                    {
                        tad2.days = daysAgo + "days ago";
                    }
                    Data2.Add(tad2);
                }
            }
            GridDisplay Reg = new GridDisplay();
            List<GridDisplay> Data3 = new List<GridDisplay>();
            DataTable Qdt = new DataTable();
            Qdt = HomeService.GetquoteFollowupnextReport();
            for (int i = 0; i < Qdt.Rows.Count; i++)
            {
                Reg = new GridDisplay();
                Reg.displaytext = Qdt.Rows[i]["QUO_ID"].ToString();
                Reg.followedby = Qdt.Rows[i]["FOLLOWED_BY"].ToString();
                Reg.status = Qdt.Rows[i]["NEXT_FOLLOW_DATE"].ToString();
                Data3.Add(Reg);

            }
            EnqDisplay tdas = new EnqDisplay();
            List<EnqDisplay> Data4 = new List<EnqDisplay>();
            DataTable Edt1 = new DataTable();
            Edt1 = HomeService.GetEnqFollowupnextReport();
            for (int i = 0; i < Edt1.Rows.Count; i++)
            {
                tdas = new EnqDisplay();
                tdas.displaytext = Edt1.Rows[i]["ENQ_ID"].ToString();
                tdas.followedby = Edt1.Rows[i]["FOLLOWED_BY"].ToString();
                tdas.status = Edt1.Rows[i]["NEXT_FOLLOW_DATE"].ToString();

                Data4.Add(tdas);

            }
            H.Folllst = Data3;
            H.Enqlllst = Data4;
            H.purlst = Data;
            H.indlst = Data1;
            H.penlst = Data2;
            H.dagrncnt = dt2.Rows.Count;
            H.minstkcnt  = Idt2.Rows.Count;
            H.indcnt = Idt3.Rows.Count;
            
            H.Quotefollowcunt = Qdt.Rows.Count;
            H.EnqFollowcunt = Edt1.Rows.Count;
            return View(H);
        }

            public IActionResult Index()
        {
            Home H = new Home();
            GridDisplay Reg = new GridDisplay();
            List<GridDisplay> Data1 = new List<GridDisplay>();
            DataTable dt = new DataTable();
            dt = HomeService.GetquoteFollowupnextReport();
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                Reg = new GridDisplay();
                Reg.displaytext = dt.Rows[i]["QUO_ID"].ToString();
                Reg.followedby = dt.Rows[i]["FOLLOWED_BY"].ToString();
                Reg.status = dt.Rows[i]["NEXT_FOLLOW_DATE"].ToString();
                Data1.Add(Reg);

            }
            EnqDisplay tdas = new EnqDisplay();
            List<EnqDisplay> Data2 = new List<EnqDisplay>();
            DataTable dt1 = new DataTable();
            dt1 = HomeService.GetEnqFollowupnextReport();
            for (int i = 0; i < dt1.Rows.Count; i++)
            {
                tdas = new EnqDisplay();
                tdas.displaytext = dt1.Rows[i]["ENQ_ID"].ToString();
                tdas.followedby = dt1.Rows[i]["FOLLOWED_BY"].ToString();
                tdas.status = dt1.Rows[i]["NEXT_FOLLOW_DATE"].ToString();

                Data2.Add(tdas);

            }

            SalesQuoteDisplay sq = new SalesQuoteDisplay();
            List<SalesQuoteDisplay> Data3 = new List<SalesQuoteDisplay>();
            DataTable dt5 = new DataTable();
            dt5 = HomeService.GetSalesQuoteFollowupnextReport();
            for (int i = 0; i < dt5.Rows.Count; i++)
            {
                sq = new SalesQuoteDisplay();
                sq.displaytext = dt5.Rows[i]["QUOTE_NO"].ToString();
                sq.followby = dt5.Rows[i]["FOLLOW_BY"].ToString();
                sq.status = dt5.Rows[i]["NEXT_FOLLOW_DATE"].ToString();

                Data3.Add(sq);

            }
            CurIn ci = new CurIn();
            List<CurIn> Data4 = new List<CurIn>();
            DataTable dt6 = new DataTable();
            dt6 = HomeService.GetCurInward();
            for (int i = 0; i < dt6.Rows.Count; i++)
            {
                ci = new CurIn();
                ci.Item = dt6.Rows[i]["ITEMID"].ToString();
                ci.Drum = dt6.Rows[i]["DRUMNO"].ToString();
                ci.Due = dt6.Rows[i]["DUEDATE"].ToString();
                ci.Id = dt6.Rows[i]["CURINPBASICID"].ToString();
                DataTable dt7 = new DataTable();
                dt7 = HomeService.GetCurInwardDoc(ci.Id);
                if (dt7.Rows.Count > 0)
                {
                    ci.Docid = dt7.Rows[0]["DOCID"].ToString();
                }
                    Data4.Add(ci);

            }

            DataTable dt4 = new DataTable();
            dt4 = HomeService.GetMaterialnot();

            //List<MatNotifys> TDatan = new List<MatNotifys>();
            //MatNotifys tdan = new MatNotifys();

            //if (dt4.Rows.Count > 0)
            //{
            //    for (int i = 0; i < dt4.Rows.Count; i++)
            //    {
            //        tdan = new MatNotifys();
            //        tdan.Date = dt4.Rows[i]["DOCDATE"].ToString();
            //        tdan.LocationName = dt4.Rows[i]["LOCID"].ToString();
            //        tdan.ItemName = dt4.Rows[i]["ITEMID"].ToString();
            //        tdan.TotalQty = dt4.Rows[i]["QTY"].ToString();
            //        tdan.Unit = dt4.Rows[i]["UNITID"].ToString();
            //        tdan.stockQty = dt4.Rows[i]["STOCK"].ToString();
            //        TDatan.Add(tdan);
            //    }
            //}

            H.Folllst = Data1;
            H.Enqlllst = Data2;
            H.SalesQuotelllst = Data3;
            H.CurInlst = Data4;
            H.Quotefollowcunt = dt.Rows.Count;
            H.EnqFollowcunt = dt1.Rows.Count;
            H.SalesQuoteFollowcunt = dt5.Rows.Count;
            //H.Materialnotification = TDatan;

            return View(H);
        }
        public ActionResult Proddashboard()
        {
            Home H = new Home();
            List<CuringGroup> TData = new List<CuringGroup>();
            CuringGroup tda = new CuringGroup();

            CuringSet tda1 = new CuringSet();
            DataTable dt2 = new DataTable();
            dt2 = HomeService.CuringGroup();
            int empty = datatrans.GetDataId("select count(*) as cunt from curingmaster where STATUS='Active'");
            double loaded = datatrans.GetDataId("select count(*) as cunt from curingmaster where STATUS!='Active'");

            //if(loaded >= empty)
            //{

            //}
            double rem = loaded - empty;
            double remain = rem / loaded;
            int res = 0;
            if (loaded > 0)
            {
                if (loaded >= empty)
                {
                    res = Convert.ToInt32((empty / loaded) * 100);
                }
                else
                {
                    res = Convert.ToInt32((loaded / empty) * 100);
                }

            }
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new CuringGroup();
                    tda.curinggroup = dt2.Rows[i]["SUBGROUP"].ToString();
                    DataTable dt3 = new DataTable();
                    dt3 = HomeService.curingsubgroup(tda.curinggroup);
                    if (dt3.Rows.Count > 0)
                    {
                        List<CuringSet> TData1 = new List<CuringSet>();
                        for (int j = 0; j < dt3.Rows.Count; j++)
                        {
                            tda1 = new CuringSet();
                            tda1.Roomno = dt3.Rows[j]["SHEDNUMBER"].ToString();
                            tda1.Capacity = dt3.Rows[j]["CAPACITY"].ToString();
                            tda1.status = dt3.Rows[j]["STATUS"].ToString();
                            tda1.Occupied = dt3.Rows[j]["OCCUPIED"].ToString();
                            TData1.Add(tda1);
                        }
                        tda.curset = TData1;
                    }
                    TData.Add(tda);
                }
            }
            H.curgroups = TData;
            H.loaded = loaded;
            H.empty = empty;
            H.res = res;
            H.rem = remain;
            return View(H);
        }
        public IActionResult Privacy()
        {
            return View();
        }
        public ActionResult GetDatewisecount(string st,string ed)
        {
            try
            {
                int indent = datatrans.GetDataId("select count(*) as cunt from PINDBASIC   where DOCDATE BETWEEN '" + st + "'  AND '" + ed + "'");
                int enq = datatrans.GetDataId("select count(*) as cunt from PURENQBASIC   where DOCDATE BETWEEN '" + st + "'  AND '" + ed + "'");
                int quot = datatrans.GetDataId("select count(*) as cunt from PURQUOTBASIC   where DOCDATE BETWEEN '" + st + "'  AND '" + ed + "'");
                int po = datatrans.GetDataId("select count(*) as cunt from POBASIC   where DOCDATE BETWEEN '" + st + "'  AND '" + ed + "'");
                int gate = datatrans.GetDataId("select count(*) as cunt from GATE_INWARD   where DOCDATE BETWEEN '" + st + "'  AND '" + ed + "' ");
                int grn = datatrans.GetDataId("select count(*) as cunt from GRNBLBASIC   where DOCDATE BETWEEN '" + st + "'  AND '" + ed + "'");
                


                var result = new { indent = indent, enq = enq, quot = quot, po= po, gate= gate , grn = grn };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public IActionResult Privacy()
        //{
        //    return View();
        //}
    }
}