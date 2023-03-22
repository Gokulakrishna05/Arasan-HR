using System.Collections.Generic;
using System.Data;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Interface.Production;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;



namespace Arasan.Controllers 
{
    public class ProductionLogController : Controller
    {
        IProductionLog productionLog;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ProductionLogController(IProductionLog _productionLog, IConfiguration _configuratio)
        {
            productionLog = _productionLog;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ProductionLog(string id)
        {
            ProductionLog ca = new ProductionLog();
            ca.Brlst = BindBranch();
            ca.Worklst = BindWorkCenter();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Supervised = Request.Cookies["UserId"];
            ca.Entered = Request.Cookies["Username"];
            ca.RecList = BindEmp();
            ca.EmpList = BindEmp();
            ca.Shiftlst = BindShift();
            ca.Processlst = BindProcess();
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<WorkCenter> TData = new List<WorkCenter>();
            WorkCenter tda = new WorkCenter();
            List<MachineItem> TData1 = new List<MachineItem>();
            MachineItem tda1 = new MachineItem();
            List<EmpDetail> TData2 = new List<EmpDetail>();
            EmpDetail tda2 = new EmpDetail();
            List<BreakDetail> TData3 = new List<BreakDetail>();
            BreakDetail tda3 = new BreakDetail();
            List<InputDetail> TData4 = new List<InputDetail>();
            InputDetail tda4 = new InputDetail();
            List<ConsumDetail> TData5 = new List<ConsumDetail>();
            ConsumDetail tda5 = new ConsumDetail();
            List<OutputDetail> TData6 = new List<OutputDetail>();
            OutputDetail tda6  = new OutputDetail();
            List<WasteDetail> TData7 = new List<WasteDetail>();
            WasteDetail tda7 = new WasteDetail();
            List<SourcingDetail> TData8 = new List<SourcingDetail>();
            SourcingDetail tda8 = new SourcingDetail();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new WorkCenter();
                    tda.WorkCenterlst = BindWorkCenterid();
                    tda.Reasonlst = BindWReason( );
                    tda.Statuslst = BindWStatus();
                    tda.PTypelst = BindWType();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda1 = new MachineItem();
                   
                    tda1.Maclst = BindMachine();
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda2 = new EmpDetail();

                    tda2.Employeelst = BindEmp();
                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda3 = new BreakDetail();

                    tda3.Machinelst = BindMachineID();
                    tda3.Emplst = BindEmployee();
                    tda3.Isvalid = "Y";
                    TData3.Add(tda3);

                }
                for (int i = 0; i < 3; i++)
                {
                    tda4 = new InputDetail();

                    tda4.Itemlst = BindItemlst();
                    tda4.Drumlst = Binddrum();
                    tda4.DrumYNlst = BindYN();
                    tda4.Lotlst = BindYN();
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda5 = new ConsumDetail();

                    tda5.Itemlst = BindItemlst();
                     
                    tda5.Lotlst = BindYN();
                 
                    tda5.Isvalid = "Y";
                    TData5.Add(tda5);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda6 = new OutputDetail();

                    tda6.Itemlst = BindItemlst();
                    tda6.Drumlst = Binddrum();
                    tda6.DrumYNlst = BindYN();
                    tda6.Lotlst = BindYN();
                    tda6.Isvalid = "Y";
                    TData6.Add(tda6);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda7 = new WasteDetail();

                    tda7.Itemlst = BindItemlst();
                     
                    tda7.Isvalid = "Y";
                    TData7.Add(tda7);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda8= new SourcingDetail();

                     

                    tda8.Isvalid = "Y";
                    TData8.Add(tda8);
                }
            }
            else
            {
                DataTable dt = new DataTable();
                
                dt = productionLog.GetProductionLog(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCH"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.WorkId = dt.Rows[0]["WCID"].ToString();
                    ca.ProcessLot = dt.Rows[0]["PROCLOTNO"].ToString();
                    ca.ID = id;
                    ca.ComplYN = dt.Rows[0]["COMPLETEDYN"].ToString();
                    ca.ProcessId = dt.Rows[0]["PROCESSID"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Supervised = dt.Rows[0]["SUPERBY"].ToString();
                    ca.ProdSieve = dt.Rows[0]["PSCHNO"].ToString();
                    ca.ProdLog = dt.Rows[0]["PROCLOTYN"].ToString();
                    ca.Shift = dt.Rows[0]["SHIFT"].ToString();
                    ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.Melting = Convert.ToDouble(dt.Rows[0]["TBMELT"].ToString() == "" ? "0" : dt.Rows[0]["TBMELT"].ToString());
                    ca.FuelQty = Convert.ToDouble(dt.Rows[0]["FUELQTY"].ToString() == "" ? "0" : dt.Rows[0]["FUELQTY"].ToString());
                    ca.InputQty = Convert.ToDouble(dt.Rows[0]["TOTALINPUT"].ToString() == "" ? "0" : dt.Rows[0]["TOTALINPUT"].ToString());
                    ca.OutputQty = Convert.ToDouble(dt.Rows[0]["TOTALOUTPUT"].ToString() == "" ? "0" : dt.Rows[0]["TOTALOUTPUT"].ToString());
                    ca.TotalWaste = Convert.ToDouble(dt.Rows[0]["TOTALWASTAGE"].ToString() == "" ? "0" : dt.Rows[0]["TOTALWASTAGE"].ToString());
                    ca.ConsQty = Convert.ToDouble(dt.Rows[0]["TOTCONSQTY"].ToString() == "" ? "0" : dt.Rows[0]["TOTCONSQTY"].ToString());
                    ca.Rmvalue = Convert.ToDouble(dt.Rows[0]["TOTRMVALUE"].ToString() == "" ? "0" : dt.Rows[0]["TOTRMVALUE"].ToString());
                    ca.TotalDust = Convert.ToDouble(dt.Rows[0]["TOTALDUST"].ToString() == "" ? "0" : dt.Rows[0]["TOTALDUST"].ToString());
                    ca.TotalPowder = Convert.ToDouble(dt.Rows[0]["TOTALPOWDER"].ToString() == "" ? "0" : dt.Rows[0]["TOTALPOWDER"].ToString());

                    ca.EUnit = Convert.ToDouble(dt.Rows[0]["EBUNITSCONS"].ToString() == "" ? "0" : dt.Rows[0]["EBUNITSCONS"].ToString());
                }
                DataTable dt2 = new DataTable();

                dt2 = productionLog.GetProWorkCenterDet(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new WorkCenter();
                        tda.WorkCenterlst = BindWorkCenterid();
                        tda.Statuslst = BindWStatus();
                        tda.PTypelst = BindWType();
                        tda.Reasonlst = BindWReason();
                        tda.WorkId = dt2.Rows[i]["WORKCENTER"].ToString();
                        tda.Status = dt2.Rows[i]["WSTATUS"].ToString();
                        tda.PType = dt2.Rows[i]["PTYPE"].ToString();
                        tda.Reason = dt2.Rows[i]["WREASON"].ToString();
                        tda.StartDate = dt2.Rows[i]["WSTARTDATE"].ToString();
                        tda.StartTime = dt2.Rows[i]["WSTARTTIME"].ToString();
                        tda.EndDate = dt2.Rows[i]["WENDDATE"].ToString();
                        tda.EndTime = dt2.Rows[i]["WENDTIME"].ToString();
                        tda.TotalHrs = Convert.ToDouble(dt2.Rows[0]["WTOTHRS"].ToString() == "" ? "0" : dt2.Rows[0]["WTOTHRS"].ToString());
                         
                        tda.ID = id;
                        TData.Add(tda);
                    }

                }
                DataTable dt3 = new DataTable();

                dt3 = productionLog.GetProMachineDet(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new MachineItem();
                        tda1.Maclst = BindMachine();


                        tda1.Machine = dt3.Rows[i]["MACHINENAME"].ToString();

                        tda1.MachineId = dt3.Rows[i]["MACHINEID"].ToString();
                        tda1.Status = dt3.Rows[i]["RSTATUS"].ToString();
                        tda1.StartDate = dt3.Rows[i]["FROMDATE"].ToString();
                        tda1.StartTime = dt3.Rows[i]["FROMTIME"].ToString();
                        tda1.EndDate = dt3.Rows[i]["TODATE"].ToString();
                        tda1.EndTime = dt3.Rows[i]["TOTIME"].ToString();
                        tda1.Reason = dt3.Rows[i]["MREASON"].ToString();
                        tda1.TotalMins = Convert.ToDouble(dt3.Rows[0]["MTOTMINS"].ToString() == "" ? "0" : dt3.Rows[0]["MTOTMINS"].ToString());
                        tda1.TotalHrs = Convert.ToDouble(dt3.Rows[0]["MTOTHRS"].ToString() == "" ? "0" : dt3.Rows[0]["MTOTHRS"].ToString());
                        tda1.TotalMachineCost = Convert.ToDouble(dt3.Rows[0]["MACHINECOST"].ToString() == "" ? "0" : dt3.Rows[0]["MACHINECOST"].ToString());

                        tda1.ID = id;
                        TData1.Add(tda1);
                    }

                }
                DataTable dt4 = new DataTable();

