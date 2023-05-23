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
            Dp.CreatedBy = Request.Cookies["UserId"];
            Dp.UpdatedBy = Request.Cookies["UserId"];
            List<Designation> TData = new List<Designation>();
            Designation tda = new Designation();

            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new Designation();
                   
                    //tda.drumlst = Binddrum("","");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

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
                    Dp.Description = dt.Rows[0]["DESCRIPTION"].ToString();

                }
            }
            Dp.Designationlst = TData;
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
            IEnumerable<Department> cmp = DepartmentService.GetAllDepartment();
            return View(cmp);
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
        }

    }
}
