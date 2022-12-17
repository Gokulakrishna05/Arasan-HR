using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class EmployeeController : Controller
    {
        IEmployee EmployeeService;
        public EmployeeController(IEmployee _EmployeeService)
        {
            EmployeeService = _EmployeeService;
        }
        public IActionResult Employee(string id)
        {
            Employee E = new Employee();
            E.Statelst = BindState();
            E.Citylst = BindCity();
            //List<EduDeatils> TData = new List<EduDeatils>();
            //EduDeatils tda = new EduDeatils();
            if (id == null)
            {

            }
            else
            {
                //E = EmployeeService.GetCityById(id);

                DataTable dt = new DataTable();
                double total = 0;
                dt = EmployeeService.GetEmployee(id);
                if (dt.Rows.Count > 0)
                {
                    E.EmpName = dt.Rows[0]["EMPNAME"].ToString();
                    E.EmpNo = dt.Rows[0]["EMPID"].ToString();
                    E.Gender = dt.Rows[0]["EMPSEX"].ToString();
                    E.DOB = dt.Rows[0]["EMPDOB"].ToString();
                    E.ID = id;
                    E.Address = dt.Rows[0]["ECADD1"].ToString();
                    E.CityId = dt.Rows[0]["ECCITY"].ToString();
                    E.StateId = dt.Rows[0]["ECSTATE"].ToString();
                    E.EmailId = dt.Rows[0]["ECMAILID"].ToString();
                    E.PhoneNo = dt.Rows[0]["ECPHNO"].ToString();
                    E.FatherName = dt.Rows[0]["FATHERNAME"].ToString();
                    E.MotherName = dt.Rows[0]["MOTHERNAME"].ToString();

                }
                //DataTable dt2 = new DataTable();
                //dt2 = EmployeeService.GetEmpEduDeatils(id);
                //if (dt2.Rows.Count > 0)
                //{
                //    for (int i = 0; i < dt2.Rows.Count; i++)
                //    {
                //        tda = new EduDeatils();
                //        tda.Education = dt.Rows[0]["EDUCATION"].ToString();
                //        tda.College = dt.Rows[0]["UC"].ToString();
                //        tda.EcPlace = dt.Rows[0]["ECPLACE"].ToString();
                //        tda.MPercentage = Convert.ToDouble(dt2.Rows[i]["MPER"].ToString());
                //        tda.YearPassing = dt.Rows[0]["YRPASSING"].ToString();
                       
                //        TData.Add(tda);
                //    }
                //}
                //ca.net = Math.Round(total, 2);

            }
            //E.EduLst = TData;
            return View(E);

        }
        [HttpPost]
        public ActionResult Employee(Employee emp, string id)
        {

            try
            {
               emp.ID = id;
                string Strout = EmployeeService.EmployeeCRUD(emp);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (emp.ID == null)
                    {
                        TempData["notice"] = " Employee Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " Employee Updated Successfully...!";
                    }
                    return RedirectToAction("ListEmployee");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Employee";
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
                DataTable dtDesg = EmployeeService.GetState();
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
                DataTable dtDesg = EmployeeService.GetCity();
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
        //public JsonResult GetItemJSON(string itemid)
        //{
        //    Employee model = new Employee();
        //    model.Citylst = BindCity(itemid);
        //    return Json(BindCity(itemid));

        //}
        public IActionResult ListEmployee()
        {
            IEnumerable<Employee> cmp = EmployeeService.GetAllEmployee();
            return View(cmp);
        }
    }
}
