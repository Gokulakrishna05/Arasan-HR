using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;


namespace Arasan.Controllers
{
    public class BatchCreationController : Controller
    {
        IBatchCreation Batch;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public BatchCreationController(IBatchCreation _Batch, IConfiguration _configuratio)
        {
            Batch = _Batch;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult BatchCreation(string id)
        {
            BatchCreation ca = new BatchCreation();

            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Worklst = BindWorkCenter();
            ca.Processlst = BindProcess("");
            ca.Enterd = Request.Cookies["UserId"];
            ca.RecList = BindEmp();
            ca.Prodlst = BindProd();
            ca.DocDate = DateTime.Now.ToString("dd-MMM-yyyy");
            List<BatchItem> TData = new List<BatchItem>();
            BatchItem tda = new BatchItem();
            List<BatchInItem> TData1 = new List<BatchInItem>();
            BatchInItem tda1 = new BatchInItem();
            List<BatchOutItem> TData2 = new List<BatchOutItem>();
            BatchOutItem tda2 = new BatchOutItem();
            List<BatchOtherItem> TData3 = new List<BatchOtherItem>();
            BatchOtherItem tda3 = new BatchOtherItem();
            List<BatchParemItem> TData4 = new List<BatchParemItem>();
            BatchParemItem tda4 = new BatchParemItem();                                                                          
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new BatchItem();
                    tda.WorkCenterlst = BindWorkCenterid();                                                                                                                                                                                          
                    tda.Processidlst = BindProcess("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda1 = new BatchInItem();
                    tda1.IProcesslst = BindProcessid();
                    tda1.Itemlst = BindItemlst();
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda2 = new BatchOutItem();
                    tda2.OProcesslst = BindProcessid();
                    tda2.OItemlst = BindItemlst();
                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda3 = new BatchOtherItem();
                    tda3.OProcessidlst = BindProcessid();
                    tda3.Isvalid = "Y";
                    TData3.Add(tda3);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda4 = new BatchParemItem();
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);
                }
            }
            else
            {

                // ca = directPurchase.GetDirectPurById(id);


                DataTable dt = new DataTable();
                double total = 0;
                dt = Batch.GetBatchCreation(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                    ca.BatchNo = dt.Rows[0]["DOCID"].ToString();
                    ca.ID = id;
                    ca.Prod = dt.Rows[0]["PSCHNO"].ToString();
                    ca.Process = dt.Rows[0]["WPROCESSID"].ToString();
                    ca.RefBatch = dt.Rows[0]["REFDOCID"].ToString();
                    ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.Narr = dt.Rows[0]["NARR"].ToString();
                    ca.Seq = dt.Rows[0]["SEQYN"].ToString();
                    ca.Shall = dt.Rows[0]["PTYPE"].ToString();
                    ca.Leaf = dt.Rows[0]["BTYPE"].ToString();
                    ca.IOFrom = Convert.ToDouble(dt.Rows[0]["IORATIOFROM"].ToString() == "" ? "0" : dt.Rows[0]["IORATIOFROM"].ToString());
                    ca.IOTo = Convert.ToDouble(dt.Rows[0]["IORATIOTO"].ToString() == "" ? "0" : dt.Rows[0]["IORATIOTO"].ToString());
                    ca.MTO = Convert.ToDouble(dt.Rows[0]["MTONO"].ToString() == "" ? "0" : dt.Rows[0]["MTONO"].ToString());


                }
                DataTable dt2 = new DataTable();
        
                dt2 = Batch.GetBatchCreationDetail(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new BatchItem();
                        tda.WorkCenterlst = BindWorkCenter();
                        tda.Processidlst = BindProcess(tda.WorkId);
                        tda.WorkId = dt2.Rows[i]["BWCID"].ToString();
                        tda.ProcessId = dt2.Rows[i]["PROCESSID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["PROCESSID"].ToString();
                        tda.Seq = dt2.Rows[i]["PSEQ"].ToString();
                        tda.Req = dt2.Rows[i]["INSREQ"].ToString();
                        tda.ID = id;
                        TData.Add(tda);
                        tda.Isvalid = "Y";
                    }

                }
                DataTable dt3 = new DataTable();
                dt3 = Batch.GetBatchCreationInputDetail(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new BatchInItem();
                        tda1.IProcesslst = BindProcessid();
                        tda1.Process = dt3.Rows[0]["IPROCESSID"].ToString();
                        tda1.Itemlst = BindItemlst();
                        tda1.Item = dt3.Rows[i]["IITEMID"].ToString();
                        tda1.Unit = dt3.Rows[i]["IUNIT"].ToString();
                        tda1.Qty = dt3.Rows[i]["IQTY"].ToString();
                        tda1.ID = id;
                        tda1.Isvalid = "Y";
                        TData1.Add(tda1);
                    }

                }
            
