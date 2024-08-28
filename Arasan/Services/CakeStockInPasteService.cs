
using Arasan.Interface.Report;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
//using DocumentFormat.OpenXml.Office2010.Excel;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;

using Microsoft.Reporting.Map.WebForms.BingMaps;
using ClosedXML.Excel;
using Microsoft.CodeAnalysis.Operations;

namespace Arasan.Services.Report
{
    public class CakeStockInPasteService : ICakeStockInPaste
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CakeStockInPasteService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        
        public DataTable GetAllCakeStockInPaste(string dtFrom, string dtTo, string Branch )
        {
            try
            {
                string SvSql = "";
                SvSql = "SELECT x.DocDate , SUM(x.OpQty) OQ , SUM(x.RQty) RQ , SUM(x.IQty) IQ , SUM(x.pqty) RQN, SUM(x.mqty) IQN  ,SUM(x.opqty+pqty)-SUM(x.mqty) CLNEW,\r\nSUM(PYROISS) PYROISS\r\nFROM \r\n(\r\nSELECT '"+dtFrom+"' DocDate , SUM(Qty) OpQty , 0 RQty , 0 IQty ,0 pqty, 0 mqty,0 PYROISS\r\nFROM \r\n(\r\nSELECT TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo , SUM(Ls.PlusQty)-SUM(Ls.MinusQty) Qty ,0 pqty, 0 mqty\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType IN ( 'PASTE') \r\nAND I.SnCategory IN ( 'CAKE','PIG-CAKE','PASTE') \r\nAND Ls.DrumNo IS NOT NULL\r\nAND Ls.DocDate < '"+dtFrom+"'\r\nGROUP BY TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo\r\nHAVING SUM(Ls.PlusQty)-SUM(Ls.MinusQty)  > 0\r\n)\r\nUNION ALL\r\nSELECT Ls.docdate , 0 opqty, 0 RQty , 0 IQty ,SUM(Ls.PlusQty) pqty,SUM(Ls.MinusQty) mQty,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType IN ( 'PASTE' , 'MIXING') \r\nAND I.SnCategory IN ('CAKE','PIG-CAKE','PASTE')\r\nAND Ls.DrumNo IS NOT NULL\r\nAND Ls.DocDate BETWEEN :SD AND '"+dtFrom+"'\r\nGROUP BY Ls.docdate ,TL.LocationType\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , SUM(Ls.PlusQty) RQty , 0 IQty ,0 pqty, 0 mqty,0\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND I.SNCategory IN ('CAKE','PIG-CAKE','PASTE')\r\nAND TL.LocationType = 'PASTE'\r\nAND Ls.PlusQty > 0\r\nAND Ls.DocDate BETWEEN :SD AND '"+dtFrom+"'\r\nAND Ls.StockTransType = 'BPROD OUTPUT'\r\nGROUP BY TL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , SUM(Ls.PlusQty) RQty , 0 IQty,0 pqty, 0 mqty,0\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL , BranchMast Br\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.BranchID = Br.BranchMastID\r\nAND FL.BranchID = Br.BranchMastID\r\nAND Br.BranchID = '"+Branch+"'\r\nAND I.SNCategory IN ('CAKE','PASTE','PYRO POWDER','POLISH POWDER','RVD POWDER','POLISHED CAKE','DG PASTE','PIG-CAKE')\r\nAND TL.LocationType in ('PASTE','MIXING')\r\nAND Ls.PlusQty > 0\r\nAND Ls.FromLocID = FL.LocDetailsID\r\nAND FL.LocationType in ('RVD','STORES','PACKING')\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtFrom+"'\r\nAND Ls.StockTransType in ( 'DRUM ISSUE','STORES PROD ISSUE')\r\nGROUP BY TL.LocationType  , FL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , SUM(Ls.PlusQty) RecQty , 0 IssQty ,0 pqty, 0 mqty,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I , LocDetails FL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType = 'PASTE'\r\nAND I.SnCategory IN ('PYRO POWDER','DG PASTE','POLISH POWDER','POLISHED CAKE','RVD POWDER')\r\nAND Ls.StockTransType IN ('CURING RECHARGE')\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtFrom+"'\r\nAND Ls.FromLocID = FL.LocDetailsID\r\nAND FL.LocationType = 'CURING' \r\nGROUP BY Ls.DocDate , TL.LocationType , FL.LocationType\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , SUM(Ls.PlusQty) RQty , 0 IQty,0 pqty, 0 mqty,0\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL , BranchMast Br\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.BranchID = Br.BranchMastID\r\nAND FL.BranchID = Br.BranchMastID\r\nAND Br.BranchID = '"+Branch+"'\r\nAND I.SNCategory IN ('CAKE','PASTE','PYRO POWDER','POLISH POWDER','RVD POWDER','POLISHED CAKE','DG PASTE','PIG-CAKE')\r\nAND TL.LocationType = 'PASTE'\r\nAND Ls.PlusQty > 0\r\nAND Ls.FromLocID = FL.LocDetailsID\r\nAND FL.LocationType = 'FG GODOWN'\r\nAND Ls.DocDate BETWEEN :SD AND '"+dtFrom+"'\r\nAND Ls.StockTransType = 'DRUM ISSUE'\r\nGROUP BY TL.LocationType  , FL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , SUM(Ls.PlusQty) IQty ,0 pqty, 0 mqty,0\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.StockTransType = 'DRUM ISSUE'\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType IN  ('RVD','MIXING')\r\nAND Ls.FromLocID = FL.LocDetailsID\r\nAND FL.LocationType = 'PASTE'\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"'AND '"+dtFrom+"'\r\nAND Ls.PlusQty > 0\r\nAND I.SNCategory IN ('CAKE','DG PASTE','PASTE','PIG-CAKE','PASTE')\r\nGROUP BY TL.LocationType , FL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , SUM(Ls.MinusQty) IQty ,0 pqty, 0 mqty,0\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND I.SNCategory IN ( 'POLISHED CAKE','PYRO POWDER','POLISH POWDER','RVD POWDER')\r\nAND TL.LocationType in ('MIXING','PASTE')\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtFrom+"'\r\nAND Ls.StockTransType = 'BPROD INPUT'\r\nGROUP BY TL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , SUM(Ls.MinusQty) IQty ,0 pqty, 0 mqty,0\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND I.SNCategory IN ( 'CAKE','PASTE','PIG-CAKE')\r\nAND TL.LocationType in ('PASTE')\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtFrom+"'\r\nAND Ls.StockTransType in ('BPROD INPUT','PAINTING & CLEANING','TANYA COMPANY PAINTING ','NMPC Painting work')\r\nGROUP BY TL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , SUM(Ls.PlusQty) IQty ,0 pqty, 0 mqty,0\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.StockTransType = 'PACKING ISSUE'\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType  IN  ('PACKING')\r\nAND Ls.FromLocID = FL.LocDetailsID\r\nAND FL.LocationType  IN  ('MIXING','PASTE')\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtFrom+"'\r\nAND Ls.PlusQty > 0\r\nAND I.SNCategory IN ('PYRO POWDER','POLISH POWDER','POLISHED CAKE','RVD POWDER')\r\nGROUP BY TL.LocationType , FL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 IQty ,0 pqty, 0 mqty, SUM(Ls.PlusQty)  DRMISS\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.StockTransType = 'DRUM ISSUE'\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType NOT IN ('MIXING','PASTE','RVD')\r\nAND Ls.FromLocID = FL.LocDetailsID\r\nAND FL.LocationType  IN ('MIXING','PASTE')\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtFrom+"'\r\nAND Ls.PlusQty > 0\r\nAND I.SNCategory IN ( 'POLISHED CAKE','PYRO POWDER','POLISH POWDER','RVD POWDER')\r\nGROUP BY TL.LocationType , FL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT '"+dtFrom+"' DocDate , SUM(Qty) OpQty , 0 RQty , 0 IQty ,0 pqty, 0 mqty,0\r\nFROM \r\n(\r\nSELECT TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo , SUM(Ls.PlusQty)-SUM(Ls.MinusQty) Qty,0\r\nFROM LStockValue Ls , LocDetails TL , ItemMaster I\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType IN ( 'PASTE' , 'MIXING')\r\nAND I.SnCategory IN ('PYRO POWDER','POLISH POWDER','POLISHED CAKE','RVD POWDER')\r\nAND Ls.DrumNo IS NOT NULL\r\nAND Ls.DocDate < '"+dtFrom+"'\r\nGROUP BY TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo\r\nHAVING SUM(Ls.PlusQty)-SUM(Ls.MinusQty)  > 0\r\n)\r\n) x\r\nGROUP BY x.DocDate\r\nORDER BY x.DocDate";

                
                OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
                DataTable dtReport = new DataTable();
                adapter.Fill(dtReport);
                return dtReport;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        

    }
}
