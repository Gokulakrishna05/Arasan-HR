using System.Collections.Generic;
using System.Data;
using System.Runtime.Intrinsics.Arm;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{

    public class DesignationController : Controller
    {
        IDesignation DesignationService;
        public DesignationController(IDesignation _DesignationService)
        {
            DesignationService = _DesignationService;
        }
        public IActionResult Designation(string id)
        {
            Designation Dp = new Designation();

            Dp.DeptNamelst = BindDeptName();
            if (id == null)
            {
                
            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = DesignationService.GetDesignation(id);
                if (dt.Rows.Count > 0)
                {
                    
                    Dp.Design = dt.Rows[0]["DESIGNATION"].ToString();
                    Dp.DeptName = dt.Rows[0]["DEPARTMENT_NAME"].ToString();

                }

            }
            return View(Dp);
        }

        [HttpPost]
        public IActionResult Designation(Designation Dp, string id)
        {
            try
            {
                Dp.ID = id;
                string Strout = DesignationService.DesignationCRUD(Dp);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Dp.ID == null)
                    {
                        TempData["notice"] = "Designation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Designation Updated Successfully...!";
                    }
                    return RedirectToAction("ListDesignation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Designation";
                    TempData["notice"] = Strout;
                   
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }



            return View(Dp);
        }
        public List<SelectListItem> BindDeptName()
        {
            try
            {
                DataTable dtDesg = DesignationService.GetDeptName();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DEPARTMENT_NAME"].ToString(), Value = dtDesg.Rows[i]["DEPARTMENT_NAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ListDesignation(string status)
        {
            IEnumerable<Designation> cmp = DesignationService.GetAllDesignation(status);
            return View(cmp);
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = DesignationService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDesignation");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDesignation");
            }
        }   public ActionResult Remove(string tag, int id)
        {

            string flag = DesignationService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDesignation");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDesignation");
            }
        }
    }
}
