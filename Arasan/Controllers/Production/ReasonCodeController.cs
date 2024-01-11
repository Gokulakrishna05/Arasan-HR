using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Interface.Production;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Arasan.Services;

namespace Arasan.Controllers.Production
{

    public class ReasonCodeController : Controller
    {
        IReasonCodeService ReasonCodeService;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ReasonCodeController(IReasonCodeService _ReasonCodeService, IConfiguration _configuratio)
        {
            ReasonCodeService = _ReasonCodeService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ReasonCode(string id)
        {
            ReasonCode ca = new ReasonCode();
            ca.assignList = BindEmp();
            ca.Proclst = BindProc();
            //ca.Categorylst = BindCategory();
            List<ReasonItem> TData = new List<ReasonItem>();
            ReasonItem tda = new ReasonItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new ReasonItem();
                    tda.Categorylst = BindCategory();
                    tda.Grouplst = BindGroup();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                dt = ReasonCodeService.GetReasonCode(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                    //ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();

                    
                }
                DataTable dt2 = new DataTable();

                dt2 = ReasonCodeService.GetReasonItem(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++) 
                    {
                        tda = new ReasonItem();
                        double toaamt = 0;
                        tda.Categorylst = BindCategory();
                        tda.Grouplst = BindGroup();
                        tda.Reason = dt2.Rows[i]["REASON"].ToString();
                        tda.Category = dt2.Rows[i]["RTYPE"].ToString();
                        tda.Description = dt2.Rows[i]["DESCRIPTION"].ToString();
                        tda.GroupId = dt2.Rows[i]["STOPID"].ToString();

                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }

            }
            ca.ReLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ReasonCode(ReasonCode Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ReasonCodeService.ReasonCodeCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ReasonCode Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ReasonCode Updated Successfully...!";
                    }
                    return RedirectToAction("ListReasonCode");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ReasonCode";
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
        public List<SelectListItem> BindCategory()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "IDLE-TIME", Value = "IDLE-TIME" });
                lstdesg.Add(new SelectListItem() { Text = "DOWN-TIME", Value = "DOWN-TIME" });
                lstdesg.Add(new SelectListItem() { Text = "RUN-TIME", Value = "RUN-TIME" });

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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        } 
        
        public List<SelectListItem> BindProc()
        {
            try
            {
                DataTable dtDesg = ReasonCodeService.Getprocess();
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
        public List<SelectListItem> BindGroup()
        {
            try
            {
                DataTable dtDesg = ReasonCodeService.Getstop();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STOPDESC"].ToString(), Value = dtDesg.Rows[i]["STOPMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListReasonCode()
        {
            return View();
        }
        public JsonResult GetItemJSON()
        {
            ReasonItem model = new ReasonItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        //public JsonResult GetItemGrpJSON()
        //{
        //    //DirectItem model = new DirectItem();
        //    //  model.ItemGrouplst = BindItemGrplst(value);
        //    return Json(BindItemGrplst());
        //}

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = ReasonCodeService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListReasonCode"); 
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListReasonCode");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = ReasonCodeService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListReasonCode");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListReasonCode");
            }
        }

        public JsonResult GetItemCatJSON()
        {
            
            return Json(BindCategory());
        }
        public JsonResult GetItemstopJSON()
        {
            
            return Json(BindGroup());
        }

        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Reasongrid> Reg = new List<Reasongrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ReasonCodeService.GetAllReason(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;
                
               

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y") 
                {

                    //ViewRow = "<a href=viewStop?id=" + dtUsers.Rows[i]["STOPMASTID"].ToString() + "><img src='../Images/view_icon.png' alt='View Details' /></a>";
                    EditRow = "<a href=ReasonCode?id=" + dtUsers.Rows[i]["REASONBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["REASONBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    //ViewRow = "";
                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["REASONBASICID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }

                Reg.Add(new Reasongrid
                {
                    id = dtUsers.Rows[i]["REASONBASICID"].ToString(),
                    process = dtUsers.Rows[i]["PROCESSID"].ToString(),
                    //viewrow = ViewRow,
                    editrow = EditRow,
                    delrow = DeleteRow,


                });
            }

            return Json(new
            {
                Reg
            });

        }

    }
}
