using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
using PdfSharp.Pdf;

namespace Arasan.Controllers.Production
{
    public class ProductionScheduleController : Controller
    {

        IProductionScheduleService ProductionScheduleService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ProductionScheduleController(IProductionScheduleService _ProductionScheduleService, IConfiguration _configuratio)
        {
            ProductionScheduleService = _ProductionScheduleService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ProductionSchedule(string id)
        {
            ProductionSchedule ca = new ProductionSchedule();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Worklst = BindWorkCenter();
            ca.Enterd = Request.Cookies["UserId"];
            ca.RecList = BindEmp();
            ca.Planlst = BindPType();
            ca.Itemlst = BindItemlst();
            ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Schdate = DateTime.Now.ToString("dd-MMM-yyyy");
            ca.Processlst = BindProcess("");
            List<ProductionScheduleItem> TData = new List<ProductionScheduleItem>();
            ProductionScheduleItem tda = new ProductionScheduleItem();
            List<ProductionItem> TData1 = new List<ProductionItem>();
            ProductionItem tda1 = new ProductionItem();
            List<ProItem> TData2 = new List<ProItem>();
            ProItem tda2 = new ProItem();
            List<ProScItem> TData3 = new List<ProScItem>();
            ProScItem tda3 = new ProScItem();
            List<ProSchItem> TData4 = new List<ProSchItem>();
            ProSchItem tda4 = new ProSchItem();
            if (id == null)
            {
                for (int i = 0; i < 3; i++)
                {
                    tda = new ProductionScheduleItem();
                    tda.ItemGrouplst = BindItemGrplst();
                    tda.Itemlst = BindItemlst("");
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda1 = new ProductionItem();

                    tda1.PItemGrouplst = BindItemGrplst();
                    tda1.PItemlst = BindItemlst("");
                    tda1.Isvalid = "Y";
                    TData1.Add(tda1);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda2 = new ProItem();
                    tda2.Isvalid = "Y";
                    TData2.Add(tda2);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda3 = new ProScItem();
                    tda3.SchDate= DateTime.Now.ToString("dd-MMM-yyyy");
                    tda3.SItemGrouplst = BindItemGrplst();
                    tda3.SItemlst = BindItemlst("");
                    tda3.Isvalid = "Y";
                    TData3.Add(tda3);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda4 = new ProSchItem();
                    tda4.Isvalid = "Y";
                    TData4.Add(tda4);
                }
            }
            else
            {
                //ca = QCResultService.GetQCResultById(id);

                DataTable dt = new DataTable();
                dt = ProductionScheduleService.GetProductionSchedule(id);
                if (dt.Rows.Count > 0)
                {
                    ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                    ca.Type = dt.Rows[0]["SCHPLANTYPE"].ToString();
                    ca.DocId = dt.Rows[0]["DOCID"].ToString();
                    ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                    ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                    ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                    ca.ID = id;
                    ca.Schdate = dt.Rows[0]["SCHDATE"].ToString();
                    ca.Formula = dt.Rows[0]["FORMULA"].ToString();
                    ca.Proddt = dt.Rows[0]["PDOCDT"].ToString();
                    ca.Itemid = dt.Rows[0]["OPITEMID"].ToString();
                    ca.Unit = dt.Rows[0]["OPUNIT"].ToString();
                    ca.Exprunhrs = dt.Rows[0]["EXPRUNHRS"].ToString();
                    ca.Refno = dt.Rows[0]["REFSCHNO"].ToString();
                    ca.Amdno = dt.Rows[0]["AMDSCHNO"].ToString();
                    ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                    ca.Qty = Convert.ToDouble(dt.Rows[0]["OPQTY"].ToString() == "" ? "0" : dt.Rows[0]["OPQTY"].ToString());
                    ca.ProdQty = Convert.ToDouble(dt.Rows[0]["PRODQTY"].ToString() == "" ? "0" : dt.Rows[0]["PRODQTY"].ToString());

                }
                DataTable dt2 = new DataTable();
                dt2 = ProductionScheduleService.GetProductionScheduleDetail(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tda = new ProductionScheduleItem();
                        tda.ItemGrouplst = BindItemGrplst();
                        DataTable dtt1 = new DataTable();
                        dtt1 = datatrans.GetItemSubGroup(dt2.Rows[i]["RITEMID"].ToString());
                        if (dtt1.Rows.Count > 0)
                        {
                            tda.ItemGroupId = dtt1.Rows[i]["SUBGROUPCODE"].ToString();
                        }
                        tda.Itemlst = BindItemlst(tda.ItemGroupId);
                        tda.ItemId = dt2.Rows[i]["RITEMID"].ToString();
                        tda.saveItemId = dt2.Rows[i]["RITEMID"].ToString();
                        tda.Desc = dt2.Rows[i]["RITEMDESC"].ToString();
                       
                        tda.Unit = dt2.Rows[i]["RUNIT"].ToString();
                        tda.Isvalid = "Y";
                        tda.Input = dt2.Rows[i]["IPER"].ToString();
                        tda.Qty = dt2.Rows[i]["RQTY"].ToString();
                        tda.ID = id;
                        TData.Add(tda);
                    }
                }
                DataTable dt3 = new DataTable();
                dt3 = ProductionScheduleService.GetProductionScheduleOutputDetail(id);
                if (dt3.Rows.Count > 0)
                {
                    for (int i = 0; i < dt3.Rows.Count; i++)
                    {
                        tda1 = new ProductionItem();
                        tda1.PItemGrouplst = BindItemGrplst();
                      
                        DataTable dtt2 = new DataTable();
                        dtt2 = datatrans.GetItemSubGroup(dt3.Rows[i]["OITEMID"].ToString());
                        if (dtt2.Rows.Count > 0)
                        {
                            tda1.ItemGroup = dtt2.Rows[i]["SUBGROUPCODE"].ToString();
                        }
                        tda1.PItemlst = BindItemlst(tda1.ItemGroup);
                        tda1.Item = dt3.Rows[i]["OITEMID"].ToString();

                       

                        tda1.Item = dt3.Rows[i]["OITEMID"].ToString();
                        tda1.Unit = dt3.Rows[i]["OUNIT"].ToString();
                        tda1.Des = dt3.Rows[i]["OITEMDESC"].ToString();
                        tda1.Output = dt3.Rows[i]["OPER"].ToString();
                        tda1.Alam = dt3.Rows[i]["ALPER"].ToString();
                        tda1.OutputType = dt3.Rows[i]["OTYPE"].ToString();
                        tda1.Sch = dt3.Rows[i]["SCHQTY"].ToString();
                        tda1.Produced = dt3.Rows[i]["PQTY"].ToString();
                        tda1.Isvalid = "Y";

                        tda1.ID = id;
                        TData1.Add(tda1);
                    }

                }
                DataTable dt4 = new DataTable();
                dt4 = ProductionScheduleService.GetProductionScheduleParametersDetail(id);
                if (dt4.Rows.Count > 0)
                {
                    for (int i = 0; i < dt4.Rows.Count; i++)
                    {
                        tda2 = new ProItem();
                       
                        tda2.Parameters = dt4.Rows[i]["PARAMETERS"].ToString();
                        tda2.Unit = dt4.Rows[i]["UNIT"].ToString();
                        tda2.Initial = dt4.Rows[i]["IPARAMVALUE"].ToString();
                        tda2.Final = dt4.Rows[i]["FPARAMVALUE"].ToString();
                        tda2.Remarks = dt4.Rows[i]["REMARKS"].ToString();
                        tda2.Isvalid = "Y";
                        tda2.ID = id;
                        TData2.Add(tda2);
                    }

                }
                DataTable dt5 = new DataTable();
                dt5 = ProductionScheduleService.GetOutputDetailsDayWiseDetail(id);
                if (dt5.Rows.Count > 0)
                {
                    for (int i = 0; i < dt5.Rows.Count; i++)
                    {
                        tda3 = new ProScItem();
                        tda3.SItemGrouplst = BindItemGrplst();
                        DataTable dtt3 = new DataTable();
                        dtt3 = datatrans.GetItemSubGroup(dt5.Rows[i]["ODITEMID"].ToString());
                        if (dtt3.Rows.Count > 0)
                        {
                            tda3.ItemGrp = dtt3.Rows[i]["SUBGROUPCODE"].ToString();
                        }
                        tda3.SItemlst = BindItemlst(tda3.ItemGrp);
                        tda3.Itemd = dt5.Rows[i]["ODITEMID"].ToString();
                        tda3.Isvalid = "Y";
                        tda3.SchDate = dt5.Rows[i]["ODDATE"].ToString();
                        tda3.Hrs = dt5.Rows[i]["ODRUNHRS"].ToString();
                        tda3.Qty = dt5.Rows[i]["ODQTY"].ToString();
                        tda3.Change = dt5.Rows[i]["NOOFCHARGE"].ToString();
                        tda3.ID = id;
                        TData3.Add(tda3);
                    }

                }
                DataTable dt6 = new DataTable();
                dt6 = ProductionScheduleService.GetPackDetail(id);
                if (dt6.Rows.Count > 0)
                {
                    for (int i = 0; i < dt6.Rows.Count; i++)
                    {
                        tda4 = new ProSchItem();

                        tda4.Pack = dt6.Rows[i]["PKITEMID"].ToString();
                        tda4.Qty = dt6.Rows[i]["PKQTY"].ToString();
                        tda4.Isvalid = "Y";
                        tda4.ID = id;
                        TData4.Add(tda4);
                    }

                }

            }
           
            ca.PrsLst = TData;
            ca.ProLst = TData1;
            ca.Prlst = TData2;
            ca.ProscLst = TData3;
            ca.ProschedLst =TData4;
            return View(ca);
        }
        [HttpPost]
        public ActionResult ProductionSchedule(ProductionSchedule Cy, string id)
        {

            try
            {
                Cy.ID = id;
                string Strout = ProductionScheduleService.ProductionScheduleCRUD(Cy);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (Cy.ID == null)
                    {
                        TempData["notice"] = "ProductionSchedule Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = "ProductionSchedule Updated Successfully...!";
                    }
                    return RedirectToAction("ListProductionSchedule");

                }
                else
                {
                    ViewBag.PageTitle = "Edit ProductionSchedule";
                    TempData["notice"] = Strout;
                    //return View();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }

            return View(Cy);
        }

        public IActionResult ListProductionSchedule()
        {
            IEnumerable<ProductionSchedule> cmp = ProductionScheduleService.GetProductionSchedule();
            return View(cmp);
        }
        public List<SelectListItem> BindProcess(string id)
        {
            try
            {
                DataTable dtDesg = ProductionScheduleService.GetProcess();
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
        public List<SelectListItem> BindPType()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "MONTHLY", Value = "MONTHLY" });
                lstdesg.Add(new SelectListItem() { Text = "YEARLY", Value = "YEARLY" });
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
                DataTable dtDesg = ProductionScheduleService.GetItem(value);
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
        public ActionResult GetItemDetails(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();

                string unit = "";
                string Desc = "";
                dt = datatrans.GetItemDetails(ItemId);
                if (dt.Rows.Count > 0)
                {
                    unit = dt.Rows[0]["UNITID"].ToString();
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                }

                var result = new { unit = unit, desc = Desc };
                return Json(result);
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
                DataTable dtDesg = ProductionScheduleService.GetItem();
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
        public List<SelectListItem> BindItemGrplst()
        {
            try
            {
                DataTable dtDesg = ProductionScheduleService.GetItemSubGrp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SGCODE"].ToString(), Value = dtDesg.Rows[i]["ITEMSUBGROUPID"].ToString() });
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
                DataTable dtDesg = ProductionScheduleService.GetWorkCenter();
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
        public ActionResult GetItemDetail(string ItemId)
        {
            try
            {
                DataTable dt = new DataTable();
                string unit = "";
                string Desc = "";
                dt = datatrans.GetItemDetails(ItemId);

                if (dt.Rows.Count > 0)
                {
                    unit = dt.Rows[0]["UNITID"].ToString();
                    Desc = dt.Rows[0]["ITEMDESC"].ToString();
                }

                var result = new { unit = unit, Desc = Desc};
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        //public JsonResult GetProcessJSON(string processid)
        //{
        //    ProductionScheduleItem model = new ProductionScheduleItem();
        //    //model.Processlst = BindProcess(processid);
        //    return Json(BindProcess(processid));

        //}
        public JsonResult GetItemGrpJSON()
        {
           // ProductionScheduleItem model = new ProductionScheduleItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public JsonResult GetItemGrp1JSON()
        {
           // ProductionItem model = new ProductionItem();
            //model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public JsonResult GetItem1JSON(string itemid)
        {
            ProductionScheduleItem model = new ProductionScheduleItem();
            model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public JsonResult GetParemJSON( )
        {
            ProItem model = new ProItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(model);

        }
        public JsonResult GetItemGrp2JSON()
        {
            ProItem model = new ProItem();
            //model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        //public JsonResult GetItem2JSON(string itemid)
       // {
           // ProItem model = new ProItem();
            //model.Itemlst = BindItemlst(itemid);
           // return Json(BindItemlst(itemid));

       // }
        public JsonResult GetItemGrp3JSON()
        {
            ProScItem model = new ProScItem();
            //model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public JsonResult GetItem3JSON(string itemid)
        {
            ProScItem model = new ProScItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public JsonResult GetItemGrp4JSON()
        {
            ProSchItem model = new ProSchItem();
            //model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemGrplst());
        }
        public JsonResult GetItem4JSON(string itemid)
        {
            ProSchItem model = new ProSchItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public JsonResult GetItemJSON(string itemid)
        {
            ProductionScheduleItem model = new ProductionScheduleItem();
            //model.Itemlst = BindItemlst(itemid);
            return Json(BindItemlst(itemid));

        }
        public IActionResult ApproveProSch(string PROID)
        {
            ProductionSchedule ca = new ProductionSchedule();
            DataTable dt = ProductionScheduleService.EditProSche(PROID);
            if (dt.Rows.Count > 0)
            {
                ca.Branch = dt.Rows[0]["BRANCHID"].ToString();
                ca.Type = dt.Rows[0]["SCHPLANTYPE"].ToString();
                ca.DocId = dt.Rows[0]["DOCID"].ToString();
                ca.Docdate = dt.Rows[0]["DOCDATE"].ToString();
                ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                
                ca.Schdate = dt.Rows[0]["SCHDATE"].ToString();
                ca.Formula = dt.Rows[0]["FORMULA"].ToString();
                ca.Proddt = dt.Rows[0]["PDOCDT"].ToString();
                ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
                ca.Unit = dt.Rows[0]["OPUNIT"].ToString();
                ca.Exprunhrs = dt.Rows[0]["EXPRUNHRS"].ToString();
                ca.Refno = dt.Rows[0]["REFSCHNO"].ToString();
                ca.Amdno = dt.Rows[0]["AMDSCHNO"].ToString();
                ca.Entered = dt.Rows[0]["ENTEREDBY"].ToString();
                ca.Qty = Convert.ToDouble(dt.Rows[0]["OPQTY"].ToString() == "" ? "0" : dt.Rows[0]["OPQTY"].ToString());
                ca.ProdQty = Convert.ToDouble(dt.Rows[0]["PRODQTY"].ToString() == "" ? "0" : dt.Rows[0]["PRODQTY"].ToString());
                //ViewBag.entrytype = ca.EntryType;
                List<ProductionScheduleItem> TData = new List<ProductionScheduleItem>();
                ProductionScheduleItem tda = new ProductionScheduleItem();
                DataTable dtproin = ProductionScheduleService.ProIndetail(PROID);
                for (int i = 0; i < dtproin.Rows.Count; i++)
                {
                    tda = new ProductionScheduleItem();
                    //tda.ItemGrouplst = BindItemGrplst();
                    //DataTable dtt1 = new DataTable();
                    //dtt1 = datatrans.GetItemSubGroup(dtproin.Rows[i]["ITEMID"].ToString());
                    //if (dtt1.Rows.Count > 0)
                    //{
                    //    tda.ItemGroupId = dtt1.Rows[0]["SUBGROUPCODE"].ToString();
                    //}
                    //tda.Itemlst = BindItemlst(tda.ItemGroupId);
                    tda.ItemId = dtproin.Rows[i]["ITEMID"].ToString();
                    tda.saveItemId = dtproin.Rows[i]["ITEMID"].ToString();
                    tda.Desc = dtproin.Rows[i]["RITEMDESC"].ToString();

                    tda.Unit = dtproin.Rows[i]["RUNIT"].ToString();
                    tda.Isvalid = "Y";
                    tda.Input = dtproin.Rows[i]["IPER"].ToString();
                    tda.Qty = dtproin.Rows[i]["RQTY"].ToString();
                    
                    TData.Add(tda);
                }

                List<ProductionItem> TData1 = new List<ProductionItem>();
                ProductionItem tda1 = new ProductionItem();
                DataTable dtProInOut = ProductionScheduleService.ProOutDetail(PROID);
                for (int i = 0; i < dtProInOut.Rows.Count; i++)
                {
                    tda1 = new ProductionItem();
                    //tda1.PItemGrouplst = BindItemGrplst();

                    //DataTable dtt2 = new DataTable();
                    //dtt2 = datatrans.GetItemSubGroup(dtProInOut.Rows[i]["ITEMID"].ToString());
                    //if (dtt2.Rows.Count > 0)
                    //{
                    //    tda1.ItemGroup = dtt2.Rows[0]["SUBGROUPCODE"].ToString();
                    //}
                    //tda1.PItemlst = BindItemlst(tda1.ItemGroup);
                    tda1.Item = dtProInOut.Rows[i]["ITEMID"].ToString();



                     
                    tda1.Unit = dtProInOut.Rows[i]["OUNIT"].ToString();
                    tda1.Des = dtProInOut.Rows[i]["OITEMDESC"].ToString();
                    tda1.Output = dtProInOut.Rows[i]["OPER"].ToString();
                    tda1.Alam = dtProInOut.Rows[i]["ALPER"].ToString();
                    tda1.OutputType = dtProInOut.Rows[i]["OTYPE"].ToString();
                    tda1.Sch = dtProInOut.Rows[i]["SCHQTY"].ToString();
                    tda1.Produced = dtProInOut.Rows[i]["PQTY"].ToString();


                    
                    TData1.Add(tda1);
                }

                List<ProItem> TData2 = new List<ProItem>();
                ProItem tda2 = new ProItem();
                DataTable dtproParm = ProductionScheduleService.ProParmDetail(PROID);
                for (int i = 0; i < dtproParm.Rows.Count; i++)
                {
                    tda2 = new ProItem();

                    tda2.Parameters = dtproParm.Rows[i]["PARAMETERS"].ToString();
                    tda2.Unit = dtproParm.Rows[i]["UNIT"].ToString();
                    tda2.Initial = dtproParm.Rows[i]["IPARAMVALUE"].ToString();
                    tda2.Final = dtproParm.Rows[i]["FPARAMVALUE"].ToString();
                    tda2.Remarks = dtproParm.Rows[i]["REMARKS"].ToString();
                     
                    TData2.Add(tda2);
                }
                List<ProScItem> TData3 = new List<ProScItem>();
                ProScItem tda3 = new ProScItem();
                DataTable dtproDay = ProductionScheduleService.ProDailyDatDetail(PROID);
                for (int i = 0; i < dtproDay.Rows.Count; i++)
                {
                    tda3 = new ProScItem();
                    //tda3.SItemGrouplst = BindItemGrplst();
                    //DataTable dtt3 = new DataTable();
                    //dtt3 = datatrans.GetItemSubGroup(dtproDay.Rows[i]["ITEMID"].ToString());
                    //if (dtt3.Rows.Count > 0)
                    //{
                    //    tda3.ItemGrp = dtt3.Rows[0]["SUBGROUPCODE"].ToString();
                    //}
                    //tda3.SItemlst = BindItemlst(tda3.ItemGrp);
                    tda3.Itemd = dtproDay.Rows[i]["ITEMID"].ToString();

                    tda3.SchDate = dtproDay.Rows[i]["ODDATE"].ToString();
                    tda3.Hrs = dtproDay.Rows[i]["ODRUNHRS"].ToString();
                    tda3.Qty = dtproDay.Rows[i]["ODQTY"].ToString();
                    tda3.Change = dtproDay.Rows[i]["NOOFCHARGE"].ToString();
                   
                    TData3.Add(tda3);
                }
                List<ProSchItem> TData4 = new List<ProSchItem>();
                ProSchItem tda4 = new ProSchItem();
                DataTable dtproPack = ProductionScheduleService.ProPackDetail(PROID);
                for (int i = 0; i < dtproPack.Rows.Count; i++)
                {
                    tda4 = new ProSchItem();

                    tda4.Pack = dtproPack.Rows[i]["PKITEMID"].ToString();
                    tda4.Qty = dtproPack.Rows[i]["PKQTY"].ToString();

                  
                    TData4.Add(tda4);
                }
                ca.PrsLst = TData;
                ca.ProLst = TData1;
                ca.Prlst = TData2;
                ca.ProscLst = TData3;
                ca.ProschedLst = TData4;
            }
            return View(ca);
        }
        //public ActionResult DeleteCustomer(string tag, int id)
        //{
        //    ProductionSchedule rgl = new ProductionSchedule();
        //    //rgl.StatusChange(tag, id);
        //    //return RedirectToAction("ListCustomer");


        //    string Strout = ProductionScheduleService.StatusChange(tag,id);

        //    if (string.IsNullOrEmpty(Strout))
        //    {

        //        return RedirectToAction("ListCustomer");
        //    }
        //    else
        //    {
        //        TempData["notice"] = Strout;
        //        return RedirectToAction("ListCustomer");
        //    }
        //}
    }
}
