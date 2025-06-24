using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using Arasan.Services;
using AspNetCore.Reporting;
using Arasan.Interface.Master;
using Arasan.Services.Master;
namespace Arasan.Controllers
{
    public class PayCategoryController : Controller
    {
        IPayCategory payCategory;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public PayCategoryController(IPayCategory _payCategory, IConfiguration _configuratio)
        {
            payCategory = _payCategory;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        [HttpPost]

        public ActionResult PayCategory(PayCategory Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = payCategory.PayCategoryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Pay Category Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Pay Category Updated Successfully...!";
                    }
                    return RedirectToAction("ListPayCategory");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Pay Category";
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

        public IActionResult PayCategory(string id)
        {
            PayCategory ca=new PayCategory();
            List<PayCat> TData = new List<PayCat>();
            PayCat tda = new PayCat();
            ca.PayTyplst = BindPayType();
            ca.BasCatlst = BindPayType();


            DataTable dtv = datatrans.GetSequence("pcat");

            if (dtv.Rows.Count > 0)
            {
                ca.DocID = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new PayCat();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                //double total = 0;
                dt = payCategory.GetEditPayCat(id);
                if (dt.Rows.Count > 0)
                {
                    ca.DocID = dt.Rows[0]["DOCID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.PayCat = dt.Rows[0]["PAYCATEGORY"].ToString();
                    ca.PayTim = dt.Rows[0]["PAYPERIODTYPE"].ToString();
                    ca.PayTyplst = BindPayType();
                    ca.BasCat = dt.Rows[0]["BASICCAT"].ToString();
                    ca.BasCatlst = BindPayType();
                    ca.ID = id;
                }
            }
            DataTable dt2 = new DataTable();
            dt2 = payCategory.GetEditPayCode(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new PayCat();

                    tda.pc = dt2.Rows[i]["PAYCODE"].ToString();
                    tda.pr = dt2.Rows[i]["PRINT"].ToString();
                    tda.prs = dt2.Rows[i]["PRINTAS"].ToString();
                    tda.aod = dt2.Rows[i]["ADDORLESS"].ToString();
                    tda.pcv = dt2.Rows[i]["PAYCODEVALUE"].ToString();
                    tda.fo = dt2.Rows[i]["FORMULA"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ca.PcLst = TData;
            return View(ca);
        }
        public IActionResult PayCatSelection()
        {
            PayCatdetailstable ca = new PayCatdetailstable();
            List<PayCatVList> TData = new List<PayCatVList>();
            PayCatVList tda = new PayCatVList();
            DataTable dt = new DataTable();
            dt = payCategory.getPayCat();
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tda = new PayCatVList();
                    tda.pcode = dt.Rows[i]["PAYCODE"].ToString();
                    tda.print = dt.Rows[i]["PRINT"].ToString();
                    tda.pas = dt.Rows[i]["PRINTAS"].ToString();
                    tda.aol = dt.Rows[i]["ADDORLESS"].ToString();
                    tda.pcoval = dt.Rows[i]["PAYCODEVALUE"].ToString();
                    tda.form = dt.Rows[i]["FORMULA"].ToString();                   
                    tda.dtid = dt.Rows[i]["PARAMETERDETAILID"].ToString();                   
                    TData.Add(tda);
                }
            }
            ca.pcalst = TData;
            return View(ca);
        }

        public JsonResult GetIndentDetail(string indentid)
        {
            PayCatdetailstable ca = new PayCatdetailstable();
            List<PayCatVList> TData = new List<PayCatVList>();
            PayCatVList tda = new PayCatVList();
            DataTable dt = new DataTable();
            dt = payCategory.getPayCatId(indentid);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tda = new PayCatVList();
                    tda.pcode = dt.Rows[i]["PAYCODE"].ToString();
                    tda.print = dt.Rows[i]["PRINT"].ToString();
                    tda.pas = dt.Rows[i]["PRINTAS"].ToString();
                    tda.aol = dt.Rows[i]["ADDORLESS"].ToString();
                    tda.pcoval = dt.Rows[i]["PAYCODEVALUE"].ToString();
                    tda.form = dt.Rows[i]["FORMULA"].ToString();
                    tda.dtid = dt.Rows[i]["PARAMETERDETAILID"].ToString();
                    TData.Add(tda);
                }
            }
            ca.pcalst = TData;
            return Json(ca.pcalst);
        }

        public List<SelectListItem> BindPayType()
        {
            try
            {
                List<SelectListItem> lstcost = new List<SelectListItem>();
                lstcost.Add(new SelectListItem() { Text = "MONTHLY", Value = "MONTHLY" });
                lstcost.Add(new SelectListItem() { Text = "DAILY", Value = "DAILY" });

                return lstcost;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ListPayCategory()
        {
            return View();
        }
        public ActionResult MyListPayCategorygrid(string strStatus)
        {
            List<ListPayCategory> Reg = new List<ListPayCategory>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = payCategory.GetAllPayCategory(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=PayCategory?id=" + dtUsers.Rows[i]["PCBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["PCBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["PCBASICID"].ToString() + "";
                }

                Reg.Add(new ListPayCategory
                {
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    paycat = dtUsers.Rows[i]["PAYCATEGORY"].ToString(),
                    paytim = dtUsers.Rows[i]["PAYPERIODTYPE"].ToString(),
                    bascat = dtUsers.Rows[i]["BASICCAT"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult DeleteItem(string tag, string id)
        {

            string flag = payCategory.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPayCategory");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPayCategory");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = payCategory.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPayCategory");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPayCategory");
            }
        }
    }


}