                dt4 = productionLog.GetProEmpDet(id);
                if (dt4.Rows.Count > 0)
                {
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        tda2 = new EmpDetail();
                        tda2.Employeelst = BindEmp();

                        
                        
                        tda2.Employee = dt4.Rows[i]["EMPNAME"].ToString();
                        tda2.EmpCode = dt4.Rows[i]["EMPCODE1"].ToString();
                        tda2.Depart = dt4.Rows[i]["DEPARTMENT"].ToString();
                        tda2.StartDate = dt4.Rows[i]["ESTARTDATE"].ToString();
                        tda2.StartTime = dt4.Rows[i]["ESTARTTIME"].ToString();
                        tda2.EndDate = dt4.Rows[i]["EENDDATE"].ToString();
                        tda2.EndTime = dt4.Rows[i]["EENDTIME"].ToString();
                        tda2.OTHrs = dt4.Rows[i]["OTHRS"].ToString();
                        tda2.ETOther = dt4.Rows[i]["ETOTHRS"].ToString();
                        tda2.Normal = dt4.Rows[i]["NORMHRS"].ToString();

                        tda2.NOW = dt4.Rows[i]["NATOFW"].ToString();
                       
                        tda2.ID = id;
                        TData2.Add(tda2);
                    }

                }
                DataTable dt5 = new DataTable();

