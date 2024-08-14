using Arasan.Models;
using System.Collections.Generic;
using System.Collections;
using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using System.Reflection.PortableExecutable;
using Intuit.Ipp.Data;
using Arasan.Services;
using System.Net;
using Microsoft.VisualBasic;
using PdfSharp.Drawing;

namespace Arasan.Controllers
{
    public class ServicePOController : Controller
    {
        IServicePO SPO;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public ServicePOController(IServicePO _SPO, IConfiguration _configuratio)
        {
            SPO = _SPO;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
       
    public IActionResult ServicePO(String id)
        {
        ServicePOModel S = new ServicePOModel();

            S.CurrLst = BindCurrency();//currency
            S.PartyIdLst = BindPartyID();//partymast
            S.PreparedLst = BindPrepared();//empmast
            S.TermLst = BindTemp();
            S.SendLst = BindPrepared();
            S.ReceiveLst = BindPrepared();

            List<ServicePO> TData = new List<ServicePO>();
            ServicePO tda = new ServicePO();

            List<ServicePOAdditional> TDatab = new List<ServicePOAdditional>();
            ServicePOAdditional tdab = new ServicePOAdditional();

            List<TermsAndCondition> TDatac = new List<TermsAndCondition>();
            TermsAndCondition tdac = new TermsAndCondition();



            DataTable dtv = datatrans.GetSequence("SerPo");
            if (dtv.Rows.Count > 0)  
            {
                S.PONo = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            if (id == null)

            {
                for (int i = 0; i < 1; i++)
                {
                    tda = new ServicePO();
                    tda.ServiceIDlst = BindServiceID();
                    tda.Unitlst = BindUnit();//unitmast
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tdab = new ServicePOAdditional();
                    tdab.AddDedeclst = BindAddDedec();
                    tdab.Isvalid = "Y";
                    TDatab.Add(tdab);
                }
                for (int i = 0; i < 1; i++)
                {
                    tdac = new TermsAndCondition();
                    tdac.TAClst = BindTAC();

                    tdac.Isvalid = "Y";
                    TDatac.Add(tdac);
                }


            }
            else
            {
                DataTable dt = new DataTable();

                dt = SPO.GetSPOBasicEdit(id);  
                if (dt.Rows.Count > 0)
                {
                    S.PONo = dt.Rows[0]["DOCID"].ToString();
                    S.PODate = dt.Rows[0]["DOCDATE"].ToString();
                    S.Curr = dt.Rows[0]["MAINCURRENCY"].ToString();
                    S.Symbol = dt.Rows[0]["SYMBOL"].ToString();
                    S.ExRate = dt.Rows[0]["EXRATE"].ToString();
                    S.RefNo = dt.Rows[0]["REFNO"].ToString();
                    S.RefDate = dt.Rows[0]["REFDT"].ToString();
                    S.PartyId = dt.Rows[0]["PARTYID"].ToString();
                    S.Party = dt.Rows[0]["PARTYNAME"].ToString();
                    S.Address1 = dt.Rows[0]["ADD1"].ToString();
                    S.Address2 = dt.Rows[0]["ADD2"].ToString();
                    S.Address3 = dt.Rows[0]["ADD3"].ToString();
                    S.Email = dt.Rows[0]["EMAIL"].ToString();
                    S.Mobile = dt.Rows[0]["MOBILE"].ToString();
                    //S.Tax = dt.Rows[0]["DEPARTMENT"].ToString();
                    //S.Prepared = dt.Rows[0]["DEPARTMENT"].ToString();
                    S.Term = dt.Rows[0]["TERMSTEMPLATE"].ToString();
                    S.DueDate = dt.Rows[0]["DUEDATE"].ToString();
                    S.Net = dt.Rows[0]["NET"].ToString();

                }
                DataTable dt1 = new DataTable();

                dt1 = SPO.GetSPOOrganizerEdit(id);  
                if (dt.Rows.Count > 0)
                {
                    S.Send = dt1.Rows[0]["POSENTBY"].ToString();
                    S.Receive = dt1.Rows[0]["ASSIGNTO"].ToString();
                    S.FollowupDate = dt1.Rows[0]["FOLLOWUPDT"].ToString();
                     S.FollowupDetails = dt1.Rows[0]["REMARKS"].ToString();
                     

                }
                DataTable dt2 = new DataTable();

                dt2 = SPO.GetSPODetailEdit(id);   


                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda.ServiceIDlst = BindServiceID();
                        tda.Unitlst = BindUnit(); 
                        tda.Isvalid = "Y";
                      

                        tda.ServiceID = dt2.Rows[0]["SERVICEID"].ToString();
                        tda.ServiceDesc = dt2.Rows[0]["SERVICEDESC"].ToString();
                        tda.Unit = dt2.Rows[0]["UNIT"].ToString();
                        tda.Qty = dt2.Rows[0]["QTY"].ToString();
                        tda.Rate = dt2.Rows[0]["RATE"].ToString();
                        tda.Amount = dt2.Rows[0]["AMOUNT"].ToString();
                        
                        TData.Add(tda);
                    }


                }
                DataTable dt3 = new DataTable();

                dt3 = SPO.GetSPOTandcEdit(id);


                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        
                        tdac.Isvalid = "Y";


                        tdac.TAC = dt3.Rows[0]["TERMSANDCOND"].ToString();
                        

                        TDatac.Add(tdac);
                    }


                }


            }
            S.ServicePOlist = TData;

