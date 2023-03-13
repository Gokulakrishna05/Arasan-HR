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
                for (int i = 0; i < 3; i++)
                {
                    tda1 = new BatchInItem();
                    tda1.IProcesslst = BindProcessid();
                    tda1.Itemlst = BindItemlst();
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda2 = new BatchOutItem();
                    tda2.OProcesslst = BindProcessid();
                    tda2.OItemlst = BindItemlst();
                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda3 = new BatchOtherItem();
                    tda3.OProcessidlst = BindProcessid();
                    tda3.Isvalid = "Y";
                    TData3.Add(tda3);
                }
                for (int i = 0; i < 3; i++)
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
                        //DataTable dt3 = new DataTable();
                        //dt3 = Batch.GetWorkCenterGr(dt2.Rows[i]["PROCESSID"].ToString());
                        //if (dt3.Rows.Count > 0)
                        //{
                        //    tda.WorkId = dt3.Rows[0]["WCID"].ToString();
                        //}
                        tda.Processidlst = BindProcess(tda.WorkId);
                    
                        tda.ProcessId = dt2.Rows[0]["PROCESSID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["PROCESSID"].ToString();
                        tda.Seq = dt2.Rows[0]["PSEQ"].ToString();
                        tda.Req = dt2.Rows[0]["INSREQ"].ToString();
                        tda.ID = id;
                       
                    }

                }
                DataTable dt3 = new DataTable();
                dt3 = Batch.GetBatchCreationInputDetail(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new BatchInItem();
                         
                        tda1.Process = dt3.Rows[0]["IPROCESSID"].ToString();
                        tda1.Item = dt3.Rows[0]["IITEMID"].ToString();
                        tda1.Unit = dt3.Rows[0]["IUNIT"].ToString();
                        tda1.Qty = dt3.Rows[0]["IQTY"].ToString();

                       
                        tda1.ID = id;

                    }

                }
            }
            
            ca.BatchParemLst = TData4;
            ca.BatchOtherLst = TData3;
            ca.BatchOutLst = TData2;
            ca.BatchInLst = TData1;
            ca.BatchLst = TData;
            
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
        public JsonResult GetProcessJSON(string processid)
        {
            BatchCreation model = new BatchCreation();
            model.Processlst = BindProcess(processid);
            return Json(BindProcess(processid));

        }
        public List<SelectListItem> BindProcess(string id)
        {
            try
            {
                DataTable dtDesg = Batch.GetProcess(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
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
                DataTable dtDesg = Batch.GetProcessid(id);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PROCESSID"].ToString(), Value = dtDesg.Rows[i]["WCBASICID"].ToString() });
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
        public IActionResult ListBatchCreation()
        {
            IEnumerable<BatchCreation> cmp = Batch.GetAllBatchCreation();
            return View(cmp);
         }

    }
}
