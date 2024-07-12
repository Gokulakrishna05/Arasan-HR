using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
//using DocumentFormat.OpenXml.Bibliography;
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

            E.createby = Request.Cookies["UserId"];
            E.EMPDeptlst = BindEMPDept();
            E.EMPDesignlst = BindEMPDesign();
            E.Statelst = BindState();
            E.Citylst = BindCity("");
            E.imgpath = "";
            E.Branch = Request.Cookies["BranchId"];

            E.BranchsLst = BindBranch();
            E.DeptLst = BindDept();
            E.PayCateLst = BindPayCate();
            E.WeekLst = BindWeek();
            E.PaymentLst = BindPayment();
            E.BankLst = BindBank();
            E.ShiftLst = BindShift();
           // E.BankNameLst = BindBankName();
            E.DesigLst = BindDesig();
            E.UserNameLst = BindUserName();
            E.BloodGroupLst = BindBloodGroup();
            E.CommunityLst = BindCommunity();
            E.DispLst = BindDisp();

            E.AdAccountLst = BindAdvancecount();
           
            //List<EduDeatils> TData = new List<EduDeatils>();60759
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
                    //E.Address = dt.Rows[0]["ECADD1"].ToString();
                    E.StateId = dt.Rows[0]["ECSTATE"].ToString();
                    E.Citylst = BindCity(E.StateId);
                    E.CityId = dt.Rows[0]["ECCITY"].ToString();
                    
                    E.EmailId = dt.Rows[0]["ECMAILID"].ToString();
                    E.PhoneNo = dt.Rows[0]["ECPHNO"].ToString();
                    E.FatherName = dt.Rows[0]["FATHERNAME"].ToString();
                    E.MotherName = dt.Rows[0]["MOTHERNAME"].ToString();
                    E.GaurdName = dt.Rows[0]["GAURDNAME"].ToString();
                  //  E.Voucher = dt.Rows[0]["MOTHERNAME"].ToString();
                    E.Address1= dt.Rows[0]["EMPADD1"].ToString();
                    E.Address2 = dt.Rows[0]["EMPADD2"].ToString();
                    E.PinCode = dt.Rows[0]["EPINCODE"].ToString();
                  //  E.EmailId = dt.Rows[0]["ECMAILID"].ToString();



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
                    E.imgpath = dt.Rows[0]["IMGPATH"].ToString();



                    E.Branchs = dt.Rows[0]["BRANCHID"].ToString();
                    E.Dept = dt.Rows[0]["EMPDEPTCODE"].ToString();
                    E.PayCate = dt.Rows[0]["EMPPAYCAT"].ToString();
                    E.Payment = dt.Rows[0]["PAYMODE"].ToString();
                    E.Bank = dt.Rows[0]["BANK"].ToString();
                    E.Shift = dt.Rows[0]["SHIFTCATEGORY"].ToString();
                    E.ESI = dt.Rows[0]["ESINO"].ToString();
                    E.Active = dt.Rows[0]["EACTIVE"].ToString();
                    E.Bonus = dt.Rows[0]["BONAPP"].ToString();
                    E.CL = dt.Rows[0]["CLAPP"].ToString();
                   // E.IFSC = dt.Rows[0]["IMGPATH"].ToString();
                   // E.EPF = dt.Rows[0]["IMGPATH"].ToString();
                    E.pfclose = dt.Rows[0]["PFCLOSE"].ToString();
                  //  E.UAN = dt.Rows[0]["IMGPATH"].ToString();
                    E.Cost = dt.Rows[0]["EMPCOST"].ToString();
                    E.OT = dt.Rows[0]["OTYN"].ToString();
                    E.BAccount = dt.Rows[0]["BANKACCNO"].ToString();
                    E.Meals = dt.Rows[0]["MEALSYN"].ToString();
                    E.Appren = dt.Rows[0]["APPRENTICE"].ToString();
                    E.LOP = dt.Rows[0]["LOPYN"].ToString();
                    //E.Desig = dt.Rows[0]["IMGPATH"].ToString();
                   // E.createby = dt.Rows[0]["BindBranch"].ToString();
                  //  E.MultipleLoc = dt.Rows[0]["IMGPATH"].ToString();
                   // E.Region = dt.Rows[0]["IMGPATH"].ToString();
                    E.Phychal = dt.Rows[0]["HANDICAPPED"].ToString();
                   /// E.Aadhar = dt.Rows[0]["IMGPATH"].ToString();
                    E.PF = dt.Rows[0]["PFNO"].ToString();
                  
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



                    E.oldpf = dt3.Rows[0]["OPFNO"].ToString();
                    E.dependantes = dt3.Rows[0]["NOOFDEP"].ToString();
                   // E.Mainexp = dt3.Rows[0]["DISP"].ToString();
                    E.Pffrom = dt3.Rows[0]["OPFFDT"].ToString();
                    E.Pfto = dt3.Rows[0]["OPFTODT"].ToString();
                   // E.Adbal = dt3.Rows[0]["DISP"].ToString();
                    E.AdAccount = dt3.Rows[0]["ADVACC"].ToString();


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
        public ActionResult Employee(Employee emp, string id, List<IFormFile> file1)
        {

            try
            {
                emp.ID = id;
                string Strout = EmployeeService.EmployeeCRUD(emp, file1);
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

       
        public JsonResult GetStateJSON(string supid)
        {
            Employee model = new Employee();
            model.Citylst = BindCitySt(supid);
            return Json(BindCitySt(supid));

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
               
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindBranch()
        {
            try
            {
                DataTable dtDesg = datatrans.GetBranch();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BRANCHID"].ToString(), Value = dtDesg.Rows[i]["BRANCHMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindDept()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT DDBASICID,DEPTNAME FROM DDBASIC WHERE IS_ACTIVE='Y' ");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DEPTNAME"].ToString(), Value = dtDesg.Rows[i]["DDBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindPayCate()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='MADEIN' ");
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
        public List<SelectListItem> BindWeek()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "MONDAY", Value = "CASH" });
                lstdesg.Add(new SelectListItem() { Text = "TUESDAY", Value = "BANK" });
                lstdesg.Add(new SelectListItem() { Text = "WEDNESDAY", Value = "BANK" });
                lstdesg.Add(new SelectListItem() { Text = "THURSDAY", Value = "BANK" });
                lstdesg.Add(new SelectListItem() { Text = "FRIDAY", Value = "BANK" });
                lstdesg.Add(new SelectListItem() { Text = "SATUREDAY", Value = "BANK" });
                lstdesg.Add(new SelectListItem() { Text = "SUNDAY", Value = "BANK" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindPayment()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "CASH", Value = "CASH" });
                lstdesg.Add(new SelectListItem() { Text = "BANK", Value = "BANK" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindBank()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='BANKNAME' ");
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
        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = datatrans.ShiftDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
      
        public List<SelectListItem> BindDesig()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT DDDETAILID,DESIGNATION FROM DDDETAIL ");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DESIGNATION"].ToString(), Value = dtDesg.Rows[i]["DESIGNATION"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
      
        public List<SelectListItem> BindUserName()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='MADEIN' ");
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

        public List<SelectListItem> BindBloodGroup()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='BLOODGROUP' ");
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
        public List<SelectListItem> BindCommunity()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='COMMUNITY' ");
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
        public List<SelectListItem> BindDisp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='CITY' ");
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

        public List<SelectListItem> BindAdvancecount()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT MASTERID,MNAME FROM MASTER ");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MNAME"].ToString(), Value = dtDesg.Rows[i]["MASTERID"].ToString() });
                }
                return lstdesg;
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
        public List<SelectListItem> BindCity(string id)
        {
            try
            {
                DataTable dtDesg = EmployeeService.GetCity(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITYNAME"].ToString(), Value = dtDesg.Rows[i]["CITYNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
         public List<SelectListItem> BindCitySt(string id)
        {
            try
            {
                DataTable dtDesg = EmployeeService.GetCityst(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CITYNAME"].ToString(), Value = dtDesg.Rows[i]["CITYNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindEMPDept()
        {
            try
            {
                DataTable dtDesg = EmployeeService.GetEMPDept();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DEPTCODE"].ToString(), Value = dtDesg.Rows[i]["DDBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindEMPDesign()
        {
            try
            {
                DataTable dtDesg = EmployeeService.GetDesign();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DESIGNATION"].ToString(), Value = dtDesg.Rows[i]["DESIGNATION"].ToString() });
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
            return View();
        }

        public ActionResult DeleteMR(string tag, string id)
        {

            string flag = EmployeeService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListEmployee");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListEmployee");
            }
        } public ActionResult Remove(string tag, string id)
        {

            string flag = EmployeeService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListEmployee");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListEmployee");
            }
        }



        public IActionResult AddBank(string id)
        {
            Employee E = new Employee();
            // ca.Brlst = BindBranch();

            return View(E);
        }
        public JsonResult SaveBank(string category)
        {
            string Strout = EmployeeService.AddBankCRUD(category);
            var result = new { msg = Strout };
            return Json(result);
        }
        public JsonResult GetBankJSON()
        {
            return Json(BindBank());
        }
        public IActionResult AddBloodGroup(string id)
        {
            Employee E = new Employee();
            // ca.Brlst = BindBranch();

            return View(E);
        }
        public JsonResult SaveBlodGroup(string category)
        {
            string Strout = EmployeeService.AddBloodGroupCRUD(category);
            var result = new { msg = Strout };
            return Json(result);
        }
        public JsonResult GetBlodGroup()
        {
            return Json(BindBloodGroup());
        }
        public IActionResult AddCommunity(string id)
        {
            Employee E = new Employee();
            // ca.Brlst = BindBranch();

            return View(E);
        }
        public JsonResult SaveCommunity(string category)
        {
            string Strout = EmployeeService.AddCommunityCRUD(category);
            var result = new { msg = Strout };
            return Json(result);
        }
        public JsonResult GetCommunityJSON()
        {
            return Json(BindCommunity());
        }
        public IActionResult AddDisp(string id)
        {
            Employee ca = new Employee();
            // ca.Brlst = BindBranch();

            return View(ca);
        }
        public JsonResult SaveDisp(string category)
        {
            string Strout = EmployeeService.AddDispCRUD(category);
            var result = new { msg = Strout };
            return Json(result);
        }
        public JsonResult GetDispJSON()
        {
            return Json(BindDisp());
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<EmployeeGrid> Reg = new List<EmployeeGrid>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = EmployeeService.GetAllEmployee(strStatus); 
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Multi = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;


                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    Multi = "<a href=MultipleLocationSelect?id=" + dtUsers.Rows[i]["EMPMASTID"].ToString() + "><img src='../Images/plus.png' alt='Edit' /></a>";
                    EditRow = "<a href=Employee?id=" + dtUsers.Rows[i]["EMPMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["EMPMASTID"].ToString() + "";

                }
                else
                {
                    Multi = "";
                    EditRow = "";
                    DeleteRow = "Remove?tag=Del&id=" + dtUsers.Rows[i]["EMPMASTID"].ToString() + "";


                }
               
                Reg.Add(new EmployeeGrid
                {
                    id = dtUsers.Rows[i]["EMPMASTID"].ToString(),
                    empno = dtUsers.Rows[i]["EMPID"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    gender = dtUsers.Rows[i]["EMPSEX"].ToString(),
                    dob = dtUsers.Rows[i]["EMPDOB"].ToString(),
                    phoneno = dtUsers.Rows[i]["ECPHNO"].ToString(),
                    emailid = dtUsers.Rows[i]["ECMAILID"].ToString(),
                    multi = Multi,
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
