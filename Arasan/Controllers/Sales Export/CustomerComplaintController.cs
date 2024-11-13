using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Arasan.Services;
using AspNetCore.Reporting;
using Intuit.Ipp.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Sales_Export
{
    public class CustomerComplaintController : Controller
    {
        ICustomerComplaint CustomerComplaint;
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public CustomerComplaintController(ICustomerComplaint _CustomerComplaint, IConfiguration _configuratio)
        {

            CustomerComplaint = _CustomerComplaint;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Customer_Complaint(string id)
        {
            CustomerComplaint ca = new CustomerComplaint();

            ca.CcirintName = BindEmp();
            ca.InvestigatedBy = BindEmp();
            ca.CcirName = BindEmp();
            ca.DisInvestigatedBy = BindEmp();
            ca.ReviewBy = BindEmp();
            ca.ComplaintDate = DateTime.Now.ToString("dd-MMM-yyyy");
           
            DataTable dtv = datatrans.GetSequence("vchsl");
            if (dtv.Rows.Count > 0)
            {
                ca.ComplaintNo = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            //ca.Assign = Request.Cookies["UserId"];

            if (id == null)
            {
                
            }
            else
            {

            }
           
            return View(ca);
        }
        [HttpPost]
        public ActionResult Customer_Complaint(CustomerComplaint Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = CustomerComplaint.CustomerComplaintCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "CustomerComplaint Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "CustomerComplaint Updated Successfully...!";
                    }
                    return RedirectToAction("ListCustomerComplaint");
                }

                else
                {
                    ViewBag.PageTitle = "Edit CustomerComplaint";
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
        public IActionResult ListCustomerComplaint()
        {
            return View();
        }
        public ActionResult MyListCustomerComplaintGrid(string strStatus)
        {
            List<CustomerComplaintItems> Reg = new List<CustomerComplaintItems>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)CustomerComplaint.GetAllListCustomerComplaint (strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string SendMail = string.Empty;
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                string DeleteRow = string.Empty;

                if (dtUsers.Rows[i]["STATUS"].ToString() == "Y")
                {
                    EditRow = "<a href=Customer_Complaint?id=" + dtUsers.Rows[i]["CMPLBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    ViewRow = "<a href=ViewCustomerComplaint?id=" + dtUsers.Rows[i]["CMPLBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='View' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["CMPLBASICID"].ToString() + "";

                }
                else
                {
                    DeleteRow = "DeleteMR?tag=Active&id=" + dtUsers.Rows[i]["CMPLBASICID"].ToString() + "";
                }
                Reg.Add(new CustomerComplaintItems
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["CMPLBASICID"].ToString()),
                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    ccno = dtUsers.Rows[i]["CCIRNO"].ToString(),
                    view = ViewRow,
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
            string flag = "";
            if (tag == "Del")
            {
                flag = CustomerComplaint.StatusChange(tag, id);
            }
            else
            {
                flag = CustomerComplaint.ActStatusChange(tag, id);
            }

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCustomerComplaint");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCustomerComplaint");
            }
        }
    }
}
