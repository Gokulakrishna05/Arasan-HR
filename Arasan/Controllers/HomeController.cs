using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Services;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

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
            Home H=new Home();
            //ViewBag.Name = Request.Cookies["UserName"];
            List<QcNotify> TData = new List<QcNotify>();
            QcNotify tda = new QcNotify();
            DataTable dt2 = new DataTable();
            dt2 = HomeService.IsQCNotify();
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                   tda = new QcNotify();
                   tda.GateDate = dt2.Rows[i]["GATE_IN_DATE"].ToString() + " & " + dt2.Rows[i]["GATE_IN_TIME"].ToString();
                   tda.PartyName = dt2.Rows[i]["PARTY"].ToString();
                   tda.ItemName = dt2.Rows[i]["ITEMID"].ToString();
                   tda.TotalQty = dt2.Rows[i]["TOTAL_QTY"].ToString();
                   tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                    TData.Add(tda);
                }
            }
            H.qcNotifies=TData;
                    return View(H);
        }

        public IActionResult Privacy()
        {
            return View();
        }
    }
}