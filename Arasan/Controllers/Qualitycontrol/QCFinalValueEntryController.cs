using System.Collections.Generic;
using Arasan.Interface.Master;
using Arasan.Interface.Qualitycontrol;
using Arasan.Models;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using System.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Arasan.Services.Qualitycontrol;

namespace Arasan.Controllers.Qualitycontrol
{
    public class QCFinalValueEntryController : Controller
    {
        IQCFinalValueEntryService QCFinalValueEntryService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public QCFinalValueEntryController(IQCFinalValueEntryService _QCFinalValueEntryService, IConfiguration _configuratio)
        {
            QCFinalValueEntryService = _QCFinalValueEntryService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult QCFinalValueEntry(string id,string tag)
        {
            QCFinalValueEntry ca = new QCFinalValueEntry();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Worklst = BindWorkCenter();
            ca.Enterd = Request.Cookies["UserName"];
            ca.RecList = BindEmp();
            ca.Processlst = BindProcess("");
            ca.drumlst = Binddrum();
            ca.Itemlst = BindItemlst("");
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Batchlst = BindBatch("");
            DataTable dtv = datatrans.GetSequence("FQTVE");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            List<QCFinalValueEntryItem> TData = new List<QCFinalValueEntryItem>();
            QCFinalValueEntryItem tda = new QCFinalValueEntryItem();
            List<QCFVItemDeatils> TData1 = new List<QCFVItemDeatils>();
            QCFVItemDeatils tda1 = new QCFVItemDeatils();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new QCFinalValueEntryItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda1 = new QCFVItemDeatils();
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }

            }
            else
            {
                DataTable dt = new DataTable();
                if (tag == null)
                {
                    dt = QCFinalValueEntryService.GetQCFVDeatil(id);
                    if (dt.Rows.Count > 0)
                    {
                        ca.Branch = dt.Rows[0]["BRANCH"].ToString();
                        ca.DocId = dt.Rows[0]["DOCID"].ToString();
                        ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                        ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                        ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                        ca.ID = id;
                        ca.DrumNo = dt.Rows[0]["DRUMNO"].ToString();
                        ca.Batch = dt.Rows[0]["BATCH"].ToString();
                        ca.BatchNo = dt.Rows[0]["BATCHNO"].ToString();
                        ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
                        ca.ProNo = dt.Rows[0]["PRODID"].ToString();
                        ca.Rate = dt.Rows[0]["RATEPHR"].ToString();
                        ca.ProDate = dt.Rows[0]["PRODDATE"].ToString();
                        ca.SampleNo = dt.Rows[0]["SAMPLENO"].ToString();
                        ca.NozzleNo = dt.Rows[0]["NOZZLENO"].ToString();
                        ca.AirPress = dt.Rows[0]["AIRPRESS"].ToString();
                        ca.Additive = dt.Rows[0]["ADDCH"].ToString();
                        ca.Stime = dt.Rows[0]["STIME"].ToString();
                        ca.CTemp = dt.Rows[0]["BCT"].ToString();
                        ca.FResult = dt.Rows[0]["FINALRESULT"].ToString();
                        ca.RType = dt.Rows[0]["RESULTTYPE"].ToString();
                        ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                        ca.Reamarks = dt.Rows[0]["REMARKS"].ToString();

                    }

                    DataTable dtt = new DataTable();
                    dtt = QCFinalValueEntryService.GetQC(id);
                    if (dtt.Rows.Count > 0)
                    {
                        ca.Type = dtt.Rows[0]["TYPE"].ToString();
                        if (ca.Type == "Production Entry")
                        {
                            ca.ProNo = dtt.Rows[0]["DOCID"].ToString();
                            ca.DocDate = dtt.Rows[0]["CREATED_ON"].ToString();
                            ca.QCID = dtt.Rows[0]["ID"].ToString();
                            DataTable dtt1 = new DataTable();
                            dtt1 = QCFinalValueEntryService.GetQCDetails(ca.QCID);
                            ca.ProDate = dtt1.Rows[0]["DOCDATE"].ToString();
                            ca.WorkCenter = dtt1.Rows[0]["WCID"].ToString();
                            ca.Process = dtt1.Rows[0]["PROCESSID"].ToString();
                            DataTable dtt2 = new DataTable();
                            dtt2 = QCFinalValueEntryService.GetQCOutDeatil(ca.QCID);
                            ca.DrumNo = dtt2.Rows[0]["ODRUMNO"].ToString();
                            ca.Itemid = dtt2.Rows[0]["ITEMID"].ToString();
                            ca.BatchNo = dtt2.Rows[0]["OBATCHNO"].ToString();
                        }
                        
                    }
                    DataTable dt2 = new DataTable();
                    dt2 = QCFinalValueEntryService.GetQCFVResultDetail(id);
                    if (dt2.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new QCFinalValueEntryItem();
                            tda.des = dt2.Rows[0]["TDESC"].ToString();
                            tda.value = dt2.Rows[0]["VALUEORMANUAL"].ToString();
                            tda.unit = dt2.Rows[0]["UNIT"].ToString();
                            tda.sta = dt2.Rows[0]["STARTVALUE"].ToString();
                            tda.en = dt2.Rows[0]["ENDVALUE"].ToString();
                            tda.test = dt2.Rows[0]["TESTVALUE"].ToString();
                            tda.manual = dt2.Rows[0]["MANUALVALUE"].ToString();
                            tda.actual = dt2.Rows[0]["ACTTESTVALUE"].ToString();
                            tda.result = dt2.Rows[0]["TESTRESULT"].ToString();
                            tda.Isvalid = "Y";
                            TData.Add(tda);
                        }
                    }

                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda = new QCFinalValueEntryItem();
                            tda.Isvalid = "Y";
                            TData.Add(tda);
                        }
                    }
                    DataTable dt3 = new DataTable();
                    dt3 = QCFinalValueEntryService.GetQCFVGasDetail(id);

