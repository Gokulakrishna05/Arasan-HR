using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Arasan.Controllers 
{
    public class ProductionLogController : Controller
    {
        IProductionLog productionLog;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ProductionLogController(IProductionLog _productionLog, IConfiguration _configuratio)
        {
            productionLog = _productionLog;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ProductionLog(string id)
        {
            ProductionLog ca = new ProductionLog();
            ca.Brlst = BindBranch();
            ca.Worklst = BindWorkCenter();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Supervised = Request.Cookies["UserId"];
            ca.RecList = BindEmp();
            ca.Shiftlst = BindShift();
            ca.Processlst = BindProcess();
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<WorkCenter> TData = new List<WorkCenter>();
            WorkCenter tda = new WorkCenter();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new WorkCenter();
                    tda.WorkCenterlst = BindWorkCenterid();
                    tda.Reasonlst = BindWReason( );
                    tda.Statuslst = BindWStatus();
                    tda.PTypelst = BindWType();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ca.WorkLst = TData;
            return View(ca);
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = productionLog.ShiftDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindProcess()
        {
            try
            {
                DataTable dtDesg = productionLog.BindProcess();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["PROCESSMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = productionLog.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWReason()
        {
            try
            {
                DataTable dtDesg = productionLog.GetReason();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["REASON"].ToString(), Value = dtDesg.Rows[i]["REASON"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWorkCenterid()
        {
            try
            {
                DataTable dtDesg = productionLog.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "None", Value = "None" });
                lstdesg.Add(new SelectListItem() { Text = "EB", Value = "EB" });
                
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Run", Value = "Run" });
                lstdesg.Add(new SelectListItem() { Text = "Stop", Value = "Stop" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListProductionLog()
        {
            IEnumerable<ProductionLog> cmp = productionLog.GetAllProductionLog();
            return View(cmp);
        }
    }
}
