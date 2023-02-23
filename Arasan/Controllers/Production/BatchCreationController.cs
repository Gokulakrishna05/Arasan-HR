using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Controllers
{
    public class BatchCreationController : Controller
    {
        IBatchCreation Batch;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public BatchCreationController(IBatchCreation _Batch, IConfiguration _configuratio)
        {
            Batch = _Batch;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult BatchCreation()
        {
            BatchCreation ca = new BatchCreation();

            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Worklst = BindWorkCenter();
            ca.Processlst = BindProcess("");
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
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
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = Batch.GetWorkCenter();
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
        public JsonResult GetProcessJSON(string processid)
        {
            BatchCreation model = new BatchCreation();
            model.Processlst = BindProcess(processid);
            return Json(BindProcess(processid));

        }
        public List<SelectListItem> BindProcess(string id)
        {
            try
            {
                DataTable dtDesg = Batch.GetProcess(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListBatchCreation()
        {
            IEnumerable<BatchCreation> cmp = Batch.GetAllBatchCreation();
            return View(cmp);
        }

    }
}