                    if (dt3.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            tda1 = new QCFVItemDeatils();
                            tda1.Time = dt3.Rows[0]["MINS"].ToString();
                            tda1.Vol = dt3.Rows[0]["VOL25C"].ToString();
                            tda1.Volat = dt3.Rows[0]["VOL35C"].ToString();
                            tda1.Volc = dt3.Rows[0]["VOL45C"].ToString();
                            tda1.Stp = dt3.Rows[0]["VOLSTP"].ToString();
                            tda1.Isvalid = "Y";
                            TData1.Add(tda1);
                        }
                    }
                    else
                    {
                        for (int i = 0; i < 1; i++)
                        {
                            tda1 = new QCFVItemDeatils();
                            tda1.Isvalid = "Y";
                            TData1.Add(tda1);
                        }
                    }
                }
                if (tag == "BPE")
                {
                    DataTable dt1 = new DataTable();
                    dt1 = QCFinalValueEntryService.GetAPOutDetails(id);
                    if (dt1.Rows.Count > 0)
                    {
                        ca.WorkCenter = dt1.Rows[0]["WCID"].ToString();
                        ca.ProDate = dt1.Rows[0]["DOCDATE"].ToString();
                        ca.ProNo = dt1.Rows[0]["DOCID"].ToString();
                        ca.Batch = dt1.Rows[0]["BATCH"].ToString();

                        ca.APID = id;
                        DataTable dtt1 = new DataTable();
                        dtt1 = QCFinalValueEntryService.GetAPOutItemDetails(id);
                        if (dtt1.Rows.Count > 0)
                        {
                            ca.DrumNo = dtt1.Rows[0]["OCDRUMNO"].ToString();
                            //ca.Stime = dtt1.Rows[0]["STIME"].ToString();
                            ca.Itemid = dtt1.Rows[0]["ITEMID"].ToString();
                            ca.Item = dtt1.Rows[0]["OITEMID"].ToString();
                            ca.BatchNo = dtt1.Rows[0]["NBATCHNO"].ToString();
                            ViewBag.Itemid = dtt1.Rows[0]["ITEMID"].ToString();
                        }
                    }
                    DataTable dtt = new DataTable();

                    //string itemid = datatrans.GetDataString(" SELECT ITEMMASTERID FROM ITEMMASTER WHERE ITEMID='" + ca.ItemId + "'");
                    string temp = datatrans.GetDataString(" SELECT TEMPLATEID FROM ITEMMASTER WHERE ITEMMASTERID='" + ca.Item + "'");
                    dtt = QCFinalValueEntryService.GetItemDetail(temp);
                    if (dtt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            tda = new QCFinalValueEntryItem();

                            tda.des = dtt.Rows[i]["TESTDESC"].ToString();
                            tda.value = dtt.Rows[i]["VALUEORMANUAL"].ToString();
                            tda.unit = dtt.Rows[i]["UNITID"].ToString();
                            tda.sta = dtt.Rows[i]["STARTVALUE"].ToString();
                            tda.en = dtt.Rows[i]["ENDVALUE"].ToString();
                            tda.Isvalid = "Y";
                            TData.Add(tda);
                        }
                    }



                    for (int i = 0; i < 1; i++)
                    {
                        tda1 = new QCFVItemDeatils();
                        tda1.Isvalid = "Y";
                        TData1.Add(tda1);
                    }
                }
                else 
                {
                    DataTable dtt1 = new DataTable();
                    dtt1 = datatrans.GetData("select ITEMMASTER.ITEMID,NPRODBASIC.WCID,SHIFT,NPRODBASIC.DOCID,to_char(NPRODBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE, NPRODOUTDET.OITEMID,NPRODOUTDET.NBATCHNO, OCDRUMNO, NPRODOUTDET.NPRODBASICID, NPRODOUTDETID from NPRODOUTDET LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID = NPRODOUTDET.OITEMID,NPRODBASIC    WHERE NPRODOUTDET.NPRODBASICID=NPRODBASIC.NPRODBASICID AND NPRODOUTDETID = '" + id + "' ");

                    if (dtt1.Rows.Count > 0)
                    {
                        ca.WorkCenter = dtt1.Rows[0]["WCID"].ToString();
                        ca.ProDate = dtt1.Rows[0]["DOCDATE"].ToString();
                        ca.ProNo = dtt1.Rows[0]["DOCID"].ToString();
                       

                        ca.APID = id;
                        
                            ca.DrumNo = dtt1.Rows[0]["OCDRUMNO"].ToString();
                            //ca.Stime = dtt1.Rows[0]["STIME"].ToString();
                            ca.Itemid = dtt1.Rows[0]["ITEMID"].ToString();
                            ca.Item = dtt1.Rows[0]["OITEMID"].ToString();
                            ca.BatchNo = dtt1.Rows[0]["NBATCHNO"].ToString();
                            ViewBag.Itemid = dtt1.Rows[0]["ITEMID"].ToString();
                        
                    }
                    DataTable dtt = new DataTable();

                    //string itemid = datatrans.GetDataString(" SELECT ITEMMASTERID FROM ITEMMASTER WHERE ITEMID='" + ca.ItemId + "'");
                    string temp = datatrans.GetDataString(" SELECT TEMPLATEID FROM ITEMMASTER WHERE ITEMMASTERID='" + ca.Item + "'");
                    dtt = QCFinalValueEntryService.GetItemDetail(temp);
                    if (dtt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            tda = new QCFinalValueEntryItem();

                            tda.des = dtt.Rows[i]["TESTDESC"].ToString();
                            tda.value = dtt.Rows[i]["VALUEORMANUAL"].ToString();
                            tda.unit = dtt.Rows[i]["UNITID"].ToString();
                            tda.sta = dtt.Rows[i]["STARTVALUE"].ToString();
                            tda.en = dtt.Rows[i]["ENDVALUE"].ToString();
                            tda.Isvalid = "Y";
                            TData.Add(tda);
                        }
                    }



                    for (int i = 0; i < 1; i++)
                    {
                        tda1 = new QCFVItemDeatils();
                        tda1.Isvalid = "Y";
                        TData1.Add(tda1);
                    }
                }

            }
            ca.QCFlst = TData;
            ca.QCFVDLst = TData1;
            return View(ca);
        }
        [HttpPost]
        public ActionResult QCFinalValueEntry(QCFinalValueEntry Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = QCFinalValueEntryService.QCFinalValueEntryCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = " QCFinalValueEntry Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " QCFinalValueEntry Updated Successfully...!";
                    }
                    return RedirectToAction("ListQCFinalValueEntry");
                }

                else
                {
                    ViewBag.PageTitle = "Edit QCFinalValueEntry";
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
        public IActionResult ListQCFinalValueEntry( )
        {
             return View( );
        }
        public ActionResult MyListQCFinalValueGrid(string strStatus)
        {
            List<qcfinalItem> Reg = new List<qcfinalItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)QCFinalValueEntryService.GetQCFinalValueGrid(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string view = string.Empty;

                view = "<a href=ViewQCFinalValueEntry?id=" + dtUsers.Rows[i]["FQTVEBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Edit' /></a>";
                // EditRow = "<a href=BatchCreation?id=" + dtUsers.Rows[i]["BCPRODBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["FQTVEBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";


                Reg.Add(new qcfinalItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["FQTVEBASICID"].ToString()),
                    work = dtUsers.Rows[i]["WCID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    doc = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    drum = dtUsers.Rows[i]["CDRUMNO"].ToString(),
                    process = dtUsers.Rows[i]["PROCESSID"].ToString(),


                    view = view,
                    editrow = EditRow,
                    delrow = DeleteRow,



                });
            }

            return Json(new
            {
                Reg
            });

        }
        //public JsonResult GetGRNItemJSON(string supid)
        //{
        //    QCFinalValueEntry model = new QCFinalValueEntry();
        //    model.Itemlst = BindItemlst(supid);
        //    return Json(BindItemlst(supid));

        //}
        //public JsonResult GetItemJSON(string supid)
        //{
        //    QCFinalValueEntry model = new QCFinalValueEntry();
        //    model.Itemlst = BindItemlst(supid);
        //    return Json(BindItemlst(supid));

        //}
        //public JsonResult GetDrumJSON(string Drmid)
        //{
        //    QCFinalValueEntry model = new QCFinalValueEntry();
        //    model.drumlst = Binddrum(Drmid); 
        //    return Json(Binddrum(Drmid));

        //}
        public JsonResult GetBatchJSON(string Batchid)
        {
            QCFinalValueEntry model = new QCFinalValueEntry();
            model.Batchlst = BindBatch(Batchid);
            return Json(BindBatch(Batchid));

        }
        public List<SelectListItem> Binddrum()
        {
            try
            {
                DataTable dtDesg = QCFinalValueEntryService.DrumDeatils();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DRUMNO"].ToString(), Value = dtDesg.Rows[i]["ODRUMNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindBatch(string value)
        {
            try
            {
                DataTable dtDesg = QCFinalValueEntryService.BatchDeatils(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["NBATCHNO"].ToString(), Value = dtDesg.Rows[i]["NBATCHNO"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {
                DataTable dtDesg = QCFinalValueEntryService.GetItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindProcess(string id)
        {
            try
            {
                DataTable dtDesg = QCFinalValueEntryService.GetProcess();
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
        public List<SelectListItem> BindEmp()
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
        public List<SelectListItem> BindWorkCenter()
        {
            try
            {
                DataTable dtDesg = QCFinalValueEntryService.GetWorkCenter();
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

        //public ActionResult DeleteMR(string tag, int id)
        //{

        //    string flag = QCFinalValueEntryService.StatusChange(tag, id);
        //    if (string.IsNullOrEmpty(flag))
        //    {

        //        return RedirectToAction("ListQCFinalValueEntry");
        //    }
        //    else
        //    {
        //        TempData["notice"] = flag;
        //        return RedirectToAction("ListQCFinalValueEntry");
        //    }
        //}
        public JsonResult GetItemGrpJSON()
        {
            QCFinalValueEntryItem model = new QCFinalValueEntryItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(model);
        }
        public JsonResult GetItemJSON()
        {
            QCFVItemDeatils model = new QCFVItemDeatils();
            // model.ItemGrouplst = BindItemGrplst(value);
            return Json(model);
        }
        public ActionResult GetItemDetails(string id)
        {
            QCFinalValueEntry model = new QCFinalValueEntry();
            DataTable dtt = new DataTable();

            List<QCFinalValueEntryItem> Datan = new List<QCFinalValueEntryItem>();
            QCFinalValueEntryItem tdan = new QCFinalValueEntryItem();
            string Itemid = datatrans.GetDataString(" SELECT ITEMMASTERID FROM ITEMMASTER WHERE ITEMID='" + id + "'");
            string temp = datatrans.GetDataString(" SELECT TEMPLATEID FROM ITEMMASTER WHERE ITEMMASTERID='" + Itemid + "'");
            dtt = QCFinalValueEntryService.GetItemDetail(temp);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tdan = new QCFinalValueEntryItem();
                    tdan.des = dtt.Rows[i]["TESTDESC"].ToString();
                    tdan.value = dtt.Rows[i]["VALUEORMANUAL"].ToString();
                    tdan.unit = dtt.Rows[i]["UNITID"].ToString();
                    tdan.sta = dtt.Rows[i]["STARTVALUE"].ToString();
                    tdan.en = dtt.Rows[i]["ENDVALUE"].ToString();
                    tdan.Isvalid = "Y";
                    Datan.Add(tdan);
                }
            }
            model.QCFlst = Datan;
            return Json(model.QCFlst);
        }


        public IActionResult ViewQCFinalValueEntry(string id)
        {
            QCFinalValueEntry ca = new QCFinalValueEntry();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            DataTable dtt1 = new DataTable();
            List<QCFVItemDeatils> DData = new List<QCFVItemDeatils>();
            QCFVItemDeatils tda1 = new QCFVItemDeatils();
            List<QCFinalValueEntryItem> Data = new List<QCFinalValueEntryItem>();
            QCFinalValueEntryItem tda = new QCFinalValueEntryItem();
            dt = QCFinalValueEntryService.GetViewQCFVDeatil(id);
            if (dt.Rows.Count > 0)
            {

                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                
                ca.DrumNo = dt.Rows[0]["DRUMNO"].ToString();
                ca.Batch = dt.Rows[0]["BATCH"].ToString();
                ca.BatchNo = dt.Rows[0]["BATCHNO"].ToString();
                ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
                ca.ProNo = dt.Rows[0]["PRODID"].ToString();
                ca.Rate = dt.Rows[0]["RATEPHR"].ToString();
                ca.ProDate = dt.Rows[0]["PRODDATE"].ToString();
                ca.SampleNo = dt.Rows[0]["SAMPLENO"].ToString();
                ca.NozzleNo = dt.Rows[0]["NOZZLENO"].ToString();
                ca.AirPress = dt.Rows[0]["AIRPRESS"].ToString();
                ca.Additive = dt.Rows[0]["ADDCH"].ToString();
                ca.Stime = dt.Rows[0]["STIME"].ToString();
                ca.CTemp = dt.Rows[0]["BCT"].ToString();
                ca.FResult = dt.Rows[0]["FINALRESULT"].ToString();
                ca.RType = dt.Rows[0]["RESULTTYPE"].ToString();
                ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                ca.Reamarks = dt.Rows[0]["REMARKS"].ToString();
                ca.ID = id;

             
                //double tot = 0;

                dtt = QCFinalValueEntryService.GetViewQCFVResultDetail(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new QCFinalValueEntryItem();
                        tda.des = dtt.Rows[0]["TDESC"].ToString();
                        tda.value = dtt.Rows[0]["VALUEORMANUAL"].ToString();
                        tda.unit = dtt.Rows[0]["UNIT"].ToString();
                        tda.sta = dtt.Rows[0]["STARTVALUE"].ToString();
                        tda.en = dtt.Rows[0]["ENDVALUE"].ToString();
                        tda.test = dtt.Rows[0]["TESTVALUE"].ToString();
                        tda.manual = dtt.Rows[0]["MANUALVALUE"].ToString();
                        tda.actual = dtt.Rows[0]["ACTTESTVALUE"].ToString();
                        tda.result = dtt.Rows[0]["TESTRESULT"].ToString();

                        Data.Add(tda);
                    }
                }



                //double tot = 0;

                dtt1 = QCFinalValueEntryService.GetViewQCFVGasDetail(id);
                if (dtt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {
                        tda1 = new QCFVItemDeatils();
                        tda1.Time = dtt1.Rows[0]["MINS"].ToString();
                        tda1.Vol = dtt1.Rows[0]["VOL25C"].ToString();
                        tda1.Volat = dtt1.Rows[0]["VOL35C"].ToString();
                        tda1.Volc = dtt1.Rows[0]["VOL45C"].ToString();
                        tda1.Stp = dtt1.Rows[0]["VOLSTP"].ToString();

                        DData.Add(tda1);
                    }
                }
               

            }
            ca.QCFlst = Data;
            ca.QCFVDLst = DData;
            return View(ca);
        }
    }
}