                dt5 = productionLog.GetProBreakDet(id);
                if (dt5.Rows.Count > 0)
                {
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        tda3 = new BreakDetail();
                        tda3.Machinelst = BindMachineID();
                        tda3.Emplst = BindEmployee();

                        tda3.MType = dt5.Rows[i]["EMPNAME"].ToString();
                        tda3.MachineId = dt5.Rows[i]["EMPCODE1"].ToString();
                        tda3.MachineDes = dt5.Rows[i]["DEPARTMENT"].ToString();
                        
                        tda3.StartTime = dt5.Rows[i]["ESTARTTIME"].ToString();
                        tda3.DType = dt5.Rows[i]["EENDDATE"].ToString();
                        tda3.EndTime = dt5.Rows[i]["EENDTIME"].ToString();
                        tda3.Alloted = dt5.Rows[i]["OTHRS"].ToString();
                        tda3.PB = dt5.Rows[i]["ETOTHRS"].ToString();
                        tda3.Reason = dt5.Rows[i]["NORMHRS"].ToString();

                        

                        tda3.ID = id;
                        TData3.Add(tda3);
                    }

                }
                DataTable dt6 = new DataTable();

                dt6 = productionLog.GetProInpDet(id);
                if (dt6.Rows.Count > 0)
                {
                    for (int i = 0; i < dt6.Rows.Count; i++)
                    {
                        tda4 = new InputDetail();
                        tda4.Itemlst = BindItemlst();
                        tda4.Drumlst = Binddrum();
                        tda4.DrumYNlst = BindYN();
                        tda4.Lotlst = BindYN();

                        tda4.Item = dt6.Rows[i]["EMPNAME"].ToString();
                        tda4.Drumyn = dt6.Rows[i]["EMPCODE1"].ToString();
                        tda4.DrumNo = dt6.Rows[i]["DEPARTMENT"].ToString();

                        tda4.LotYN = dt6.Rows[i]["ESTARTTIME"].ToString();
                        tda4.Batch = dt6.Rows[i]["EENDDATE"].ToString();
                        tda4.BatchQty = Convert.ToDouble(dt6.Rows[0]["MTOTMINS"].ToString() == "" ? "0" : dt6.Rows[0]["MTOTMINS"].ToString());
                        tda4.IQty = Convert.ToDouble(dt6.Rows[0]["MTOTHRS"].ToString() == "" ? "0" : dt6.Rows[0]["MTOTHRS"].ToString());
                        tda4.IBRate = Convert.ToDouble(dt6.Rows[0]["MACHINECOST"].ToString() == "" ? "0" : dt6.Rows[0]["MACHINECOST"].ToString());
                        tda4.Stock = Convert.ToDouble(dt6.Rows[0]["MACHINECOST"].ToString() == "" ? "0" : dt6.Rows[0]["MACHINECOST"].ToString());



                        tda4.ID = id;
                        TData4.Add(tda4);
                    }

                }
            }
            ca.SourcingLst = TData8;
            ca.WasteLst = TData7;
            ca.OutLst = TData6;
            ca.ConsLst = TData5;
            ca.InputLst = TData4;
            ca.BreakLst = TData3;
            ca.EmpLst = TData2;
            ca.MachineLst = TData1;
            ca.WorkLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ProductionLog(ProductionLog Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = productionLog.ProductionLogCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ProductionLog Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ProductionLog Updated Successfully...!";
                    }
                    return RedirectToAction("ListProductionLog");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ProductionLog";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
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
        public List<SelectListItem> BindShift()
        {
            try
            {
                DataTable dtDesg = productionLog.ShiftDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SHIFTNO"].ToString(), Value = dtDesg.Rows[i]["SHIFTNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindProcess()
        {
            try
            {
                DataTable dtDesg = productionLog.BindProcess();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["PROCESSMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindMachine()
        {
            try
            {
                DataTable dtDesg = productionLog.GetMachine();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MNAME"].ToString(), Value = dtDesg.Rows[i]["MNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindMachineID()
        {
            try
            {
                DataTable dtDesg = productionLog.GetMachine();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MCODE"].ToString(), Value = dtDesg.Rows[i]["MACHINEINFOBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = productionLog.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWReason()
        {
            try
            {
                DataTable dtDesg = productionLog.GetReason();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["REASON"].ToString(), Value = dtDesg.Rows[i]["REASON"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindWorkCenterid()
        {
            try
            {
                DataTable dtDesg = productionLog.GetWorkCenter();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["WCID"].ToString(), Value = dtDesg.Rows[i]["WCID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public List<SelectListItem> BindEmp()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Binddrum()
        {
            try
            {
                DataTable dtDesg = productionLog.DrumDetails();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["DRUMMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemlst( )
        {
            try
            {
                DataTable dtDesg = productionLog.GetItem();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMMASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindEmployee()
        {
            try
            {
                DataTable dtDesg = datatrans.GetEmp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPNAME"].ToString(), Value = dtDesg.Rows[i]["EMPNAME"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public JsonResult GetItemJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindWorkCenter());
        }
        public List<SelectListItem> BindWType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "None", Value = "None" });
                lstdesg.Add(new SelectListItem() { Text = "EB", Value = "EB" });
                
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindYN()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "YES", Value = "YES" });
                lstdesg.Add(new SelectListItem() { Text = "NO", Value = "NO" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
       
        public List<SelectListItem> BindWStatus()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Run", Value = "Run" });
                lstdesg.Add(new SelectListItem() { Text = "Stop", Value = "Stop" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetEmployeeDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string code = "";
              

                dt = productionLog.GetEmployeeDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    code = dt.Rows[0]["EMPID"].ToString();
                    


                }

                var result = new { code = code  };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetMachineDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
               
                string name = "";
                string type = "";
               
                dt = productionLog.GetMachineDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    name = dt.Rows[0]["MNAME"].ToString();
                    type = dt.Rows[0]["MTYPE"].ToString();
                    
                     
                }

                var result = new { name = name, type = type };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string unit = "";
              
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {

                    unit = dt.Rows[0]["UNITID"].ToString();
                   
                }

                var result = new { unit = unit  };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public IActionResult ListProductionLog()
        {
            IEnumerable<ProductionLog> cmp = productionLog.GetAllProductionLog();
            return View(cmp);
        }
    }
}
