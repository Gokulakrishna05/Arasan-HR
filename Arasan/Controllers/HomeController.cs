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
using DocumentFormat.OpenXml.Spreadsheet;

namespace Arasan.Controllers
{
    public class HomeController : Controller
    {
        IHomeService HomeService;
        private string? _connectionString;
        IConfiguration? _configuratio;
        DataTransactions datatrans;
        private readonly ILogger<HomeController> _logger;
        public HomeController(IHomeService _HomeService, IConfiguration _configuratio,ILogger<HomeController> logger)
        {
            HomeService = _HomeService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            _logger = logger;
        }
        
        public IActionResult SalesDash()
        {
            salesdash S =new salesdash();
            DataTable dt = new DataTable();
            dt = datatrans.GetData("select I.ITEMID,SUM(D.QTY) as QTY from EXINVBASIC E,EXINVDETAIL D,ITEMMASTER I where E.EXINVBASICID=D.EXINVBASICID AND I.ITEMMASTERID=D.ITEMID AND I.IGROUP='FINISHED' AND I.RAWMATCAT IN('AP POWDER','DUST POWDER') AND E.DOCDATE BETWEEN '01-APR-2021' AND  '31-MAR-2022' GROUP BY I.ITEMID order by SUM(D.QTY) DESC FETCH FIRST 5 ROWS ONLY");
            List<topsellpro> Tdata = new List<topsellpro>();
            topsellpro tda = new topsellpro();
            List<Salespar> Tdata1 = new List<Salespar>();
            Salespar tda1 = new Salespar();
            double totqty = 0;
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    totqty += Convert.ToDouble(dt.Rows[i]["QTY"].ToString());
                }
            }

            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    double qty= Convert.ToDouble(dt.Rows[i]["QTY"].ToString()=="" ? 0 : dt.Rows[i]["QTY"].ToString());
                    double per =Math.Round(((qty / totqty) * 100),0);
                    tda = new topsellpro();
                    tda.itemname = dt.Rows[i]["ITEMID"].ToString();
                    tda.sno = (i + 1);
                    tda.per = per;
                    Tdata.Add(tda);
                }
            }
            DataTable dt1 = datatrans.GetData("select I.ITEMID,SUM(D.QTY) as QTY from EXINVBASIC E,EXINVDETAIL D,ITEMMASTER I where E.EXINVBASICID=D.EXINVBASICID AND I.ITEMMASTERID=D.ITEMID AND I.IGROUP='FINISHED' AND I.RAWMATCAT IN('AP POWDER','DUST POWDER') AND E.DOCDATE BETWEEN '01-MAR-2022' AND  '31-MAR-2022' GROUP BY I.ITEMID order by SUM(D.QTY) DESC FETCH FIRST 17 ROWS ONLY ");
            string str = "";
            string color = "";
            if (dt1.Rows.Count > 0)
            {
                 str += "[";
                for (int i = 0; i < dt1.Rows.Count; i++)
                {
                    double qty = Convert.ToDouble(dt1.Rows[i]["QTY"].ToString());
                    if(qty >= 50000)
                    {
                        color = "#ff4d4d";
                    }
                    else if(qty >= 35000 && qty < 50000)
                    {
                        color = "##ff9933";
                    }
                    else if (qty >= 30000 && qty < 35000 && qty < 50000)
                    {
                        color = "#0073e6";
                    }
                    else if (qty >= 10000 && qty < 30000 && qty < 35000 && qty < 50000)
                    {
                        color = "#29a329";
                    }
                    str +=  "{"+
                    " \"itemname\": \"" + dt1.Rows[i]["ITEMID"].ToString() + "\", " +
                    " \"sales\": \"" + dt1.Rows[i]["QTY"].ToString() + "\"" +
                    //" \"color\": \"" + color + "\"" +
                    "  },";
                    
                    
                }
                str = str.Remove(str.Length - 1);
                str +="]";
            }
            ViewBag.Item = str;
            S.topsellpros = Tdata;
            S.Salesparlst = Tdata1;
                    return View(S);
        }
       
        public IActionResult PurchaseDash( )
        {
            Home H = new Home();
             
                int indent = datatrans.GetDataId("select count(*) as cunt from PINDBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
                int enq = datatrans.GetDataId("select count(*) as cunt from PURENQBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
                int quot = datatrans.GetDataId("select count(*) as cunt from PURQUOTBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
                int po = datatrans.GetDataId("select count(*) as cunt from POBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
                int direct = datatrans.GetDataId("select count(*) as cunt from DPBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE) ");
                int grn = datatrans.GetDataId("select count(*) as cunt from GRNBLBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE) ");
                H.indent = indent;
                H.enqury = enq;
                H.qout = quot;
                H.po = po;
                H.direct = direct;
                H.grn = grn;
            H.suppliar = datatrans.GetDataString("select count(*) as cunt from PARTYMAST   where TYPE  in ('Supplier','BOTH') ");
            H.itemcnt = datatrans.GetDataString("select count(*) as cunt from ITEMMASTER ");

            H.user = Request.Cookies["UserName"];


            PurchaseDash tad = new PurchaseDash();
            List<PurchaseDash> Data = new List<PurchaseDash>();
            IndentCreate tad1 = new IndentCreate();
            List<IndentCreate> Data1 = new List<IndentCreate>();
            indentapp1 tada = new indentapp1();
            List<indentapp1> Dataa = new List<indentapp1>();
            indentsup tads = new indentsup();
            List<indentsup> Datas = new List<indentsup>();

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

                        DateTime Current = DateTime.Parse(tad.Date);
                        TimeSpan difference = DateTime.Now - Current;
                        int daysAgo = (int)difference.TotalDays;
                        if (daysAgo == 0)
                        {
                            tad.days = "Today";
                        }
                        else
                        {
                            tad.days = daysAgo + "days ago";


                        }
                    }

                    Data.Add(tad);
                }
                }
            
             
           
            DataTable appdt = new DataTable();
            string app1 = datatrans.GetDataString("SELECT AUSERNAME FROM APPROVDETAIL WHERE AUSERNAME='"+H.user+"'");
            if ("nages" == app1)
            {
                appdt = HomeService.Getinddentapprove();
            }
            if("srrajan" == app1)
            {
                appdt = datatrans.GetData("select P.DOCID,to_char(P.DOCDATE,'dd-MON-yyyy')DOCDATE,ITEMMASTER.ITEMID,PD.QTY,LOCDETAILS.LOCID from PINDBASIC P,PINDDETAIL PD LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PD.ITEMID  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PD.DEPARTMENT WHERE P.PINDBASICID=PD.PINDBASICID AND PD.APPROVED2 is null");
            }
            for (int i = 0; i < appdt.Rows.Count; i++)
            {
                tada = new indentapp1();
                tada.indentno = appdt.Rows[i]["DOCID"].ToString();
                tada.indentdate = appdt.Rows[i]["DOCDATE"].ToString();
                tada.ItemName = appdt.Rows[i]["ITEMID"].ToString();
                tada.Qty = appdt.Rows[i]["QTY"].ToString();
                tada.loc = appdt.Rows[i]["LOCID"].ToString();
                Dataa.Add(tada);

            }
            DataTable sudt = new DataTable();

            sudt = datatrans.GetData("select P.DOCID,to_char(P.DOCDATE,'dd-MON-yyyy')DOCDATE,ITEMMASTER.ITEMID,PD.QTY,LOCDETAILS.LOCID from PINDBASIC P,PINDDETAIL PD LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PD.ITEMID  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PD.DEPARTMENT WHERE P.PINDBASICID=PD.PINDBASICID AND PD.APPROVED1 ='YES' AND PD.APPROVED2 ='YES' AND IS_SUPPALLOCATE='N'");
           
            for (int i = 0; i < sudt.Rows.Count; i++)
            {
                tads = new indentsup();
                tads.indentno = sudt.Rows[i]["DOCID"].ToString();
                tads.indentdate = sudt.Rows[i]["DOCDATE"].ToString();
                tads.ItemName = sudt.Rows[i]["ITEMID"].ToString();
                tads.Qty = sudt.Rows[i]["QTY"].ToString();
                tads.loc = sudt.Rows[i]["LOCID"].ToString();
                Datas.Add(tads);

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
            ChartData tda1=new ChartData();
            List<ChartData> TData3 = new List<ChartData>();
            DataTable topitem = datatrans.GetData("SELECT SUM(net) stk, PARTYNAME  FROM (SELECT SUM(net) as net, PARTYNAME FROM dpbasic WHERE DOCDATE between sysdate - 365 and sysdate GROUP BY PARTYNAME   UNION ALL SELECT SUM(net) as net, PARTYNAME FROM GRNBLBASIC WHERE DOCDATE between sysdate - 365 and sysdate GROUP BY PARTYNAME) GROUP BY PARTYNAME order by SUM(net) desc");
            string str = "";
            string color = "";
            if (topitem.Rows.Count > 0)
            {
                str += "[";
                for (int i = 0; i < topitem.Rows.Count; i++)
                {
                    tda1=new ChartData();
                    tda1.ctext = topitem.Rows[i]["PARTYNAME"].ToString();
                    tda1.cvalue = topitem.Rows[i]["stk"].ToString();
                    TData3.Add(tda1);
                //    str += "{" +
                //    " \"party\": \"" + topitem.Rows[i]["PARTYNAME"].ToString() + "\", " +
                //    " \"amount\": \"" + topitem.Rows[i]["stk"].ToString() + "\"" +


                //    "  },";


                }
                //str = str.Remove(str.Length - 1);
                //str += "]";
            }
           // ViewBag.Item = str;
            ViewBag.chrtlst = TData3;
            //DataTable intent = datatrans.GetData("select count(pindbasicID) as cunt,to_char(PINDBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE from PINDBASIC  where    PINDBASIC.DOCDATE BETWEEN '01-MAR-2022' AND  '8-MAR-2022' GROUP BY DOCDATE  ");
            //string str = "";
            //string color = "";
            //if (intent.Rows.Count > 0)
            //{
            //    str += "[";
            //    for (int i = 0; i < intent.Rows.Count; i++)
            //    {
            //        string intentcom = datatrans.GetDataString("select count(GR.INDENTDT) as cunt  from pindbasic P ,pinddetail PD,GRNBLDETAIL GR,GRNBLBASIC G where   PD.pindbasicID=P.pindbasicID AND GR.INDENTNO=P.DOCID AND    GR.GRNBLBASICID=G.GRNBLBASICID AND GR.INDENTDT ='" + intent.Rows[i]["DOCDATE"].ToString() + "' GROUP BY P.DOCDATE   ");
            //        if (intentcom == "") { intentcom = "0"; }
            //        str += "{" +
            //        " \"date\": \"" + intent.Rows[i]["DOCDATE"].ToString() + "\", " +
            //        " \"create\": \"" + intent.Rows[i]["cunt"].ToString() + "\"," +
            //        " \"complete\": \"" + intentcom + "\"," +
            //        " \"pending\": \"" + intent.Rows[i]["cunt"].ToString() + "\"" +
            //        " \"color\": \"" + color + "\"" +
            //        "  },";


            //    }
            //    str = str.Remove(str.Length - 1);
            //    str += "]";
            //}
            //ViewBag.Item = str;
            H.Folllst = Data3;
            H.Enqlllst = Data4;
            H.purlst = Data;
            H.indlst = Data1;
            H.applst = Dataa;
            H.suplst = Datas;
          
            H.dagrncnt = dt2.Rows.Count;
          
            H.appcnt  = appdt.Rows.Count;
            H.supcnt  = sudt.Rows.Count;
      
            
            H.Quotefollowcunt = Qdt.Rows.Count;
            H.EnqFollowcunt = Edt1.Rows.Count;
            return View(H);
        }
        public IActionResult StoreDash()
        {
            Home H = new Home();

            //int indent = datatrans.GetDataId("select count(*) as cunt from PINDBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
            //int enq = datatrans.GetDataId("select count(*) as cunt from PURENQBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
            //int quot = datatrans.GetDataId("select count(*) as cunt from PURQUOTBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
            //int po = datatrans.GetDataId("select count(*) as cunt from POBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE)");
            //int direct = datatrans.GetDataId("select count(*) as cunt from DPBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE) ");
            //int grn = datatrans.GetDataId("select count(*) as cunt from GRNBLBASIC   where TRUNC(DOCDATE) = TRUNC(SYSDATE) ");
            //H.indent = indent;
            //H.enqury = enq;
            //H.qout = quot;
            //H.po = po;
            //H.direct = direct;
            //H.grn = grn;



         
          
          
            IssuePen tad2 = new IssuePen();
            List<IssuePen> Data2 = new List<IssuePen>();
            DataTable dt2 = new DataTable();
            List<GateIn> TDatag = new List<GateIn>();
            GateIn tdag = new GateIn();
            

            //DataTable Idt2 = new DataTable();
            //Idt2 = HomeService.GetMatDetail();
            //if (Idt2.Rows.Count > 0)
            //{
            //    for (int i = 0; i < Idt2.Rows.Count; i++)
            //    {
            //        tad1 = new IndentCreate();

            //        tad1.ItemName = Idt2.Rows[i]["ITEMID"].ToString();
            //        tad1.qty = Idt2.Rows[i]["QTY"].ToString();
            //        tad1.basicid = Idt2.Rows[i]["STORESREQBASICID"].ToString();
            //        DataTable dt1 = new DataTable();
            //        dt1 = HomeService.GetMat(tad1.basicid);
            //        if (dt1.Rows.Count > 0)
            //        {
            //            tad1.docid = dt1.Rows[0]["DOCID"].ToString();
            //            tad1.Date = dt1.Rows[0]["DOCDATE"].ToString();
            //            tad1.location = dt1.Rows[0]["LOCID"].ToString();
            //        }
            //        else
            //        {
            //            tad1.Date = DateTime.Now.ToString("dd-MMM-yyyy");
            //        }
            //        DateTime Current = DateTime.Parse(tad1.Date);

            //        TimeSpan difference = DateTime.Now - Current;
            //        int daysAgo = (int)difference.TotalDays;
            //        if (daysAgo == 0)
            //        {
            //            tad1.days = "Today";
            //        }
            //        else
            //        {
            //            tad1.days = daysAgo + "days ago";
            //        }
            //        Data1.Add(tad1);
            //    }
            //}
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
                    else
                    {
                        tad2.Date = DateTime.Now.ToString("dd-MMM-yyyy");
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
           DataTable dtg = datatrans.GetData("select to_char(GATE_IN_DATE,'dd-MON-yyyy') GATE_IN_DATE,GATE_IN_TIME,TOTAL_QTY,PARTYMAST.PARTYNAME,ITEMMASTER.ITEMID,UNITMAST.UNITID,POBASICID from GATE_INWARD_DETAILS LEFT OUTER JOIN GATE_INWARD on GATE_INWARD.GATE_IN_ID=GATE_INWARD_DETAILS.GATE_IN_ID LEFT OUTER JOIN  PARTYMAST on GATE_INWARD.PARTYID=PARTYMAST.PARTYMASTID LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=GATE_INWARD_DETAILS.ITEM_ID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT WHERE GATE_INWARD.STATUS='Waiting' AND GATE_INWARD_DETAILS.QCFLAG='YES' AND PARTYMAST.TYPE IN ('Supplier','BOTH')");
            if (dtg.Rows.Count > 0)
            {
                for (int i = 0; i < dtg.Rows.Count; i++)
                {
                    tdag = new GateIn();
                    tdag.GateDate = dtg.Rows[i]["GATE_IN_DATE"].ToString() + " & " + dtg.Rows[i]["GATE_IN_TIME"].ToString();
                    tdag.PartyName = dtg.Rows[i]["PARTYNAME"].ToString();
                    tdag.ItemName = dtg.Rows[i]["ITEMID"].ToString();
                    tdag.TotalQty = dtg.Rows[i]["TOTAL_QTY"].ToString();
                    tdag.Unit = dtg.Rows[i]["UNITID"].ToString();
                    tdag.id = dtg.Rows[i]["POBASICID"].ToString();
                    TDatag.Add(tdag);
                }
            }

            DataTable topitem = datatrans.GetData("select  SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) as QTY,I.ITEMID  from STOCKVALUE S ,ITEMMASTER I where S.ITEMID=I.ITEMMASTERID AND S.LOCID='10001000000827'   HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0  GROUP BY I.ITEMID order by SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) desc FETCH FIRST 5 ROWS ONLY ");
            string str = "";
            string color = "";
            if (topitem.Rows.Count > 0)
            {
                str += "[";
                for (int i = 0; i < topitem.Rows.Count; i++)
                {
                      
                    str += "{" +
                    " \"item\": \"" + topitem.Rows[i]["ITEMID"].ToString() + "\", " +
                    " \"qty\": \"" + topitem.Rows[i]["QTY"].ToString() + "\"" +
                    
                  
                    "  },";


                }
                str = str.Remove(str.Length - 1);
                str += "]";
            }
            ViewBag.Item = str;
            //DataTable intent = datatrans.GetData("select count(pindbasicID) as cunt,to_char(PINDBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE from PINDBASIC  where    PINDBASIC.DOCDATE BETWEEN '01-MAR-2022' AND  '8-MAR-2022' GROUP BY DOCDATE  ");
            //string str = "";
            //string color = "";
            //if (intent.Rows.Count > 0)
            //{
            //    str += "[";
            //    for (int i = 0; i < intent.Rows.Count; i++)
            //    {
            //        string intentcom = datatrans.GetDataString("select count(GR.INDENTDT) as cunt  from pindbasic P ,pinddetail PD,GRNBLDETAIL GR,GRNBLBASIC G where   PD.pindbasicID=P.pindbasicID AND GR.INDENTNO=P.DOCID AND    GR.GRNBLBASICID=G.GRNBLBASICID AND GR.INDENTDT ='" + intent.Rows[i]["DOCDATE"].ToString() + "' GROUP BY P.DOCDATE   ");
            //        if (intentcom == "") { intentcom = "0"; }
            //        str += "{" +
            //        " \"date\": \"" + intent.Rows[i]["DOCDATE"].ToString() + "\", " +
            //        " \"create\": \"" + intent.Rows[i]["cunt"].ToString() + "\"," +
            //        " \"complete\": \"" + intentcom + "\"," +
            //        " \"pending\": \"" + intent.Rows[i]["cunt"].ToString() + "\"" +
            //        " \"color\": \"" + color + "\"" +
            //        "  },";


            //    }
            //    str = str.Remove(str.Length - 1);
            //    str += "]";
            //}
            //ViewBag.Item = str;


            H.penlst = Data2;
            H.GateInlst = TDatag;
            H.dagrncnt = dt2.Rows.Count;
            H.gatcnt = dtg.Rows.Count;
          
            H.indcnt = Idt3.Rows.Count;

          
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
        //public IActionResult _MenuBar()
        //{
        //    Home H = new Home();
        //    string userid = Request.Cookies["UserId"];
        //    MenuList tda = new MenuList();
        //    List<MenuList> Tdata = new List<MenuList>();
        //    DataTable dt6 = new DataTable();
        //    dt6 = HomeService.GetAllMenu(userid);
        //    if (dt6.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt6.Rows.Count; i++)
        //        {
        //            tda = new MenuList();
        //            tda.Title = dt6.Rows[i]["TITLE"].ToString();
        //            tda.Parent = dt6.Rows[i]["PARENT"].ToString();
        //            tda.Groupid = dt6.Rows[i]["GROUP_ID"].ToString();
        //            tda.IsHead = dt6.Rows[i]["IS_HEAD"].ToString();
        //            Tdata.Add(tda);
        //        }
        //    }
        //    H.Menulst = Tdata;
        //    return View("_MenuBar", H);
        //}

        //public IActionResult _MenuBar()
        //{
        //    Home H = new Home();
        //    string userid = Request.Cookies["UserId"];
        //    MenuList tda = new MenuList();
        //    List<MenuList> Tdata = new List<MenuList>();
        //    DataTable dt6 = new DataTable();
        //    dt6 = HomeService.GetAllMenu(userid);
        //    if (dt6.Rows.Count > 0)
        //    {
        //        for (int i = 0; i < dt6.Rows.Count; i++)
        //        {
        //            tda = new MenuList();
        //            tda.Title = dt6.Rows[i]["TITLE"].ToString();   
        //            tda.Parent = dt6.Rows[i]["PARENT"].ToString();  
        //            tda.Groupid = dt6.Rows[i]["GROUP_ID"].ToString(); 
        //            tda.IsHead = dt6.Rows[i]["IS_HEAD"].ToString();  
        //            Tdata.Add(tda);
        //        }
        //    }
        //    H.Menulst = Tdata;
        //    ViewData["ViewDataMessage"] = "Success from ViewData!";
        //    return PartialView(H);  
        //}
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
                int direct = datatrans.GetDataId("select count(*) as cunt from DPBASIC   where DOCDATE BETWEEN '" + st + "'  AND '" + ed + "' ");
                int grn = datatrans.GetDataId("select count(*) as cunt from GRNBLBASIC   where DOCDATE BETWEEN '" + st + "'  AND '" + ed + "'");
                


                var result = new { indent = indent, enq = enq, quot = quot, po= po, direct = direct, grn = grn };
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