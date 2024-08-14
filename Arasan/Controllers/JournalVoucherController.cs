using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Reflection.PortableExecutable;
using Intuit.Ipp.Data;
using Arasan.Services;
using DocumentFormat.OpenXml.Math;

namespace Arasan.Controllers
{
    public class JournalVoucherController : Controller

    {
        IJournalVoucher Journal;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public JournalVoucherController(IJournalVoucher _Journal, IConfiguration _configuratio)
        {
            Journal = _Journal;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult JournalVoucher(String id)
        {
            JournalVoucherModel J = new JournalVoucherModel();

            J.LocLst = BindLoc();
            J.VTypeLst = BindVType();
            J.CurLst = BindCurrency();


            J.Branch = Request.Cookies["BranchId"];
            J.SecIDLst = BindSection();
            J.PartyIDLst = BindParty();
            J.VocDate = DateTime.Now.ToString("dd-MON-yyyy");
 

            List<Journal> TData = new List<Journal>();
            Journal tda = new Journal();


            DataTable dtv = datatrans.GetSequence("vchjm");//JV-F
            if (dtv.Rows.Count > 0)
            {
                J.VocNo = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)

            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new Journal();
                    tda.DBCRlst = BindDBCR();
                    tda.AccNamelst = BindAccName();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }
            else
            {
                DataTable dt = new DataTable();

               
            }
            J.JournalVoucherlist = TData;

            return View(J);
        }


        public List<SelectListItem> BindSection()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "COMMISSION", Value = "COMMISSION" });
                lstdesg.Add(new SelectListItem() { Text = "CONTRACT", Value = "CONTRACT" });
                lstdesg.Add(new SelectListItem() { Text = "INTEREST", Value = "INTEREST" });
                lstdesg.Add(new SelectListItem() { Text = "PROFESSIONAL", Value = "PROFESSIONAL" });
              
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetDebJSON()
        {
            Journal model = new Journal();
            return Json(BindDBCR());

        }
        public JsonResult GetAccJSON()
        {
            Journal model = new Journal();
            return Json(BindAccName());

        }



        public List<SelectListItem> BindLoc()
        {
            try
            {
                DataTable dtDesg = Journal.GetLocation();
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
        public List<SelectListItem> BindVType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
              
                lstdesg.Add(new SelectListItem() { Text = "PAP", Value = "PAP" });
                lstdesg.Add(new SelectListItem() { Text = "PPY", Value = "PPY" });
                lstdesg.Add(new SelectListItem() { Text = "PPA", Value = "PPA" });
                lstdesg.Add(new SelectListItem() { Text = "PPG", Value = "PPG" });
                lstdesg.Add(new SelectListItem() { Text = "RMC", Value = "RMC" });
                lstdesg.Add(new SelectListItem() { Text = "BLD", Value = "BLD" });
                lstdesg.Add(new SelectListItem() { Text = "IA", Value = "IA" });
              
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
                DataTable dtDesg = Journal.GetCurrency();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MAINCURR"].ToString(), Value = dtDesg.Rows[i]["CURRENCYID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindDBCR()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Dr", Value = "Dr" });
                lstdesg.Add(new SelectListItem() { Text = "Cr", Value = "Cr" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindAccName()
        {
            try
            {
                DataTable dtDesg = Journal.GetAcc();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MNAME"].ToString(), Value = dtDesg.Rows[i]["MASTERID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public ActionResult GetCurDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string emp = "";
                string dep = "";

                dt = datatrans.GetData("SELECT EXRATE FROM CRATE WHERE CURRENCYID='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {

                    emp = dt.Rows[0]["EXRATE"].ToString();

                }

                var result = new { emp = emp };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public List<SelectListItem> BindParty()
        {
            try
            {
                DataTable dtDesg = Journal.GetParty();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYID"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });


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
