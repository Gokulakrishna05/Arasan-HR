using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Arasan.Models;
using Arasan.Services;

using Arasan.Interface;
using Arasan.Services;
namespace Arasan.Controllers
{
    public class PayTransactionController : Controller
    {
        IPayTransaction payTransaction;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public PayTransactionController(IPayTransaction _payTransaction, IConfiguration _configuratio)
        {
            payTransaction = _payTransaction;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        [HttpPost]

        public ActionResult PayTransaction(PayTransaction Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = payTransaction.PayTransactionCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "Pay Transaction Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Pay Transaction Updated Successfully...!";
                    }
                    return RedirectToAction("ListPayTransaction");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Pay Transaction";
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

        public IActionResult PayTransaction(string id)
        {
            PayTransaction ca = new PayTransaction();
            List<PayTra> TData = new List<PayTra>();
            PayTra tda = new PayTra();
            ca.Brchlst = BindBranch();
            ca.PayPerlst = BindPayPeriod();
            ca.PayCodlst = BindPayCode();
            ca.PayCatlst = BindPayCat();


            DataTable dtv = datatrans.GetSequence("pcat");

            if (dtv.Rows.Count > 0)
            {
                ca.DocID = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new PayTra();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();

                //double total = 0;
                dt = payTransaction.GetEditPayTra(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Brc = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Brchlst = BindBranch();
                    ca.DocID = dt.Rows[0]["DOCID"].ToString();
                    ca.PayCod = dt.Rows[0]["PAYCODE"].ToString();
                    ca.PayCodlst = BindPayCode();
                    ca.PayPer = dt.Rows[0]["PAYPERIOD"].ToString();
                    ca.PayPerlst = BindPayPeriod();
                    ca.PayCat = dt.Rows[0]["PAYCATEGORY"].ToString();
                    ca.PayCatlst = BindPayCat();
                    ca.ID = id;
                }
            }
            DataTable dt2 = new DataTable();
            dt2 = payTransaction.GetEditEPayTra(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new PayTra();

                    tda.eid = dt2.Rows[i]["EMPID"].ToString();
                    tda.ename = dt2.Rows[i]["EMPNAME"].ToString();
                    tda.amo = dt2.Rows[i]["AMOUNT"].ToString();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            ca.BrLst = TData;
            return View(ca);
        }
       
        

       
        public List<SelectListItem> BindPayPeriod()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "JAN2023", Value = "JAN2023" });
                lstdesg.Add(new SelectListItem() { Text = "FEB2023", Value = "FEB2023" });
                lstdesg.Add(new SelectListItem() { Text = "MAR2023", Value = "MAR2023" });
                lstdesg.Add(new SelectListItem() { Text = "APR2023", Value = "APR2023" });
                lstdesg.Add(new SelectListItem() { Text = "MAY2023", Value = "MAY2023" });
                lstdesg.Add(new SelectListItem() { Text = "JUN2023", Value = "JUN2023" });
                lstdesg.Add(new SelectListItem() { Text = "JUL2023", Value = "JUL2023" });
                lstdesg.Add(new SelectListItem() { Text = "AUG2023", Value = "AUG2023" });
                lstdesg.Add(new SelectListItem() { Text = "SEP2023", Value = "SEP2023" });
                lstdesg.Add(new SelectListItem() { Text = "OCT2023", Value = "OCT2023" });
                lstdesg.Add(new SelectListItem() { Text = "NOV2023", Value = "NOV2023" });
                lstdesg.Add(new SelectListItem() { Text = "DEC2023", Value = "DEC2023" });
                lstdesg.Add(new SelectListItem() { Text = "JAN2024", Value = "JAN2024" });
                lstdesg.Add(new SelectListItem() { Text = "FEB2024", Value = "FEB2024" });
                lstdesg.Add(new SelectListItem() { Text = "MAR2024", Value = "MAR2024" });
                lstdesg.Add(new SelectListItem() { Text = "APR2024", Value = "APR2024" });
                lstdesg.Add(new SelectListItem() { Text = "MAY2024", Value = "MAY2024" });
                lstdesg.Add(new SelectListItem() { Text = "JUN2024", Value = "JUN2024" });
                lstdesg.Add(new SelectListItem() { Text = "JUL2024", Value = "JUL2024" });
                lstdesg.Add(new SelectListItem() { Text = "AUG2024", Value = "AUG2024" });
                lstdesg.Add(new SelectListItem() { Text = "SEP2024", Value = "SEP2024" });
                lstdesg.Add(new SelectListItem() { Text = "OCT2024", Value = "OCT2024" });
                lstdesg.Add(new SelectListItem() { Text = "NOV2024", Value = "NOV2024" });
                lstdesg.Add(new SelectListItem() { Text = "DEC2024", Value = "DEC2024" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindPayCat()
        {
            try
            {
                DataTable dtDesg = payTransaction.GetPayCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PAYCATEGORY"].ToString(), Value = dtDesg.Rows[i]["PCBASICID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindPayCode()
        {
            try
            {
                DataTable dtDesg = payTransaction.GetPayCode();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PAYCODE"].ToString(), Value = dtDesg.Rows[i]["PARAMETERDETAILID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = payTransaction.GetBranch();
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
        public IActionResult ListPayTransaction()
        {
            return View();
        }

        public IActionResult PayTraSelection(string id)
        {
            PayTransaction ca = new PayTransaction();
            List<PayTraVList> TData = new List<PayTraVList>();
            PayTraVList tda = new PayTraVList();
            DataTable dt = new DataTable();
            dt = payTransaction.getPayTra(id);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tda = new PayTraVList();
                    tda.empid = dt.Rows[i]["EMPID"].ToString();
                    tda.empname = dt.Rows[i]["EMPNAME"].ToString();
                    tda.dtid = dt.Rows[i]["EMPMASTID"].ToString();
                    TData.Add(tda);
                }
            }
            ca.ptrlst = TData;
            return View(ca);
        }

        public JsonResult GetIndentDetail(string indentid)
        {
            PayTransaction ca = new PayTransaction();
            List<PayTraVList> TData = new List<PayTraVList>();
            PayTraVList tda = new PayTraVList();
            DataTable dt = new DataTable();
            dt = payTransaction.getPayTraId(indentid);
            if (dt.Rows.Count > 0)
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    tda = new PayTraVList();
                    tda.empid = dt.Rows[i]["EMPID"].ToString();
                    tda.empname = dt.Rows[i]["EMPNAME"].ToString();
                    tda.dtid = dt.Rows[i]["EMPMASTID"].ToString();
                    TData.Add(tda);
                }
            }
            ca.ptrlst = TData;
            return Json(ca.ptrlst);
        }

        public ActionResult MyListPayTransactiongrid(string strStatus)
        {
            List<ListPayTransaction> Reg = new List<ListPayTransaction>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = payTransaction.GetAllPayTransaction(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {
                    EditRow = "<a href=PayTransaction?id=" + dtUsers.Rows[i]["APBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["APBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["APBASICID"].ToString() + "";
                }

                Reg.Add(new ListPayTransaction
                {
                    brc = dtUsers.Rows[i]["BRANCHID"].ToString(),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    paycod = dtUsers.Rows[i]["PAYCODE"].ToString(),
                    payper = dtUsers.Rows[i]["PAYPERIOD"].ToString(),
                    paycat = dtUsers.Rows[i]["PAYCATEGORY"].ToString(),
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

            string flag = payTransaction.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPayTransaction");
            }
            else
            {
                TempData["notice"] = flag;

                return RedirectToAction("ListPayTransaction");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = payTransaction.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListPayTransaction");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListPayTransaction");
            }
        }
    }


}
