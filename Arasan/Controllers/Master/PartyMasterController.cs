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

            }
            return View(ca);
        }
        [HttpPost]
        //public ActionResult PartyMaster(PartyMaster emp, string id)
        //{

        //    try
        //    {
        //        emp.ID = id;
        //        string Strout = PartyMasterService.PartyCRUD(emp);
        //        if (string.IsNullOrEmpty(Strout))
        //        {
        //            if (emp.ID == null)
        //            {
        //                TempData["notice"] = " PartyMaster Inserted Successfully...!";
        //            }
        //            else
        //            {
        //                TempData["notice"] = " PartyMaster Updated Successfully...!";
        //            }
        //            return RedirectToAction("ListParty");
        //        }

        //        else
        //        {
        //            ViewBag.PageTitle = "Edit PartyMaster";
        //            TempData["notice"] = Strout;
        //            //return View();
        //        }

        //        // }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return View(emp);
        //}



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
           // IEnumerable<PartyMaster> cmp = PartyMasterService.GetAllParty();
            return View();
        }
    }
}
