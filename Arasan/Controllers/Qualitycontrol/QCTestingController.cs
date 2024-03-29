using Arasan.Interface;
using System.Collections.Generic;
using Arasan.Models;

using Microsoft.AspNetCore.Mvc;
using Arasan.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using Arasan.Services.Qualitycontrol;
//using PdfSharp.Pdf;

namespace Arasan.Controllers
{
    public class QCTestingController : Controller
    {
        IQCTestingService QCTestingService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public QCTestingController(IQCTestingService _QCTestingService, IConfiguration _configuratio)
        {
            QCTestingService = _QCTestingService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult QCTesting(string id,string tag)
        {
            QCTesting ca = new QCTesting();
           
            ca.lst = BindGRNlist("");
            ca.assignList = BindEmp();
            ca.Branch = Request.Cookies["BranchId"];
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            DataTable dtv = datatrans.GetSequence("testv");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            //ca.lst = BindGRNlist("");
            List<QCItem> TData = new List<QCItem>();
            QCItem tda = new QCItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new QCItem();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
               
            }
            else
            {
                if (tag == null)
                {
                    DataTable dt = new DataTable();
                    dt = QCTestingService.GetQCTesting(id);
                    if (dt.Rows.Count > 0)
                    {
                        ca.DocId = dt.Rows[0]["DOCID"].ToString();
                        ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                        ca.GRNNo = dt.Rows[0]["GRNNO"].ToString();
                        ca.GRNDate = dt.Rows[0]["GRNDATE"].ToString();
                        ca.ID = id;
                        ca.Supplst = BindSupplst(ca.GRNNo);
                        ca.Party = dt.Rows[0]["PARTYID"].ToString();
                        ca.Itemlst = BindItemlst(ca.GRNNo);
                        ca.ItemId = dt.Rows[0]["ITEMID"].ToString();
                        ca.SNo = dt.Rows[0]["SLNO"].ToString();
                        ca.LotNo = dt.Rows[0]["LOTSERIALNO"].ToString();
                        ca.TestResult = dt.Rows[0]["TESTRESULT"].ToString();
                        ca.TestBy = dt.Rows[0]["TESTBY"].ToString();
                        ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                        ca.ClassCode = dt.Rows[0]["CLASSCODE"].ToString();
                    }
                    DataTable dt2 = new DataTable();
                    dt2 = QCTestingService.GetViewQCDetail(id);
                    if (dt2.Rows.Count > 0)
                    {

                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new QCItem();
                            tda.TestDec = dt2.Rows[0]["TESTDESC"].ToString();
                            tda.TestValue = dt2.Rows[0]["TESTVALUE"].ToString();
                            tda.Result = dt2.Rows[0]["RESULT"].ToString();
                            tda.AcTestValue = dt2.Rows[0]["ACTTESTVALUE"].ToString();
                            tda.AccVale = dt2.Rows[0]["ACVAL"].ToString();
                            tda.ManualValue = dt2.Rows[0]["MANUALVALUE"].ToString();
                            TData.Add(tda);
                        }
                    }

                }
                else
                {
                    DataTable dtt1 = new DataTable();
                    dtt1 = QCTestingService.GetGrnItemDetails(id);
                    if (dtt1.Rows.Count > 0)
                    {
                        ca.GRNProd = dtt1.Rows[0]["QTY"].ToString();
                        ca.ItemId = dtt1.Rows[0]["ITEMID"].ToString();
                        ca.Item = dtt1.Rows[0]["item"].ToString();
                        ca.detid = dtt1.Rows[0]["GRNBLDETAILID"].ToString();
                        ca.basicid = dtt1.Rows[0]["GRNBLBASICID"].ToString();
                        string template = datatrans.GetDataString(" SELECT TEMPLATEID FROM ITEMMASTER WHERE ITEMMASTERID='" + ca.Item + "'");
                        ca.temp = datatrans.GetDataString(" SELECT TEMPLATEID FROM TESTTBASIC WHERE TESTTBASICID='" + template + "'");
                        ViewBag.ItemId = dtt1.Rows[0]["ITEMID"].ToString();
                    }
                    DataTable dt1 = new DataTable();
                    dt1 = QCTestingService.GetGrnDetails(ca.basicid);
                    if (dt1.Rows.Count > 0)
                    {
                        ca.GRNNo = dt1.Rows[0]["DOCID"].ToString();
                        ca.GRNDate = dt1.Rows[0]["DOCDATE"].ToString();
                        ca.Party = dt1.Rows[0]["PARTYNAME"].ToString();
                        ca.Partyid = dt1.Rows[0]["PARTYID"].ToString();
                        ca.APID = id;
                       

                    }
                    DataTable dtt = new DataTable();

                    List<QCGRNItem> Data = new List<QCGRNItem>();
                    QCGRNItem tda1 = new QCGRNItem();
                    //string itemid = datatrans.GetDataString(" SELECT ITEMMASTERID FROM ITEMMASTER WHERE ITEMID='" + ca.ItemId + "'");
                    string temp = datatrans.GetDataString(" SELECT TEMPLATEID FROM ITEMMASTER WHERE ITEMMASTERID='" + ca.Item + "'");
                    dtt = QCTestingService.GetItemDetail(temp);
                    if (dtt.Rows.Count > 0)
                    {
                        for (int i = 0; i < dtt.Rows.Count; i++)
                        {
                            tda1 = new QCGRNItem();
                            tda1.testid = temp;
                            tda1.description = dtt.Rows[i]["TESTDESC"].ToString();
                            tda1.value = dtt.Rows[i]["VALUEORMANUAL"].ToString();
                            tda1.unit = dtt.Rows[i]["UNITID"].ToString();
                            tda1.startvalue = dtt.Rows[i]["STARTVALUE"].ToString();
                            tda1.endvalue = dtt.Rows[i]["ENDVALUE"].ToString();

                            Data.Add(tda1);
                        }
                    }
                    ca.QCGRNLst = Data;
                }
            }
            ca.QCLst = TData;
            return View(ca);
        }
        [HttpPost]
        public ActionResult QCTesting(QCTesting Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = QCTestingService.QCTestingCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "QCTesting Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "QCTesting Updated Successfully...!";
                    }
                    return RedirectToAction("ListQCTesting");
                }

                else
                {
                    ViewBag.PageTitle = "Edit QCTesting";
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
        public IActionResult ListQCTesting( )
        {

            //IEnumerable<QCTesting> cmp = QCTestingService.GetAllQCTesting(st,ed);
            return View();
        }
        public ActionResult MyListQCTestingGrid(string strStatus)
        {
            List<qcItem> Reg = new List<qcItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)QCTestingService.GetQCTestingGrid(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {

                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string view = string.Empty;

                view = "<a href=ViewQCTesting?id=" + dtUsers.Rows[i]["QCVALUEBASICID"].ToString() + " class='fancybox' data-fancybox-type='iframe'><img src='../Images/view_icon.png' alt='Edit' /></a>";
               // EditRow = "<a href=BatchCreation?id=" + dtUsers.Rows[i]["BCPRODBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteMR?tag=Del&id=" + dtUsers.Rows[i]["QCVALUEBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";

                
                Reg.Add(new qcItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["QCVALUEBASICID"].ToString()),
                    party = dtUsers.Rows[i]["PARTYNAME"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    doc = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    test = dtUsers.Rows[i]["TESTRESULT"].ToString(),

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
        public IActionResult POQcTesting(string id)
        {
            QCTesting ca = new QCTesting();
            DataTable dtv = datatrans.GetSequence("testv");
            if (dtv.Rows.Count > 0)
            {
                ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
            }
            DataTable dt1 = new DataTable();
            dt1 = QCTestingService.GetPoQcTesting(id);
            if (dt1.Rows.Count > 0)
            {
                ca.Po = dt1.Rows[0]["DOCID"].ToString();
                ca.PoDate = dt1.Rows[0]["DOCDATE"].ToString();
                ca.Partyid = dt1.Rows[0]["PARTYNAME"].ToString();
                ca.Par = dt1.Rows[0]["par"].ToString();
                ca.PoId = id;
                DataTable dtt1 = new DataTable();
                dtt1 = QCTestingService.GetGetPoQcTestingDetails(id);
                if (dtt1.Rows.Count > 0)
                {
                    ca.Qty = dtt1.Rows[0]["QTY"].ToString();
                    ca.ItemId = dtt1.Rows[0]["ITEMID"].ToString();
                    ca.Item = dtt1.Rows[0]["item"].ToString();
                    ViewBag.ItemId = dtt1.Rows[0]["ITEMID"].ToString();
                }
                DataTable dtt = new DataTable();

                List<QCPOItem> Data = new List<QCPOItem>();
                QCPOItem tda1 = new QCPOItem();
                //string itemid = datatrans.GetDataString(" SELECT ITEMMASTERID FROM ITEMMASTER WHERE ITEMID='" + ca.ItemId + "'");
                string temp = datatrans.GetDataString(" SELECT TEMPLATEID FROM ITEMMASTER WHERE ITEMMASTERID='" + ca.Item + "'");
                dtt = QCTestingService.GetPOItemDetail(temp);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda1 = new QCPOItem();

                        tda1.description = dtt.Rows[i]["TESTDESC"].ToString();
                        tda1.value = dtt.Rows[i]["VALUEORMANUAL"].ToString();
                        tda1.unit = dtt.Rows[i]["UNITID"].ToString();
                        tda1.startvalue = dtt.Rows[i]["STARTVALUE"].ToString();
                        tda1.endvalue = dtt.Rows[i]["ENDVALUE"].ToString();

                        Data.Add(tda1);
                    }
                }
                ca.QCPOLst = Data;

            }
            return View(ca);
        }
        [HttpPost]
        public ActionResult POQcTesting(QCTesting Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = QCTestingService.POQCTestingCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "QCTesting Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "QCTesting Updated Successfully...!";
                    }
                    return RedirectToAction("ListQCTesting");
                }

                else
                {
                    ViewBag.PageTitle = "Edit QCTesting";
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
        //public List<SelectListItem> BindType()
        //{
        //    try
        //    {
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();

        //        lstdesg.Add(new SelectListItem() { Text = "PO", Value = "PO" });
        //        lstdesg.Add(new SelectListItem() { Text = "GRN", Value = "GRN" });
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        //public JsonResult GetTypeJSON(string GPID)
        //{
        //    QCTesting model = new QCTesting();
        //    model.Typlst = BindGRNlist(GPID);
        //    return Json(BindGRNlist(GPID));

        //}
        public List<SelectListItem> BindGRNlist(string value)
        {
            try

            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
               
                    DataTable dtDesg = QCTestingService.GetGRN(value);

                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["GRNBLBASICID"].ToString() });
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
     
        public JsonResult GetItemJSON()
        {
            QCItem model = new QCItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        public ActionResult GetGRNDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string grndate = "";
             
                 
                    dt = QCTestingService.GetGRNDetails(ItemId);
                    if (dt.Rows.Count > 0)
                    {
                        grndate = dt.Rows[0]["DOCDATE"].ToString();

                    }
                
                var result = new { grndate = grndate};
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public ActionResult GetPODetail(string Item)
        //{
        //    try
        //    {
        //        DataTable dt = new DataTable();
        //        string podate = "";

        //        dt = QCTestingService.GetPODetails(Item);
        //        if (dt.Rows.Count > 0)
        //        {
        //            podate = dt.Rows[0]["DOCDATE"].ToString();

        //        }

        //        var result = new { podate = podate };
        //        return Json(result);
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public JsonResult GetGRNItemJSON(string supid )
        {
            QCTesting model = new QCTesting();
            model.Itemlst = BindItemlst(supid );
            return Json(BindItemlst(supid));

        }
        //public JsonResult GetPOItemJSON(string Poid)
        //{
        //    QCTesting model = new QCTesting();
        //    model.Itemlst = BindPOItemlst(Poid);
        //    return Json(BindPOItemlst(Poid));

        //}
        public JsonResult GetGRNSuppJSON(string suppid )
        {
            QCTesting model = new QCTesting();
            model.Supplst = BindSupplst(suppid );
            return Json(BindSupplst(suppid ));

        }
        public ActionResult GetItemDetails(string id)
        {
            QCTesting model = new QCTesting();
            DataTable dtt = new DataTable();

            List<QCPOItem> Data = new List<QCPOItem>();
            QCPOItem tda1 = new QCPOItem();
            string itemid = datatrans.GetDataString(" SELECT ITEMMASTERID FROM ITEMMASTER WHERE ITEMID='" + id + "'");
            string temp = datatrans.GetDataString(" SELECT TEMPLATEID FROM ITEMMASTER WHERE ITEMMASTERID='" + itemid + "'");
            dtt = QCTestingService.GetPOItemDetail(temp);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda1 = new QCPOItem();
                    tda1.testid = temp;
                    tda1.description = dtt.Rows[i]["TESTDESC"].ToString();
                    tda1.value = dtt.Rows[i]["VALUEORMANUAL"].ToString();
                    tda1.unit = dtt.Rows[i]["UNITID"].ToString();
                    tda1.startvalue = dtt.Rows[i]["STARTVALUE"].ToString();
                    tda1.endvalue = dtt.Rows[i]["ENDVALUE"].ToString();

                    Data.Add(tda1);
                }
            }
            model.QCPOLst = Data;
            return Json(model.QCPOLst);
        }
        //public JsonResult GetPOSuppJSON(string POsuppid)
        //{
        //    QCTesting model = new QCTesting();
        //    model.Supplst = BindPOSupplst(POsuppid);
        //    return Json(BindPOSupplst(POsuppid));

        //}
        public List<SelectListItem> BindItemlst(string value)
        {
            try
            {

                List<SelectListItem> lstdesg = new List<SelectListItem>();
               
                DataTable dtDesg = QCTestingService.GetItembyId(value);
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
            //public List<SelectListItem> BindPOItemlst(string id)
            //{
            //    try
            //    {

            //        List<SelectListItem> lstdesg = new List<SelectListItem>();

            //        DataTable dtDesg = QCTestingService.GetPOItembyId(id);
            //        for (int i = 0; i < dtDesg.Rows.Count; i++)
            //        {
            //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["PODETAILID"].ToString() });

            //        }


            //        return lstdesg;

            //    }
            //    catch (Exception ex)
            //    {
            //        throw ex;
            //    }
            //}
        
        public List<SelectListItem> BindSupplst(string value)
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                
                    DataTable dtDesg = QCTestingService.GetParty(value);
                    for (int i = 0; i < dtDesg.Rows.Count; i++)
                    {
                        lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTYNAME"].ToString(), Value = dtDesg.Rows[i]["PARTYID"].ToString() });
                    }
               
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public List<SelectListItem> BindPOSupplst(string value1)
        //{
        //    try
        //    {
        //        DataTable dtDesg = QCTestingService.GetPOParty(value1);
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTY"].ToString(), Value = dtDesg.Rows[i]["POBASICID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public IActionResult QCTestValueEntry()
        {
            return View();
        }
        public IActionResult QCFinalValueEntry()
        {
            return View();
        }
        public IActionResult PackingQCFinalValueEntry()
        {
            return View();
        }
        public IActionResult ItemConversionEntry()
        {
            return View();
        }
        public IActionResult NCRelease()
        {
            return View();
        }
        public IActionResult ORSATEntry()
        {
            return View();
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = QCTestingService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListQCTesting");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListQCTesting");
            }
        }
        public ActionResult GetItemDetails1(string id)
        {
            QCTesting model = new QCTesting();
            DataTable dtt = new DataTable();
            List<QCGRNItem> Data = new List<QCGRNItem>();
            QCGRNItem tda1 = new QCGRNItem();
            string itemid = datatrans.GetDataString(" SELECT ITEMMASTERID FROM ITEMMASTER WHERE ITEMID='" + id + "'");
            string temp = datatrans.GetDataString(" SELECT TEMPLATEID FROM ITEMMASTER WHERE ITEMMASTERID='" + itemid + "'");
            dtt = QCTestingService.GetItemDetail(temp);
            if (dtt.Rows.Count > 0)
            {
                for (int i = 0; i < dtt.Rows.Count; i++)
                {
                    tda1 = new QCGRNItem();
                    tda1.testid = dtt.Rows[i]["TESTTDETAILID"].ToString();
                    tda1.description = dtt.Rows[i]["TESTDESC"].ToString();
                    tda1.value = dtt.Rows[i]["VALUEORMANUAL"].ToString();
                    tda1.unit = dtt.Rows[i]["UNITID"].ToString();
                    tda1.startvalue = dtt.Rows[i]["STARTVALUE"].ToString();
                    tda1.endvalue = dtt.Rows[i]["ENDVALUE"].ToString();

                    Data.Add(tda1);
                }
            }
            model.QCGRNLst = Data;
            return Json(model.QCGRNLst);
        }

        //public IActionResult ViewQCTesting(string id)
        //{
        //    QCTesting ca = new QCTesting();
        //    DataTable dt = new DataTable();
        //    DataTable dtt = new DataTable();

        //    dt = QCTestingService.GetViewQCTesting(id);
        //    if (dt.Rows.Count > 0)
        //    {
        //        ca.DocId = dt.Rows[0]["DOCID"].ToString();
        //        ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
        //        ca.GRNNo = dt.Rows[0]["DOCID"].ToString();
        //        ca.GRNDate = dt.Rows[0]["GRNDATE"].ToString();
        //        ca.ID = id;
        //        //ca.Supplst = BindSupplst(ca.GRNNo);
        //        ca.Party = dt.Rows[0]["PARTYID"].ToString();
        //        //ca.Itemlst = BindItemlst(ca.GRNNo);
        //        ca.ItemId = dt.Rows[0]["ITEMID"].ToString();
        //        ca.SNo = dt.Rows[0]["SLNO"].ToString();
        //        ca.LotNo = dt.Rows[0]["LOTSERIALNO"].ToString();
        //        ca.TestResult = dt.Rows[0]["TESTRESULT"].ToString();
        //        ca.TestBy = dt.Rows[0]["TESTBY"].ToString();
        //        ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
        //        ca.ClassCode = dt.Rows[0]["CLASSCODE"].ToString();
        //        ca.GRNProd = dt.Rows[0]["GRNPROD"].ToString();
        //        ca.Procedure = dt.Rows[0]["TESTPROCEDURE"].ToString();


        //        List<QCItem> Data = new List<QCItem>();
        //        QCItem tda = new QCItem();
        //        //double tot = 0;

        //        dtt = QCTestingService.GetViewQCDetail(id);
        //        if (dtt.Rows.Count > 0)
        //        {
        //            for (int i = 0; i < dtt.Rows.Count; i++)
        //            {
        //                tda.TestDec = dtt.Rows[0]["TESTDESC"].ToString();
        //                tda.TestValue = dtt.Rows[0]["TESTVALUE"].ToString();
        //                tda.Result = dtt.Rows[0]["RESULT"].ToString();
        //                tda.AcTestValue = dtt.Rows[0]["ACTTESTVALUE"].ToString();
        //                tda.AccVale = dtt.Rows[0]["ACVAL"].ToString();
        //                tda.ManualValue = dtt.Rows[0]["MANUALVALUE"].ToString();
        //                Data.Add(tda);
        //            }
        //        }

        //        ca.QCLst = Data;
        //    }
        //    return View(ca);
        //}

        public IActionResult ViewQCTesting(string id)
        {
            QCTesting ca = new QCTesting();
            DataTable dt = new DataTable();
            DataTable dtt = new DataTable();
            List<QCItem> Data = new List<QCItem>();
            QCItem tda = new QCItem();
            dt = QCTestingService.GetViewQCTesting(id);
            if (dt.Rows.Count > 0)
            {
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.GRNNo = dt.Rows[0]["DOCID"].ToString();
                ca.GRNDate = dt.Rows[0]["GRNDATE"].ToString();
                ca.ID = id;
                //ca.Supplst = BindSupplst(ca.GRNNo);
                ca.Partyid = dt.Rows[0]["PARTYID"].ToString();
                //ca.Itemlst = BindItemlst(ca.GRNNo);
                ca.ItemId = dt.Rows[0]["ITEMID"].ToString();
                ca.SNo = dt.Rows[0]["SLNO"].ToString();
                ca.LotNo = dt.Rows[0]["LOTSERIALNO"].ToString();
                ca.TestResult = dt.Rows[0]["TESTRESULT"].ToString();
                ca.TestBy = dt.Rows[0]["TESTBY"].ToString();
                ca.Remarks = dt.Rows[0]["REMARKS"].ToString();
                ca.ClassCode = dt.Rows[0]["CLASSCODE"].ToString();
                ca.GRNProd = dt.Rows[0]["GRNPROD"].ToString();
                ca.Procedure = dt.Rows[0]["TESTPROCEDURE"].ToString();


              
                //double tot = 0;

                dtt = QCTestingService.GetViewQCDetail(id);
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new QCItem();
                        tda.TestDec = dtt.Rows[0]["TESTDESC"].ToString();
                        tda.TestValue = dtt.Rows[0]["TESTVALUE"].ToString();
                        tda.Result = dtt.Rows[0]["RESULT"].ToString();
                        tda.AcTestValue = dtt.Rows[0]["ACTTESTVALUE"].ToString();
                        tda.AccVale = dtt.Rows[0]["ACVAL"].ToString();
                        tda.ManualValue = dtt.Rows[0]["MANUALVALUE"].ToString();
                        Data.Add(tda);
                    }
                }

             
            }
            ca.QCLst = Data;
            return View(ca);
        }
    }
}
