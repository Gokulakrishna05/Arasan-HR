using Microsoft.AspNetCore.Mvc;
using Arasan.Interface;
using Arasan.Interface.Report;
using Arasan.Models;
using Arasan.Services;
using Arasan.Services.Report;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Data;
using ClosedXML.Excel;
using System.Data.SqlClient;
using Oracle.ManagedDataAccess.Client;
using Microsoft.CodeAnalysis.Operations;
using Intuit.Ipp.DataService;

namespace Arasan.Controllers
{
    public class APSDailyReportController : Controller
    {
        IAPSDailyReport APSDailyReportService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public APSDailyReportController(IAPSDailyReport _APSDailyReportService, IConfiguration _configuratio)
        {
            APSDailyReportService = _APSDailyReportService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult APSDailyReport(string strfrom, string strTo)
        {

            try
            {
                APSDailyReportModel objR = new APSDailyReportModel();

                objR.dtFrom = strfrom;
                objR.dtTo = strTo;
                return View(objR);
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public ActionResult MyListPurchaseRepItemReportGrid(string dtFrom, string dtTo)
        {
            List<APSDailyReportModelItem> Reg = new List<APSDailyReportModelItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)APSDailyReportService.GetAllAPSDailyReport(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new APSDailyReportModelItem
                {
                    docdate = dtUsers.Rows[i]["DOCDATE"].ToString(),
                    o = dtUsers.Rows[i]["0"].ToString(),
                    irecqty = dtUsers.Rows[i]["IRECQTY"].ToString(),
                    o_1 = dtUsers.Rows[i]["0_1"].ToString(),
                    recqty = dtUsers.Rows[i]["RECQTY"].ToString(),
                    o_2 = dtUsers.Rows[i]["0_2"].ToString(),
                    o_3 = dtUsers.Rows[i]["0_3"].ToString(),
                    o_4 = dtUsers.Rows[i]["0_4"].ToString(),
                    o_5 = dtUsers.Rows[i]["0_5"].ToString(),
                    o_6 = dtUsers.Rows[i]["0_6"].ToString(),
                    o_7 = dtUsers.Rows[i]["0_7"].ToString(),
                    o_8 = dtUsers.Rows[i]["0_8"].ToString(),


                });
            }

            return Json(new
            {
                Reg
            });

        }




        public IActionResult ExportToExcel(string dtFrom, string dtTo)
        {
            DataTransactions _datatransactions;
            DataTable dtNew1 = new DataTable();

            string SvSql = "";

           SvSql = "Select DocDate, Sum(OpQty) OpQty,Sum(iRecQty) iRecQty,Sum(RecQty) RecQty,Sum(ORecQty) ORecQty,\r\nSum(ARQty) ARQty,Sum(PyroIssQty) PyroIssQty, \r\nSum(PasteIssQty) PasteIssQty, Sum(PackIssQty) PackIssQty,  Sum(RMIssQty) RMIssQty,SUM(ORQTY) ORQTY,Sum(DCQ) DCQ   \r\nFrom (\r\nSelect '" + dtFrom + "' DocDate , Sum(Qty) OpQty, 0 iRecQty , 0 ORecQty , 0 RecQty, 0 ArQty , 0 PyroIssQty,\r\n 0 PasteIssQty, 0 PackIssQty,  0 RMIssQty,0 ORQTY, 0 DCQ\r\nfrom \r\n(\r\nSelect Sum(Ls.PlusQty)-Sum(Ls.MinusQty) Qty\r\nFrom LStockValue Ls , LocDetails TL , ItemMaster I\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocID = 'APS'\r\nAnd I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd Ls.DocDate < '" + dtFrom + "'\r\nGroup By TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo\r\nHaving Sum(Ls.PlusQty)-Sum(Ls.MinusQty)  > 0\r\n)\r\nUnion All\r\nSelect Ls.DocDate, 0, 0 iRecQty , 0 , Sum(Ls.PlusQty) RecQty,0,0,0,0,0,0,0 \r\nFrom LStockValue Ls, LocDetails FL , LocDetails TL , ItemMaster I\r\nWhere Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd FL.LocationType IN ('AP MILL','FG GODOWN')\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'SIEVE & BLEND'\r\nAnd I.SnCategory In  ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate , FL.LocationType , TL.LocationType\r\nUnion All\r\nSelect Ls.DocDate, 0 , 0 iRecQty, 0 , Sum(Ls.PlusQty) RecQty,0,0,0,0,0,0,0\r\nFrom LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I\r\nWhere Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd FL.LocID = 'APS PACKING'\r\nAnd TL.LocID = 'APS'\r\nAnd I.SnCategory In  ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate, TL.LocID , FL.LocID\r\nUnion All\r\nSelect Ls.DocDate, 0, 0 iRecQty ,Sum(Ls.PlusQty)  , 0 RecQty,0,0,0,0,0,0,0 \r\nFrom LStockValue Ls, LocDetails FL , LocDetails TL , ItemMaster I\r\nWhere Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd FL.LocationType IN ('BALL MILL','PASTE')\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'SIEVE & BLEND'\r\nAnd I.SnCategory In  ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate , FL.LocationType , TL.LocationType\r\nUnion All\r\nSelect Ls.DocDate , 0 oq , Sum(Ls.PlusQty) irq , 0 , 0 ,0, 0 rq , 0 iq , 0 ,  0,0,0 \r\nFrom LStockValue Ls , LocDetails TL , ItemMaster I , LocDetails FL\r\nWhere Ls.StockTransType In ( 'STORES PROD ISSUE' , 'STORES CONS ISSUE' , 'DRUM ISSUE' )\r\nAnd Ls.LocID = TL.LocDetailsID \r\nAnd Ls.ItemID = I.ItemMasterID\r\nAnd TL.LocationType In ( 'AP MILL','SIEVE & BLEND' )\r\nAnd Ls.FromLocID = FL.LocDetailsID \r\nAnd FL.LocationType ='STORES'\r\nAnd I.SnCategory In  ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate , FL.LocationType , TL.LocationType\r\nUnion All\r\nSelect Ls.DocDate, 0, 0 iRecQty , Sum(Ls.PlusQty) ORecQty, 0,0 ,0,0,0,0,0,0 \r\nFrom LStockValue Ls, LocDetails TL , ItemMaster I\r\nWhere Ls.StockTransType In ('Conv.Receipt','SUB CONT REC','Direct Addition')\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType = 'SIEVE & BLEND'\r\nAnd I.SnCategory In ('JOB WORK POWDER','AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate ,  TL.LocationType\r\nUnion All\r\nSELECT b.DocDate, 0, 0 iRecQty , 0 , 0 , SUM(d.IQty) RecQty,0, 0,0,0,0,0 \r\nFROM nprodbasic b , nprodinpdet d , locdetails l , itemmaster i\r\nWHERE b.nprodbasicid = d.nprodbasicid \r\nAND b.ilocdetailsid = l.locdetailsid\r\nAND d.iitemid = i.itemmasterid\r\nAND i.sncategory in ( 'STEARIC ACID','OTHERS','DG MIXING ADDITIVE')\r\nAND L.LocationType = 'SIEVE & BLEND'\r\nAND b.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nGROUP BY b.DocDate , L.LocationType\r\nUnion All\r\nSELECT b.DocDate, 0, 0 iRecQty , 0 , 0 , SUM(d.ConsQty) RecQty,0, 0,0,0,0,0 \r\nFROM nprodbasic b , nprodconsdet d , locdetails l , itemmaster i\r\nWHERE b.nprodbasicid = d.nprodbasicid \r\nAND b.ilocdetailsid = l.locdetailsid\r\nAND d.Citemid = i.itemmasterid\r\nAND i.sncategory in ( 'STEARIC ACID','OTHERS')\r\nAND L.LocationType = 'SIEVE & BLEND'\r\nAND b.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nGROUP BY b.DocDate , L.LocationType\r\nUnion All\r\nSelect Ls.DocDate, 0,0,0 , 0 , 0, Sum(Ls.PlusQty) PYROISSQTY,0,0,0,0 ,0\r\nFrom LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I\r\nWhere Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd FL.LocID = 'APS'\r\nAnd TL.LocationType = 'BALL MILL'\r\nAnd I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate, FL.LocID , TL.LocationType\r\nUnion All\r\nSelect Ls.DocDate, 0,0, 0,0,0,0 , Sum(Ls.PlusQty) PasteISSQty,0,0,0,0 \r\nFrom LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I\r\nWhere Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd FL.LocID = 'APS'\r\nAnd TL.LocationType = 'PASTE'\r\nAnd I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate , FL.LocID , TL.LocationType\r\nUnion All\r\nSelect Ls.DocDate, 0,0,0,0,0, 0, 0 , Sum(Ls.PlusQty) PackIssQty,0,0,0 \r\nFrom LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I\r\nWhere Ls.StockTransType = 'PACKING ISSUE'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd FL.LocID = 'APS'\r\nAnd TL.LocID = 'APS PACKING'\r\nAnd I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate, TL.LocID , FL.LocID\r\nUnion All\r\nSelect Ls.DocDate, 0,0,0,0,0,0, 0 , 0 , Sum(Ls.PlusQty) RMIssQty,0,0 \r\nFrom LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I\r\nWhere Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd FL.LocID = 'APS'\r\nAnd TL.LocationType = 'REMELTING'\r\nAnd I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate , FL.LocID , TL.LocationType\r\nUNION ALL\r\nSelect Ls.DocDate, 0,0, 0,0,0 ,0, 0 PasteISSQty,0,0,SUM(LS.PLUSQTY) ORQTY,0\r\nFrom LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I\r\nWhere Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocID = 'APS'\r\nAnd FL.LocationType = 'REMELTING'\r\nAnd I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate , FL.LocID , TL.LocationType\r\nUnion all\r\nSelect Ls.DocDate, 0,0, 0,0,0 , 0 , 0 PasteISSQty,0,0,0,SUM(Ls.MINUSQTY) DC\r\nFrom LStockValue Ls , LocDetails L, ItemMaster I\r\nWhere Ls.StockTransType = 'SUB DC'\r\nAnd Ls.LocID = L.LocDetailsID\r\nAnd L.LocID = 'APS'\r\nAnd I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate , L.LocID \r\nUnion all\r\nSelect Ls.DocDate, 0,0, 0,0,0, 0 , 0 PasteISSQty,0,0,0,0 DC\r\nFrom LStockValue Ls , LocDetails L, ItemMaster I\r\nWhere Ls.StockTransType in( 'SUB CONT REC','Conv.Receipt')\r\nAnd Ls.LocID = L.LocDetailsID\r\nAnd L.LocID = 'APS'\r\nAnd I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER')\r\nAnd I.ItemMasterID = Ls.ItemID\r\nAnd Ls.DocDate Between '" + dtFrom + "' And '" + dtTo + "'\r\nGroup By Ls.DocDate , L.LocID\r\n)\r\nGroup By DocDate\r\nOrder By 1";
            //SvSql = " Select Ls.DocDate, 0, 0 iRecQty , 0 , Sum(Ls.PlusQty) RecQty,0,0,0,0,0,0,0  From LStockValue Ls, LocDetails FL , LocDetails TL , ItemMaster I Where Ls.StockTransType = 'DRUM ISSUE' And Ls.FromLocID = FL.LocDetailsID And I.ItemMasterID = Ls.ItemID And FL.LocationType IN ('AP MILL','FG GODOWN') And Ls.LocID = TL.LocDetailsID And TL.LocationType = 'SIEVE & BLEND' And I.SnCategory In  ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And Ls.DocDate Between '"+dtFrom+"' And :ED Group By Ls.DocDate , FL.LocationType , TL.LocationType Union All Select Ls.DocDate, 0 , 0 iRecQty, 0 , Sum(Ls.PlusQty) RecQty,0,0,0,0,0,0,0 From LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I Where Ls.StockTransType = 'DRUM ISSUE' And Ls.FromLocID = FL.LocDetailsID And Ls.LocID = TL.LocDetailsID And FL.LocID = 'APS PACKING' And TL.LocID = 'APS' And I.SnCategory In  ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And I.ItemMasterID = Ls.ItemID And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate, TL.LocID , FL.LocID Union All Select Ls.DocDate, 0, 0 iRecQty ,Sum(Ls.PlusQty)  , 0 RecQty,0,0,0,0,0,0,0  From LStockValue Ls, LocDetails FL , LocDetails TL , ItemMaster I Where Ls.StockTransType = 'DRUM ISSUE' And Ls.FromLocID = FL.LocDetailsID And I.ItemMasterID = Ls.ItemID And FL.LocationType IN ('BALL MILL','PASTE') And Ls.LocID = TL.LocDetailsID And TL.LocationType = 'SIEVE & BLEND' And I.SnCategory In  ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate , FL.LocationType , TL.LocationType Union All Select Ls.DocDate , 0 oq , Sum(Ls.PlusQty) irq , 0 , 0 ,0, 0 rq , 0 iq , 0 ,  0,0,0 From LStockValue Ls , LocDetails TL , ItemMaster I , LocDetails FL Where Ls.StockTransType In ( 'STORES PROD ISSUE' , 'STORES CONS ISSUE' , 'DRUM ISSUE' ) And Ls.LocID = TL.LocDetailsID And Ls.ItemID = I.ItemMasterID And TL.LocationType In ( 'AP MILL','SIEVE & BLEND' ) And Ls.FromLocID = FL.LocDetailsID  And FL.LocationType ='STORES' And I.SnCategory In  ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate , FL.LocationType , TL.LocationType Union All Select Ls.DocDate, 0, 0 iRecQty , Sum(Ls.PlusQty) ORecQty, 0,0 ,0,0,0,0,0,0 From LStockValue Ls, LocDetails TL , ItemMaster I Where Ls.StockTransType In ('Conv.Receipt','SUB CONT REC','Direct Addition') And I.ItemMasterID = Ls.ItemID And Ls.LocID = TL.LocDetailsID AND TL.LocationType = 'SIEVE & BLEND' And I.SnCategory In ('JOB WORK POWDER','AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate ,  TL.LocationType Union All SELECT b.DocDate, 0, 0 iRecQty , 0 , 0 , SUM(d.IQty) RecQty,0, 0,0,0,0,0  FROM nprodbasic b , nprodinpdet d , locdetails l , itemmaster i WHERE b.nprodbasicid = d.nprodbasicid AND b.ilocdetailsid = l.locdetailsid AND d.iitemid = i.itemmasterid AND i.sncategory in ( 'STEARIC ACID','OTHERS','DG MIXING ADDITIVE') AND L.LocationType = 'SIEVE & BLEND' AND b.DocDate BETWEEN '"+dtFrom+ "' AND '"+dtTo+"' GROUP BY b.DocDate , L.LocationType Union All SELECT b.DocDate, 0, 0 iRecQty , 0 , 0 , SUM(d.ConsQty) RecQty,0, 0,0,0,0,0 FROM nprodbasic b , nprodconsdet d , locdetails l , itemmaster i WHERE b.nprodbasicid = d.nprodbasicid AND b.ilocdetailsid = l.locdetailsid AND d.Citemid = i.itemmasterid AND i.sncategory in ( 'STEARIC ACID','OTHERS') AND L.LocationType = 'SIEVE & BLEND' AND b.DocDate BETWEEN '" + dtFrom+"' AND '"+dtTo+"' GROUP BY b.DocDate , L.LocationType Union All Select Ls.DocDate, 0,0,0 , 0 , 0, Sum(Ls.PlusQty) PYROISSQTY,0,0,0,0 ,0 From LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I Where Ls.StockTransType = 'DRUM ISSUE' And Ls.FromLocID = FL.LocDetailsID And Ls.LocID = TL.LocDetailsID And FL.LocID = 'APS' And TL.LocationType = 'BALL MILL' And I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And I.ItemMasterID = Ls.ItemID And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate, FL.LocID , TL.LocationType Union All Select Ls.DocDate, 0,0, 0,0,0,0 , Sum(Ls.PlusQty) PasteISSQty,0,0,0,0 From LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I Where Ls.StockTransType = 'DRUM ISSUE' And Ls.FromLocID = FL.LocDetailsID And Ls.LocID = TL.LocDetailsID And FL.LocID = 'APS' And TL.LocationType = 'PASTE' And I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And I.ItemMasterID = Ls.ItemID And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate , FL.LocID , TL.LocationType Union All Select Ls.DocDate, 0,0,0,0,0, 0, 0 , Sum(Ls.PlusQty) PackIssQty,0,0,0 From LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I Where Ls.StockTransType = 'PACKING ISSUE' And Ls.FromLocID = FL.LocDetailsID And Ls.LocID = TL.LocDetailsID And FL.LocID = 'APS' And TL.LocID = 'APS PACKING' And I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And I.ItemMasterID = Ls.ItemID And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate, TL.LocID , FL.LocID Union All Select Ls.DocDate, 0,0,0,0,0,0, 0 , 0 , Sum(Ls.PlusQty) RMIssQty,0,0 From LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I Where Ls.StockTransType = 'DRUM ISSUE' And Ls.FromLocID = FL.LocDetailsID And Ls.LocID = TL.LocDetailsID And FL.LocID = 'APS' And TL.LocationType = 'REMELTING' And I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And I.ItemMasterID = Ls.ItemID And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate , FL.LocID , TL.LocationType UNION ALL Select Ls.DocDate, 0,0, 0,0,0 ,0, 0 PasteISSQty,0,0,SUM(LS.PLUSQTY) ORQTY,0 From LStockValue Ls , LocDetails FL, LocDetails TL , ItemMaster I Where Ls.StockTransType = 'DRUM ISSUE' And Ls.FromLocID = FL.LocDetailsID And Ls.LocID = TL.LocDetailsID And TL.LocID = 'APS' And FL.LocationType = 'REMELTING' And I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And I.ItemMasterID = Ls.ItemID And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate , FL.LocID , TL.LocationType Union all Select Ls.DocDate, 0,0, 0,0,0 , 0 , 0 PasteISSQty,0,0,0,SUM(Ls.MINUSQTY) DC From LStockValue Ls , LocDetails L, ItemMaster I Where Ls.StockTransType = 'SUB DC' And Ls.LocID = L.LocDetailsID And L.LocID = 'APS' And I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And I.ItemMasterID = Ls.ItemID And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate , L.LocID  Union all Select Ls.DocDate, 0,0, 0,0,0, 0 , 0 PasteISSQty,0,0,0,0 DC From LStockValue Ls , LocDetails L, ItemMaster I Where Ls.StockTransType in( 'SUB CONT REC','Conv.Receipt') And Ls.LocID = L.LocDetailsID And L.LocID = 'APS' And I.SnCategory In ('AP POWDER - FG','AP POWDER - SFG','PYRO POWDER','POLISH POWDER') And I.ItemMasterID = Ls.ItemID And Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"' Group By Ls.DocDate , L.LocID";
 


            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "APPowderStockInPyroDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=APPowderStockInPyroDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "APPowderStockInPyroDetails.xlsx");
                    }
                }

            }

        }
    }
}
