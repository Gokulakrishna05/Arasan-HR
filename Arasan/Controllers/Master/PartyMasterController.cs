using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Arasan.Interface.Master;
using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Services.Master;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
//using DocumentFormat.OpenXml.Bibliography;

namespace Arasan.Controllers.Master
{
    public class PartyMasterController : Controller
    {

        IPartyMasterService PartyMasterService;
       
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public PartyMasterController(IPartyMasterService _PartyMasterService, IConfiguration _configuratio)
        {
            PartyMasterService = _PartyMasterService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult PartyMaster(string id)
        {
            PartyMaster ca = new PartyMaster();

            ca.createby = Request.Cookies["UserName"];
            ca.branch = Request.Cookies["BranchId"];
            ca.Countrylst = BindCountry();
            ca.Statelst = BindState();
            ca.Citylst = BindCity();
            ca.assignList = BindEmp();
            ca.Categorylst = BindCategory();
            ca.Ledgerlst = BindLedger();
            ca.ratelst = BindRateCode();
            ca.commlst = BindCommCode();
            ca.typelst = Bindpartytype();
            ca.saleloclst = Bindsalloc();
            ca.saleperlst = Bindsalep();
            ca.grouplist = BindPartyGroup();
            ca.concodelst = Bindconcode("");
            ca.Regular = "NO";
            List<PartyItem> TData = new List<PartyItem>();
            PartyItem tda = new PartyItem();
            List<ratedet> TData1 = new List<ratedet>();
            ratedet tda1 = new ratedet();
            List<shipping> TDatas = new List<shipping>();
            shipping tdas = new shipping();
            if (id == null)
            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new PartyItem();
                    tda.purlst = Bindpur();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new ratedet();
                    tda1.ratelist = BindRatetype();
                    tda1.ratecodelist = BindRateCode();
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 1; i++)
                {
                    tdas = new shipping();
                    tdas.statelst = BindState();
                  
                    tdas.Isvalid = "Y";
                    TDatas.Add(tdas);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);

                DataTable dt = new DataTable();

                dt = PartyMasterService.GetParty(id);
                if (dt.Rows.Count > 0)
                {
                    ca.createby = Request.Cookies["UserName"];
                    ca.branch = Request.Cookies["BranchId"];
                    ca.PartyCode = dt.Rows[0]["PARTYID"].ToString();
                    ca.PartyName = dt.Rows[0]["PARTYNAME"].ToString();
                    ca.PartyCategory = dt.Rows[0]["PARTYCAT"].ToString();
                    //ca.PartyType = dt.Rows[0]["TYPE"].ToString();
                    ca.ID = id;
                    ca.ConPartyID = dt.Rows[0]["CSGNPARTYID"].ToString();
                    ca.Comm = dt.Rows[0]["COMMCODE"].ToString();
                    ca.CreditLimit = dt.Rows[0]["CREDITLIMIT"].ToString();
                    ca.CreditDate = dt.Rows[0]["CREDITDAYS"].ToString();
                    ca.TransationLimit = dt.Rows[0]["TRANSLMT"].ToString();
                    ca.RateCode = dt.Rows[0]["RATECODE"].ToString();
                    ca.Regular = dt.Rows[0]["REGULARYN"].ToString();
                   
                    ca.Active = dt.Rows[0]["ACTIVE"].ToString();
                    ca.GST = dt.Rows[0]["GSTNO"].ToString();
                    ca.PartyGroup = dt.Rows[0]["PARTYCAT"].ToString(); //PARTYGROUP
                    ca.SectionID = dt.Rows[0]["SECTIONID"].ToString();
                    ca.LUTDate = dt.Rows[0]["LUTDT"].ToString();
                    ca.JoinDate = dt.Rows[0]["PJOINDATE"].ToString();
                    ca.LUTNumber = dt.Rows[0]["LUTNO"].ToString();
                    ca.Mobile = dt.Rows[0]["MOBILE"].ToString();
                    ca.Phone = dt.Rows[0]["PHONENO"].ToString();
                    ca.PanNumber = dt.Rows[0]["PANNO"].ToString();
                    ca.City = dt.Rows[0]["CITY"].ToString();
                    ca.State = dt.Rows[0]["STATE"].ToString();
                    ca.Country = dt.Rows[0]["COUNTRY"].ToString();
                    ca.Pincode = dt.Rows[0]["PINCODE"].ToString();
                    ca.concodelst = Bindconcode(ca.Country);
                    ca.CountryCode = dt.Rows[0]["COUNTRYCODE"].ToString();
                    ca.Email = dt.Rows[0]["EMAIL"].ToString();
                    ca.Fax = dt.Rows[0]["FAX"].ToString();
                    ca.Commisionerate = dt.Rows[0]["COMMISIONERATE"].ToString();
                    ca.Range = dt.Rows[0]["RANGEDIVISION"].ToString();
                    ca.EccID = dt.Rows[0]["ECCNO"].ToString();
                    ca.Excise = dt.Rows[0]["EXCISEAPPLICABLE"].ToString();
                    //ca.Type = dt.Rows[0]["PARTYTYPE"].ToString();
                    ca.Http = dt.Rows[0]["HTTP"].ToString();
                    ca.OverDueInterest = dt.Rows[0]["OVERDUEINTEREST"].ToString();
                    ca.Address = dt.Rows[0]["ADD1"].ToString();
                    ca.Address2 = dt.Rows[0]["ADD2"].ToString();
                    ca.Address3 = dt.Rows[0]["ADD3"].ToString();
                    ca.salloc = dt.Rows[0]["SALLOC"].ToString();
                    ca.salper = dt.Rows[0]["SALPERNAME"].ToString();
                    ca.Remark = dt.Rows[0]["REMARKS"].ToString();
                    ca.Intred = dt.Rows[0]["INTRODUCEDBY"].ToString();
                    ca.Ledger = dt.Rows[0]["ACCOUNTNAME"].ToString();
                    ca.typelst = Bindpartytype();
                    ca.Type = dt.Rows[0]["TYPE"].ToString();
                    

                }
                DataTable dt2 = new DataTable();
                dt2 = PartyMasterService.GetPartyContact(id);
                if (dt2.Rows.Count > 0)
                { 
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new PartyItem();
                        tda.purlst = Bindpur();
                        tda.Purpose = dt2.Rows[i]["CONTACTPURPOSE"].ToString();
                        tda.ContactPerson = dt2.Rows[i]["CONTACTNAME"].ToString();
                        tda.Designation = dt2.Rows[i]["CONTACTDESIG"].ToString();
                        tda.CPhone = dt2.Rows[i]["CONTACTPHONE"].ToString();
                        tda.CEmail = dt2.Rows[i]["CONTACTEMAIL"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);


                    }
                }
                DataTable dt3 = new DataTable();
                dt3 = datatrans.GetData("SELECT PARTYMASTID,PARTYMASTBRCODEROW,BRATECODE,BRATETYPE,BRATEDESC,ACCNAME,ACOUNTRY FROM PARTYMASTBRCODE WHERE PARTYMASTID='"+id+"'");
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new ratedet();
                     
