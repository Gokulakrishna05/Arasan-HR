using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using Arasan.Interface.Production;
using Arasan.Models;
using System.Data;
using System.Security.Cryptography.Pkcs;
using System.Xml.Linq;
using Microsoft.AspNetCore.Mvc.Rendering;
//using PdfSharp.Pdf;

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
            ca.Enterd = Request.Cookies["UserName"];
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
                    tda3.schdate = DateTime.Now.ToString("dd-MMM-yyyy");
                    tda3.SItemGrouplst = BindItemGrplst();
                    tda3.SItemlst = BindItemlst("");
                    tda3.isvalid = "Y";
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
                    ca.Proddt = dt.Rows[0]["PDUEDATE"].ToString();
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
                            for (int j = 0; j < dtt1.Rows.Count; j++)
                            {
                                tda.ItemGroupId = dtt1.Rows[j]["SUBGROUPCODE"].ToString();
                            }
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
                            for (int j = 0; j < dtt2.Rows.Count; j++)
                            {
                                tda1.ItemGroup = dtt2.Rows[j]["SUBGROUPCODE"].ToString();
                            }
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
                        tda3.itemd = dt5.Rows[i]["ODITEMID"].ToString();
                        tda3.isvalid = "Y";
                        tda3.schdate = dt5.Rows[i]["ODDATE"].ToString();
                        tda3.hrs = dt5.Rows[i]["ODRUNHRS"].ToString();
                       // tda3.qty = dt5.Rows[i]["ODQTY"].ToString();
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
             
            return View();
        }
        public ActionResult MyListDirectPurchaseGrid(string strStatus)
        {
            List<ProdSchItem> Reg = new List<ProdSchItem>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = (DataTable)ProductionScheduleService.GetAllProdSch(strStatus);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
               
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                string MoveToGRN = string.Empty;

                 EditRow = "<a href=DirectPurchase?id=" + dtUsers.Rows[i]["PSBASICID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";
                DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["PSBASICID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
              
                //if (Reg.Status == "GRN Generated")
                //{
                //    @Html.DisplayFor(Reg => Reg.Status);
                //}
                //else
                //{
                //MoveToGRN = "<a href=MoveToGRN?id=" + dtUsers.Rows[i]["DPBASICID"].ToString() + "><img src='../Images/move_quote.png' alt='View Details' width='20' /></a>";

                //}
                Reg.Add(new ProdSchItem
                {
                    id = Convert.ToInt64(dtUsers.Rows[i]["PSBASICID"].ToString()),
                    work = dtUsers.Rows[i]["WCID"].ToString(),
                    item = dtUsers.Rows[i]["ITEMID"].ToString(),
                    doc = dtUsers.Rows[i]["DOCID"].ToString(),
                    docDate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                   
                    editrow = EditRow,
                    delrow = DeleteRow,
                 


                });
            }

            return Json(new
            {
                Reg
            });

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
            return Json(BindItemlst());
        }
        public JsonResult GetItemGrp1JSON()
        {
           // ProductionItem model = new ProductionItem();
            //model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindItemlst());
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
                    tda3.itemd = dtproDay.Rows[i]["ITEMID"].ToString();

                    tda3.schdate = dtproDay.Rows[i]["ODDATE"].ToString();
                    tda3.hrs = dtproDay.Rows[i]["ODRUNHRS"].ToString();
                   // tda3.qty = dtproDay.Rows[i]["ODQTY"].ToString();
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
        public ActionResult DeleteCustomer(string tag )
        {
            ProductionSchedule rgl = new ProductionSchedule();
            //rgl.StatusChange(tag, id);
            //return RedirectToAction("ListCustomer");


            string Strout = ProductionScheduleService.StatusChange(tag );

            if (string.IsNullOrEmpty(Strout))
            {

                return RedirectToAction("ListProductionSchedule");
            }
            else
            {
                TempData["notice"] = Strout;
                return RedirectToAction("ListProductionSchedule");
            }
        }

        public ActionResult DeleteMR(string tag, int id)
        {

            string flag = ProductionScheduleService.StatusChange(tag, id);
            if (string.IsNullOrEmpty(flag))
            {

                return RedirectToAction("ListProductionSchedule");
            }
            else
            {
                TempData["notice"] = flag;
                return RedirectToAction("ListProductionSchedule");
            }
        }

        public IActionResult ProdSchedule(string id,string Ptype)
        {
            ProductionSchedule ca = new ProductionSchedule();
            ca.Brlst = BindBranch();
            ca.Branch = Request.Cookies["BranchId"];
            ca.Worklst = BindWorkCenter();
            ca.Enterd = Request.Cookies["UserName"];
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
                    
                    tda.Itemlst = BindItemlst();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 3; i++)
                {
                    tda1 = new ProductionItem();

                   
                    tda1.PItemlst = BindItemlst();
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
                    tda3.schdate = DateTime.Now.ToString("dd-MMM-yyyy");
                    
                    tda3.SItemlst = BindItemlst();
                    tda3.isvalid = "Y";
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
                
                if (Ptype == "Pyro")
                {
                    //ca = QCResultService.GetQCResultById(id);

                    DataTable dt = new DataTable();
                    dt = ProductionScheduleService.GetProdSche(id);
                    if (dt.Rows.Count > 0)
                    {

                        ca.Branch = Request.Cookies["BranchId"];
                        ca.Type = "MONTHLY";
                        DataTable dtv = datatrans.GetSequence("ProdS");
                        if (dtv.Rows.Count > 0)
                        {
                            ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
                        }
                        ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                        ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                        ca.WorkCenterid = dt.Rows[0]["PYWCID"].ToString();
                        ca.Days = dt.Rows[0]["PYPRODCAPD"].ToString();
                        ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                        ca.Processid = dt.Rows[0]["process"].ToString();
                        ca.detid = id;
                        ca.ttype = Ptype;
                        ca.Schdate = DateTime.Now.ToString("dd-MMM-yyyy");

                        ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
                        ca.saveitemid = dt.Rows[0]["PYITEMID"].ToString();
                        ca.Unit = dt.Rows[0]["UNITID"].ToString();


                        ca.Enterd = Request.Cookies["UserName"];
                        ca.Qty = Convert.ToDouble(dt.Rows[0]["PYPRODQTY"].ToString() == "" ? "0" : dt.Rows[0]["PYPRODQTY"].ToString());
                        ca.ProdQty = Convert.ToDouble(dt.Rows[0]["PYPRODQTY"].ToString() == "" ? "0" : dt.Rows[0]["PYPRODQTY"].ToString());

                        DataTable forcasid = datatrans.GetData("SELECT FORDETID,to_char(FROMDATE,'dd-MM-yy')FROMDATE,to_char(TODATE,'dd-MM-yy')TODATE FROM PSBASIC WHERE FORDETID ='" + id + "' and FORTYPE='Pyro'");
                        if(forcasid.Rows.Count>0)
                        {
                            string end= forcasid.Rows[0]["TODATE"].ToString();
                            DateTime start = DateTime.Parse(end);
                            start = start.AddDays(1);
                            ca.startdate = start.ToString("dd-MMM-yyyy");
                        }

                    }
                    DataTable dt2 = new DataTable();
                    dt2 = ProductionScheduleService.GetProdScheInputDetail(id);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new ProductionScheduleItem();


                            DataTable rawdt = new DataTable();
                            rawdt = datatrans.GetData("SELECT I.ITEMFROM,I2.ITEMID FROM ITEMMASTER I, ITEMMASTER I2 WHERE I.ITEMFROM = I2.ITEMMASTERID AND I.ITEMMASTERID = '" + dt2.Rows[i]["PYITEMID"].ToString() + "'");

                            tda.ItemId = rawdt.Rows[0]["ITEMID"].ToString();
                            tda.saveItemId = rawdt.Rows[0]["ITEMFROM"].ToString();
                            tda.Desc = dt2.Rows[i]["ITEMDESC"].ToString();

                            tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                            tda.Isvalid = "Y";
                            tda.Addict = dt2.Rows[i]["item"].ToString();
                            tda.AddictPer = dt2.Rows[i]["PYADDPER"].ToString();
                            tda.Qty = dt2.Rows[i]["PYREQAP"].ToString();
                            tda.ID = id;
                            TData.Add(tda);
                        }
                    }
                    DataTable dt3 = new DataTable();
                    dt3 = ProductionScheduleService.GetProdScheOutputDetail(id);
                    if (dt3.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            tda1 = new ProductionItem();

                            tda1.Item = dt3.Rows[i]["ITEMID"].ToString();


                            tda1.Output = "100";
                            tda1.OutputType = "C";
                            tda1.Alam = "100";
                            tda1.saveItemId = dt3.Rows[i]["PYITEMID"].ToString();
                            tda1.Unit = dt3.Rows[i]["UNITID"].ToString();
                            tda1.Des = dt3.Rows[i]["ITEMDESC"].ToString();
                            //tda1.Output = dt3.Rows[i]["OPER"].ToString();
                            //tda1.Alam = dt3.Rows[i]["ALPER"].ToString();
                            //tda1.OutputType = dt3.Rows[i]["OTYPE"].ToString();
                            tda1.Sch = dt3.Rows[i]["PYREQAP"].ToString();
                            tda1.Produced = dt3.Rows[i]["PYREQAP"].ToString();
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
                            tda3.itemd = dt5.Rows[i]["ODITEMID"].ToString();
                            tda3.isvalid = "Y";
                            tda3.schdate = dt5.Rows[i]["ODDATE"].ToString();
                            tda3.hrs = dt5.Rows[i]["ODRUNHRS"].ToString();
                            //tda3.qty = dt5.Rows[i]["ODQTY"].ToString();
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
                    for (int i = 0; i < 1; i++)
                    {
                        tda4 = new ProSchItem();
                        tda4.Itemlst = BindPackItem(ca.Itemid);
                    tda4.Isvalid = "Y";
                        TData4.Add(tda4);
                    }
                }
                if (Ptype == "Polish")
                {
                    //ca = QCResultService.GetQCResultById(id);

                    DataTable dt = new DataTable();
                    dt = ProductionScheduleService.GetProdSchePolish(id);
                    if (dt.Rows.Count > 0)
                    {

                        ca.Branch = Request.Cookies["BranchId"];
                        ca.Type = "MONTHLY";
                        DataTable dtv = datatrans.GetSequence("ProdS");
                        if (dtv.Rows.Count > 0)
                        {
                            ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
                        }
                        ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                        ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                        ca.WorkCenterid = dt.Rows[0]["PIGWCID"].ToString();
                        ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                        ca.Processid = dt.Rows[0]["process"].ToString();
                        ca.detid = id;
                        ca.ttype = Ptype;
                        ca.Schdate = DateTime.Now.ToString("dd-MMM-yyyy");
                        ca.Days = dt.Rows[0]["PIGPRODD"].ToString();
                        ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
                        ca.saveitemid = dt.Rows[0]["PIGITEMID"].ToString();
                        ca.Unit = dt.Rows[0]["UNITID"].ToString();


                        ca.Enterd = Request.Cookies["UserName"];
                        ca.Qty = Convert.ToDouble(dt.Rows[0]["PIGRAWREQPY"].ToString() == "" ? "0" : dt.Rows[0]["PIGRAWREQPY"].ToString());
                        ca.ProdQty = Convert.ToDouble(dt.Rows[0]["PIGRAWREQPY"].ToString() == "" ? "0" : dt.Rows[0]["PIGRAWREQPY"].ToString());
                        DataTable forcasid = datatrans.GetData("SELECT FORDETID,to_char(FROMDATE,'dd-MM-yy')FROMDATE,to_char(TODATE,'dd-MM-yy')TODATE FROM PSBASIC WHERE FORDETID ='" + id + "' and FORTYPE='Polish'");
                        if (forcasid.Rows.Count > 0)
                        {
                            string end = forcasid.Rows[0]["TODATE"].ToString();
                            DateTime start = DateTime.Parse(end);
                            start = start.AddDays(1);
                            ca.startdate = start.ToString("dd-MMM-yyyy");
                        }

                    }
                    DataTable dt2 = new DataTable();
                    dt2 = ProductionScheduleService.GetPolishInputDetail(id);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new ProductionScheduleItem();


                           
                            tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                            tda.saveItemId = dt2.Rows[i]["PIGRAWMAT"].ToString();
                            tda.Desc = dt2.Rows[i]["ITEMDESC"].ToString();

                            tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                            tda.Isvalid = "Y";
                            tda.Addict = dt2.Rows[i]["item"].ToString();
                            tda.AddictPer = dt2.Rows[i]["PIGADDPER"].ToString();
                            tda.Qty = dt2.Rows[i]["PIGRAWREQPY"].ToString();
                            tda.ID = id;
                            TData.Add(tda);
                        }
                    }
                    DataTable dt3 = new DataTable();
                    dt3 = ProductionScheduleService.GetPolishOutputDetail(id);
                    if (dt3.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            tda1 = new ProductionItem();

                            tda1.Item = dt3.Rows[i]["ITEMID"].ToString();


                            tda1.Output = "100";
                            tda1.OutputType = "C";
                            tda1.Alam = "100";
                            tda1.saveItemId = dt3.Rows[i]["PIGITEMID"].ToString();
                            tda1.Unit = dt3.Rows[i]["UNITID"].ToString();
                            tda1.Des = dt3.Rows[i]["ITEMDESC"].ToString();
                            //tda1.Output = dt3.Rows[i]["OPER"].ToString();
                            //tda1.Alam = dt3.Rows[i]["ALPER"].ToString();
                            //tda1.OutputType = dt3.Rows[i]["OTYPE"].ToString();
                            tda1.Sch = dt3.Rows[i]["PIGRAWREQPY"].ToString();
                            tda1.Produced = dt3.Rows[i]["PIGRAWREQPY"].ToString();
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
                            tda3.itemd = dt5.Rows[i]["ODITEMID"].ToString();
                            tda3.isvalid = "Y";
                            tda3.schdate = dt5.Rows[i]["ODDATE"].ToString();
                            tda3.hrs = dt5.Rows[i]["ODRUNHRS"].ToString();
                            //tda3.qty = dt5.Rows[i]["ODQTY"].ToString();
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
                    for (int i = 0; i < 1; i++)
                    {
                        tda4 = new ProSchItem();
                        tda4.Itemlst = BindPackItem(ca.Itemid);
                        tda4.Isvalid = "Y";
                        TData4.Add(tda4);
                    }
                }
                if (Ptype == "RVD")
                {
                    //ca = QCResultService.GetQCResultById(id);

                    DataTable dt = new DataTable();
                    dt = ProductionScheduleService.GetProdScheRVD(id);
                    if (dt.Rows.Count > 0)
                    {

                        ca.Branch = Request.Cookies["BranchId"];
                        ca.Type = "MONTHLY";
                        DataTable dtv = datatrans.GetSequence("ProdS");
                        if (dtv.Rows.Count > 0)
                        {
                            ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
                        }
                        ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                        ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                        ca.WorkCenterid = dt.Rows[0]["RVDWCID"].ToString();
                        ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                        ca.Processid = dt.Rows[0]["process"].ToString();
                        ca.Days = dt.Rows[0]["RVDPRODD"].ToString();
                        ca.detid = id;
                        ca.ttype = Ptype;
                        ca.Schdate = DateTime.Now.ToString("dd-MMM-yyyy");

                        ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
                        ca.saveitemid = dt.Rows[0]["RVDITEMID"].ToString();
                        ca.Unit = dt.Rows[0]["UNITID"].ToString();


                        ca.Enterd = Request.Cookies["UserName"];
                        ca.Qty = Convert.ToDouble(dt.Rows[0]["RVDRAWQTY"].ToString() == "" ? "0" : dt.Rows[0]["RVDRAWQTY"].ToString());
                        ca.ProdQty = Convert.ToDouble(dt.Rows[0]["RVDRAWQTY"].ToString() == "" ? "0" : dt.Rows[0]["RVDRAWQTY"].ToString());
                        DataTable forcasid = datatrans.GetData("SELECT FORDETID,to_char(FROMDATE,'dd-MM-yy')FROMDATE,to_char(TODATE,'dd-MM-yy')TODATE FROM PSBASIC WHERE FORDETID ='" + id + "' and FORTYPE='RVD'");
                        if (forcasid.Rows.Count > 0)
                        {
                            string end = forcasid.Rows[0]["TODATE"].ToString();
                            DateTime start = DateTime.Parse(end);
                            start = start.AddDays(1);
                            ca.startdate = start.ToString("dd-MMM-yyyy");
                        }

                    }
                    DataTable dt2 = new DataTable();
                    dt2 = ProductionScheduleService.GetRVDInputDetail(id);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new ProductionScheduleItem();



                            tda.ItemId = dt2.Rows[i]["ITEMID"].ToString();
                            tda.saveItemId = dt2.Rows[i]["RVDRAWMAT"].ToString();
                            tda.Desc = dt2.Rows[i]["ITEMDESC"].ToString();

                            tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                            tda.Isvalid = "Y";
                            
                             string Per = datatrans.GetDataString("SELECT add1per FROM ITEMMASTER WHERE ITEMMASTERID='" + dt2.Rows[i]["RVDITEMID"].ToString() + "'");
                            tda.Addict = dt2.Rows[i]["item"].ToString();
                            tda.AddictPer = Per;
                            tda.Qty = dt2.Rows[i]["RVDRAWQTY"].ToString();
                            tda.ID = id;
                            TData.Add(tda);
                        }
                    }
                    DataTable dt3 = new DataTable();
                    dt3 = ProductionScheduleService.GetRVDOutputDetail(id);
                    if (dt3.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            tda1 = new ProductionItem();

                            tda1.Item = dt3.Rows[i]["ITEMID"].ToString();


                            tda1.Output = "100";
                            tda1.OutputType = "C";
                            tda1.Alam = "100";
                            tda1.saveItemId = dt3.Rows[i]["RVDITEMID"].ToString();
                            tda1.Unit = dt3.Rows[i]["UNITID"].ToString();
                            tda1.Des = dt3.Rows[i]["ITEMDESC"].ToString();
                            //tda1.Output = dt3.Rows[i]["OPER"].ToString();
                            //tda1.Alam = dt3.Rows[i]["ALPER"].ToString();
                            //tda1.OutputType = dt3.Rows[i]["OTYPE"].ToString();
                            tda1.Sch = dt3.Rows[i]["RVDRAWQTY"].ToString();
                            tda1.Produced = dt3.Rows[i]["RVDRAWQTY"].ToString();
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
                            tda3.itemd = dt5.Rows[i]["ODITEMID"].ToString();
                            tda3.isvalid = "Y";
                            tda3.schdate = dt5.Rows[i]["ODDATE"].ToString();
                            tda3.hrs = dt5.Rows[i]["ODRUNHRS"].ToString();
                            //tda3.qty = dt5.Rows[i]["ODQTY"].ToString();
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
                    for (int i = 0; i < 1; i++)
                    {
                        tda4 = new ProSchItem();
                        tda4.Itemlst = BindPackItem(ca.Itemid);
                        tda4.Isvalid = "Y";
                        TData4.Add(tda4);
                    }
                }
                if (Ptype == "Paste")
                {
                    //ca = QCResultService.GetQCResultById(id);

                    DataTable dt = new DataTable();
                    dt = ProductionScheduleService.GetProdSchePaste(id);
                    if (dt.Rows.Count > 0)
                    {

                        ca.Branch = Request.Cookies["BranchId"];
                        ca.Type = "MONTHLY";
                        DataTable dtv = datatrans.GetSequence("ProdS");
                        if (dtv.Rows.Count > 0)
                        {
                            ca.DocId = dtv.Rows[0]["PREFIX"].ToString() + " " + dtv.Rows[0]["last"].ToString();
                        }
                        ca.Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
                        ca.WorkCenter = dt.Rows[0]["WCID"].ToString();
                        ca.WorkCenterid = dt.Rows[0]["PAWCID"].ToString();
                        ca.Process = dt.Rows[0]["PROCESSID"].ToString();
                        ca.Processid = dt.Rows[0]["process"].ToString();
                        ca.Days = dt.Rows[0]["PAPRODD"].ToString();
                        ca.detid = id;
                        ca.ttype = Ptype;
                        ca.Schdate = DateTime.Now.ToString("dd-MMM-yyyy");

                        ca.Itemid = dt.Rows[0]["ITEMID"].ToString();
                        ca.saveitemid = dt.Rows[0]["PAITEMID"].ToString();
                        ca.Unit = dt.Rows[0]["UNITID"].ToString();


                        ca.Enterd = Request.Cookies["UserName"];
                        ca.Qty = Convert.ToDouble(dt.Rows[0]["PAAPPOW"].ToString() == "" ? "0" : dt.Rows[0]["PAAPPOW"].ToString());
                        ca.ProdQty = Convert.ToDouble(dt.Rows[0]["PAAPPOW"].ToString() == "" ? "0" : dt.Rows[0]["PAAPPOW"].ToString());
                        DataTable forcasid = datatrans.GetData("SELECT FORDETID,to_char(FROMDATE,'dd-MM-yy')FROMDATE,to_char(TODATE,'dd-MM-yy')TODATE FROM PSBASIC WHERE FORDETID ='" + id + "'and FORTYPE='Paste'");
                        if (forcasid.Rows.Count > 0)
                        {
                            string end = forcasid.Rows[0]["TODATE"].ToString();
                            DateTime start = DateTime.Parse(end);
                            start = start.AddDays(1);
                            ca.startdate = start.ToString("dd-MMM-yyyy");
                        }

                    }
                    DataTable dt2 = new DataTable();
                    dt2 = ProductionScheduleService.GetPasteInputDetail(id);
                    if (dt2.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt2.Rows.Count; i++)
                        {
                            tda = new ProductionScheduleItem();

                            DataTable rawdt = new DataTable();
                            rawdt = datatrans.GetData("SELECT I.ITEMFROM,I2.ITEMID FROM ITEMMASTER I, ITEMMASTER I2 WHERE I.ITEMFROM = I2.ITEMMASTERID AND I.ITEMMASTERID = '" + dt2.Rows[i]["PAITEMID"].ToString() + "'");


                            tda.ItemId = rawdt.Rows[0]["ITEMID"].ToString();
                            tda.saveItemId = rawdt.Rows[0]["ITEMFROM"].ToString();
                            tda.Desc = dt2.Rows[i]["ITEMDESC"].ToString();

                            tda.Unit = dt2.Rows[i]["UNITID"].ToString();
                            tda.Isvalid = "Y";
                           
                             tda.Addict = dt2.Rows[i]["ITEMID"].ToString();
                            tda.AddictPer = dt2.Rows[i]["PAALLADDIT"].ToString();
                            tda.Qty = dt2.Rows[i]["PAAPPOW"].ToString();
                            tda.ID = id;
                            TData.Add(tda);
                        }
                    }
                    DataTable dt3 = new DataTable();
                    dt3 = ProductionScheduleService.GetPasteOutputDetail(id);
                    if (dt3.Rows.Count > 0)
                    {
                        for (int i = 0; i < dt3.Rows.Count; i++)
                        {
                            tda1 = new ProductionItem();

                            tda1.Item = dt3.Rows[i]["ITEMID"].ToString();


                            tda1.Output = "100";
                            tda1.OutputType = "C";
                            tda1.Alam = "100";
                            tda1.saveItemId = dt3.Rows[i]["PAITEMID"].ToString();
                            tda1.Unit = dt3.Rows[i]["UNITID"].ToString();
                            tda1.Des = dt3.Rows[i]["ITEMDESC"].ToString();
                            //tda1.Output = dt3.Rows[i]["OPER"].ToString();
                            //tda1.Alam = dt3.Rows[i]["ALPER"].ToString();
                            //tda1.OutputType = dt3.Rows[i]["OTYPE"].ToString();
                            tda1.Sch = dt3.Rows[i]["PAAPPOW"].ToString();
                            tda1.Produced = dt3.Rows[i]["PAAPPOW"].ToString();
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
                            tda3.itemd = dt5.Rows[i]["ODITEMID"].ToString();
                            tda3.isvalid = "Y";
                            tda3.schdate = dt5.Rows[i]["ODDATE"].ToString();
                            tda3.hrs = dt5.Rows[i]["ODRUNHRS"].ToString();
                            //tda3.qty = dt5.Rows[i]["ODQTY"].ToString();
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
                    for (int i = 0; i < 1; i++)
                    {
                        tda4 = new ProSchItem();
                        tda4.Itemlst = BindPackItem(ca.Itemid);
                        tda4.Isvalid = "Y";
                        TData4.Add(tda4);
                    }
                }

            }

            ca.PrsLst = TData;
            ca.ProLst = TData1;
            ca.Prlst = TData2;
            ca.ProscLst = TData3;
            ca.ProschedLst = TData4;
            return View(ca);
        }
        public ActionResult GetDaywice(string stdate, string enddate, string outitem,double outqty)
        {
            ProductionSchedule ca = new ProductionSchedule();
            List<ProScItem> TData = new List<ProScItem>();
            ProScItem tda = new ProScItem();

            DateTime start = DateTime.Parse(stdate);
            DateTime end = DateTime.Parse(enddate);

            TimeSpan difference = end-start;
            int daysAgo = (int)difference.TotalDays;
            for (int i = 0; i <= daysAgo; i++)
            {
                tda = new ProScItem();

                tda.schdate = start.ToString("dd-MMM-yy");
                tda.hrs = "22";
                double h = Convert.ToDouble(tda.hrs);
                
                tda.qty = outqty/h;
                tda.itemd = outitem;
                 
                tda.isvalid = "Y";

                TData.Add(tda);
                start = start.AddDays(1);

            }


            ca.ProscLst = TData;
            return Json(ca.ProscLst);



        }

        public List<SelectListItem> BindPackItem(string value)
        {
            try
            {
                DataTable dtDesg = ProductionScheduleService.GetPackItem(value);
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["PACKMAT"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
