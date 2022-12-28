using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;

namespace Arasan.Controllers.Master
{
    public class PartyMasterController : Controller
    {

        IPartyMasterService PartyMasterService;
       
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PartyMasterController(IPartyMasterService _PartyMasterService, IConfiguration _configuratio)
        {
            PartyMasterService = _PartyMasterService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PartyMaster(string id)
        {
            PartyMaster ca = new PartyMaster();
            ca.Countrylst = BindCountry();
            ca.Statelst = BindState();
            ca.Citylst = BindCity();
            ca.assignList = BindEmp();
            ca.Categorylst = BindCategory();
            //List<PartyItem> TData = new List<PartyItem>();
            //PartyItem tda = new PartyItem();
            if (id == null)
            {
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);

                DataTable dt = new DataTable();

                dt = PartyMasterService.GetParty(id);
                if (dt.Rows.Count > 0)
                {
                    ca.PartyCode = dt.Rows[0]["PARTYID"].ToString();
                    ca.PartyName = dt.Rows[0]["PARTYNAME"].ToString();
                    ca.PartyCategory = dt.Rows[0]["PARTYCAT"].ToString();
                    ca.PartyType = dt.Rows[0]["TYPE"].ToString();
                    ca.ID = id;
                    ca.ConPartyID = dt.Rows[0]["CSGNPARTYID"].ToString();
                    ca.Comm = dt.Rows[0]["COMMCODE"].ToString();
                    ca.CreditLimit = dt.Rows[0]["CREDITLIMIT"].ToString();
                    ca.CreditDate = dt.Rows[0]["CREDITDAYS"].ToString();
                    ca.TransationLimit = dt.Rows[0]["TRANSLMT"].ToString();
                    ca.RateCode = dt.Rows[0]["RATECODE"].ToString();
                    ca.Regular = dt.Rows[0]["REGULARYN"].ToString();
                    ca.AccName = dt.Rows[0]["ACCOUNTNAME"].ToString();
                    ca.Active = dt.Rows[0]["ACTIVE"].ToString();
                    ca.GST = dt.Rows[0]["GSTNO"].ToString();
                    ca.PartyGroup = dt.Rows[0]["PARTYGROUP"].ToString();
                    ca.SectionID = dt.Rows[0]["SECTIONID"].ToString();
                    ca.LUTDate = dt.Rows[0]["LUTDT"].ToString();
                    ca.JoinDate = dt.Rows[0]["PJOINDATE"].ToString();
                    ca.LUTNumber = dt.Rows[0]["LUTNO"].ToString();
                    ca.Mobile = dt.Rows[0]["MOBILE"].ToString();
                    ca.Phone = dt.Rows[0]["PHONENO"].ToString();
                    ca.PanNumber = dt.Rows[0]["PANNO"].ToString();
                    ca.City = dt.Rows[0]["CITY"].ToString();
                    ca.State = dt.Rows[0]["STATE"].ToString();
                    ca.Country = dt.Rows[0]["COUNTRY"].ToString();
                    ca.Pincode = dt.Rows[0]["PINCODE"].ToString();
                    ca.CountryCode = dt.Rows[0]["COUNTRYCODE"].ToString();
                    ca.Email = dt.Rows[0]["EMAIL"].ToString();
                    ca.Fax = dt.Rows[0]["FAX"].ToString();
                    ca.Commisionerate = dt.Rows[0]["COMMISIONERATE"].ToString();
                    ca.Range = dt.Rows[0]["RANGEDIVISION"].ToString();
                    ca.EccID = dt.Rows[0]["ECCNO"].ToString();
                    ca.Excise = dt.Rows[0]["EXCISEAPPLICABLE"].ToString();
                    ca.Type = dt.Rows[0]["PARTYTYPE"].ToString();
                    ca.Http = dt.Rows[0]["HTTP"].ToString();
                    ca.OverDueInterest = dt.Rows[0]["OVERDUEINTEREST"].ToString();
                    ca.Address = dt.Rows[0]["ADD1"].ToString();
                    ca.Remark = dt.Rows[0]["REMARKS"].ToString();
                    ca.Intred = dt.Rows[0]["INTRODUCEDBY"].ToString();

                }
                DataTable dt2 = new DataTable();
                dt2 = PartyMasterService.GetPartyContact(id);
                if (dt2.Rows.Count > 0)
                { 
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {


                        ca.Purpose = dt2.Rows[0]["CONTACTPURPOSE"].ToString();
                        ca.ContactPerson = dt2.Rows[0]["CONTACTNAME"].ToString();
                        ca.Designation = dt2.Rows[0]["CONTACTDESIG"].ToString();
                        ca.CPhone = dt2.Rows[0]["CONTACTPHONE"].ToString();
                        ca.CEmail = dt2.Rows[0]["CONTACTEMAIL"].ToString();

                    }
                }
            }
                    //ca.PartyLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult PartyMaster(PartyMaster emp, string id)    
        {  

            try
            {
                emp.ID = id;
                string Strout = PartyMasterService.PartyCRUD(emp);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (emp.ID == null)
                    {
                        TempData["notice"] = " PartyMaster Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " PartyMaster Updated Successfully...!";
                    }
                    return RedirectToAction("ListParty");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PartyMaster";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(emp);
        }


        public List<SelectListItem> BindCategory()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "CUSTOMER", Value = "CUSTOMER" });
                lstdesg.Add(new SelectListItem() { Text = "SUPPLIER", Value = "SUPPLIER" });
                lstdesg.Add(new SelectListItem() { Text = "BOTH", Value = "BOTH" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = PartyMasterService.GetState();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE"].ToString(), Value = dtDesg.Rows[i]["STATE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCity()
        {
            try
            {
                DataTable dtDesg = PartyMasterService.GetCity();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITYNAME"].ToString(), Value = dtDesg.Rows[i]["CITYNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = PartyMasterService.GetCountry();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRYNAME"].ToString(), Value = dtDesg.Rows[i]["COUNTRYNAME"].ToString() });
                }
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
        public IActionResult ListParty()
        {
           IEnumerable<PartyMaster> cmp = PartyMasterService.GetAllParty();
            return View(cmp);
        }
        public JsonResult GetItemGrpJSON()
        {
            PartyMaster model = new PartyMaster();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(model);
        }
        public ActionResult GetCountryDetail(string CID)
        {
            try
            {
                DataTable dt = new DataTable();
               
                string country = "";
                
                dt = PartyMasterService.GetCountryDetails(CID);

                if (dt.Rows.Count > 0)
                {

                    country= dt.Rows[0]["COUNTRYCODE"].ToString();
                   
                  
                }

                var result = new { country = country };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
