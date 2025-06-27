using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;
using System.Text;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Master;
using AspNetCore.Reporting;

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
        private readonly IWebHostEnvironment _WebHostEnvironment;
        DataTransactions datatrans;
        private static readonly int KeySize = 256; // You can choose 128, 192, or 256 bits
        private static readonly int BlockSize = 128; // Block size for AES
        private static readonly string EncryptionKey = "Arasan"; // Use a strong key
        private static readonly string IV = "TaaiErp"; // Use a strong IV

        public EmployeeController(IEmployee _EmployeeService, IConfiguration _configuratio, IWebHostEnvironment WebHostEnvironment)
        {
            this._WebHostEnvironment = WebHostEnvironment;
            EmployeeService = _EmployeeService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

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
            E.Appren = "No";
            E.LOP = "No";
            E.OT = "No";
            E.Meals = "No";
            E.CL = "No";
            E.pfclose = "No";
            E.Phychal = "No";
            E.Bonus = "NO";
            E.PF = "NO";
            E.ESI = "NO";
            Pcod wc = new Pcod();
            List<Pcod> Data = new List<Pcod>();

            Edet ed = new Edet();
            List<Edet> Data1 = new List<Edet>();

            Eatt ad = new Eatt();
            List<Eatt> Data14 = new List<Eatt>();

            Brch br = new Brch();
            List<Brch> Data13 = new List<Brch>();

            Dcod dc = new Dcod();
            List<Dcod> Data12 = new List<Dcod>();

            Perf pe = new Perf();
            List<Perf> Data11 = new List<Perf>();

            Emrc ec = new Emrc();
            List<Emrc> Data10 = new List<Emrc>();

            Eins ei = new Eins();
            List<Eins> Data9 = new List<Eins>();

            Prhs ph = new Prhs();
            List<Prhs> Data8 = new List<Prhs>();
            LeaveDet le = new LeaveDet();
            List<LeaveDet> Datale = new List<LeaveDet>();
            //List<EduDeatils> TData = new List<EduDeatils>();60759
            //EduDeatils tda = new EduDeatils();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    wc = new Pcod();
                    wc.Isvalid = "Y";
                    Data.Add(wc);
                }
                for (int i = 0; i < 1; i++)
                {
                    ed = new Edet();
                    ed.Isvalid = "Y";
                    Data1.Add(ed);
                }
                for (int i = 0; i < 1; i++)
                {
                    ad = new Eatt();
                    ad.Isvalid = "Y";
                    Data14.Add(ad);
                }
                for (int i = 0; i < 1; i++)
                {
                    br = new Brch();
                    br.wlst = BindBranch();
                    br.Isvalid = "Y";
                    Data13.Add(br);
                }
                for (int i = 0; i < 1; i++)
                {
                    dc = new Dcod();
                    dc.wlst = BindDepCode();
                    dc.Isvalid = "Y";
                    Data12.Add(dc);
                }
                for (int i = 0; i < 1; i++)
                {
                    pe = new Perf();
                    pe.Isvalid = "Y";
                    Data11.Add(pe);
                }
                for (int i = 0; i < 1; i++)
                {
                    ec = new Emrc();
                    ec.Isvalid = "Y";
                    Data10.Add(ec);
                }
                for (int i = 0; i < 1; i++)
                {
                    ei = new Eins();
                    ei.Isvalid = "Y";
                    Data9.Add(ei);
                }
                for (int i = 0; i < 1; i++)
                {
                    ph = new Prhs();
                    ph.wlst = BindDesigs();
                    ph.Isvalid = "Y";
                    Data8.Add(ph);
                }
                for (int i = 0; i < 1; i++)
                {
                    le = new LeaveDet();
                    le.lelst = BindLeaveCode();

                    le.Isvalid = "Y";
                    Datale.Add(le);
                }
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
                    E.Address1 = dt.Rows[0]["ECADD1"].ToString();
                    E.Address2 = dt.Rows[0]["ECADD2"].ToString();
                    E.PinCode = dt.Rows[0]["ECPCODE"].ToString();
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
                    E.ESI = dt.Rows[0]["ESIAPP"].ToString();
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
                    E.Week = dt.Rows[0]["WOFF"].ToString();
                    E.LOP = dt.Rows[0]["LOPYN"].ToString();
                    //E.Desig = dt.Rows[0]["IMGPATH"].ToString();
                    // E.createby = dt.Rows[0]["BindBranch"].ToString();
                    //  E.MultipleLoc = dt.Rows[0]["IMGPATH"].ToString();
                    // E.Region = dt.Rows[0]["IMGPATH"].ToString();
                    E.Phychal = dt.Rows[0]["HANDICAPPED"].ToString();
                    /// E.Aadhar = dt.Rows[0]["IMGPATH"].ToString();
                    E.PF = dt.Rows[0]["PFAPP"].ToString();

                }
                DataTable dt2 = new DataTable();
                dt2 = EmployeeService.GetEmpEduDeatils(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        ed = new Edet();
                        ed.Isvalid = "Y";

                        ed.qua = dt2.Rows[i]["EDUCATION"].ToString();
                        ed.clg = dt2.Rows[i]["UC"].ToString();
                        ed.plc = dt2.Rows[i]["ECPLACE"].ToString();
                        ed.peo = dt2.Rows[i]["MPER"].ToString();
                        ed.yop = dt2.Rows[i]["YRPASSING"].ToString();
                        Data1.Add(ed);
                    }
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
                DataTable dt5 = new DataTable();
                dt5 = EmployeeService.GetPayCodeDeatils(id);
                if (dt5.Rows.Count > 0)
                {
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        wc = new Pcod();
                        wc.Isvalid = "Y";
                        wc.pc = dt5.Rows[i]["PAYCODE"].ToString();
                        wc.pf = dt5.Rows[i]["FORMULA"].ToString();
                        Data.Add(wc);
                    }
                }

                DataTable dt8 = new DataTable();
                dt8 = EmployeeService.GetPrvHisDeatils(id);
                if (dt8.Rows.Count > 0)
                {
                    for (int i = 0; i < dt8.Rows.Count; i++)
                    {
                        ph = new Prhs();
                        ph.Isvalid = "Y";
                        ph.emp = dt8.Rows[i]["EMPLOYER"].ToString();
                        ph.city = dt8.Rows[i]["CITY"].ToString();
                        ph.wlst = BindDesigs();
                        ph.wc = dt8.Rows[i]["EDESIG"].ToString();
                        ph.lsd = dt8.Rows[i]["LSALARYDRAWN"].ToString();
                        ph.wrm = dt8.Rows[i]["WM"].ToString();
                        Data8.Add(ph);
                    }
                }

                DataTable dt9 = new DataTable();
                dt9 = EmployeeService.GetInsuranceDeatils(id);
                if (dt9.Rows.Count > 0)
                {
                    for (int i = 0; i < dt9.Rows.Count; i++)
                    {
                        ei = new Eins();
                        ei.Isvalid = "Y";
                        ei.pno = dt9.Rows[i]["POLICYNO"].ToString();
                        ei.nop = dt9.Rows[i]["NATUREOFPOLICY"].ToString();
                        ei.bad = dt9.Rows[i]["BADD"].ToString();
                        ei.apr = dt9.Rows[i]["ACTPREMIUM"].ToString();
                        ei.dpr = dt9.Rows[i]["PREMIUM"].ToString();
                        ei.psd = dt9.Rows[i]["PSTDT"].ToString();
                        ei.ped = dt9.Rows[i]["PEDDT"].ToString();
                        Data9.Add(ei);
                    }
                }

                DataTable dt10 = new DataTable();
                dt10 = EmployeeService.GetEmrgConDeatils(id);
                if (dt10.Rows.Count > 0)
                {
                    for (int i = 0; i < dt10.Rows.Count; i++)
                    {
                        ec = new Emrc();
                        ec.Isvalid = "Y";
                        ec.cna = dt10.Rows[0]["ECNAME"].ToString();
                        ec.nor = dt10.Rows[0]["NREL"].ToString();
                        ec.pho = dt10.Rows[0]["ECPHONE"].ToString();
                        ec.mob = dt10.Rows[0]["ECMOBILE"].ToString();
                        ec.fax = dt10.Rows[0]["ECFAX"].ToString();
                        Data10.Add(ec);
                    }
                }

                DataTable dt11 = new DataTable();
                dt11 = EmployeeService.GetPeforDeatils(id);
                if (dt11.Rows.Count > 0)
                {
                    for (int i = 0; i < dt11.Rows.Count; i++)
                    {
                        pe = new Perf();
                        pe.Isvalid = "Y";
                        pe.pps = dt11.Rows[i]["PERFDESC"].ToString();
                        pe.res = dt11.Rows[i]["RESULT"].ToString();
                        pe.rat = dt11.Rows[i]["RATING"].ToString();
                        pe.awd = dt11.Rows[i]["AWDGN"].ToString();
                        Data11.Add(pe);
                    }
                }

                DataTable dt12 = new DataTable();
                dt12 = EmployeeService.GetDepCodeDeatils(id);
                if (dt12.Rows.Count > 0)
                {
                    for (int i = 0; i < dt12.Rows.Count; i++)
                    {
                        dc = new Dcod();
                        dc.wlst = BindDepCode();
                        dc.wc = dt12.Rows[i]["DEPTCODE"].ToString();
                        dc.Isvalid = "Y";
                        Data12.Add(dc);
                    }
                }
                DataTable dt13 = new DataTable();
                dt13 = EmployeeService.GetBranchDeatils(id);
                if (dt13.Rows.Count > 0)
                {
                    for (int i = 0; i < dt13.Rows.Count; i++)
                    {
                        br = new Brch();
                        br.wlst = BindBranch();
                        br.wc = dt13.Rows[i]["BRANCH"].ToString();
                        br.Isvalid = "Y";
                        Data13.Add(br);
                    }
                }
                DataTable dt14 = new DataTable();
                dt14 = EmployeeService.GetEmpAtted(id);
                if (dt14.Rows.Count > 0)
                {
                    for (int i = 0; i < dt14.Rows.Count; i++)
                    {
                        ad = new Eatt();
                        ad.Isvalid = "Y";

                        ad.mon = dt14.Rows[i]["AMONTH"].ToString();
                        ad.pre = dt14.Rows[i]["PDAYS"].ToString();
                        ad.abs = dt14.Rows[i]["ADAYS"].ToString();
                        ad.lea = dt14.Rows[i]["LDAYS"].ToString();
                        ad.nhd = dt14.Rows[i]["NHD"].ToString();
                        ad.nhw = dt14.Rows[i]["NHW"].ToString();
                        ad.wo = dt14.Rows[i]["WO"].ToString();
                        ad.wds = dt14.Rows[i]["WDAYS"].ToString();
                        ad.hds = dt14.Rows[i]["HDAYS"].ToString();

                        Data14.Add(ad);
                    }
                }
                DataTable dte = new DataTable();
                dte = EmployeeService.GetEmpPersonalDeatils(id);
                if (dte.Rows.Count > 0)
                {


                    E.MaterialStatus = dte.Rows[0]["MARITALSTATUS"].ToString();
                    E.BloodGroup = dte.Rows[0]["BLOODGROUP"].ToString();
                    E.Community = dte.Rows[0]["COMMUNITY"].ToString();
                    E.PayType = dte.Rows[0]["PAYTYPE"].ToString();
                    E.EmpType = dte.Rows[0]["EMPTYPE"].ToString();
                    E.Disp = dte.Rows[0]["DISP"].ToString();



                    E.oldpf = dte.Rows[0]["OPFNO"].ToString();
                    E.dependantes = dte.Rows[0]["NOOFDEP"].ToString();
                    // E.Mainexp = dt3.Rows[0]["DISP"].ToString();
                    E.Pffrom = dte.Rows[0]["OPFFDT"].ToString();
                    E.Pfto = dte.Rows[0]["OPFTODT"].ToString();
                    // E.Adbal = dt3.Rows[0]["DISP"].ToString();
                    E.AdAccount = dte.Rows[0]["ADVACC"].ToString();


                }
                DataTable dtl = new DataTable();
                dtl = datatrans.GetData("SELECT E.LEAVECODE,E.LEAVESALLOWED ,L.LEAVETYPE,L.CARRIEDSTATUS,L.ENCASHABLE FROM EMPMLEAVE E,LCODEMAST L WHERE L.LCODEMASTID=E.LEAVECODE AND E.EMPMASTID='" + id + "'");
                if (dtl.Rows.Count > 0)
                {
                    for (int i = 0; i < dtl.Rows.Count; i++)
                    {
                        le = new LeaveDet();
                        le.Isvalid = "Y";
                        le.lelst = BindLeaveCode();
                        le.leavecode = dtl.Rows[i]["LEAVECODE"].ToString();
                        le.allwoleaves = dtl.Rows[i]["LEAVESALLOWED"].ToString();
                        le.leavetype = dtl.Rows[i]["LEAVETYPE"].ToString();
                        le.caried = dtl.Rows[i]["CARRIEDSTATUS"].ToString();
                        le.encash = dtl.Rows[i]["ENCASHABLE"].ToString();
                        Datale.Add(le);
                    }
                }


            }
            E.Pclst = Data;
            E.Edlst = Data1;
            E.Atlst = Data14;
            E.Brlst = Data13;
            E.Dclst = Data12;
            E.pelst = Data11;
            E.Emlst = Data10;
            E.Inlst = Data9;
            E.Phlst = Data8;
            E.Leavelst = Datale;
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

        public JsonResult GetItemJSON()
        {
            Pcod model = new Pcod();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        public JsonResult GetItemJSON1()
        {
            Edet model = new Edet();
            return Json(model);
        }
        public JsonResult GetItemJSON8()
        {
            Prhs model = new Prhs();
            return Json(BindDesigs());
        }
        public JsonResult GetItemJSON9()
        {
            Eins model = new Eins();
            return Json(model);
        }
        public JsonResult GetItemJSON10()
        {
            Emrc model = new Emrc();
            return Json(model);
        }
        public JsonResult GetItemJSON11()
        {
            Perf model = new Perf();
            return Json(model);
        }
        public JsonResult GetItemJSON12()
        {
            //Promst model = new Promst();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindDepCode());

        }
        public JsonResult GetItemJSON13()
        {
            //Promst model = new Promst();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindBranch());

        }
        public JsonResult GetItemJSON14()
        {
            Eatt model = new Eatt();
            return Json(model);
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
                DataTable dtDesg = datatrans.GetData("SELECT PAYCATEGORY,PCBASICID FROM PCBASIC ");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PAYCATEGORY"].ToString(), Value = dtDesg.Rows[i]["PCBASICID"].ToString() });
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
                lstdesg.Add(new SelectListItem() { Text = "MONDAY", Value = "MONDAY" });
                lstdesg.Add(new SelectListItem() { Text = "TUESDAY", Value = "TUESDAY" });
                lstdesg.Add(new SelectListItem() { Text = "WEDNESDAY", Value = "WEDNESDAY" });
                lstdesg.Add(new SelectListItem() { Text = "THURSDAY", Value = "THURSDAY" });
                lstdesg.Add(new SelectListItem() { Text = "FRIDAY", Value = "FRIDAY" });
                lstdesg.Add(new SelectListItem() { Text = "SATUREDAY", Value = "SATUREDAY" });
                lstdesg.Add(new SelectListItem() { Text = "SUNDAY", Value = "SUNDAY" });
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

        public List<SelectListItem> BindLeaveCode()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT LCODEMASTID,LEAVECODE FROM LCODEMAST");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LEAVECODE"].ToString(), Value = dtDesg.Rows[i]["LCODEMASTID"].ToString() });
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
        public JsonResult GetLeaJSON()
        {
            return Json(BindLeaveCode());
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
        }
        public ActionResult Remove(string tag, string id)
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
                string GeneratePdf = string.Empty;
                string pass = string.Empty;



                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    Multi = "<a href=MultipleLocationSelect?id=" + dtUsers.Rows[i]["EMPMASTID"].ToString() + "><img src='../Images/plus.png' alt='Edit' /></a>";
                    GeneratePdf = "<a href=EmployeeInformation?id=" + dtUsers.Rows[i]["EMPMASTID"].ToString() + " target='_blank'><img src='../Images/pdficon.png' alt='View Details' width='20' /></a>";
                    pass = "<a href=ChangePassword?id=" + dtUsers.Rows[i]["EMPMASTID"].ToString() + "  ><img src='../Images/password.png' alt='View Details' width='20' /></a>";
                    EditRow = "<a href=Employee?id=" + dtUsers.Rows[i]["EMPMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["EMPMASTID"].ToString() + "";

                }
                else
                {

                    Multi = "";
                    GeneratePdf = "";
                    pass = "";
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
                    depart = dtUsers.Rows[i]["DEPTNAME"].ToString(),

                    multi = Multi,
                    print = GeneratePdf,
                    editrow = EditRow,
                    delrow = DeleteRow,
                    pass = pass,


                });
            }

            return Json(new
            {
                Reg
            });

        }
        public async Task<IActionResult> EmployeeInformation(string id)
        {

            string mimtype = "";
            int extension = 1;
            //string DrumID = datatrans.GetDataString("Select PARTYID from POBASIC where POBASICID='" + id + "' ");

            System.Data.DataSet ds = new System.Data.DataSet();
            var path = $"{this._WebHostEnvironment.WebRootPath}\\Reports\\EmpDeatils.rdlc";
            Dictionary<string, string> Parameters = new Dictionary<string, string>();
            //  Parameters.Add("rp1", " Hi Everyone");
            var emp = await EmployeeService.GetEmployeeDetails(id);
            var emp1 = await EmployeeService.GetEmpEduDetails(id);
            var emp2 = await EmployeeService.GetEmpOthDetails(id);

            AspNetCore.Reporting.LocalReport localReport = new AspNetCore.Reporting.LocalReport(path);
            localReport.AddDataSource("EmpMast", emp);
            localReport.AddDataSource("EmpEdu", emp1);
            localReport.AddDataSource("EmpMoi", emp2);
            //localReport.AddDataSource("DataSet1_DataTable1", po);
            var result = localReport.Execute(RenderType.Pdf, extension, Parameters, mimtype);

            return File(result.MainStream, "application/Pdf");

        }
        public List<SelectListItem> BindDesigs()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "DIP.Apprentice", Value = "DIP.Apprentice" });
                lstdesg.Add(new SelectListItem() { Text = "Elect.Supr.", Value = "Elect.Supr." });
                lstdesg.Add(new SelectListItem() { Text = "Electrical Supervisor", Value = "Electrical Supervisor" });


                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<SelectListItem> BindDepCode()
        {
            try
            {
                DataTable dtDesg = EmployeeService.GetDCode();
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
        public ActionResult GetLeaveDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable stock = new DataTable();

                string type = "";
                string cash = "";
                string caried = "";

                dt = datatrans.GetData("SELECT LEAVETYPE,CARRIEDSTATUS,ENCASHABLE FROM LCODEMAST WHERE LCODEMASTID='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {

                    type = dt.Rows[0]["LEAVETYPE"].ToString();
                    cash = dt.Rows[0]["ENCASHABLE"].ToString();
                    caried = dt.Rows[0]["CARRIEDSTATUS"].ToString();


                }


                var result = new { type = type, cash = cash, caried = caried };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ChangePassword(string id)
        {

            Employee e = new Employee();

            string chiper = "";
            if (id == null)
            {
                e.createby = Request.Cookies["UserId"];
                DataTable emp = datatrans.GetData("SELECT EMPNAME ,USERNAME,PASSWORD FROM EMPMAST WHERE EMPMASTID='" + e.createby + "'");
                if (emp.Rows[0]["PASSWORD"].ToString().Length > 10)
                {
                    chiper = Decrypt(emp.Rows[0]["PASSWORD"].ToString());
                }
                else
                {
                    chiper = emp.Rows[0]["PASSWORD"].ToString();
                }
                e.EmpName = emp.Rows[0]["EMPNAME"].ToString();
                e.UserName = emp.Rows[0]["USERNAME"].ToString();
                
            }
            else
            {
                e.createby = id;
                DataTable emp = datatrans.GetData("SELECT EMPNAME ,USERNAME,PASSWORD FROM EMPMAST WHERE EMPMASTID='" + id + "'");
                if (emp.Rows[0]["PASSWORD"].ToString().Length > 10)
                {
                    chiper = Decrypt(emp.Rows[0]["PASSWORD"].ToString());
                }
                else
                {
                    chiper = emp.Rows[0]["PASSWORD"].ToString();
                }
                e.EmpName = emp.Rows[0]["EMPNAME"].ToString();
                e.UserName = emp.Rows[0]["USERNAME"].ToString();
                
            }

            return View(e);
        }
        [HttpPost]
        public ActionResult ChangePassword(Employee emp, string id)
        {

            try
            {
                emp.ID = id;
                string Strout = EmployeeService.Changepass(emp);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (emp.ID == null)
                    {
                        TempData["notice"] = " Password Change Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " Password Change Successfully...!";
                    }
                    return RedirectToAction("ChangePassword");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ChangePassword";
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
        public static string Decrypt(string cipherText)
        {
            try
            {
                // Ensure the cipher text is Base64 encoded
                byte[] cipherBytes = Convert.FromBase64String(cipherText);

                using (var aes = Aes.Create())
                {
                    aes.Key = GetKey(EncryptionKey);  // Ensure correct key size
                    aes.IV = GetIV(IV);               // Ensure IV is 16 bytes

                    var decryptor = aes.CreateDecryptor(aes.Key, aes.IV);
                    using (var ms = new MemoryStream(cipherBytes))
                    {
                        using (var cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                        {
                            using (var sr = new StreamReader(cs))
                            {
                                return sr.ReadToEnd();
                            }
                        }
                    }
                }
            }
            catch (FormatException)
            {
                // Handle invalid Base64 string case
                throw new ArgumentException("Input is not a valid Base64 string.");
            }
            catch (CryptographicException ex)
            {
                // Handle encryption-specific exceptions
                throw new CryptographicException("Decryption failed. The input may have been tampered with or the wrong key/IV was used.", ex);
            }
        }
        private static byte[] GetKey(string key)
        {
            byte[] keyBytes = Encoding.UTF8.GetBytes(key);

            // If the key is too long, truncate it
            if (keyBytes.Length > 32)
            {
                Array.Resize(ref keyBytes, 32); // For AES-256
            }
            // If the key is too short, pad it with zeros
            else if (keyBytes.Length < 32)
            {
                Array.Resize(ref keyBytes, 32); // For AES-256
            }

            return keyBytes;
        }
        // Function to adjust the IV to 16 bytes (128-bit block size for AES)
        private static byte[] GetIV(string iv)
        {
            byte[] ivBytes = Encoding.UTF8.GetBytes(iv);

            // If the IV is too long, truncate it
            if (ivBytes.Length > 16)
            {
                Array.Resize(ref ivBytes, 16); // IV must be 16 bytes for AES
            }
            // If the IV is too short, pad it with zeros
            else if (ivBytes.Length < 16)
            {
                Array.Resize(ref ivBytes, 16); // IV must be 16 bytes for AES
            }

            return ivBytes;
        }
    }
}
