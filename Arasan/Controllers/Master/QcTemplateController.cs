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


                DataTable dt = new DataTable();

                dt = QcTemplateService.GetQcTemplateEdit(id);
                if (dt.Rows.Count > 0)
                {
                    br.Qc = dt.Rows[0]["TEMPLATEID"].ToString();
                    br.Test = dt.Rows[0]["TESTTYPE"].ToString();
                    //br.assignList = BindEmp();
                    br.Set = dt.Rows[0]["SETBY"].ToString();
                    br.Description = dt.Rows[0]["TEMPLATEDESC"].ToString();
                    br.ID = id;
                    br.Type = dt.Rows[0]["QCTYPE"].ToString();
                    br.Procedure = dt.Rows[0]["TESTPROCEDURE"].ToString();
                    br.Samplingper = dt.Rows[0]["SAMPLINGPER"].ToString();
                    br.Level = dt.Rows[0]["ILEVEL"].ToString();
                    


                }
                DataTable dt2 = new DataTable();

                dt2 = QcTemplateService.GetQcTemplateItemEdit(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new QcTemplateItem();
                        tda.Desclst = BindTDesc();
                        tda.ItemDesc = dt2.Rows[i]["TESTDESC"].ToString();
                        tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                        tda.Value = dt2.Rows[i]["VALUEORMANUAL"].ToString();
                        tda.Start = dt2.Rows[i]["STARTVALUE"].ToString();
                        tda.End = dt2.Rows[i]["ENDVALUE"].ToString();
                        tda.ID = id;
                        TData.Add(tda);
                        tda.Isvalid = "Y";
                    }

                }

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
             return View();
        }
        public IActionResult ViewQcTemplate(string id)
        {
            QcTemplate br = new QcTemplate();
            DataTable dt = new DataTable();
            //DataTable dtt = new DataTable();
            dt = QcTemplateService.GetQcTemplateViewEdit(id);
            if (dt.Rows.Count > 0)
            {
                br.Qc = dt.Rows[0]["TEMPLATEID"].ToString();
                br.Test = dt.Rows[0]["TESTTYPE"].ToString();
                //br.assignList = BindEmp();
                br.Set = dt.Rows[0]["EMPNAME"].ToString();
                br.Description = dt.Rows[0]["TEMPLATEDESC"].ToString();
                br.ID = id;
                br.Type = dt.Rows[0]["QCTYPE"].ToString();
                br.Procedure = dt.Rows[0]["TESTPROCEDURE"].ToString();
                br.Samplingper = dt.Rows[0]["SAMPLINGPER"].ToString();
                br.Level = dt.Rows[0]["ILEVEL"].ToString();

                List<QcTemplateItem> TData = new List<QcTemplateItem>();
                QcTemplateItem tda = new QcTemplateItem();
                DataTable dt2 = QcTemplateService.GetQcTemplateViewItemEdit(id);
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new QcTemplateItem();
                    tda.Desclst = BindTDesc();
                    tda.ItemDesc = dt2.Rows[i]["TESTDESC"].ToString();
                    tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                    tda.Value = dt2.Rows[i]["VALUEORMANUAL"].ToString();
                    tda.Start = dt2.Rows[i]["STARTVALUE"].ToString();
                    tda.End = dt2.Rows[i]["ENDVALUE"].ToString();
                    tda.ID = id;
                    TData.Add(tda);
                    
                }
                br.QcLst = TData;
               
            }
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
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<QcTemplateItemGrid> Reg = new List<QcTemplateItemGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = QcTemplateService.GetAllQCTemp(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string View = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    View = "<a href=ViewQcTemplate?id=" + dtUsers.Rows[i]["TESTTBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Edit' /></a>";
                    EditRow = "<a href=QcTemplate?id=" + dtUsers.Rows[i]["TESTTBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["TESTTBASICID"].ToString() + "";
                }

                else
                {

                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["TESTTBASICID"].ToString() + "";
                }
                Reg.Add(new QcTemplateItemGrid
                {
                    id = dtUsers.Rows[i]["TESTTBASICID"].ToString(),
                    qc = dtUsers.Rows[i]["TEMPLATEID"].ToString(),
                    test = dtUsers.Rows[i]["TESTTYPE"].ToString(),
                    description = dtUsers.Rows[i]["TEMPLATEDESC"].ToString(),
                    view = View,
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = QcTemplateService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListQcTemplate");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListQcTemplate");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = QcTemplateService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListQcTemplate");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListQcTemplate");
            }
        }
    }
}
