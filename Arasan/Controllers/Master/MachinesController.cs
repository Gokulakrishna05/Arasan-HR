using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using System.Data;
using static Nest.JoinField;

namespace Arasan.Controllers.Master
{
    public class MachinesController : Controller
    {
        IMchine Mach;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public MachinesController(IMchine _Mach, IConfiguration _configuratio)
        {
            Mach = _Mach;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult Machine(string id)
        {
            Machine M = new Machine();
            M.LocLst = Bindloc();
            M.WrkCentlst = Bindwrkcent();
            M.SubProclst = Bindsub();
            M.MMadelst = BindMade();
            M.MMTypelst = BindType();
            M.MMModelst = Bindmode();

            M.MCaplst = Bindcap();
            M.MELifeLst = Bindcap();
            M.MPowerlst = Bindcap();
           
            M.MRunlst = Bindcap();
            M.MRunHLst = Bindcap();
            M.MLeadlst = Bindcap();
            M.MMaintainLst = Bindcap();

            List<Compdetails> TData = new List<Compdetails>();
            Compdetails tda = new Compdetails();

            List<Majorpart> TDatab = new List<Majorpart>();
            Majorpart tdab = new Majorpart();

            List<Checklistdetails> TDatau = new List<Checklistdetails>();
            Checklistdetails tdau = new Checklistdetails();
            //List<LocdetItem> TDatal = new List<LocdetItem>();
            //LocdetItem tdal = new LocdetItem();

            if (id == null)

            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new Compdetails();
                    tda.Itemlst = BindItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tdab = new Majorpart();
                    tdab.MajorPartlst = BindMajor();
                    tdab.Isvalid = "Y";
                    TDatab.Add(tdab);
                }
                for (int i = 0; i < 1; i++)
                {
                    tdau = new Checklistdetails();
                    tdau.Checklistlst = BindChecklist();
                    tdau.Isvalid = "Y";
                    TDatau.Add(tdau);
                }
               
            }
            else
            {
               
                DataTable dt = new DataTable();
            //DataTable dtt = new DataTable();
            dt = Mach.GetMachineEdit(id);
            if (dt.Rows.Count > 0)
            {
                M.ID = dt.Rows[0]["MACHINEINFOBASICID"].ToString();
                M.MId = dt.Rows[0]["MCODE"].ToString();
                M.MName = dt.Rows[0]["MNAME"].ToString();
                M.MLoc = dt.Rows[0]["MLOCATION"].ToString();
                M.MWrkCent = dt.Rows[0]["MWCID"].ToString();

                M.MSupply = dt.Rows[0]["MSUPPLY"].ToString();
                M.MSerial = dt.Rows[0]["MSERIALNO"].ToString();
                M.MModel = dt.Rows[0]["MMODEL"].ToString();
                M.MManname = dt.Rows[0]["MMANFNAME"].ToString();
                M.MSubProc = dt.Rows[0]["MSPROCESSID"].ToString();
                M.MMade = dt.Rows[0]["MMADEIN"].ToString();
                M.MPur = dt.Rows[0]["MMODEOFPUR"].ToString();
                M.MSer = dt.Rows[0]["MSERVDET"].ToString();
                M.MSerCmp = dt.Rows[0]["MSERCOMP"].ToString();
                M.MIncharge = dt.Rows[0]["MINCHARGE"].ToString();
                //M.MMType = dt.Rows[0]["CITY"].ToString();
                M.Aux = dt.Rows[0]["AUXYN"].ToString();
                //M.MUse = dt.Rows[0]["CITY"].ToString();
                M.MMMode = dt.Rows[0]["MMODE"].ToString();
                M.MCap = dt.Rows[0]["MCAPACITY"].ToString();
                M.MCapUnit = dt.Rows[0]["MUNIT"].ToString();
                M.MELife = dt.Rows[0]["ESTLIFE"].ToString();
                M.MELifeUnit = dt.Rows[0]["EUNIT"].ToString();
                M.MPower = dt.Rows[0]["POWKVA"].ToString();
                M.MPowerUnit = dt.Rows[0]["PUNIT"].ToString();
                M.MPFactor = dt.Rows[0]["POWUTILFAC"].ToString();
                M.MRun = dt.Rows[0]["RUNHR"].ToString();
                //M.MRunUnit = dt.Rows[0]["CITY"].ToString();
                //M.MRunH = dt.Rows[0]["CITY"].ToString();
                M.MRunHUnit = dt.Rows[0]["MCID"].ToString();
                M.MLead = dt.Rows[0]["LEADHRS"].ToString();
                M.MLeadUnit = dt.Rows[0]["LEADUNIT"].ToString();
                M.MMaintain = dt.Rows[0]["MAINTHRS"].ToString();
                M.MMaintainUnit = dt.Rows[0]["MAINTUNI"].ToString();
                M.DOP = dt.Rows[0]["MPURDT"].ToString();
                M.DOS = dt.Rows[0]["MSUPDT"].ToString();
                M.InsDate = dt.Rows[0]["MINSTDT"].ToString();
                M.DOLMain = dt.Rows[0]["MLAMAINDT"].ToString();
                M.NMainDate = dt.Rows[0]["MNEMAINDT"].ToString();
                M.MLCost = dt.Rows[0]["MACLCOST"].ToString();
                M.Dep = dt.Rows[0]["DEPPER"].ToString();
                M.Int = dt.Rows[0]["INTPER"].ToString();
                M.SOYear = dt.Rows[0]["SALOP"].ToString();
                M.MCYear = dt.Rows[0]["MAINCOST"].ToString();
                M.Rent = dt.Rows[0]["RENTMAC"].ToString();
                M.Tot = dt.Rows[0]["TOT"].ToString();
                M.PCUnit = dt.Rows[0]["POWRATEUNIT"].ToString();
                //M.AvgCost = dt.Rows[0]["CITY"].ToString();
                M.FixCost = dt.Rows[0]["FIXCOSTHR"].ToString();
                M.CostRH = dt.Rows[0]["RATESQFT"].ToString();
                //M.PUtilize = dt.Rows[0]["CITY"].ToString();
                M.DepValue = dt.Rows[0]["DEPVAL"].ToString();
                M.IntValue = dt.Rows[0]["INTVAL"].ToString();
                M.InsValue = dt.Rows[0]["INSVAL"].ToString();
                M.PCostH = dt.Rows[0]["POWUTILHR"].ToString();
              ///  M.AddMCost = dt.Rows[0]["MSPROCESSID"].ToString();
                //M.ddlStatus = dt.Rows[0]["CITY"].ToString();

            }
                DataTable dt2 = new DataTable();
                
                dt2 = datatrans.GetData("Select MACHINEINFOBASICID,PARTNO,LIFETIME,WARRANTYTILLDT from COMPDETAILS Where MACHINEINFOBASICID='"+id+"'");

                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                      
                      
                       
                        tda = new Compdetails();
                        tda.PartNumber = dt2.Rows[0]["PARTNO"].ToString();
                        tda.LifeTimeInHrs = dt2.Rows[0]["LIFETIME"].ToString();
                        tda.DateOfIssue = dt2.Rows[0]["WARRANTYTILLDT"].ToString();
                        //tda.Itemlst = 
                            
                            
                         
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }

                DataTable dtt = new DataTable();
                dtt = datatrans.GetData("Select MACHINEINFOBASICID,MCMAJORPARTSROW,MPARTID,ACTIVEYN,RUNHRS,CRYN from MCMAJORPARTS Where MACHINEINFOBASICID='"+id+"'");

                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tdab = new Majorpart();
                        tdab.Majorparts = dtt.Rows[0]["MPARTID"].ToString();
                        tdab.Active = dtt.Rows[0]["ACTIVEYN"].ToString();
                        tdab.Critical = dtt.Rows[0]["CRYN"].ToString();
                        tdab.MajorPartlst = BindMajor();
                        tdab.Isvalid = "Y";
                        TDatab.Add(tdab);
                        //tda = new Majorpart();
                        //tdab.Suplierlst = BindSupplier();
                        //tdab.SupName = dtt.Rows[i]["SUPPLIERID"].ToString();
                        //tdab.SupplierPart = dtt.Rows[i]["SUPPLIERPARTNO"].ToString();
                        //tdab.PurchasePrice = dtt.Rows[i]["SPURPRICE"].ToString();
                        //tdab.Delivery = dtt.Rows[i]["DELDAYS"].ToString();
                        //tdab.Isvalid = "Y";
                        //TDatab.Add(tdab);
                    }
                }
                DataTable dtt1 = new DataTable();
                dtt1 = datatrans.GetData("Select MACHINEINFOBASICID,MACHINECHECKROW,SERVICE,CHTYPE from MACHINECHECK Where MACHINEINFOBASICID='" + id+"'");

                if (dtt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {
                        tdau = new Checklistdetails();
                        tdau.Checklistlst = BindChecklist();
                        tdau.Service = dtt1.Rows[0]["SERVICE"].ToString();
                        tdau.Type = dtt1.Rows[0]["CHTYPE"].ToString();
                       
                       
                        tdau.Isvalid = "Y";
                        TDatau.Add(tdau);
                        //tdau = new Checklistdetails();
                        //tdau.UnitLst = BindUnit();
                        //tdau.Unit = dtt1.Rows[i]["UNIT"].ToString();
                        //tdau.cf = dtt1.Rows[i]["CF"].ToString();
                        //tdau.unittype = dtt1.Rows[i]["UNITTYPE"].ToString();
                        //tdau.uniqid = dtt1.Rows[i]["UNITUNIQUEID"].ToString();
                        //tdau.Isvalid = "Y";
                        //TDatau.Add(tdau);
                    }
                }
            }



            M.Complst = TData;
            M.Majorlst = TDatab;
            M.Checklistlst = TDatau;
            return View(M);
        }
      
        public IActionResult Machinelist()
        {
            return View();
        }


        public ActionResult MyListMachinegrid(string strStatus)
        {
            List<MachineList> Reg = new List<MachineList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = Mach.GetAllMachine(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Regenerate = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {


                    EditRow = "<a href=Machine?id=" + dtUsers.Rows[i]["MACHINEINFOBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";


                    ViewRow = "<a href=ViewMachine?id=" + dtUsers.Rows[i]["MACHINEINFOBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";


                    DeleteRow = "  href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["MACHINEINFOBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";


                    DeleteRow = "<a href=DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["MACHINEINFOBASICID"].ToString() + "><img src='../Images/active.png' alt='Activate' /></a>";
                }




                Reg.Add(new MachineList
                {
                    //id = dtUsers.Rows[i]["MACHINEINFOBASICID"].ToString(),
                    mcode = dtUsers.Rows[i]["MCODE"].ToString(),
                    mname = dtUsers.Rows[i]["MNAME"].ToString(),




                    mloc = dtUsers.Rows[i]["LOCID"].ToString(),

                    mserialno = dtUsers.Rows[i]["MSERIALNO"].ToString(),
                    mmodel = dtUsers.Rows[i]["MMODEL"].ToString(),


                    editrow = EditRow,
                    viewrow = ViewRow,
                    delrow = DeleteRow,
                    rrow = Regenerate
                });
            }
        
            return Json(new
            {
                Reg
            });

        }


        public ActionResult DeleteItem(string tag, string id)
        {
            string flag = Mach.StatusChange(tag, id);
           
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("MachineList");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("MachineList");
            }

        }
        public JsonResult GetItemJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItem());
        }
        public JsonResult GetMajorJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindMajor());
        }
        public JsonResult GetCheckJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindChecklist());
        }




        [HttpPost]
        public ActionResult Machine(Machine ss, string id)
        {

            try
            {
                ss.ID = id;
                string Strout = Mach.Homereturn(ss);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " Machine Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " Machine Updated Successfully...!";
                    }
                    return RedirectToAction("Machinelist");
                }

                else
                {
                    ViewBag.PageTitle = "Edit MachineName";
                    TempData["notice"] = Strout;
                    //return View();
                }


                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(ss);
        }
        public List<SelectListItem> Bindloc()
        {
            try
            {
                DataTable dtDesg = datatrans.GetLocation();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["LOCID"].ToString(), Value = dtDesg.Rows[i]["LOCDETAILSID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> Bindwrkcent()
        {
            try
            {
                DataTable dtDesg = datatrans.GetWorkCenter();
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

        public List<SelectListItem> Bindsub()
        {
            try
            {
                DataTable dtDesg = datatrans.BindProcess();
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


        public List<SelectListItem> BindMade()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "TIWAN", Value = "TIWAN" });
                lstdesg.Add(new SelectListItem() { Text = "USA", Value = "USA" });
                lstdesg.Add(new SelectListItem() { Text = "INDIA", Value = "INDIA" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<SelectListItem> BindType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "MACHANICAL", Value = "MACHANICAL" });
                lstdesg.Add(new SelectListItem() { Text = "ELECTRICAL", Value = "ELECTRICAL" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> Bindmode()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "NORMAL", Value = "NORMAL" });
                lstdesg.Add(new SelectListItem() { Text = "CRITICAL", Value = "CRITICAL" });
                lstdesg.Add(new SelectListItem() { Text = "BOTH", Value = "BOTH" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //2nd tab
        public List<SelectListItem> Bindcap()
        {
            try
            {
                DataTable dtDesg = Mach.GetUnit();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["UNITID"].ToString(), Value = dtDesg.Rows[i]["UNITMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = Mach.GetItem();
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

        public List<SelectListItem> BindMajor()
        {
            try
            {
                DataTable dtDesg = Mach.GetMajor();
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


        public List<SelectListItem> BindChecklist()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "MACHANICAL", Value = "MACHANICAL" });
                lstdesg.Add(new SelectListItem() { Text = "ELECTRICAL", Value = "ELECTRICAL" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public IActionResult ViewMachine(string id)
        {
            Machine M = new Machine();
            List<Compdetails> TData = new List<Compdetails>();
            Compdetails tda = new Compdetails();

            List<Majorpart> TDatab = new List<Majorpart>();
            Majorpart tdab = new Majorpart();

            List<Checklistdetails> TDatau = new List<Checklistdetails>();
            Checklistdetails tdau = new Checklistdetails();
            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select MAINTHRS.UNITID AS MAINTAIN,RUNHR.UNITID AS RUNHRS,EUNIT.UNITID AS ESUNIT, LUNIT.UNITID AS LEAD, UNIT.UNITID AS POW,UNITMAST.UNITID,LOCDETAILS.LOCID,WCBASIC.WCID,MACHINEINFOBASICID,MCODE,MNAME,MACHINEINFOBASIC.MLOCATION,MACHINEINFOBASIC.MWCID,MSUPPLY,MSERIALNO,MMODEL,MMANFNAME,MSPROCESSID,MMADEIN,MMODEOFPUR,MSERVDET,MSERCOMP,MINCHARGE,AUXYN,MMODE,MCAPACITY,MACHINEINFOBASIC.MUNIT,ESTLIFE,MACHINEINFOBASIC.EUNIT,POWKVA,MACHINEINFOBASIC.PUNIT,POWUTILFAC,RUNHR,MACHINEINFOBASIC.MCID,LEADHRS,MACHINEINFOBASIC.LEADUNIT,MAINTHRS,MACHINEINFOBASIC.MAINTUNI,to_char(MPURDT,'dd-MON-yyyy')MPURDT,to_char(MSUPDT,'dd-MON-yyyy')MSUPDT,to_char(MINSTDT,'dd-MONyyyy')MINSTDT,to_char(MLAMAINDT,'dd-MON-yyyy')MLAMAINDT,to_char(MNEMAINDT,'dd-MON-yyyy')MNEMAINDT,MACLCOST,DEPPER,INTPER,SALOP,MAINCOST,RENTMAC,TOT,POWRATEUNIT,FIXCOSTHR,RATESQFT,DEPVAL,INTVAL,INSVAL,POWUTILHR,MSPROCESSID from MACHINEINFOBASIC LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=MACHINEINFOBASIC.MWCID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=MACHINEINFOBASIC.MLOCATION LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=MACHINEINFOBASIC.MUNIT LEFT OUTER JOIN UNITMAST UNIT ON UNIT.UNITMASTID=MACHINEINFOBASIC.PUNIT LEFT OUTER JOIN UNITMAST LUNIT ON LUNIT.UNITMASTID=MACHINEINFOBASIC.LEADUNIT LEFT OUTER JOIN UNITMAST EUNIT ON EUNIT.UNITMASTID=MACHINEINFOBASIC.EUNIT LEFT OUTER JOIN UNITMAST RUNHR ON RUNHR.UNITMASTID=MACHINEINFOBASIC.MCID LEFT OUTER JOIN UNITMAST MAINTHRS ON MAINTHRS.UNITMASTID=MACHINEINFOBASIC.MAINTUNI WHERE MACHINEINFOBASICID='" + id + "'");

           //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                M.ID = dt.Rows[0]["MACHINEINFOBASICID"].ToString();
                M.MId = dt.Rows[0]["MCODE"].ToString();
                M.MName = dt.Rows[0]["MNAME"].ToString();
                M.MLoc = dt.Rows[0]["LOCID"].ToString();
                M.MWrkCent = dt.Rows[0]["WCID"].ToString();

                M.MSupply = dt.Rows[0]["MSUPPLY"].ToString();
                M.MSerial = dt.Rows[0]["MSERIALNO"].ToString();
                M.MModel = dt.Rows[0]["MMODEL"].ToString();
                M.MManname = dt.Rows[0]["MMANFNAME"].ToString();
                M.MSubProc = dt.Rows[0]["MSPROCESSID"].ToString();
                M.MMade = dt.Rows[0]["MMADEIN"].ToString();
                M.MPur = dt.Rows[0]["MMODEOFPUR"].ToString();
                M.MSer = dt.Rows[0]["MSERVDET"].ToString();
                M.MSerCmp = dt.Rows[0]["MSERCOMP"].ToString();
                M.MIncharge = dt.Rows[0]["MINCHARGE"].ToString();
                //M.MMType = dt.Rows[0]["CITY"].ToString();
                M.Aux = dt.Rows[0]["AUXYN"].ToString();
                //M.MUse = dt.Rows[0]["CITY"].ToString();
                M.MMMode = dt.Rows[0]["MMODE"].ToString();
                M.MCap = dt.Rows[0]["MCAPACITY"].ToString();
                M.MCapUnit = dt.Rows[0]["UNITID"].ToString();
                M.MELife = dt.Rows[0]["ESTLIFE"].ToString();
                M.MELifeUnit = dt.Rows[0]["ESUNIT"].ToString();
                M.MPower = dt.Rows[0]["POWKVA"].ToString();
                M.MPowerUnit = dt.Rows[0]["POW"].ToString();
                M.MPFactor = dt.Rows[0]["POWUTILFAC"].ToString();
                M.MRun = dt.Rows[0]["RUNHR"].ToString();
                //M.MRunUnit = dt.Rows[0]["CITY"].ToString();
                //M.MRunH = dt.Rows[0]["CITY"].ToString();
                M.MRunHUnit = dt.Rows[0]["RUNHRS"].ToString();
                M.MLead = dt.Rows[0]["LEADHRS"].ToString();
                M.MLeadUnit = dt.Rows[0]["LEAD"].ToString();
                M.MMaintain = dt.Rows[0]["MAINTHRS"].ToString();
                M.MMaintainUnit = dt.Rows[0]["MAINTAIN"].ToString();
                M.DOP = dt.Rows[0]["MPURDT"].ToString();
                M.DOS = dt.Rows[0]["MSUPDT"].ToString();
                M.InsDate = dt.Rows[0]["MINSTDT"].ToString();
                M.DOLMain = dt.Rows[0]["MLAMAINDT"].ToString();
                M.NMainDate = dt.Rows[0]["MNEMAINDT"].ToString();
                M.MLCost = dt.Rows[0]["MACLCOST"].ToString();
                M.Dep = dt.Rows[0]["DEPPER"].ToString();
                M.Int = dt.Rows[0]["INTPER"].ToString();
                M.SOYear = dt.Rows[0]["SALOP"].ToString();
                M.MCYear = dt.Rows[0]["MAINCOST"].ToString();
                M.Rent = dt.Rows[0]["RENTMAC"].ToString();
                M.Tot = dt.Rows[0]["TOT"].ToString();
                M.PCUnit = dt.Rows[0]["POWRATEUNIT"].ToString();
                //M.AvgCost = dt.Rows[0]["CITY"].ToString();
                M.FixCost = dt.Rows[0]["FIXCOSTHR"].ToString();
                M.CostRH = dt.Rows[0]["RATESQFT"].ToString();
                //M.PUtilize = dt.Rows[0]["CITY"].ToString();
                M.DepValue = dt.Rows[0]["DEPVAL"].ToString();
                M.IntValue = dt.Rows[0]["INTVAL"].ToString();
                M.InsValue = dt.Rows[0]["INSVAL"].ToString();
                M.PCostH = dt.Rows[0]["POWUTILHR"].ToString();
                M.AddMCost = dt.Rows[0]["MSPROCESSID"].ToString();
            }
            DataTable dt2 = new DataTable();

            dt2 = datatrans.GetData("SELECT MACHINEINFOBASICID,PARTNO,LIFETIME,WARRANTYTILLDT,ITEMMASTER.ITEMID from COMPDETAILS  left outer join ITEMMASTER ON ITEMMASTERID=COMPDETAILS.PARTNO Where MACHINEINFOBASICID='" + id + "'");

            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt2.Rows.Count; i++)
                {



                    tda = new Compdetails();
                    tda.PartNumber = dt2.Rows[0]["ITEMID"].ToString();
                    tda.LifeTimeInHrs = dt2.Rows[0]["LIFETIME"].ToString();
                    tda.DateOfIssue = dt2.Rows[0]["WARRANTYTILLDT"].ToString();
                    tda.Itemlst = BindItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
            }

            DataTable dtt = new DataTable();
            dtt = datatrans.GetData("SELECT MACHINEINFOBASICID,MCMAJORPARTSROW,MPARTID,ACTIVEYN,MCMAJORPARTS.RUNHRS,CRYN ,ITEMMASTER.ITEMID from MCMAJORPARTS  left outer join ITEMMASTER ON ITEMMASTERID=MCMAJORPARTS.MPARTID Where MACHINEINFOBASICID='" + id + "'");

            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tdab = new Majorpart();
                    tdab.Majorparts = dtt.Rows[0]["ITEMID"].ToString();
                    tdab.Active = dtt.Rows[0]["ACTIVEYN"].ToString();
                    tdab.Critical = dtt.Rows[0]["CRYN"].ToString();
                    tdab.MajorPartlst = BindMajor();
                    tdab.Isvalid = "Y";
                    TDatab.Add(tdab);
                    //tda = new Majorpart();
                    //tdab.Suplierlst = BindSupplier();
                    //tdab.SupName = dtt.Rows[i]["SUPPLIERID"].ToString();
                    //tdab.SupplierPart = dtt.Rows[i]["SUPPLIERPARTNO"].ToString();
                    //tdab.PurchasePrice = dtt.Rows[i]["SPURPRICE"].ToString();
                    //tdab.Delivery = dtt.Rows[i]["DELDAYS"].ToString();
                    //tdab.Isvalid = "Y";
                    //TDatab.Add(tdab);
                }
            }
            DataTable dtt1 = new DataTable();
            dtt1 = datatrans.GetData("Select MACHINEINFOBASICID,MACHINECHECKROW,SERVICE,CHTYPE from MACHINECHECK Where MACHINEINFOBASICID='" + id + "'");

            if (dtt1.Rows.Count > 0)
            {
                for (int i = 0; i < dtt1.Rows.Count; i++)
                {
                    tdau = new Checklistdetails();
                    tdau.Checklistlst = BindChecklist();
                    tdau.Service = dtt1.Rows[0]["SERVICE"].ToString();
                    tdau.Type = dtt1.Rows[0]["CHTYPE"].ToString();


                    tdau.Isvalid = "Y";
                    TDatau.Add(tdau);
                    //tdau = new Checklistdetails();
                    //tdau.UnitLst = BindUnit();
                    //tdau.Unit = dtt1.Rows[i]["UNIT"].ToString();
                    //tdau.cf = dtt1.Rows[i]["CF"].ToString();
                    //tdau.unittype = dtt1.Rows[i]["UNITTYPE"].ToString();
                    //tdau.uniqid = dtt1.Rows[i]["UNITUNIQUEID"].ToString();
                    //tdau.Isvalid = "Y";
                    //TDatau.Add(tdau);
                }
            }

            M.Complst = TData;
            M.Majorlst = TDatab;
            M.Checklistlst = TDatau;
            return View(M);
        }

    }


}
