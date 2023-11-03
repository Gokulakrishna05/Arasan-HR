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

            List<MatNotifys> TDatan = new List<MatNotifys>();
            MatNotifys tdan = new MatNotifys();

            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    tdan = new MatNotifys();
                    tdan.Date = dt4.Rows[i]["DOCDATE"].ToString();
                    tdan.LocationName = dt4.Rows[i]["LOCID"].ToString();
                    tdan.ItemName = dt4.Rows[i]["ITEMID"].ToString();
                    tdan.TotalQty = dt4.Rows[i]["QTY"].ToString();
                    tdan.Unit = dt4.Rows[i]["UNITID"].ToString();
                    tdan.stockQty = dt4.Rows[i]["STOCK"].ToString();
                    TDatan.Add(tdan);
                }
            }

            H.Folllst = Data1;
            H.Enqlllst = Data2;
            H.SalesQuotelllst = Data3;
            H.CurInlst = Data4;
            H.Quotefollowcunt = dt.Rows.Count;
            H.EnqFollowcunt = dt1.Rows.Count;
            H.SalesQuoteFollowcunt = dt5.Rows.Count;
            H.Materialnotification = TDatan;

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
    }
}