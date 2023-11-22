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
    public class DepartmentsController : Controller
    {
        IDepartment DepartmentService;
        public DepartmentsController(IDepartment _DepartmentService)
        {
            DepartmentService = _DepartmentService;
        }
        public IActionResult Departments(string id)
        {
            Department Dp = new Department();
            //Dp.CreatedBy = Request.Cookies["UserId"];
            //Dp.UpdatedBy = Request.Cookies["UserId"];

            //List<Designation> TData = new List<Designation>();
            //Designation tda = new Designation();

            if (id == null)
            {
                //for (int i = 0; i < 3; i++)
                //{
                //    tda = new Designation();
                   
                //    
                //    tda.Isvalid = "Y";
                //    TData.Add(tda);
                //}

            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = DepartmentService.GetDepartment(id);
                if (dt.Rows.Count > 0)
                {
                    Dp.DepartmentCode = dt.Rows[0]["DEPARTMENT_CODE"].ToString();
                    Dp.DepartmentName = dt.Rows[0]["DEPARTMENT_NAME"].ToString();
                    Dp.Description = dt.Rows[0]["DESCRIPTION"].ToString();

                }

                //DataTable dt2 = new DataTable();

                //dt2 = DepartmentService.GetDepartmentDetail(id);
                //if (dt2.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt2.Rows.Count; i++)
                //    {
                //        tda = new Designation();

                //        tda.Design = dt2.Rows[i]["DESIGNATION"].ToString();
                //        tda.ID = id;
                //        TData.Add(tda);
                //    }

                //}
            }
            //Dp.Designationlst = TData;
            return View(Dp);
        }
        
        [HttpPost]
        public IActionResult Departments(Department Dp, string id)
        {
            try
            {
                Dp.ID = id;
                string Strout = DepartmentService.DepartmentCRUD(Dp);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Dp.ID == null)
                    {
                        TempData["notice"] = "Department Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Department Updated Successfully...!";
                    }
                    return RedirectToAction("ListDepartment");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Departments";
                    TempData["notice"] = Strout;
                    //return View();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }



            return View(Dp);
        }

        public IActionResult ListDepartment()
        {
            
            return View();
        }


        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = DepartmentService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDepartment");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDepartment");
            }
        }public ActionResult Remove(string tag, int id)
        {

            string flag = DepartmentService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListDepartment");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListDepartment");
            }
        }
        //public ActionResult MyListItemgrid(string strStatus)
        //{
        //    List<Citygrid> Reg = new List<Citygrid>();
        //    DataTable dtUsers = new DataTable();
        //    strStatus = strStatus == "" ? "Y" : strStatus;
        //    dtUsers = city.GetAllCitys(strStatus);
        //    for (int i = 0; i < dtUsers.Rows.Count; i++)
        //    {

        //        string DeleteRow = string.Empty;
        //        string EditRow = string.Empty;

        //        EditRow = "<a href=city?id=" + dtUsers.Rows[i]["CITYID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
        //        DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["CITYID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

        //        Reg.Add(new Citygrid
        //        {
        //            id = dtUsers.Rows[i]["CITYID"].ToString(),
        //            countryid = dtUsers.Rows[i]["COUNTRY"].ToString(),
        //            state = dtUsers.Rows[i]["STATEID"].ToString(),
        //            cit = dtUsers.Rows[i]["CITYNAME"].ToString(),
        //            editrow = EditRow,
        //            delrow = DeleteRow,

        //        });
        //    }

        //    return Json(new
        //    {
        //        Reg
        //    });

        //}
    }
}
