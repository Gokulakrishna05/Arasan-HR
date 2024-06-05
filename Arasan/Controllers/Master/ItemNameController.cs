using System.Collections.Generic;
using System.Data;
using Arasan.Interface;
using Arasan.Interface.Master;
using Arasan.Models;
using Arasan.Services.Master;
using Arasan.Services.Store_Management;
//using DocumentFormat.OpenXml.Presentation;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Arasan.Controllers.Master
{
    public class ItemNameController : Controller
    {

        IItemNameService ItemNameService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        public ItemNameController(IItemNameService _ItemNameService, IConfiguration _configuratio)
        {
            ItemNameService = _ItemNameService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult ItemName(string id)
        {
            ItemName ca = new ItemName();
            ca.IgLst = BindItemGroup();
            ca.Iclst = BindItemCategory();
            ca.Isglst = BindItemSubGroup();
            ca.Tarrifflst = BindTarrif();
            ca.Hsn = BindHSNcode();
            ca.Bin = BindBinID();
            ca.qclst = BindQCTemp();
            ca.fqclst = BindQCTemp();
            ca.Ledgerlst = BindLedger();
            ca.Itemlst = BindItem();
            ca.unitlst = BindUnit();
            ca.purlst = Bindpurchase();
            ca.costlst = Bindcostcate();
            ca.createdby = Request.Cookies["UserId"];
            ca.Selling = "0";
            ca.runhrs = "0";
            ca.runhrsqty = "0";
            ca.rundet = "NO";
            List<SupItem> TData = new List<SupItem>();
            SupItem tda = new SupItem();

            List<BinItem> TDatab = new List<BinItem>();
            BinItem tdaB = new BinItem();

            List<UnitItem> TDatau = new List<UnitItem>();
            UnitItem tdau = new UnitItem();
            List<LocdetItem> TDatal = new List<LocdetItem>();
            LocdetItem tdal = new LocdetItem();

            if (id == null)

            {
                for (int i = 0; i < 1; i++)
                {
                    tdau = new UnitItem();
                    tdau.UnitLst = BindUnit();
                    tdau.Isvalid = "Y";
                    TDatau.Add(tdau);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda = new SupItem();
                    tda.Suplierlst = BindSupplier();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }
                for (int i = 0; i < 1; i++)
                {
                    tdal = new LocdetItem();
                    tdal.locLst = BindLoc();
                    tdal.Isvalid = "Y";
                    TDatal.Add(tdal);
                }
                for (int i = 0; i < 1; i++)
                {
                    tdaB = new BinItem();
                    tdaB.Isvalid = "Y";
                    TDatab.Add(tdaB);
                }
            }
            else
            {
                // ca = ItemNameService.GetSupplierDetailById(id);

                DataTable dt = new DataTable();
                dt = ItemNameService.GetItemNameDetails(id);
                if (dt.Rows.Count > 0)
                {
                    ca.ItemG = dt.Rows[0]["IGROUP"].ToString();
                    ca.ItemSub = dt.Rows[0]["ISUBGROUP"].ToString();
                    ca.SubCat = dt.Rows[0]["SUBCATEGORY"].ToString();
                    
                    ca.Item = dt.Rows[0]["ITEMID"].ToString();
                    ca.ItemDes = dt.Rows[0]["ITEMDESC"].ToString();
                    ca.Reorderqu = dt.Rows[0]["REORDERQTY"].ToString();
                    ca.Reorderlvl = dt.Rows[0]["REORDERLVL"].ToString();
                    ca.lot = dt.Rows[0]["LOTYN"].ToString();
                    ca.qctest = dt.Rows[0]["QCT"].ToString();
                    ca.Drumyn = dt.Rows[0]["DRUMYN"].ToString();
                     
                    ca.Minlvl = dt.Rows[0]["MINSTK"].ToString();
                    
                    ca.Unit = dt.Rows[0]["PRIUNIT"].ToString();
                    ca.Hcode = dt.Rows[0]["HSN"].ToString();
                    ca.Selling = dt.Rows[0]["SELLINGPRICE"].ToString();
                    ca.itemfrom = dt.Rows[0]["ITEMFROM"].ToString();
                    ca.Tarriff = dt.Rows[0]["TARIFFID"].ToString();
                    ca.purchasecate = dt.Rows[0]["PURCAT"].ToString();
                    ca.major = dt.Rows[0]["MAJORYN"].ToString();
                    ca.lastdate = dt.Rows[0]["LATPURDT"].ToString();
                     
                    ca.Expiry = dt.Rows[0]["EXPYN"].ToString();
                    ca.ValuationMethod = dt.Rows[0]["VALMETHOD"].ToString();
                    ca.Serial = dt.Rows[0]["SERIALYN"].ToString();
                    ca.Batch = dt.Rows[0]["BSTATEMENTYN"].ToString();
                    ca.QCTemp = dt.Rows[0]["TEMPLATEID"].ToString();
                    ca.QCRequired = dt.Rows[0]["QCCOMPFLAG"].ToString();
                    ca.Latest = dt.Rows[0]["LATPURPRICE"].ToString();
                     
                    ca.Rejection = dt.Rows[0]["REJRAWMATPER"].ToString();
                    ca.Percentage = dt.Rows[0]["RAWMATPER"].ToString();
                    ca.PercentageAdd = dt.Rows[0]["ADD1PER"].ToString();
                    ca.AddItem = dt.Rows[0]["ADD1"].ToString();
                    ca.RawMaterial = dt.Rows[0]["RAWMATCAT"].ToString();
                    ca.Ledger = dt.Rows[0]["ITEMACC"].ToString();

                     
                    ca.FQCTemp = dt.Rows[0]["PTEMPLATEID"].ToString();

                    //ca.QCTemp = dt.Rows[0]["IQCTEMP"].ToString();
                    //ca.FQCTemp = dt.Rows[0]["FGQCTEMP"].ToString();

                    ca.BinId = dt.Rows[0]["BINNO"].ToString();
                    ca.BinYN = dt.Rows[0]["BINYN"].ToString();
                    ca.Curing = dt.Rows[0]["CURINGDAY"].ToString();
                    ca.Auto = dt.Rows[0]["AUTOINDENT"].ToString();
                    ca.rundet = dt.Rows[0]["RHYN"].ToString();
                    ca.runhrs = dt.Rows[0]["RUNHRS"].ToString();
                    ca.runhrsqty = dt.Rows[0]["RUNPERQTY"].ToString();
                    ca.costcat = dt.Rows[0]["COSTCATEGORY"].ToString();
                    ca.autocon = dt.Rows[0]["AUTOCONSYN"].ToString();
                    ca.createdby = Request.Cookies["UserId"];
                }
                DataTable dt2 = new DataTable();
                dt2 = ItemNameService.GetBinDeatils(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tdaB = new BinItem();
                        tdaB.BinID = dt2.Rows[0]["BINID"].ToString();
                        tdaB.BinYN = dt2.Rows[0]["BINYN"].ToString();
                        tdaB.Isvalid = "Y";
                        TDatab.Add(tdaB);
                    }
                }

                DataTable dtt = new DataTable();
                dtt = ItemNameService.GetAllSupplier(id);

                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new SupItem();
                        tda.Suplierlst = BindSupplier();
                        tda.SupName = dtt.Rows[i]["SUPPLIERID"].ToString();
                        tda.SupplierPart = dtt.Rows[i]["SUPPLIERPARTNO"].ToString();
                        tda.PurchasePrice = dtt.Rows[i]["SPURPRICE"].ToString();
                        tda.Delivery = dtt.Rows[i]["DELDAYS"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                DataTable dtt1 = new DataTable();
                dtt1 = ItemNameService.GetAllUnit(id);

                if (dtt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {
                        tdau = new UnitItem();
                        tdau.UnitLst = BindUnit();
                        tdau.Unit = dtt1.Rows[i]["UNIT"].ToString();
                        tdau.cf = dtt1.Rows[i]["CF"].ToString();
                        tdau.unittype = dtt1.Rows[i]["UNITTYPE"].ToString();
                        tdau.uniqid = dtt1.Rows[i]["UNITUNIQUEID"].ToString();
                        tdau.Isvalid = "Y";
                        TDatau.Add(tdau);
                    }
                }
                DataTable dtt2 = new DataTable();
                dtt2 = datatrans.GetData("SELECT ITEMMASTERID,LOCID,REORDERLEVEL,MINQTYN,MAXQTYN,BSTMGROUP,LOCINVDETAILROW FROM LOCINVDETAIL WHERE ITEMMASTERID='"+id+"'");

                if (dtt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt2.Rows.Count; i++)
                    {
                        tdal = new LocdetItem();
                        tdal.locLst = BindLoc();
                        tdal.loc = dtt2.Rows[i]["LOCID"].ToString();
                        tdal.minlevel = dtt2.Rows[i]["MINQTYN"].ToString();
                        tdal.maxlevel = dtt2.Rows[i]["MAXQTYN"].ToString();
                        tdal.bank = dtt2.Rows[i]["BSTMGROUP"].ToString();
                        tdal.reorder = dtt2.Rows[i]["REORDERLEVEL"].ToString();
                        tdal.Isvalid = "Y";
                        TDatal.Add(tdal);
                    }
                }
            }
            ca.Binlst = TDatab;
            ca.Suplst = TData;
            ca.locdetlst = TDatal;
            ca.unititemlst = TDatau;
            ca.locdetlst = TDatal;
            return View(ca);
        }
        public IActionResult ItemCreate(string id)
        {
            ItemName ca = new ItemName();
            ca.IgLst = BindItemGroup();
            ca.Iclst = BindItemCategory();
            ca.Isglst = BindItemSubGroup();
            ca.Hsn = BindHSNcode();
            ca.Bin = BindBinID();
            ca.qclst = BindQCTemp();
            ca.Tarrifflst = BindTarrif();
            ca.fqclst = BindQCTemp();
            ca.Ledgerlst = BindLedger();
            ca.Itemlst = BindItem();
            ca.unitlst = BindUnit();
            ca.createdby = Request.Cookies["UserId"];
            List<SupItem> TData = new List<SupItem>();
            SupItem tda = new SupItem();

            List<BinItem> TDatab = new List<BinItem>();
            BinItem tdaB = new BinItem();

            List<UnitItem> TDatau = new List<UnitItem>();
            UnitItem tdau = new UnitItem();

            if (id == null)

            {
                for (int i = 0; i < 1; i++)
                {
                    tdau = new UnitItem();
                    tdau.UnitLst = BindUnit();
                    tdau.Isvalid = "Y";
                    TDatau.Add(tdau);
                }
                for (int i = 0; i < 1; i++)
                {
                    tda = new SupItem();
                    tda.Suplierlst = BindSupplier();
                    tda.Isvalid = "Y";
                    TData.Add(tda);
                }

                for (int i = 0; i < 1; i++)
                {
                    tdaB = new BinItem();
                    tdaB.Isvalid = "Y";
                    TDatab.Add(tdaB);
                }
            }
            else
            {
                // ca = ItemNameService.GetSupplierDetailById(id);

                DataTable dt = new DataTable();
                dt = ItemNameService.GetItemNameDetails(id);
                if (dt.Rows.Count > 0)
                {
                    ca.ItemG = dt.Rows[0]["IGROUP"].ToString();
                    ca.ItemSub = dt.Rows[0]["ISUBGROUP"].ToString();
                    ca.SubCat = dt.Rows[0]["SUBCATEGORY"].ToString();

                    ca.Item = dt.Rows[0]["ITEMID"].ToString();
                    ca.ItemDes = dt.Rows[0]["ITEMDESC"].ToString();
                    ca.Reorderqu = dt.Rows[0]["REORDERQTY"].ToString();
                    ca.Reorderlvl = dt.Rows[0]["REORDERLVL"].ToString();

                    ca.Minlvl = dt.Rows[0]["MINSTK"].ToString();

                    ca.Unit = dt.Rows[0]["PRIUNIT"].ToString();
                    ca.Hcode = dt.Rows[0]["HSN"].ToString();
                    ca.Selling = dt.Rows[0]["SELLINGPRICE"].ToString();

                    ca.Expiry = dt.Rows[0]["EXPYN"].ToString();
                    ca.ValuationMethod = dt.Rows[0]["VALMETHOD"].ToString();
                    ca.Serial = dt.Rows[0]["SERIALYN"].ToString();
                    ca.Batch = dt.Rows[0]["BSTATEMENTYN"].ToString();
                    ca.QCTemp = dt.Rows[0]["TEMPLATEID"].ToString();
                    ca.QCRequired = dt.Rows[0]["QCCOMPFLAG"].ToString();
                    ca.Latest = dt.Rows[0]["LATPURPRICE"].ToString();

                    ca.Rejection = dt.Rows[0]["REJRAWMATPER"].ToString();
                    ca.Percentage = dt.Rows[0]["RAWMATPER"].ToString();
                    ca.PercentageAdd = dt.Rows[0]["ADD1PER"].ToString();
                    ca.AddItem = dt.Rows[0]["ADD1"].ToString();
                    ca.RawMaterial = dt.Rows[0]["RAWMATCAT"].ToString();
                    ca.Ledger = dt.Rows[0]["LEDGERNAME"].ToString();


                    ca.FQCTemp = dt.Rows[0]["PTEMPLATEID"].ToString();

                    //ca.QCTemp = dt.Rows[0]["IQCTEMP"].ToString();
                    //ca.FQCTemp = dt.Rows[0]["FGQCTEMP"].ToString();

                    ca.Curing = dt.Rows[0]["CURINGDAY"].ToString();
                    ca.Auto = dt.Rows[0]["AUTOINDENT"].ToString();
                    ca.createdby = Request.Cookies["UserId"];
                }
                DataTable dt2 = new DataTable();
                dt2 = ItemNameService.GetBinDeatils(id);
                if (dt2.Rows.Count > 0)
                {
                    for (int i = 0; i < dt2.Rows.Count; i++)
                    {
                        tdaB = new BinItem();
                        tdaB.BinID = dt2.Rows[0]["BINID"].ToString();
                        tdaB.BinYN = dt2.Rows[0]["BINYN"].ToString();
                        tdaB.Isvalid = "Y";
                        TDatab.Add(tdaB);
                    }
                }

                DataTable dtt = new DataTable();
                dtt = ItemNameService.GetAllSupplier(id);

                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new SupItem();
                        tda.Suplierlst = BindSupplier();
                        tda.SupName = dtt.Rows[i]["SUPPLIERID"].ToString();
                        tda.SupplierPart = dtt.Rows[i]["SUPPLIERPARTNO"].ToString();
                        tda.PurchasePrice = dtt.Rows[i]["SPURPRICE"].ToString();
                        tda.Delivery = dtt.Rows[i]["DELDAYS"].ToString();
                        tda.Isvalid = "Y";
                        TData.Add(tda);
                    }
                }
                DataTable dtt1 = new DataTable();
                dtt1 = ItemNameService.GetAllUnit(id);

                if (dtt1.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt1.Rows.Count; i++)
                    {
                        tdau = new UnitItem();
                        tdau.UnitLst = BindUnit();
                        tdau.Unit = dtt1.Rows[i]["UNIT"].ToString();
                        tdau.cf = dtt1.Rows[i]["CF"].ToString();
                        tdau.unittype = dtt1.Rows[i]["UNITTYPE"].ToString();
                        tdau.uniqid = dtt1.Rows[i]["UNITUNIQUEID"].ToString();
                        tdau.Isvalid = "Y";
                        TDatau.Add(tdau);
                    }
                }

            }
            ca.Binlst = TDatab;
            ca.Suplst = TData;
            ca.unititemlst = TDatau;
            return View(ca);
        }
        public ActionResult GetSupDetail(string SupId)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();

                string suppart = "";
                dt = ItemNameService.GetSupplierName(SupId);

                if (dt.Rows.Count > 0)
                {
                    suppart = dt.Rows[0]["UNITID"].ToString();
                }

                var result = new { suppart = suppart };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindBinID()
        {
            try
            {
                DataTable dtDesg = ItemNameService.BindBinID();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["BINID"].ToString(), Value = dtDesg.Rows[i]["BINBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindLoc()
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
        public List<SelectListItem> Bindpurchase()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "OTHERS", Value = "OTHERS" });
                lstdesg.Add(new SelectListItem() { Text = "CONSUMABLES", Value = "CONSUMABLES" });
                lstdesg.Add(new SelectListItem() { Text = "MAJOR RAW MATERIALS", Value = "MAJOR RAW MATERIALS" });
                lstdesg.Add(new SelectListItem() { Text = "PACKING OLD DRUMS", Value = "PACKING OLD DRUMS" });
                lstdesg.Add(new SelectListItem() { Text = "MAJOR CONSUMABLES", Value = "MAJOR CONSUMABLES" });
                lstdesg.Add(new SelectListItem() { Text = "MAJOR FUEL", Value = "MAJOR FUEL" });
                lstdesg.Add(new SelectListItem() { Text = "MAJOR PACKING MATERIAL", Value = "MAJOR PACKING MATERIAL" });

                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> Bindcostcate()
        {
            try
            {
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                lstdesg.Add(new SelectListItem() { Text = "NONE", Value = "NONE" });
                lstdesg.Add(new SelectListItem() { Text = "HIGH SIEVE", Value = "HIGH SIEVE" });
                lstdesg.Add(new SelectListItem() { Text = "LOW SIEVE", Value = "LOW SIEVE" });
               

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
                DataTable dtDesg = ItemNameService.GetLedger();
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
        public List<SelectListItem> BindItem()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetItem();
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
        public List<SelectListItem> BindUnit()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetUnit();
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
        //public List<SelectListItem> BindItem()
        //{
        //    try
        //    {
        //        DataTable dtDesg = ItemNameService.GetItem();
        //        List<SelectListItem> lstdesg = new List<SelectListItem>();
        //        for (int i = 0; i < dtDesg.Rows.Count; i++)
        //        {
        //            lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["ITEMID"].ToString(), Value = dtDesg.Rows[i]["ITEMID"].ToString() });
        //        }
        //        return lstdesg;
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }
        //}
        public List<SelectListItem> BindItemGroup()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetItemGroup();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["GROUPCODE"].ToString(), Value = dtDesg.Rows[i]["GROUPCODE"].ToString() });
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

        //    string flag = ItemNameService.StatusChange(tag, id);
        //    if (string.IsNullOrEmpty(flag))
        //    {

        //        return RedirectToAction("ListLedger");
        //    }
        //    else
        //    {
        //        TempData["notice"] = flag;
        //        return RedirectToAction("ListLedger");
        //    }
        //}
        public List<SelectListItem> BindSupplier()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetSupplier();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["PARTY"].ToString(), Value = dtDesg.Rows[i]["PARTYMASTID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemCategory()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetItemCategory();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["CATEGORY"].ToString(), Value = dtDesg.Rows[i]["CATEGORY"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindItemSubGroup()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetItemSubGroup();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["SGCODE"].ToString(), Value = dtDesg.Rows[i]["SGCODE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindTarrif()
        {
            try
            {
                DataTable dtDesg = datatrans.GetData("select ETARIFFMASTERID,TARIFFID from ETARIFFMASTER");
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TARIFFID"].ToString(), Value = dtDesg.Rows[i]["ETARIFFMASTERID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public List<SelectListItem> BindQCTemp()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetQCTemp();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["TEMPLATEID"].ToString(), Value = dtDesg.Rows[i]["TESTTBASICID"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        [HttpPost]
        public ActionResult ItemName(ItemName ss, string id, List<IFormFile> file)
        {

            try
            {
                ss.ID = id;
                string Strout = ItemNameService.ItemNameCRUD(ss, file);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " ItemName Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " ItemName Updated Successfully...!";
                    }
                    return RedirectToAction("ListItem");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ItemName";
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

        [HttpPost]
        public ActionResult ItemCreate(ItemName ss, string id, List<IFormFile> file)
        {

            try
            {
                ss.ID = id;
                string Strout = ItemNameService.ItemNameCRUD(ss, file);
                if (string.IsNullOrEmpty(Strout))
                {
                    if (ss.ID == null)
                    {
                        TempData["notice"] = " Item Inserted Successfully...!";
                    }
                    else
                    {
                        TempData["notice"] = " Item Updated Successfully...!";
                    }
                    return RedirectToAction("ListItem");
                }

                else
                {
                    ViewBag.PageTitle = "Edit ItemName";
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
        public List<SelectListItem> BindHSNcode()
        {
            try
            {
                DataTable dtDesg = ItemNameService.GetHSNcode();
                List<SelectListItem> lstdesg = new List<SelectListItem>();
                for (int i = 0; i < dtDesg.Rows.Count; i++)
                {
                    lstdesg.Add(new SelectListItem() { Text = dtDesg.Rows[i]["HSNCODE"].ToString(), Value = dtDesg.Rows[i]["HSNCODE"].ToString() });
                }
                return lstdesg;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public IActionResult ItemList()
        {

            IEnumerable<ItemName> sta = ItemNameService.GetAllItemName();
            return View(sta);
        }

        public IActionResult ListItem()
        {
            return View();
        }
        public ActionResult MyListItemgrid(string strStatus)
        {
            List<ItemList> Reg = new List<ItemList>();
            DataTable dtUsers = new DataTable();
            strStatus = strStatus == "" ? "Y" : strStatus;
            dtUsers = ItemNameService.GetAllItems(strStatus);
           
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {
                string Regenerate = string.Empty;
                string DeleteRow = string.Empty;
                string EditRow = string.Empty;
                if (dtUsers.Rows[i]["ACTIVE"].ToString() == "Y")
                {

                    EditRow = "<a href=ItemName?id=" + dtUsers.Rows[i]["ITEMMASTERID"].ToString() + "><img src='../Images/edit.png' alt='Edit' /></a>";


                    DeleteRow = "<a href=DeleteItem?tag=Del&id=" + dtUsers.Rows[i]["ITEMMASTERID"].ToString() + "><img src='../Images/Inactive.png' alt='Deactivate' /></a>";
                }
                else
                {
                    EditRow = "";


                    DeleteRow = "<a href=DeleteItem?tag=Active&id=" + dtUsers.Rows[i]["ITEMMASTERID"].ToString() + "><img src='../Images/active.png' alt='Activate' /></a>";
                }
                Regenerate = "<a href=ItemCreate?id=" + dtUsers.Rows[i]["ITEMMASTERID"].ToString() + "><img src='../Images/Change.png' alt='Edit' /></a>";

                Reg.Add(new ItemList
                {
                    id = dtUsers.Rows[i]["ITEMMASTERID"].ToString(),
                    itemgroup = dtUsers.Rows[i]["IGROUP"].ToString(),
                    itemsubgroup = dtUsers.Rows[i]["ISUBGROUP"].ToString(),

                     

                  //  itemcode = dtUsers.Rows[i]["ITEMCODE"].ToString(),

                    itemname = dtUsers.Rows[i]["ITEMID"].ToString(),
                    //Reorderqu = dtUsers.Rows[i]["REORDERQTY"].ToString(),
                    //Reorderlvl = dtUsers.Rows[i]["REORDERLVL"].ToString(),
                    //Maxlvl = dtUsers.Rows[i]["MAXSTOCKLVL"].ToString(),
                    //Minlvl = dtUsers.Rows[i]["MINSTOCKLVL"].ToString(),

                    itemcate= dtUsers.Rows[i]["GROUPTYPE"].ToString(),
                    uom = dtUsers.Rows[i]["UNITID"].ToString(),

                  //  cf = dtUsers.Rows[i]["CONVERAT"].ToString(),
                  //  uom = dtUsers.Rows[i]["UOM"].ToString(),

                    hsncode = dtUsers.Rows[i]["HSN"].ToString(),
                    //sellingprice = dtUsers.Rows[i]["SELLINGPRI"].ToString(),
                    editrow = EditRow,
                    delrow = DeleteRow,
                    rrow= Regenerate
                });
            }

            return Json(new
            {
                Reg
            });

        }

        public ActionResult DeleteItem(string tag, int id)
        {
            ItemNameService.StatusChange(tag, id);
            return RedirectToAction("ListItem");

        }
       
        public JsonResult GetItemGrpJSON()
        {
           //EnqItem model = new EnqItem();
           //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindSupplier());
        }
        public JsonResult GetUnitJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindUnit());
        }
        public JsonResult GetLocJSON()
        {
            //EnqItem model = new EnqItem();
            //  model.ItemGrouplst = BindItemGrplst(value);
            return Json(BindLoc());
        }
        //public IActionResult SupplierDetail(String id)
        //{
        //    ItemName ca = new ItemName();
        //    if (id == null)
        //    {

        //    }
        //    else
        //    {

        //        //ca = ItemNameService.GetSupplierById(id);


        //    }

        //    return View(ca);
        //}
        public IActionResult GetSupplier(string id)
        {
            ItemName cmp = new ItemName();
            if (!string.IsNullOrEmpty(id))
            {
                DataTable dtt = new DataTable();
                dtt = ItemNameService.GetAllSupplier(id);
                ItemName tda = new ItemName();
                List<ItemName> TData = new List<ItemName>();
                if (dtt.Rows.Count > 0)
                {
                    for (int i = 0; i < dtt.Rows.Count; i++)
                    {
                        tda = new ItemName();
                        tda.SupName = dtt.Rows[i]["SUPPLIERID"].ToString();
                        tda.SupPartNo = dtt.Rows[i]["SUPPLIERPARTNO"].ToString();
                        tda.Price = dtt.Rows[i]["SPURPRICE"].ToString();
                        tda.Dy = dtt.Rows[i]["DELDAYS"].ToString();
                        TData.Add(tda);
                    }
                }
                cmp.pflst = TData;

            }
            //IEnumerable<PurchaseFollowup> cmp = PurenqService.GetAllPurchaseFollowup();
            return View(cmp);
        }
        //public ActionResult Supplier(ItemName Pf, string id)
        //{

        //    try
        //    {
        //        Pf.ID = id;
        //        string Strout = ItemNameService.SupplierCRUD(Pf);
        //        if (string.IsNullOrEmpty(Strout))
        //        {
        //            if (Pf.ID == null)
        //            {
        //                TempData["notice"] = "ItemName Inserted Successfully...!";
        //            }
        //            else
        //            {
        //                TempData["notice"] = "ItemName Updated Successfully...!";
        //            }
        //            return RedirectToAction("ItemName");
        //        }

        //        else
        //        {
        //            ViewBag.PageTitle = "Edit ItemName";
        //            TempData["notice"] = Strout;
        //            //return View();
        //        }

        //        // }
        //    }
        //    catch (Exception ex)
        //    {
        //        throw ex;
        //    }

        //    return View(Pf);
        //}
        public ActionResult GetUniqueDetail(string ItemId,string unit)
        {
            try
            {
                DataTable dt = new DataTable();
                DataTable dt1 = new DataTable();
                string unique = "";

                string unitid = datatrans.GetDataString("Select UNITID from UNITMAST where UNITMASTID='" + unit + "'");

                unique = unitid + " " + ItemId;



                var result = new { unique = unique };
                return Json(result);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        
    }
}