            S.ServicePOAdditionallist = TDatab;
            S.TermsAndConditionlist = TDatac;


        return View(S);
    }
        public List<SelectListItem> BindCurrency()
        {
            try
            {
                DataTable dtDesg = SPO.GetCurrency();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["MAINCURR"].ToString(), Value = dtDesg.Rows[i]["CURRENCYID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindTemp()
        {
            try
            {
                DataTable dtDesg = SPO.GetTemp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TEMPLATEDESC"].ToString(), Value = dtDesg.Rows[i]["TESTTBASICID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //for ajax //SELECT AND TO DISPLAY OTHERS AUTOMATICALLY
        public ActionResult GetCurrencyDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string cur = "";
                string sym = "";

                dt = datatrans.GetData("SELECT SYMBOL FROM CURRENCY WHERE CURRENCYID='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {

                     sym = dt.Rows[0]["SYMBOL"].ToString();

                }

                var result = new {   sym = sym };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<SelectListItem> BindPartyID()
        {
            try
            {
                DataTable dtDesg = SPO.GetPartyID();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYID"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public ActionResult GetPartyDetails(string ItemIds)
        {
            try
            {
                DataTable dt = new DataTable();

                string name = "";
                string add1 = "";
                string add2 = "";
                string add3 = "";
                string email = "";
                string mobile = "";

                dt = datatrans.GetData("SELECT PARTYNAME,ADD1,ADD2,ADD3,EMAIL,MOBILE FROM PARTYMAST WHERE PARTYMASTID='" + ItemIds + "'");

                if (dt.Rows.Count > 0)
                {

                    name = dt.Rows[0]["PARTYNAME"].ToString();
                    add1 = dt.Rows[0]["ADD1"].ToString();
                    add2 = dt.Rows[0]["ADD2"].ToString();
                    add3 = dt.Rows[0]["ADD3"].ToString();
                    email = dt.Rows[0]["EMAIL"].ToString();
                    mobile = dt.Rows[0]["MOBILE"].ToString();

                }

                var result = new { name = name, add1= add1, add2= add2,add3= add3, email= email, mobile = mobile };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public List<SelectListItem> BindPrepared()
        {
            try
            {
                DataTable dtDesg = SPO.GetPrepared();
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
        public List<SelectListItem> BindTAC()
        {
            try
            {
                DataTable dtDesg = SPO.GetTAN();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TANDC"].ToString(), Value = dtDesg.Rows[i]["TANDCDETAILID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public List<SelectListItem> BindServiceID()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "ANNUAL MAINTENANCE", Value = "ANNUAL MAINTENANCE" });
                lstdesg.Add(new SelectListItem() { Text = "NIL", Value = "NIL" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindUnit()
        {
            try
            {
                DataTable dtDesg = SPO.GetUnit();
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
        public List<SelectListItem> BindAddDedec()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "GROSS", Value = "GROSS" });
                lstdesg.Add(new SelectListItem() { Text = "ROUNDOFF", Value = "ROUNDOFF" });
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public JsonResult GetTable1JSON()
        {

            return Json(BindServiceID());
        }
        public JsonResult GetTables1JSON()
        {

            return Json(BindUnit());
        }
        public JsonResult GetTable2JSON()
        {

            return Json(BindAddDedec());
        }
        public JsonResult GetTable3JSON()
        {

            return Json(BindTAC());
        }
        [HttpPost]
        public ActionResult ServicePO(ServicePOModel S, string id)
        {


            try
            {
                S.ID = id;
                string Strout = SPO.GetInsService(S);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (S.ID == null)
                    {
                        TempData["notice"] = "ServicePO Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ServicePO Updated Successfully...!";
                    }
                    return RedirectToAction("ServicePO");//list
                }
                else
                {
                    TempData["notice"] = Strout;
                    ViewBag.PageTitle = "Edit ServicePO";//list

                    return View(S);
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(S);

        }




        public IActionResult ServicePOlist()
        {
            return View();
        }


        public ActionResult MyListServicePOgrid(string strStatus)
        {
            List<ServicePOList> Reg = new List<ServicePOList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = SPO.GetAllServicePo(strStatus);

            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Regenerate = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string ViewRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {


                    EditRow = "<a href=ServicePO?id=" + dtUsers.Rows[i]["SPOBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";


                    ViewRow = "<a href=ViewServicePO?id=" + dtUsers.Rows[i]["SPOBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";


                    DeleteRow = "DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["SPOBASICID"].ToString() + "";
                }
                else
                {
                    EditRow = "";


                    DeleteRow = " DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["SPOBASICID"].ToString() + "";
                }




                Reg.Add(new ServicePOList
                {
                    id = dtUsers.Rows[i]["SPOBASICID"].ToString(),
                    pono = dtUsers.Rows[i]["DOCID"].ToString(),
                    curr = dtUsers.Rows[i]["MAINCURRENCY"].ToString(),
                    term = dtUsers.Rows[i]["TEMPLATEDESC"].ToString(),
                    duedate= dtUsers.Rows[i]["DUEDATE"].ToString(),

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
        public IActionResult DeleteItem(string tag, string id)
        {
            string flag = SPO.StatusChange(tag, id);

            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ServicePOlist");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ServicePOlist");
            }

        }
        public IActionResult ViewServicePO(string id)
        {
            ServicePOModel S = new ServicePOModel();

            List<ServicePO> TData = new List<ServicePO>();
            ServicePO tda = new ServicePO();

            List<ServicePOAdditional> TDatab = new List<ServicePOAdditional>();
            ServicePOAdditional tdab = new ServicePOAdditional();

            List<TermsAndCondition> TDatac = new List<TermsAndCondition>();
            TermsAndCondition tdac = new TermsAndCondition();

            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select S.SPOBASICID, S.DOCID, to_char(S.DOCDATE, 'dd-MON-yyyy')DOCDATE, C.MAINCURR, S.EXRATE, S.REFNO, to_char(S.REFDT, 'dd-MON-yyyy')REFDT, P.PARTYID, S.TERMSTEMPLATE, to_char(S.DUEDATE, 'dd-MON-yyyy')DUEDATE, S.NET, P.PARTYNAME, P.ADD1, P.ADD2, P.ADD3, P.EMAIL, P.MOBILE, C.SYMBOL from SPOBASIC S, PARTYMAST P, CURRENCY C WHERE S.PARTYID = P.PARTYMASTID AND S.MAINCURRENCY = C.CURRENCYID AND SPOBASICID = '" + id + "'");


            if (dt.Rows.Count > 0)
            {
                S.PONo = dt.Rows[0]["DOCID"].ToString();
                S.PODate = dt.Rows[0]["DOCDATE"].ToString();
                S.Curr = dt.Rows[0]["MAINCURR"].ToString();
                S.Symbol = dt.Rows[0]["SYMBOL"].ToString();
                S.ExRate = dt.Rows[0]["EXRATE"].ToString();
                S.RefNo = dt.Rows[0]["REFNO"].ToString();
                S.RefDate = dt.Rows[0]["REFDT"].ToString();
                S.PartyId = dt.Rows[0]["PARTYID"].ToString();
                S.Party = dt.Rows[0]["PARTYNAME"].ToString();
                S.Address1 = dt.Rows[0]["ADD1"].ToString();
                S.Address2 = dt.Rows[0]["ADD2"].ToString();
                S.Address3 = dt.Rows[0]["ADD3"].ToString();
                S.Email = dt.Rows[0]["EMAIL"].ToString();
                S.Mobile = dt.Rows[0]["MOBILE"].ToString();
                //S.Tax = dt.Rows[0]["DEPARTMENT"].ToString();
                //S.Prepared = dt.Rows[0]["DEPARTMENT"].ToString();
                S.Term = dt.Rows[0]["TERMSTEMPLATE"].ToString();
                S.DueDate = dt.Rows[0]["DUEDATE"].ToString();
                S.Net = dt.Rows[0]["NET"].ToString();

            }
            DataTable dt1 = new DataTable();
            dt1 = datatrans.GetData("Select SPOBASICID,EMPMAST.EMPNAME,NEWEMPMAST.EMPNAME,to_char(FOLLOWUPDT,'dd-MON-yyyy')FOLLOWUPDT,REMARKS from SPOORGANISER  left outer join EMPMAST ON EMPMASTID=SPOORGANISER.POSENTBY left outer join EMPMAST NEWEMPMAST ON EMPMAST.EMPMASTID=SPOORGANISER.ASSIGNTO WHERE SPOBASICID='" + id + "' ");
 
            if (dt.Rows.Count > 0)
            {
                S.Send = dt1.Rows[0]["EMPNAME"].ToString();
                S.Receive = dt1.Rows[0]["EMPNAME"].ToString();
                S.FollowupDate = dt1.Rows[0]["FOLLOWUPDT"].ToString();
                S.FollowupDetails = dt1.Rows[0]["REMARKS"].ToString();


            }

            DataTable dt2 = new DataTable();

            dt2 = datatrans.GetData("Select SPOBASICID,SERVICEID,SERVICEDESC,UNITMAST.UNITID,QTY,RATE,AMOUNT from  SPODETAIL  left outer join UNITMAST ON UNITMASTID=SPODETAIL.UNIT WHERE SPOBASICID='" + id + "'");

            
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda.ServiceIDlst = BindServiceID();
                            tda.Unitlst = BindUnit();


                            tda.ServiceID = dt2.Rows[0]["SERVICEID"].ToString();
                            tda.ServiceDesc = dt2.Rows[0]["SERVICEDESC"].ToString();
                            tda.Unit = dt2.Rows[0]["UNITID"].ToString();
                            tda.Qty = dt2.Rows[0]["QTY"].ToString();
                            tda.Rate = dt2.Rows[0]["RATE"].ToString();
                            tda.Amount = dt2.Rows[0]["AMOUNT"].ToString();
                            tda.Isvalid = "Y";

                            TData.Add(tda);
                        }
 
                    }

            DataTable dt3 = new DataTable();

            dt3 = datatrans.GetData("Select SPOBASICID,TERMSANDCOND  from  SPOTANDC WHERE SPOBASICID='" + id + "'");


            if (dt2.Rows.Count > 0)
            {
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
 
                    tdac.TAC = dt3.Rows[0]["TERMSANDCOND"].ToString();
                    tdac.TAClst = BindTAC();

                    tdac.Isvalid = "Y";

                    TDatac.Add(tdac);
                }


            }


            S.ServicePOlist = TData;

            S.ServicePOAdditionallist = TDatab;
            S.TermsAndConditionlist = TDatac;


            return View(S);

        }
    }
}
