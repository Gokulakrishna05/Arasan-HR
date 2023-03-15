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
            ic.STypelst = BindSType();
            ic.statuslst = BindStatus();
            //ic.Sublst = BindSubgroup();
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
        public List<SelectListItem> BindSType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "ADD1", Value = "ADD1" });
                lstdesg.Add(new SelectListItem() { Text = "ADD2", Value = "ADD2" });
                lstdesg.Add(new SelectListItem() { Text = "ADD3", Value = "ADD3" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "PENDING", Value = "PENDING" });
                lstdesg.Add(new SelectListItem() { Text = "COMPLETED", Value = "COMPLETED" });
                lstdesg.Add(new SelectListItem() { Text = "APPROVED", Value = "APPROVED" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<SelectListItem> BindSubgroup()
        //{
        //    try
        //    {
        //        DataTable dtDesg = CuringService.GetSubgroup();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SUBGROUP"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
    }
}
