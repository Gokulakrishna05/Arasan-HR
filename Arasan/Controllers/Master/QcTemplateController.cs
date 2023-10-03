using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class QcTemplateController : Controller
    {
        IQcTemplateService QcTemplateService;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public QcTemplateController(IQcTemplateService _QcTemplateService, IConfiguration _configuratio)
        {
            QcTemplateService = _QcTemplateService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult QcTemplate(string id)
        {
            QcTemplate br = new QcTemplate();
            br.assignList = BindEmp();
           
            List<QcTemplateItem> TData = new List<QcTemplateItem>();
            QcTemplateItem tda = new QcTemplateItem();


            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new QcTemplateItem();
                    tda.Desclst = BindTDesc();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
               
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                //DataTable dt = new DataTable();
                
                //dt = Batch.GetBatchCreation(id);
                //if (dt.Rows.Count > 0)
                //{
                //    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                //    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                //    ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                //    ca.BatchNo = dt.Rows[0]["DOCID"].ToString();
                //    ca.ID = id;
                //    ca.Prod = dt.Rows[0]["PSCHNO"].ToString();
                //    ca.Process = dt.Rows[0]["WPROCESSID"].ToString();
                //    ca.RefBatch = dt.Rows[0]["REFDOCID"].ToString();
                //    ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                  

                //}
                //DataTable dt2 = new DataTable();

                //dt2 = Batch.GetBatchCreationDetail(id);
                //if (dt2.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt2.Rows.Count; i++)
                //    {
                //        tda = new BatchItem();
                //        tda.WorkCenterlst = BindWorkCenter();
                //        tda.Processidlst = BindProcess(tda.WorkId);
                //        tda.WorkId = dt2.Rows[i]["BWCID"].ToString();
                //        tda.ProcessId = dt2.Rows[i]["PROCESSID"].ToString();
                //        tda.saveItemId = dt2.Rows[i]["PROCESSID"].ToString();
                //        tda.Seq = dt2.Rows[i]["PSEQ"].ToString();
                //        tda.Req = dt2.Rows[i]["INSREQ"].ToString();
                //        tda.ID = id;
                //        TData.Add(tda);
                //        tda.Isvalid = "Y";
                //    }

                //}
               
            }

            br.QcLst = TData;
            return View(br);

        }
        [HttpPost]
        public ActionResult QcTemplate(QcTemplate Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = QcTemplateService.QcTemplateCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "QcTemplate Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "QcTemplate Updated Successfully...!";
                    }
                    return RedirectToAction("ListQcTemplate");
                }

                else
                {
                    ViewBag.PageTitle = "Edit QcTemplate";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }
        public IActionResult ListQcTemplate()
        {
            IEnumerable<QcTemplate> br = QcTemplateService.GetAllQcTemplate();
            return View(br);
        }
        public ActionResult GetQCTestDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string unit = "";
                string value = "";
                string un = "";
                dt = QcTemplateService.GetQCTestDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                    value = dt.Rows[0]["VALUEORMANUAL"].ToString();
                    un = dt.Rows[0]["UNIT"].ToString();

                }

                var result = new { unit = unit, value = value, un  = un };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetQcItemGrpJSON()
        {
            QcTemplateItem model = new QcTemplateItem();
            model.Desclst = BindTDesc();
            return Json(BindTDesc());
        }
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindTDesc()
        {
            try
            {
                DataTable dtDesg = QcTemplateService.GetDesc();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TESTDESC"].ToString(), Value = dtDesg.Rows[i]["TESTDESC"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