                        tda1.ratecodelist = BindRateCode();
                        tda1.ratecode = dt3.Rows[i]["BRATECODE"].ToString();
                        tda1.ratelist = BindRatetype();
                        tda1.ratetype = dt3.Rows[i]["BRATETYPE"].ToString();
                        tda1.acco = dt3.Rows[i]["ACOUNTRY"].ToString();
                        
                        tda1.Isvalid = "Y";
                        TData1.Add(tda1);

                    }
                }
                DataTable dt4 = new DataTable();
                dt4 = datatrans.GetData("SELECT ADDBOOKTYPE,ADDBOOKCOMPANY,SPHONE,SEMAIL,SADD1,SADD2,SADD3,SCITY,SSTATE,SPINCODE,SGSTNO FROM PARTYMASTADDRESS WHERE PARTYMASTID='" + id + "'");
                if (dt4.Rows.Count > 0)
                {
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        tdas = new shipping();

                      
                        tdas.addtype = dt4.Rows[i]["ADDBOOKTYPE"].ToString();
                         tdas.consingn = dt4.Rows[i]["ADDBOOKCOMPANY"].ToString();
                        tdas.add1 = dt4.Rows[i]["SADD1"].ToString();
                        tdas.add2 = dt4.Rows[i]["SADD2"].ToString();
                        tdas.add3 = dt4.Rows[i]["SADD3"].ToString();
                        tdas.email = dt4.Rows[i]["SEMAIL"].ToString();
                        tdas.phone = dt4.Rows[i]["SPHONE"].ToString();
                        tdas.pincode = dt4.Rows[i]["SPINCODE"].ToString();
                        tdas.gstno = dt4.Rows[i]["SGSTNO"].ToString();
                        tdas.statelst = BindState();
                        tdas.state = dt4.Rows[i]["SSTATE"].ToString();
                        tdas.city = dt4.Rows[i]["SCITY"].ToString();
                         tdas.Isvalid = "Y";
                        TDatas.Add(tdas);

                    }
                }
            }
                   ca.PartyLst = TData;
                   ca.rateLst = TData1;
                   ca.shLst = TDatas;
            return View(ca);
        }
        [HttpPost]
        public ActionResult PartyMaster(PartyMaster emp, string id)    
        {  

            try
            {
                emp.ID = id;
                string Strout = PartyMasterService.PartyCRUD(emp);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (emp.ID == null)
                    {
                        TempData["notice"] = " PartyMaster Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " PartyMaster Updated Successfully...!";
                    }
                    return RedirectToAction("ListParty");
                }

                else
                {
                    ViewBag.PageTitle = "Edit PartyMaster";
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
        public List<SelectListItem> Bindpur()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "Purchase", Value = "Purchase" });
                lstdesg.Add(new SelectListItem() { Text = "QC", Value = "QC" });
                lstdesg.Add(new SelectListItem() { Text = "Payments", Value = "Payments" });
                lstdesg.Add(new SelectListItem() { Text = "Accounts", Value = "Accounts" });
                lstdesg.Add(new SelectListItem() { Text = "Sales", Value = "Sales" });
                lstdesg.Add(new SelectListItem() { Text = "Stores", Value = "Stores" });


                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindRatetype()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "OUTRIGHT", Value = "OUTRIGHT" });
                lstdesg.Add(new SelectListItem() { Text = "CONVERSION", Value = "CONVERSION" });
 


                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLedger()
        {
            try
            {
                DataTable dtDesg = PartyMasterService.GetLedger();
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
        public List<SelectListItem> Bindsalloc()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT LOCID,LOCDETAILSID FROM LOCDETAILS WHERE LOCATIONTYPE IN ('FG GODOWN','STORES')");
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
        public List<SelectListItem> Bindsalep()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT NEMPID,EMPMASTID FROM EMPMAST");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["NEMPID"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Bindconcode(string id)
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT CONCODE,CONMASTID FROM CONMAST WHERE COUNTRY='"+id+"'");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CONCODE"].ToString(), Value = dtDesg.Rows[i]["CONMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindCommCode()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMCODE FROM COMMBASIC");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COMMCODE"].ToString(), Value = dtDesg.Rows[i]["COMMCODE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindRateCode()
        {
            try
            {
                DataTable dtDesg = PartyMasterService.Getratecode();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["RATECODE"].ToString(), Value = dtDesg.Rows[i]["RATECODE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<SelectListItem> BindState()
        //{
        //    try
        //    {
        //        DataTable dtDesg = datatrans.GetData("SELECT STATE FROM STATEMAST");
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE"].ToString(), Value = dtDesg.Rows[i]["STATE"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> BindCategory()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "CUSTOMER", Value = "CUSTOMER" });
                lstdesg.Add(new SelectListItem() { Text = "SUPPLIER", Value = "SUPPLIER" });
                lstdesg.Add(new SelectListItem() { Text = "BOTH", Value = "BOTH" });
                lstdesg.Add(new SelectListItem() { Text = "TRANSPORTER", Value = "TRANSPORTER" });
                lstdesg.Add(new SelectListItem() { Text = "BRANCH", Value = "BRANCH" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Bindpartytype()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='PARTYTYPE' ");
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
        public List<SelectListItem> BindState()
        {
            try
            {
                DataTable dtDesg = PartyMasterService.GetState();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["STATE"].ToString(), Value = dtDesg.Rows[i]["STATE"].ToString() });
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
        public List<SelectListItem> BindPartyGroup()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("SELECT COMMON_VALUE FROM COMMONMASTER WHERE COMMON_TEXT='PARTYGROUP' ");
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
        public List<SelectListItem> BindCountry()
        {
            try
            {
                DataTable dtDesg = PartyMasterService.GetCountry();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["COUNTRY"].ToString(), Value = dtDesg.Rows[i]["COUNTRY"].ToString() });
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
        public IActionResult ListParty()
        {
            return View();
        }
        public JsonResult GetItemGrpJSON()
        {
            PartyMaster model = new PartyMaster();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(Bindpur());
        }
        public JsonResult GetStateJSON()
        {
            PartyMaster model = new PartyMaster();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindState());
        }
        public JsonResult GetrateJSON()
        {
            PartyMaster model = new PartyMaster();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindRatetype());
        }
        public JsonResult GetratecodeJSON()
        {
            PartyMaster model = new PartyMaster();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindRateCode());
        }
        public JsonResult GetCountryDetail(string cid)
        {
            PartyMaster model = new PartyMaster();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(Bindconcode(cid));
        }


        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = PartyMasterService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListParty");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListParty");
            }
        } public ActionResult Remove(string tag, int id)
        {

            string flag = PartyMasterService.RemoveChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListParty");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListParty");
            }
        }

            public ActionResult MyListItemgrid(string strStatus)
            {
                List<PartyGrid> Reg = new List<PartyGrid>();
                DataTable dtUsers = new DataTable();
                strStatus = strStatus == "" ? "Y" : strStatus;
                dtUsers = PartyMasterService.GetAllParty(strStatus);
                for (int i = 0; i < dtUsers.Rows.Count; i++)
                {

                    string DeleteRow = string.Empty;
                    string EditRow = string.Empty;

                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=PartyMaster?id=" + dtUsers.Rows[i]["PARTYMASTID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                   // DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["PARTYMASTID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                    DeleteRow = "DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["PARTYMASTID"].ToString() + "";

                }
                else
                {

                    EditRow = "";
                    //DeleteRow = "<a href=Remove?tag=Del&id=" + dtUsers.Rows[i]["PARTYMASTID"].ToString() + "><img src='../Images/close_icon.png' alt='Deactivate' /></a>";
                    DeleteRow = "Remove?tag=Active&id=" + dtUsers.Rows[i]["PARTYMASTID"].ToString() + "";


                }

                Reg.Add(new PartyGrid
                    {
                        id = dtUsers.Rows[i]["PARTYMASTID"].ToString(),
                        partyname = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                        partycategory = dtUsers.Rows[i]["PARTYCAT"].ToString(),
                        partygroup = dtUsers.Rows[i]["PARTYGROUP"].ToString(),
                        joindate = dtUsers.Rows[i]["PJOINDATE"].ToString(),
                        ratecode = dtUsers.Rows[i]["RATECODE"].ToString(),
                        editrow = EditRow,
                        delrow = DeleteRow,

                    });
                }

                return Json(new
                {
                    Reg
                });

            }

        public IActionResult AddPartyType(string id)
        {
            PartyMaster ca = new PartyMaster();
            // ca.Brlst = BindBranch();

            return View(ca);
        }
        public JsonResult SavePartyType(string category)
        {
            string Strout = PartyMasterService.PartyTypeCRUD(category);
            var result = new { msg = Strout };
            return Json(result);
        }
        public JsonResult GetPartyTypeJSON()
        {
            return Json(Bindpartytype());
        }
        public IActionResult AddCity(string id)
        {
            PartyMaster ca = new PartyMaster();
            // ca.Brlst = BindBranch();

            return View(ca);
        }
        public JsonResult SaveCity(string category)
        {
            string Strout = PartyMasterService.CityCRUD(category);
            var result = new { msg = Strout };
            return Json(result);
        }
        public JsonResult GetCityJSON()
        {
            return Json(BindCity());
        }
        public IActionResult AddPartyGroup(string id)
        {
            PartyMaster ca = new PartyMaster();
            // ca.Brlst = BindBranch();

            return View(ca);
        }
        public JsonResult SavePartyGroup(string category)
        {
            string Strout = PartyMasterService.PartyGroup(category);
            var result = new { msg = Strout };
            return Json(result);
        }
        public JsonResult GetPartyGroupJSON()
        {
            return Json(BindPartyGroup());
        }
    }
 }

