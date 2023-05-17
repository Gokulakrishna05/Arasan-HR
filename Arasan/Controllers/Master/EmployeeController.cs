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
        IConfiguration? _configuratio;
        private string? _connectionString;
        DataTransactions datatrans;
        public EmployeeController(IEmployee _EmployeeService, IConfiguration _configuratio)
        {
            EmployeeService = _EmployeeService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
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
                    E.EMPPayCategory = dt.Rows[0]["EMPPAYCAT"].ToString();
                    E.EMPBasic = dt.Rows[0]["EMPBASIC"].ToString();
                    E.PFNo = dt.Rows[0]["PFNO"].ToString();
                    E.ESINo = dt.Rows[0]["ESINO"].ToString();
                    E.EMPCost = dt.Rows[0]["EMPCOST"].ToString();
                    E.PFdate = dt.Rows[0]["PFDT"].ToString();
                    E.ESIDate = dt.Rows[0]["ESIDT"].ToString();
                    E.UserName = dt.Rows[0]["USERNAME"].ToString();
                    E.Password = dt.Rows[0]["PASSWORD"].ToString();
                    E.EMPDeptment = dt.Rows[0]["EMPDEPT"].ToString();
                    E.EMPDesign = dt.Rows[0]["EMPDESIGN"].ToString();
                    E.EMPDeptCode = dt.Rows[0]["EMPDEPTCODE"].ToString();
                    E.JoinDate = dt.Rows[0]["JOINDATE"].ToString();
                    E.ResignDate = dt.Rows[0]["RESIGNDATE"].ToString();

                }
                DataTable dt2 = new DataTable();
                dt2 = EmployeeService.GetEmpEduDeatils(id);
                if (dt2.Rows.Count > 0)
                {


                    E.Education = dt2.Rows[0]["EDUCATION"].ToString();
                    E.College = dt2.Rows[0]["UC"].ToString();
                    E.EcPlace = dt2.Rows[0]["ECPLACE"].ToString();
                    E.MPercentage = Convert.ToDouble(dt2.Rows[0]["MPER"].ToString());
                    E.YearPassing = dt2.Rows[0]["YRPASSING"].ToString();



                }
                DataTable dt3 = new DataTable();
                dt3 = EmployeeService.GetEmpPersonalDeatils(id);
                if (dt3.Rows.Count > 0)
                {


                    E.MaterialStatus = dt3.Rows[0]["MARITALSTATUS"].ToString();
                    E.BloodGroup = dt3.Rows[0]["BLOODGROUP"].ToString();
                    E.Community = dt3.Rows[0]["COMMUNITY"].ToString();
                    E.PayType = dt3.Rows[0]["PAYTYPE"].ToString();
                    E.EmpType = dt3.Rows[0]["EMPTYPE"].ToString();
                    E.Disp = dt3.Rows[0]["DISP"].ToString();


                }
                DataTable dt4 = new DataTable();
                dt4 = EmployeeService.GetEmpSkillDeatils(id);
                if (dt4.Rows.Count > 0)
                {


                    E.SkillSet = dt4.Rows[0]["SKILL"].ToString();



                }
            }

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
        public List<SelectListItem> BindLocation(string id)
        {
            try
            {
                List<SelectListItem> items = new List<SelectListItem>();
                DataTable dtCity = new DataTable();
                dtCity = datatrans.GetLocation();
                if (dtCity.Rows.Count > 0)
                {
                    for (int i = 0; i < dtCity.Rows.Count; i++)
                    {
                        bool sel = false;
                        long Region_id = EmployeeService.GetMregion(dtCity.Rows[i]["LOCDETAILSID"].ToString(), id);
                        if (Region_id == 0)
                        {
                            sel = false;
                        }
                        else
                        {
                            sel = true;
                        }
                        items.Add(new SelectListItem
                        {
                            Text = dtCity.Rows[i]["LOCID"].ToString(),
                            Value = dtCity.Rows[i]["LOCDETAILSID"].ToString(),
                            Selected = sel
                        });
                    }
                }
                return items;






                //DataTable dtDesg = datatrans.GetLocation();
                //List<SelectListItem> lstdesg = new List<SelectListItem>();
                //for (int i = 0; i < dtDesg.Rows.Count; i++)
                //{
                //    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                //}
                //return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult MultipleLocationSelect(string id)
        {
            MultipleLocation E = new MultipleLocation();
			E.CreadtedBy = Request.Cookies["UserId"];
			DataTable dt = new DataTable();
            dt = EmployeeService.GetCurrentUser(id);
            if (dt.Rows.Count > 0)
            {
                  E.EmpName = dt.Rows[0]["EMPNAME"].ToString();
            }
                 
                E.ID = id;
            E.Loclst = BindLocation(id);


            return View(E);
        }
        [HttpPost]
        public ActionResult MultipleLocationSelect(MultipleLocation mp, string id)
        {
            if (ModelState.IsValid)
            {
                id = id != null ? id : "0";
                try
                {
                    mp.ID = id;
                    string Strout = EmployeeService.GetMultipleLocation(mp);
                    if (string.IsNullOrEmpty(Strout))
                    {
                        if (mp.ID == null)
                        {
                            TempData["notice"] = "Multiple Location Inserted Successfully...!";
                        }
                        else
                        {
                            TempData["notice"] = "Multiple Location Updated Successfully...!";
                        }
                        return RedirectToAction("ListEmployee");
                    }
                    else
                    {
                        TempData["notice"] = Strout;
                        ViewBag.PageTitle = "Edit MultipleLocation";

                        return View(mp);
                    }
                }
                catch (Exception ex)
                {
                    throw ex;
                }
            }
            return View(mp);

        }

            //private static List<SelectListItem> PopulateRegion(string id)
            //{
            //    List<SelectListItem> items = new List<SelectListItem>();
            //MultipleLocation Cy = new MultipleLocation();
            //    DataTable dtCity = new DataTable();
            //    dtCity = Cy.GetRegion();
            //    if (dtCity.Rows.Count > 0)
            //    {
            //        for (int i = 0; i < dtCity.Rows.Count; i++)
            //        {
            //            bool sel = false;
            //            int Region_id = Cy.GetMregion(dtCity.Rows[i]["ID"].ToString(), id);
            //            if (Region_id == 0)
            //            {
            //                sel = false;
            //            }
            //            else
            //            {
            //                sel = true;
            //            }
            //            items.Add(new SelectListItem
            //            {
            //                Text = dtCity.Rows[i]["STATE_NAME"].ToString(),
            //                Value = dtCity.Rows[i]["ID"].ToString(),
            //                Selected = sel
            //            });
            //        }
            //    }
            //    return items;
            //}
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

        public IActionResult ListEmployee()
        {
            IEnumerable<Employee> cmp = EmployeeService.GetAllEmployee();
            return View(cmp);
        }
    }
}
