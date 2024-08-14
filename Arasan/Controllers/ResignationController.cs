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
using System.Diagnostics.Contracts;

namespace Arasan.Controllers
{
    public class ResignationController : Controller
    {
        IResignation Resig;

        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;

        public ResignationController(IResignation _Resig, IConfiguration _configuratio)
        {
            Resig = _Resig;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }



        public IActionResult Resignation(String id)
        {
            ResignationModel A = new ResignationModel();

          
            A.EmpIDLst = BindEmpId();
            A.TReasonLst = BindTReason();
            DataTable dtv = datatrans.GetSequence("Resig");
            if (dtv.Rows.Count > 0)
            {
                A.DocId = dtv.Rows[0]["PREFIX"].ToString() + "" + dtv.Rows[0]["last"].ToString();
            }
            dtv = Resig.GetEditResignation(id);
            if (dtv.Rows.Count > 0)
            {
                A.EmpIDLst = BindEmpId();
                A.EmpID = dtv.Rows[0]["EMPID"].ToString();
                A.TReasonLst = BindTReason();
                A.TReason = dtv.Rows[0]["REASONTYPE"].ToString();
                A.DocId = dtv.Rows[0]["DOCID"].ToString();
                A.Date = dtv.Rows[0]["DOCDATE"].ToString();
                A.EmpName = dtv.Rows[0]["EMPNAME"].ToString();
                A.EmpJoin = dtv.Rows[0]["JOINDATE"].ToString();
                A.EmpResignation = dtv.Rows[0]["RESIGNDATE"].ToString();
                A.Reason = dtv.Rows[0]["REASON"].ToString();
                A.ID = id;

            }
            return View(A);
        }

      
        public List<SelectListItem> BindEmpId()
        {
            try
            {
                DataTable dtDesg = Resig.GetEmpId();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["EMPID"].ToString(), Value = dtDesg.Rows[i]["EMPMASTID"].ToString() });


                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //AUTO CHANGE BY EMPID
        public ActionResult GetEmpDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string emp = "";
                string join = "";

                dt = datatrans.GetData("SELECT EMPNAME,to_char(JOINDATE,'dd-MON-yyyy')JOINDATE FROM EMPMAST WHERE EMPMASTID='" + ItemId + "'");

                if (dt.Rows.Count > 0)
                {

                    emp = dt.Rows[0]["EMPNAME"].ToString();
                    join = dt.Rows[0]["JOINDATE"].ToString();

                }

