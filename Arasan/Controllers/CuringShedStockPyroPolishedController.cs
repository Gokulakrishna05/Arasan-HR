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
    public class CuringShedStockPyroPolishedController : Controller
    {
        ICuringShedStockPyroPolished CuringShedStockPyroPolishedService;
        IConfiguration? _configuratio;
        private string? _connectionString;

        DataTransactions datatrans;
        private object excelData;

        public CuringShedStockPyroPolishedController(ICuringShedStockPyroPolished _CuringShedStockPyroPolishedService, IConfiguration _configuratio)
        {
            CuringShedStockPyroPolishedService = _CuringShedStockPyroPolishedService;
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public IActionResult CuringShedStockPyroPolished(string strfrom, string strTo)
        {

            try
            {
                CuringShedStockPyroPolishedModel objR = new CuringShedStockPyroPolishedModel();

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
            List<CuringShedStockPyroPolishedModelItem> Reg = new List<CuringShedStockPyroPolishedModelItem>();
            DataTable dtUsers = new DataTable();

            dtUsers = (DataTable)CuringShedStockPyroPolishedService.GetAllCuringShedStockPyroPolished(dtFrom, dtTo);
            for (int i = 0; i < dtUsers.Rows.Count; i++)
            {


                Reg.Add(new CuringShedStockPyroPolishedModelItem
                {
                    dt = dtUsers.Rows[i]["DT"].ToString(),
                    opdrum = dtUsers.Rows[i]["OPDRUM"].ToString(),
                    opqty = dtUsers.Rows[i]["OPQTY"].ToString(),
                    pyrecdrum = dtUsers.Rows[i]["PYRECDRUM"].ToString(),
                    pyrecqty = dtUsers.Rows[i]["PYRECQTY"].ToString(),
                    pyissdrum = dtUsers.Rows[i]["PYISSDRUM"].ToString(),
                    pyissqty = dtUsers.Rows[i]["PYISSQTY"].ToString(),
                    polissdrum = dtUsers.Rows[i]["POLISSDRUM"].ToString(),
                    polissqty = dtUsers.Rows[i]["POLISSQTY"].ToString(),
                    pypkdrum = dtUsers.Rows[i]["PYPKDRUM"].ToString(),
                    pypkqty = dtUsers.Rows[i]["PYPKQTY"].ToString(),
                    stkpdrm = dtUsers.Rows[i]["STKPDRUM"].ToString(),
                    stkpqty = dtUsers.Rows[i]["STKPQTY"].ToString(),
                    itemconv = dtUsers.Rows[i]["ITEMCONV"].ToString(),
                    fdrm = dtUsers.Rows[i]["FDRM"].ToString(),
                    stkmqty = dtUsers.Rows[i]["STKQTY"].ToString(),
                    pyisqty = dtUsers.Rows[i]["PYISQTY"].ToString(),
 

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

            // SvSql = "SELECT X.Dt , SUM(X.OpDrum) OpD , SUM(X.OpQty) OpQ , SUM(X.PyRecDrum) PyRecD , SUM(X.PyRecQty) PyRecQ , SUM(X.PyIssDrum) \r\nPyIssD , SUM(X.PyIssQty) PyIssQ , SUM(X.PolIssDrum) PolIssD , SUM(X.PolIssQty) PolIssQ , SUM(X.PyPkDrum) PyPkD , \r\nSUM(X.PyPkQty) PyPkQ,SUM(stkpdrm) stkpdrm,SUM(stkpqty) stkpqty,SUM(itemconv) itemconv,SUM(fdrm) fdrm,\r\nSUM(stkmqty) stkmqty,SUM(PYISQTY) PYISQTY,sUM(iTEMcONVm) To_Pyro\r\nFROM\r\n(\r\nSELECT :SD Dt , COUNT(LotNo) OpDrum, SUM(Qty) OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty ,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0 iTEMcONVm\r\nFROM\r\n(\r\nSELECT TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo , SUM(Ls.PlusQty)-SUM(Ls.MinusQty) Qty\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND Ls.DrumNo IS NOT NULL\r\nAND Ls.DocDate < :SD\r\nGROUP BY TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo\r\nHAVING SUM(Ls.PlusQty)-SUM(Ls.MinusQty)  > 0\r\n)\r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , COUNT(Ls.LotNo) PyRecDrum , SUM(Ls.PlusQty) PyRecQty , \r\n0 PyIssDrum , 0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType = 'CURING'\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND Ls.PlusQty > 0\r\nAND Ls.DocDate BETWEEN :SD AND :ED\r\nAND Ls.StockTransType = 'BPROD OUTPUT'\r\nGROUP BY Ls.DocDate , TL.LocID\r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , COUNT(Ls.LotNo) PyRecDrum , SUM(Ls.PlusQty) PyRecQty , \r\n0 PyIssDrum , 0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I,LOCDETAILS FL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType in('PACKING','CURING')\r\nAND FL.LOCDETAILSID=LS.FROMLOCID\r\nAND FL.LOCATIONTYPE NOT IN ('CURING','PACKING')\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND (Ls.PlusQty > 0 OR LS.MINUSQTY>0)\r\nAND Ls.DocDate BETWEEN :SD AND :ED\r\nGROUP BY Ls.DocDate , TL.LocID\r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , COUNT(Ls.LotNo) PyPkDrum , SUM(Ls.mINUSQty) PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocATIONTYPE  in  ('PACKING')\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND Ls.MINUSQty > 0\r\nAND Ls.DocDate BETWEEN :SD AND :ED\r\nAND Ls.StockTransType = 'PACKING INPUT'\r\nGROUP BY Ls.DocDate , TL.LocID \r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,COUNT(Ls.LotNo) stkpdrm , \r\nSUM(Ls.PlusQty) stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I \r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND TL.LOCATIONTYPE IN ('CURING')\r\nAND I.SNCATEGORY IN ('POLISHED PYRO POWDER')\r\nAND LS.PLUSQTY > 0\r\nAND TL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND LS.DOCDATE BETWEEN :SD AND :ED\r\nAND UPPER(LS.STOCKTRANSTYPE) = 'STOCK RECONCILATION' \r\nGROUP BY LS.DOCDATE , TL.LOCATIONTYPE \r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,SUM(plusqty) itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I, ICEDETAIL ICD, ICEBASIC ICB, ITEMMASTER FI\r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND LS.LOCID = TL.LOCDETAILSID\r\nAND ICB.ICEBASICID = ICD.ICEBASICID\r\nAND ICD.ICEDETAILID = LS.T1SOURCEID\r\nAND ICB.FITEMID = FI.ITEMMASTERID\r\nAND FI.SNCATEGORY <> 'POLISHED PYRO POWDER'\r\nAND TL.LOCATIONTYPE  IN ('CURING','PACKING')\r\nAND I.SNCATEGORY = 'POLISHED PYRO POWDER'\r\nAND LS.PLUSQTY > 0\r\nAND LS.DOCDATE BETWEEN :SD AND :ED\r\nAND LS.STOCKTRANSTYPE = 'ITEM CONV' \r\nGROUP BY LS.DOCDATE , TL.LOCID\r\nuNION aLL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,SUM(PLUSQTY) iTEMcONVM \r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I, ICEDETAIL ICD, ICEBASIC ICB, ITEMMASTER FI\r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND LS.LOCID = TL.LOCDETAILSID\r\nAND ICB.ICEBASICID = ICD.ICEBASICID\r\nAND ICD.ICEDETAILID = LS.T1SOURCEID\r\nAND ICB.FITEMID = FI.ITEMMASTERID\r\nAND FI.SNCATEGORY = 'POLISHED PYRO POWDER'\r\nAND TL.LOCATIONTYPE IN  ('CURING','PACKING')\r\nAND I.SNCATEGORY <> 'POLISHED PYRO POWDER'\r\nAND LS.PLUSQTY > 0\r\nAND LS.DOCDATE BETWEEN :SD AND :ED\r\nAND LS.STOCKTRANSTYPE = 'ITEM CONV' \r\nGROUP BY LS.DOCDATE , TL.LOCID\r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,SUM(minusqty) fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I \r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND LS.LOCID = TL.LOCDETAILSID\r\nAND TL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND I.SNCATEGORY IN ('POLISHED PYRO POWDER')\r\nAND LS.MINUSQTY > 0\r\nAND LS.DOCDATE BETWEEN :SD AND :ED\r\nAND LS.STOCKTRANSTYPE = 'FIRE DRUMS'\r\nGROUP BY LS.DOCDATE , TL.LOCATIONTYPE\r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,SUM(minusqty) stkmqty,0  PYISQTY,0\r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I \r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND LS.LOCID = TL.LOCDETAILSID\r\nAND TL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND I.SNCATEGORY IN ('POLISHED PYRO POWDER')\r\nAND LS.MINUSQTY > 0\r\nAND LS.DOCDATE BETWEEN :SD AND :ED\r\nAND UPPER(LS.STOCKTRANSTYPE) = 'STOCK RECONCILATION' \r\nGROUP BY LS.DOCDATE , TL.LOCATIONTYPE \r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,SUM(PLUSQTY)  PYISQTY,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I,LOCDETAILS FL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND LS.FROMLOCID = FL.LOCDETAILSID\r\nAND FL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND TL.LOCID = 'PYRO'\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND Ls.DrumNo IS NOT NULL\r\nAND LS.STOCKTRANSTYPE = 'CURING RECHARGE'\r\nAND Ls.DocDate BETWEEN :SD AND :ED\r\nGROUP BY LS.DOCDATE \r\nUnion All\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , Count(ls.LOTNO) PolIssDrum , SUM(PLUSQTY) PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I , LOCDETAILS FL\r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND LS.LOCID = TL.LOCDETAILSID\r\nAND TL.LOCATIONTYPE = 'POLISH'\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND LS.PLUSQTY > 0\r\nAND LS.FROMLOCID = FL.LOCDETAILSID\r\nAND FL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND LS.DOCDATE BETWEEN :SD AND :ED\r\nGROUP BY LS.Docdate\r\n) X\r\nGROUP BY \r\n      X.Dt \r\nORDER BY \r\n      X.Dt";
            SvSql = "SELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , COUNT(Ls.LotNo) PyRecDrum , SUM(Ls.PlusQty) PyRecQty , \r\n0 PyIssDrum , 0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType = 'CURING'\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND Ls.PlusQty > 0\r\nAND Ls.DocDate BETWEEN :SD AND :ED\r\nAND Ls.StockTransType = 'BPROD OUTPUT'\r\nGROUP BY Ls.DocDate , TL.LocID\r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , COUNT(Ls.LotNo) PyRecDrum , SUM(Ls.PlusQty) PyRecQty , \r\n0 PyIssDrum , 0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I,LOCDETAILS FL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType in('PACKING','CURING')\r\nAND FL.LOCDETAILSID=LS.FROMLOCID\r\nAND FL.LOCATIONTYPE NOT IN ('CURING','PACKING')\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND (Ls.PlusQty > 0 OR LS.MINUSQTY>0)\r\nAND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nGROUP BY Ls.DocDate , TL.LocID\r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , COUNT(Ls.LotNo) PyPkDrum , SUM(Ls.mINUSQty) PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocATIONTYPE  in  ('PACKING')\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND Ls.MINUSQty > 0\r\nAND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND Ls.StockTransType = 'PACKING INPUT'\r\nGROUP BY Ls.DocDate , TL.LocID \r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,COUNT(Ls.LotNo) stkpdrm , \r\nSUM(Ls.PlusQty) stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I \r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND TL.LOCATIONTYPE IN ('CURING')\r\nAND I.SNCATEGORY IN ('POLISHED PYRO POWDER')\r\nAND LS.PLUSQTY > 0\r\nAND TL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND LS.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND UPPER(LS.STOCKTRANSTYPE) = 'STOCK RECONCILATION' \r\nGROUP BY LS.DOCDATE , TL.LOCATIONTYPE \r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,SUM(plusqty) itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I, ICEDETAIL ICD, ICEBASIC ICB, ITEMMASTER FI\r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND LS.LOCID = TL.LOCDETAILSID\r\nAND ICB.ICEBASICID = ICD.ICEBASICID\r\nAND ICD.ICEDETAILID = LS.T1SOURCEID\r\nAND ICB.FITEMID = FI.ITEMMASTERID\r\nAND FI.SNCATEGORY <> 'POLISHED PYRO POWDER'\r\nAND TL.LOCATIONTYPE  IN ('CURING','PACKING')\r\nAND I.SNCATEGORY = 'POLISHED PYRO POWDER'\r\nAND LS.PLUSQTY > 0\r\nAND LS.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND LS.STOCKTRANSTYPE = 'ITEM CONV' \r\nGROUP BY LS.DOCDATE , TL.LOCID\r\nuNION aLL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,SUM(PLUSQTY) iTEMcONVM \r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I, ICEDETAIL ICD, ICEBASIC ICB, ITEMMASTER FI\r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND LS.LOCID = TL.LOCDETAILSID\r\nAND ICB.ICEBASICID = ICD.ICEBASICID\r\nAND ICD.ICEDETAILID = LS.T1SOURCEID\r\nAND ICB.FITEMID = FI.ITEMMASTERID\r\nAND FI.SNCATEGORY = 'POLISHED PYRO POWDER'\r\nAND TL.LOCATIONTYPE IN  ('CURING','PACKING')\r\nAND I.SNCATEGORY <> 'POLISHED PYRO POWDER'\r\nAND LS.PLUSQTY > 0\r\nAND LS.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND LS.STOCKTRANSTYPE = 'ITEM CONV' \r\nGROUP BY LS.DOCDATE , TL.LOCID\r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,SUM(minusqty) fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I \r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND LS.LOCID = TL.LOCDETAILSID\r\nAND TL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND I.SNCATEGORY IN ('POLISHED PYRO POWDER')\r\nAND LS.MINUSQTY > 0\r\nAND LS.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND LS.STOCKTRANSTYPE = 'FIRE DRUMS'\r\nGROUP BY LS.DOCDATE , TL.LOCATIONTYPE\r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,SUM(minusqty) stkmqty,0  PYISQTY,0\r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I \r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND LS.LOCID = TL.LOCDETAILSID\r\nAND TL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND I.SNCATEGORY IN ('POLISHED PYRO POWDER')\r\nAND LS.MINUSQTY > 0\r\nAND LS.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nAND UPPER(LS.STOCKTRANSTYPE) = 'STOCK RECONCILATION' \r\nGROUP BY LS.DOCDATE , TL.LOCATIONTYPE \r\nUNION ALL\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , 0 PolIssDrum , 0 PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,SUM(PLUSQTY)  PYISQTY,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I,LOCDETAILS FL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND LS.FROMLOCID = FL.LOCDETAILSID\r\nAND FL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND TL.LOCID = 'PYRO'\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND Ls.DrumNo IS NOT NULL\r\nAND LS.STOCKTRANSTYPE = 'CURING RECHARGE'\r\nAND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nGROUP BY LS.DOCDATE \r\nUnion All\r\nSELECT Ls.DocDate Dt , 0 OpDrum, 0 OpQty , 0 PyRecDrum , 0 PyRecQty , 0 PyIssDrum , \r\n0 PyIssQty , Count(ls.LOTNO) PolIssDrum , SUM(PLUSQTY) PolIssQty , 0 PyPkDrum , 0 PyPkQty,\r\n0 stkpdrm , 0 stkpqty,0 itemconv,0 fdrm,0 stkmqty,0  PYISQTY,0\r\nFROM LSTOCKVALUE LS , LOCDETAILS TL , ITEMMASTER I , LOCDETAILS FL\r\nWHERE LS.ITEMID = I.ITEMMASTERID\r\nAND LS.LOCID = TL.LOCDETAILSID\r\nAND TL.LOCATIONTYPE = 'POLISH'\r\nAND I.SnCategory IN ('POLISHED PYRO POWDER')\r\nAND LS.PLUSQTY > 0\r\nAND LS.FROMLOCID = FL.LOCDETAILSID\r\nAND FL.LOCATIONTYPE in ('CURING','PACKING')\r\nAND LS.DOCDATE BETWEEN '" + dtFrom + "' AND '" + dtTo + "'\r\nGROUP BY LS.Docdate";

            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            using (DataTable dtReport = new DataTable())
            {
                adapter.Fill(dtReport);

                DataView dv1 = dtReport.DefaultView;
                // dv1.RowFilter = " TotalOutstandingAmount > 0 AND  OutstandingPrinciple > 0  ";

                dtNew1 = dv1.ToTable();
                using (XLWorkbook wb = new XLWorkbook())
                {
                    wb.Worksheets.Add(dtNew1, "CuringShedStockPyroPolishedDetails");


                    using (MemoryStream MyMemoryStream = new MemoryStream())
                    {
                        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                        Response.Headers.Add("content-disposition", "attachment;  filename=CuringShedStockPyroPolishedDetails.xlsx");
                        wb.SaveAs(MyMemoryStream);
                        //MyMemoryStream.WriteTo(Response.OutputStream);
                        //Response.Flush();
                        //Response.End();
                        //wb.SaveAs(MyMemoryStream);
                        return File(MyMemoryStream.ToArray(), "application/ms-excel", "CuringShedStockPyroPolishedDetails.xlsx");
                    }
                }

            }

        }
    }
}