            DataTable dt4 = new DataTable();
            dt4 = Batch.GetBatchCreationOutputDetail(id);
            if (dt4.Rows.Count > 0)
            {
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    tda2 = new BatchOutItem();
                    tda2.OProcesslst = BindProcessid();
                    tda2.OProcess = dt4.Rows[i]["OPROCESSID"].ToString();
                    tda2.OItemlst = BindItemlst();
                    tda2.OItem = dt4.Rows[i]["OITEMID"].ToString();
                    tda2.OUnit = dt4.Rows[i]["OUNIT"].ToString();
                    tda2.OutType = dt4.Rows[i]["OTYPE"].ToString();
                    tda2.OQty = dt4.Rows[i]["OQTY"].ToString();
                    tda2.Waste = dt4.Rows[i]["OWPER"].ToString();
                    tda2.Vmper = dt4.Rows[i]["VMPER"].ToString();
                    tda2.Greas = dt4.Rows[i]["GPER"].ToString();
                    tda2.ID = id;
                    TData2.Add(tda2);
                        tda2.Isvalid = "Y";
                    }

            }
                DataTable dt5 = new DataTable();
                dt5 = Batch.GetBatchCreationOtherDetail(id);
                if (dt5.Rows.Count > 0)
                {
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        tda3 = new BatchOtherItem();
                        tda3.OProcessidlst = BindProcessid();
                        tda3.OtProcessId = dt5.Rows[i]["EPROCESSID"].ToString();

                        tda3.Seqe = dt5.Rows[i]["EPSEQ"].ToString();
                        tda3.Start = dt5.Rows[i]["ESDT"].ToString();
                        tda3.StartT = dt5.Rows[i]["EST"].ToString();
                        tda3.End = dt5.Rows[i]["EEDT"].ToString();
                        tda3.Isvalid = "Y";
                        tda3.EndT = dt5.Rows[i]["EET"].ToString();
                        tda3.Total = dt5.Rows[i]["ETOTHRS"].ToString();
                        tda3.Break = dt5.Rows[i]["EBRHRS"].ToString();
                        tda3.RunHrs = dt5.Rows[i]["ERUNHRS"].ToString();
                        tda3.Remark = dt5.Rows[i]["ENARR"].ToString();
                        tda3.ID = id;
                        TData3.Add(tda3);
                    }

                }
                DataTable dt6 = new DataTable();
                dt6 = Batch.GetBatchCreationParmDetail(id);
                if (dt6.Rows.Count > 0)
                {
                    for (int i = 0; i < dt6.Rows.Count; i++)
                    {
                        tda4 = new BatchParemItem();

                        tda4.Param = dt6.Rows[i]["PROCPARAM"].ToString();

                        tda4.PUnit = dt6.Rows[i]["PARAMUNIT"].ToString();
                        tda4.StartDate = dt6.Rows[i]["PSDT"].ToString();
                        tda4.StartTime = dt6.Rows[i]["PSTIME"].ToString();
                        tda4.EndDate = dt6.Rows[i]["PEDT"].ToString();
                        tda4.Isvalid = "Y";
                        tda4.EndTime = dt6.Rows[i]["PETIME"].ToString();
                        tda4.Value = dt6.Rows[i]["PARAMVALUE"].ToString();

                        tda4.ID = id;
                        TData4.Add(tda4);
                    }

                }
            }



            ca.BatchLst = TData;
            ca.BatchInLst = TData1;
            ca.BatchOutLst = TData2;
          
                 ca.BatchOtherLst = TData3;
            ca.BatchParemLst = TData4;
            return View(ca);
        }
        [HttpPost]
        public ActionResult BatchCreation(BatchCreation Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = Batch.BatchCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "BatchCreation Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "BatchCreation Updated Successfully...!";
                    }
                    return RedirectToAction("ListBatchCreation");
                }

                else
                {
                    ViewBag.PageTitle = "Edit BatchCreation";
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
        public IActionResult ViewBatch(string id)
        {
            BatchCreation ca = new BatchCreation();
            DataTable dt = new DataTable();
            //DataTable dtt = new DataTable();
            dt = Batch.GetBatchCreationByName(id);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.BatchNo = dt.Rows[0]["DOCID"].ToString();
                ca.DocDate = dt.Rows[0]["DOCDATE"].ToString();
                ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                ca.ID = id;
                ca.Prod = dt.Rows[0]["DOCID"].ToString();
                ca.RefBatch = dt.Rows[0]["REFDOCID"].ToString();
                ca.Enterd = dt.Rows[0]["ENTEREDBY"].ToString();
                ca.Narr = dt.Rows[0]["NARR"].ToString();
                ca.Seq = dt.Rows[0]["SEQYN"].ToString();
                ca.Shall = dt.Rows[0]["PTYPE"].ToString();
                ca.Leaf = dt.Rows[0]["BTYPE"].ToString();
                ca.IOFrom = Convert.ToDouble(dt.Rows[0]["IORATIOFROM"].ToString() == "" ? "0" : dt.Rows[0]["IORATIOFROM"].ToString());
                ca.IOTo = Convert.ToDouble(dt.Rows[0]["IORATIOTO"].ToString() == "" ? "0" : dt.Rows[0]["IORATIOTO"].ToString());
                ca.MTO = Convert.ToDouble(dt.Rows[0]["MTONO"].ToString() == "" ? "0" : dt.Rows[0]["MTONO"].ToString());


                List<BatchItem> TData = new List<BatchItem>();
                BatchItem tda = new BatchItem();
                DataTable dt2 = Batch.BatchDetail(id);
                for (int i = 0; i < dt2.Rows.Count; i++)
                {
                    tda = new BatchItem();
                    tda.WorkCenterlst = BindWorkCenter();
                    tda.Processidlst = BindProcess(tda.WorkId);
                    tda.WorkId = dt2.Rows[i]["WCID"].ToString();
                    tda.ProcessId = dt2.Rows[i]["PROCESSID"].ToString();
                    tda.saveItemId = dt2.Rows[i]["PROCESSID"].ToString();
                    tda.Seq = dt2.Rows[i]["PSEQ"].ToString();
                    tda.Req = dt2.Rows[i]["INSREQ"].ToString();
                    tda.ID = id;
                    TData.Add(tda);
                }
                List<BatchInItem> TData1 = new List<BatchInItem>();
                BatchInItem tda1 = new BatchInItem();
                DataTable dt3 = Batch.BatchInDetail(id);
                for (int i = 0; i < dt3.Rows.Count; i++)
                {
                    tda1 = new BatchInItem();
                    tda1.IProcesslst = BindProcessid();
                    tda1.Process = dt3.Rows[i]["PROCESSID"].ToString();
                    tda1.Itemlst = BindItemlst();
                    tda1.Item = dt3.Rows[i]["ITEMID"].ToString();
                    tda1.Unit = dt3.Rows[i]["IUNIT"].ToString();
                    tda1.Qty = dt3.Rows[i]["IQTY"].ToString();
                    tda1.ID = id;
                    TData1.Add(tda1);
                }
                List<BatchOutItem> TData2 = new List<BatchOutItem>();
                BatchOutItem tda2 = new BatchOutItem();
                DataTable dt4 = Batch.BatchOutDetail(id);
                for (int i = 0; i < dt4.Rows.Count; i++)
                {
                    tda2 = new BatchOutItem();
                    tda2.OProcesslst = BindProcessid();
                    tda2.OProcess = dt4.Rows[i]["PROCESSID"].ToString();
                    tda2.OItemlst = BindItemlst();
                    tda2.OItem = dt4.Rows[i]["ITEMID"].ToString();
                    tda2.OUnit = dt4.Rows[i]["OUNIT"].ToString();
                    tda2.OutType = dt4.Rows[i]["OTYPE"].ToString();
                    tda2.OQty = dt4.Rows[i]["OQTY"].ToString();
                    tda2.Waste = dt4.Rows[i]["OWPER"].ToString();
                    tda2.Vmper = dt4.Rows[i]["VMPER"].ToString();
                    tda2.Greas = dt4.Rows[i]["GPER"].ToString();
                    tda2.ID = id;
                    TData2.Add(tda2);
                }
                List<BatchOtherItem> TData3 = new List<BatchOtherItem>();
                BatchOtherItem tda3 = new BatchOtherItem();
                DataTable dt5 = Batch.BatchOtherDetail(id);
                for (int i = 0; i < dt5.Rows.Count; i++)
                {
                    tda3 = new BatchOtherItem();
                    tda3.OProcessidlst = BindProcessid();
                    tda3.OtProcessId = dt5.Rows[i]["PROCESSID"].ToString();
                    tda3.Seqe = dt5.Rows[i]["EPSEQ"].ToString();
                    tda3.Start = dt5.Rows[i]["ESDT"].ToString();
                    tda3.StartT = dt5.Rows[i]["EST"].ToString();
                    tda3.End = dt5.Rows[i]["EEDT"].ToString();
                    tda3.EndT = dt5.Rows[i]["EET"].ToString();
                    tda3.Total = dt5.Rows[i]["ETOTHRS"].ToString();
                    tda3.Break = dt5.Rows[i]["EBRHRS"].ToString();
                    tda3.RunHrs = dt5.Rows[i]["ERUNHRS"].ToString();
                    tda3.Remark = dt5.Rows[i]["ENARR"].ToString();
                    tda3.ID = id;
                    TData3.Add(tda3);
                }
                List<BatchParemItem> TData4 = new List<BatchParemItem>();
                BatchParemItem tda4 = new BatchParemItem();
                DataTable dt6 = Batch.BatchParemItemDetail(id);
                for (int i = 0; i < dt6.Rows.Count; i++)
                {
                    tda4 = new BatchParemItem();
                    tda4.Param = dt6.Rows[i]["PROCPARAM"].ToString();
                    tda4.PUnit = dt6.Rows[i]["PARAMUNIT"].ToString();
                    tda4.StartDate = dt6.Rows[i]["PSDT"].ToString();
                    tda4.StartTime = dt6.Rows[i]["PSTIME"].ToString();
                    tda4.EndDate = dt6.Rows[i]["PEDT"].ToString();
                    tda4.EndTime = dt6.Rows[i]["PETIME"].ToString();
                    tda4.Value = dt6.Rows[i]["PARAMVALUE"].ToString();

                    tda4.ID = id;
                    TData4.Add(tda4);
                }
                ca.BatchLst = TData;
                ca.BatchInLst = TData1;
                ca.BatchOutLst = TData2;
                ca.BatchOtherLst = TData3;
                ca.BatchParemLst = TData4;
            }
            return View(ca);
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
        public List<SelectListItem> BindProd()
        {
            try
            {
                DataTable dtDesg = Batch.GetProd();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["DOCID"].ToString(), Value = dtDesg.Rows[i]["PSBASICID"].ToString() });
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
                DataTable dtDesg = Batch.GetWorkCenter();
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
        public List<SelectListItem> BindWorkCenterid()
        {
            try
            {
                DataTable dtDesg = Batch.GetWorkCenter();
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
        public JsonResult GetItemJSON(string itemid)
        {
            BatchItem model = new BatchItem();
            model.Processidlst = BindProcessid(itemid);
            return Json(BindProcessid(itemid));

        }
        public JsonResult GetItemGrpJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindWorkCenter());
        }
        //public JsonResult GetProcessJSON(string processid)
        //{
        //    BatchCreation model = new BatchCreation();
        //    model.Processlst = BindProcess(processid);
        //    return Json(BindProcess(processid));

        //}
        public List<SelectListItem> BindProcess(string id)
        {
            try
            {
                DataTable dtDesg = Batch.GetProcess();
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
        public List<SelectListItem> BindProcessid(string id)
        {
            try
            {
                DataTable dtDesg = Batch.GetProcessid();
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
        public List<SelectListItem> BindItemlst()
        {
            try
            {
                DataTable dtDesg = Batch.GetItem();
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
       
        public List<SelectListItem> BindProcessid()
        {
            try
            {
                DataTable dtDesg = Batch.GetProcessid();
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
       

        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                
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

        public JsonResult GetProcessidJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindProcessid());
        }
        public JsonResult GetItemidJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemlst());
        }
        public JsonResult GetParamJSON()
        {
            BatchParemItem model = new BatchParemItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(model.Param);
        }
        public IActionResult ListBatchCreation()
        {
            IEnumerable<BatchCreation> cmp = Batch.GetAllBatchCreation();
            return View(cmp);
         }

    }
}
