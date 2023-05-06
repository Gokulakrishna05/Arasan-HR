using System.Collections.Generic;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers
{
    public class CuringController : Controller
    {

        ICuringService CuringService;
        public CuringController(ICuringService _CuringService)
        {
            CuringService = _CuringService;
        }
        public IActionResult Curing(string id)
        {
            Curing ic = new Curing();
            ic.Cur = BindCuring();
            //ic.STypelst = BindSType();
            //ic.statuslst = BindStatus();
            ic.Sublst = BindSubgroup();
            if (id == null)
            {

            }
            else
            {
                ic = CuringService.GetCuringById(id);

            }
            return View(ic);
        }
        [HttpPost]
        public ActionResult Curing(Curing Ic, string id)
        {

            try
            {
                Ic.ID = id;
                string Strout = CuringService.CuringCRUD(Ic);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Ic.ID == null)
                    {
                        TempData["notice"] = "Curing Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Curing Updated Successfully...!";
                    }
                    return RedirectToAction("ListCuring");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Curing";
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
        public IActionResult ListCuring()
        {
            IEnumerable<Curing> ic = CuringService.GetAllCuring();
            return View(ic);
        }
        public IActionResult Status(string id)
        {
            Curing ic = new Curing();
            ic.statuslst = BindStatus();
            DataTable dt = new DataTable();
            dt = CuringService.GetCuringDeatil(id);
            if (dt.Rows.Count > 0)
            {
                ic.Location = dt.Rows[0]["LOCID"].ToString();
                ic.Sub = dt.Rows[0]["SUBGROUP"].ToString();
                ic.Shed = dt.Rows[0]["SHEDNUMBER"].ToString();
                ic.Cap = dt.Rows[0]["CAPACITY"].ToString();
                ic.ID = id;
            }
            return View(ic);
        }
        public List<SelectListItem> BindCuring()
        {
            try
            {
                DataTable dtDesg = CuringService.GetCuring();
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
        //public List<SelectListItem> BindSType()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        lstdesg.Add(new SelectListItem() { Text = "Curing Set 1", Value = "ADD1" });
        //        lstdesg.Add(new SelectListItem() { Text = "Curing Set 2", Value = "ADD2" });
        //        lstdesg.Add(new SelectListItem() { Text = "Curing Set 3", Value = "ADD3" });
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> BindStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Occupied", Value = "Occupied" });
                lstdesg.Add(new SelectListItem() { Text = "blocked", Value = "blocked" });
                lstdesg.Add(new SelectListItem() { Text = "Active", Value = "Active" });
                lstdesg.Add(new SelectListItem() { Text = "Waiting", Value = "Waiting" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindSubgroup()
        {
            try
            {
                DataTable dtDesg = CuringService.GetSubgroup();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SUBGROUP"].ToString(), Value = dtDesg.Rows[i]["CURINGMASTERID"].ToString() });
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
