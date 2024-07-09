using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class CountryController : Controller
    {

        ICountryService CountryService;
        
        public CountryController(ICountryService _CountryService)
        {
            CountryService = _CountryService;
        }
        public IActionResult Country(string id)
        {
            Country ic = new Country();
            ic.createby = Request.Cookies["UserId"];
            ic.Cur = BindCurrency();

            CurItem pr = new CurItem();
            List<CurItem> TData = new List<CurItem>();

            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    pr = new CurItem();
                    pr.pur = BindState();
                    pr.Isvalid = "Y";
                    TData.Add(pr);
                }
            }
            else  
            {
                DataTable dt = new DataTable();

                //double total = 0;
                dt = CountryService.GetEditCountDetail(id);
                if (dt.Rows.Count > 0)
                {
                    ic.ID = dt.Rows[0]["CONMASTID"].ToString();
                    ic.ConName = dt.Rows[0]["COUNTRY"].ToString();
                    ic.ConCode = dt.Rows[0]["CONCODE"].ToString();
                    ic.Curr = dt.Rows[0]["CURRENCY"].ToString();                  

                }

            }
            DataTable dt2 = new DataTable();
            dt2 = CountryService.GetEditPortDetail(id);
            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    pr = new CurItem();

                    pr.pcode = dt2.Rows[i]["PORTC"].ToString();
                    pr.pnum = dt2.Rows[i]["PORTN"].ToString();
                    pr.ppin = dt2.Rows[i]["PPINC"].ToString();
                    pr.psta = dt2.Rows[i]["PORTS"].ToString();
                    pr.Isvalid = "Y";
                    TData.Add(pr);
                }
            }

        
            ic.Curlst = TData;
            return View(ic);
        }

        public JsonResult GetItemJSON()
        {
            CurItem model = new CurItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindState());

        }
        public List<SelectListItem> BindCurrency()
        {
            try
            {
                DataTable dtDesg = CountryService.GetCur();
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

        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = CountryService.GetSta();
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
        [HttpPost]
        public ActionResult Country(Country Ic, string id)
        {

            try
            {
                Ic.ID = id;
                string Strout = CountryService.CountryCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "Country Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Country Updated Successfully...!";
                    }
                    return RedirectToAction("ListCountry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Country";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Ic);
        }

        public IActionResult ListCountry()
        {
            return View();
        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = CountryService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCountry");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCountry");
            }
        }
        public ActionResult Remove(string tag, string id)
        {

            string flag = CountryService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListCountry");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListCountry");
            }
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Countrygrid> Reg = new List<Countrygrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = CountryService.GetAllCountryGRID(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=Country?id=" + dtUsers.Rows[i]["CONMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["CONMASTID"].ToString() + "";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["CONMASTID"].ToString() + "";
                }

               
                Reg.Add(new Countrygrid
                {
                    id = dtUsers.Rows[i]["CONMASTID"].ToString(),
                    coname = dtUsers.Rows[i]["COUNTRY"].ToString(),
                    concode = dtUsers.Rows[i]["CONCODE"].ToString(),
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