                var result = new { emp = emp, join = join };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult MyListResignationgrid(string strStatus)
        {
            List<ResignationList> Res = new List<ResignationList>();

            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;

            dtUsers = Resig.GetAllResignation(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string ViewRow = string.Empty;
                string EditRow = string.Empty;
                if (dtUsers.Rows[i]["IS_ACTIVE"].ToString() == "Y")
                {

               
                EditRow = "<a href=Resignation?id=" + dtUsers.Rows[i]["RESIGID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                ViewRow = "<a href=ViewResignation?id=" + dtUsers.Rows[i]["RESIGID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Waiting for approval' /></a>";
                DeleteRow = "Delete?tag=Del&id=" + dtUsers.Rows[i]["RESIGID"].ToString() + "";

                //DeleteRow = "<a href=Delete?tag=Del&id=" + dtUsers.Rows[i]["RESIGID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {
                    EditRow = "";
                     DeleteRow = "Delete?tag=Act&id=" + dtUsers.Rows[i]["RESIGID"].ToString() + "";

                }

                Res.Add(new ResignationList
                {
                    id = dtUsers.Rows[i]["RESIGID"].ToString(),

                    docid = dtUsers.Rows[i]["DOCID"].ToString(),
                    empid = dtUsers.Rows[i]["EMPID"].ToString(),
                    empname = dtUsers.Rows[i]["EMPNAME"].ToString(),
                    joindate = dtUsers.Rows[i]["JOINDATE"].ToString(),
                    resignationdate = dtUsers.Rows[i]["RESIGNDATE"].ToString(),
                    editrow = EditRow,
                    viewrow = ViewRow,
                    delrow = DeleteRow,

                });
            }

            return Json(new
            {
                Res
            });

        }

        [HttpPost]
        public ActionResult Resignation(ResignationModel R, string id)
        {

            try
            {
                R.ID = id;
                string Strout = Resig.ResignationCRUD(R);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (R.ID == null)
                    {
                        TempData["notice"] = "Resignation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "Resignation Updated Successfully...!";
                    }
                    return RedirectToAction("Resignationlist");
                }

                else
                {
                    ViewBag.PageTitle = "Edit Resignation";
                    TempData["notice"] = Strout;
                    //return View();
                }

                // }
            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(R);
        }

        public IActionResult ViewResignation(string id)
        {
            ResignationModel A = new ResignationModel();

            List<ResignationList> TData = new List<ResignationList>();
            ResignationModel tda = new ResignationModel();


            DataTable dt = new DataTable();

            dt = datatrans.GetData("Select RESIGID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,EMPMAST.EMPID,RESIG.EMPNAME,to_char(RESIG.JOINDATE,'dd-MON-yyyy')JOINDATE,to_char(RESIG.RESIGNDATE,'dd-MON-yyyy')RESIGNDATE,REASONTYPE,REASON FROM RESIG left outer join EMPMAST ON EMPMASTID=RESIG.EMPID  WHERE RESIGID='" + id + "'");

            //"Select IGROUP,ISUBGROUP,ITEMGROUP,SUBGROUPCODE,SUBCATEGORY,BINNO,BINYN,LOTYN,RHYN,RUNPERQTY,RUNHRS,COSTCATEGORY,AUTOCONSYN,QCT,DRUMYN,ITEMFROM,ETARIFFMASTER.TARIFFID,PURCAT,MAJORYN,to_char(LATPURDT,'dd-MON-yyyy')LATPURDT,ITEMID,ITEMDESC,REORDERQTY,REORDERLVL,MINSTK,UNITMAST.UNITID,MASTER.MNAME,HSN,SELLINGPRICE,EXPYN,VALMETHOD,SERIALYN,BSTATEMENTYN,TESTTBASIC.TEMPLATEID,QCCOMPFLAG,LATPURPRICE,REJRAWMATPER,RAWMATPER,ADD1PER,ADD1,RAWMATCAT,ITEMACC,PTEMPLATEID,CURINGDAY,AUTOINDENT from ITEMMASTER LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=ITEMMASTER.PRIUNIT LEFT OUTER JOIN MASTER ON MASTER.MASTERID=ITEMMASTER.ITEMACC LEFT OUTER JOIN TESTTBASIC ON TESTTBASIC.TESTTBASICID=ITEMMASTER.TEMPLATEID LEFT OUTER JOIN ETARIFFMASTER ON ETARIFFMASTER.ETARIFFMASTERID=ITEMMASTER.TARIFFID   where ITEMMASTERID=" + id + "");

            if (dt.Rows.Count > 0)
            {
                A.ID = dt.Rows[0]["RESIGID"].ToString();
                A.DocId = dt.Rows[0]["DOCID"].ToString();
                A.Date = dt.Rows[0]["DOCDATE"].ToString();
                A.EmpID = dt.Rows[0]["EMPID"].ToString();
                A.EmpName = dt.Rows[0]["EMPNAME"].ToString();
                A.EmpJoin = dt.Rows[0]["JOINDATE"].ToString();
                A.EmpResignation = dt.Rows[0]["RESIGNDATE"].ToString();
                A.TReason = dt.Rows[0]["REASONTYPE"].ToString();
                A.Reason = dt.Rows[0]["REASON"].ToString();


            }
            //A.EmpResignationlist = TData;
            return View(A);

        }
        public ActionResult Delete(string tag, string id)
        {

            string flag = Resig.StatusDelete(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("Resignationlist");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("Resignationlist");
            }
        }

        public List<SelectListItem> BindTReason()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "RESIGN", Value = "RESIGN" });
                lstdesg.Add(new SelectListItem() { Text = "PF BENIFIT", Value = "PF BENIFIT" });
                
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }




        public IActionResult Resignationlist()
        {
            return View();
        }



    }
}
