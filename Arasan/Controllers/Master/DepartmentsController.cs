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

namespace Arasan.Controllers
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

            Dp.createby = Request.Cookies["UserId"];

            if (id == null)
            {
               

            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = DepartmentService.GetDepartment(id);
                if (dt.Rows.Count > 0)
                {
                    Dp.Departmentcode = dt.Rows[0]["DEPARTMENT_CODE"].ToString();
                    Dp.DepartmentName = dt.Rows[0]["DEPARTMENT_NAME"].ToString();
                    Dp.Descrip = dt.Rows[0]["DESCRIPTION"].ToString();

                }

            }
            
            return View(Dp);
        }

        public IActionResult PDept(string id)
        {
            Department Dp = new Department();

            Dp.createby = Request.Cookies["UserId"];

            if (id == null)
            {


            }
            else
            {
                DataTable dt = new DataTable();
                double total = 0;
                dt = DepartmentService.GetPDepartment(id);
                if (dt.Rows.Count > 0)
                {
                    Dp.Pos = dt.Rows[0]["POSITION"].ToString();
                    Dp.DepartmentName = dt.Rows[0]["DEPARTMENT"].ToString();
                }

            }

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

        [HttpPost]
        public IActionResult PDept(Department Dp, string id)
        {
            try
            {
                Dp.ID = id;
                string Strout = DepartmentService.PDepartmentCRUD(Dp);
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
                    return RedirectToAction("ListPDept");
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
        public IActionResult ListPDept()
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

        public ActionResult MyListPDept(string strStatus)
        {
            List<Departmentgrid> Reg = new List<Departmentgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = DepartmentService.GetAllPDEPARTMENT(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=PDept?id=" + dtUsers.Rows[i]["PDEPTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeletePDept?tag=Del&id=" + dtUsers.Rows[i]["PDEPTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {
                    EditRow = "";
                    DeleteRow = "<a href=DeletePDept?tag=Del&id=" + dtUsers.Rows[i]["PDEPTID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";
                }

                Reg.Add(new Departmentgrid
                {
                    id = dtUsers.Rows[i]["PDEPTID"].ToString(),
                    pos = dtUsers.Rows[i]["POSITION"].ToString(),
                    departmentname = dtUsers.Rows[i]["DEPARTMENT"].ToString(),
                    //description = dtUsers.Rows[i]["DESCRIPTION"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Reg
            });

        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<Departmentgrid> Reg = new List<Departmentgrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = DepartmentService.GetAllDEPARTMENT(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=Departments?id=" + dtUsers.Rows[i]["DEPARTMENTMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["DEPARTMENTMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["DEPARTMENTMASTID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";

                }
                
                Reg.Add(new Departmentgrid
                {
                    id = dtUsers.Rows[i]["DEPARTMENTMASTID"].ToString(),
                    departmentcode = dtUsers.Rows[i]["DEPARTMENT_CODE"].ToString(),
                    departmentname = dtUsers.Rows[i]["DEPARTMENT_NAME"].ToString(),
                    //description = dtUsers.Rows[i]["DESCRIPTION"].ToString(),
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
