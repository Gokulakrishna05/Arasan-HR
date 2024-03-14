using System.Collections.Generic;
using System.Data;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Sales;
using Arasan.Models;
using Arasan.Services.Sales;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace Arasan.Controllers 
{
    public class MaterialSplitController : Controller
    {

        IMaterialSplit split;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public MaterialSplitController(IMaterialSplit _split, IConfiguration _configuratio)
        {
            split = _split;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult MaterialSplit()
        {
            MaterialSplit ca = new MaterialSplit();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Enterd = Request.Cookies["UserName"];
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Loclst = BindLoc();
            DataTable dtv = datatrans.GetSequence("maspl");
            if (dtv.Rows.Count > 0)
            {
                ca.Docid = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            List<splitdetail> TData = new List<splitdetail>();
            splitdetail tda = new splitdetail();
            for (int i = 0; i < 1; i++)
            {
                tda = new splitdetail();
                tda.Itemlst = BindItem("");
                tda.ConItemlst = BindConItem();

                tda.Isvalid = "Y";
                TData.Add(tda);
            }
            ca.splitLst = TData;
            return View(ca);
        }
        public List<SelectListItem> BindLoc()
        {
            try
            {
                DataTable dtDesg = datatrans.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItem(string id)
        {
            try
            {
                DataTable dtDesg = split.GetItem(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["item"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindConItem()
        {
            try
            {
                DataTable dtDesg = datatrans.GetItem();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON(string loc)
        {
            //BatchItem model = new BatchItem();
            //model.Processidlst = BindProcessid(itemid);
            return Json(BindItem(loc));

        }
    }
}
