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
            //List<DirItem> TData = new List<DirItem>();
            //DirItem tda = new DirItem();
            if (id == null)
            {
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);

                DataTable dt = new DataTable();
                double total = 0;
                dt = PartyMasterService.GetParty(id);
                if (dt.Rows.Count > 0)
                {
                    ca.PartyCode = dt.Rows[0]["PARTYID"].ToString();
                    ca.PartyName = dt.Rows[0]["PARTYNAME"].ToString();
                    ca.PartyCategory = dt.Rows[0]["PARTYCAT"].ToString();
                    ca.PartyType = dt.Rows[0]["PARTYTYPE"].ToString();
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

                }
            }
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



        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = PartyMasterService.GetState();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE"].ToString(), Value = dtDesg.Rows[i]["STATEMASTID"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITYNAME"].ToString(), Value = dtDesg.Rows[i]["CITYID"].ToString() });
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
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRYNAME"].ToString(), Value = dtDesg.Rows[i]["COUNTRYMASTID"].ToString() });
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
    }
}
