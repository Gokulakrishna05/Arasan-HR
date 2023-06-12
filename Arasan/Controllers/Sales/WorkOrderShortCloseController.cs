using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class WorkOrderShortCloseController : Controller
    {
        IWorkOrderShortClose ShortClose;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public WorkOrderShortCloseController(IWorkOrderShortClose _ShortClose, IConfiguration _configuratio)
        {
            ShortClose = _ShortClose;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult WorkOrderShortClose(string id)
        {
            WorkOrderShortClose ca = new WorkOrderShortClose();
            //ca.Branch = Request.Cookies["BranchId"];
            //ca.Curlst = BindCurrency();
            //ca.Loc = BindLocation();
            //ca.Qolst = BindQuotation();
            //ca.JopDate = DateTime.Now.ToString("dd-MMM-yyyy");
            //List<WorkItem> TData = new List<WorkItem>();
            //WorkItem tda = new WorkItem();
               DataTable dt = new DataTable();

            dt = ShortClose.GetWorkOrder(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
               


                 

                 
                ca.Customer = dt.Rows[0]["PARTY"].ToString();
                ca.ID = id;
                ca.Location = dt.Rows[0]["LOCID"].ToString();
                ca.OrderType = dt.Rows[0]["ORDTYPE"].ToString();
                
                
              
                ca.JopId = dt.Rows[0]["DOCID"].ToString();
                
               
            }

        
        //ca.Worklst = TData;
           return View(ca);
        }
        public IActionResult Index()
        {
            return View();
        }
    }
}
