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
        DataTransactions datatrans;
        IConfiguration? _configuratio;
        private string? _connectionString;
        public DepartmentsController(IDepartment _DepartmentService, IConfiguration _configuratio)
        {
            DepartmentService = _DepartmentService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Departments(string id)
        {
            Department Dp = new Department();
            List<Designationdet> TData = new List<Designationdet>();
            Designationdet tda = new Designationdet();
            Dp.createby = Request.Cookies["UserName"];

            if (id == null)
            {

                for (int i = 0; i < 1; i++)
                {
                    tda = new Designationdet();
                    tda.deslst = BindDesig();
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
                    Dp.Departmentcode = dt.Rows[0]["DEPTCODE"].ToString();
                    Dp.DepartmentName = dt.Rows[0]["DEPTNAME"].ToString();
                    Dp.Descrip = dt.Rows[0]["USERID"].ToString();

                }
                DataTable dt2 = new DataTable();
                dt2 = datatrans.GetData("SELECT DESIGNATION FROM DDDETAIL WHERE DDBASICID='"+ id +"'");
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new Designationdet();
                        tda.deslst = BindDesig();
                        tda.designation = dt2.Rows[i]["DESIGNATION"].ToString();
                       
                        tda.Isvalid = "Y";
                        TData.Add(tda);


                    }
                }

            }
            Dp.Designationlst = TData;
            return View(Dp);
        }
        public List<SelectListItem> BindDesig()
        {
            try
            {
                DataTable dtDesg = DepartmentService.GetDesign();// datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='DESIGNATION'");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COMMON_VALUE"].ToString(), Value = dtDesg.Rows[i]["COMMON_VALUE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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
        
        public ActionResult DeleteMR(string tag, string id)
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
        }public ActionResult Remove(string tag, string id)
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

                    EditRow = "<a href=Departments?id=" + dtUsers.Rows[i]["DDBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = " DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["DDBASICID"].ToString() + "";
                }
                else
                {

                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["DDBASICID"].ToString() + " ";

                }
                
                Reg.Add(new Departmentgrid
                {
                    id = dtUsers.Rows[i]["DDBASICID"].ToString(),
                    departmentcode = dtUsers.Rows[i]["DEPTCODE"].ToString(),
                    departmentname = dtUsers.Rows[i]["DEPTNAME"].ToString(),
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
        public JsonResult GetDesigJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindDesig());
        }
    }
}
