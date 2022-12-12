using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class SalesEnquiryController : Controller
    {
       
            ISalesEnq Sales;
            IConfiguration? _configuratio;
            private string? _connectionString;

            DataTransactions datatrans;
            public SalesEnquiryController(ISalesEnq _Sales, IConfiguration _configuratio)
            {
                Sales = _Sales;
                _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
                datatrans = new DataTransactions(_connectionString);
            }
            public IActionResult SalesEnquiry( )
            {
                SalesEnquiry ca = new SalesEnquiry();
                ca.Brlst = BindBranch();
                //ca.Suplst = BindSupplier();
                ca.Curlst = BindCurrency();
                //ca.Loclst = GetLoc();
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

            public List<SelectListItem> BindCurrency()
            {
                try
                {
                    DataTable dtDesg = datatrans.GetCurency();
                    List<SelectListItem> lstdesg = new List<SelectListItem>();
                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["Cur"].ToString(), Value = dtDesg.Rows[i]["CURRENCYID"].ToString() });
                    }
                    return lstdesg;
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            //public List<SelectListItem> BindSupplier()
            //{
            //    try
            //    {
            //        DataTable dtDesg = datatrans.GetSupplier();
            //        List<SelectListItem> lstdesg = new List<SelectListItem>();
            //        for (int i = 0; i < dtDesg.Rows.Count; i++)
            //        {
            //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTY"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
            //        }
            //        return lstdesg;
            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
           //// public List<SelectListItem> GetLoc()
           // {
           //     try
           //     {
           //         DataTable dtDesg = datatrans.GetLocation();


           //         List<SelectListItem> lstdesg = new List<SelectListItem>();
           //         for (int i = 0; i < dtDesg.Rows.Count; i++)
           //         {
           //             lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
           //         }
           //         return lstdesg;

           //     }
           //     catch (Exception ex)
           //     {
           //         throw ex;
           //     }
           // }

            public IActionResult Sales_Enquiry()
        {
            return View();
        }
        public IActionResult Sales_Quotation()
        {
            return View();
        }
        public IActionResult Proforma_Invoice()
        {
            return View();
        }
        public IActionResult Excise_Invoice()
        {
            return View();
        }
        public IActionResult Supplimantry_Invoice()
        {
            return View();
        }
        public IActionResult Depot_Invoice()
        {
            return View();
        }

        public IActionResult Work_Order()
        {
            return View();
        }
        public IActionResult Work_Order_Amedment()
        {
            return View();
        }
        public IActionResult Work_Orde_ShortClose()
        {
            return View();
        }
        public IActionResult Sales_Return()
        {
            return View();
        }
        public IActionResult Debit_Note_Bill()
        {
            return View();
        }
        public IActionResult Credit_Note_Bill()
        {
            return View();
        }
        public IActionResult Credit_Note_Approval()
        {
            return View();
        }
        public IActionResult Sales_Forecasting()
        {
            return View();
        }
    }
 }
