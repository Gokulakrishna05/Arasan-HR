
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
    public class PasteStockInMixingService : IPasteStockInMixing
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PasteStockInMixingService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllPasteStockInMixing(string dtFrom, string dtTo)
        {
            try
            {
                string SvSql = "";

                SvSql = "SELECT x.DocDate , SUM(x.OpQty) OQ , SUM(x.RQty) RQ , SUM(x.PIQty) IQ , SUM(x.RIQty) RIQ , SUM(x.RmIQty) RmIQ\r\nFROM \r\n(\r\nSELECT  '"+dtFrom+"'  DocDate , SUM(Ls.PlusQty)-SUM(Ls.MinusQty) OpQty , 0 RQty , 0 PIQty , 0 RIQty , 0 RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType in  ('MIXING','PACKING')\r\nAND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE')\r\nAND Ls.DocDate < '"+dtFrom+"'\r\nGROUP BY TL.LocID\r\nUNION ALL\r\nSELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty , 0 RIQty , 0 RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE')\r\nAND TL.LocationType in ('MIXING','PACKING')\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"'\r\nAND Ls.StockTransType in ('BPROD OUTPUT','Direct Addition')\r\nGROUP BY TL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty  , 0 RIQty , 0 RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.StockTransType IN ('DRUM ISSUE','PACKING ISSUE','STORES PROD ISSUE')\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND FL.LocationType NOT in ('MIXING')\r\nAND Ls.FromLocID = FL.LocDetailsID\r\nAND TL.LocationType  IN ('MIXING','PACKING') \r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"'\r\nAND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE')\r\nGROUP BY TL.LocID , FL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , SUM(Ls.MinusQty) PIQty  , 0 RIQty , 0 RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL \r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.StockTransType = 'PACKING INPUT'\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType in ('PACKING')\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"'\r\nAND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE')\r\nGROUP BY TL.LocID , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty  , SUM(Ls.PlusQty) RIQty , 0 RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.StockTransType = 'DRUM ISSUE'\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND TL.LocationType NOT IN ('MIXING','PACKING')\r\nAND Ls.FromLocID = FL.LocDetailsID\r\nAND FL.LocationType  IN ('MIXING','PACKING')\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"'\r\nAND I.SNCategory IN ('CAKE' , 'PASTE','PIG-CAKE')\r\nGROUP BY TL.LocID , FL.LocationType , Ls.DocDate\r\nUNION ALL\r\nSELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty , 0 RIQty , SUM(Ls.MinusQty) RmIQty\r\nFROM LStockValue Ls , ItemMaster I , LocDetails TL\r\nWHERE Ls.ItemID = I.ItemMasterID\r\nAND Ls.LocID = TL.LocDetailsID\r\nAND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE')\r\nAND TL.LocationType IN ('MIXING','PACKING')\r\nAND Ls.DocDate BETWEEN '"+dtFrom+"' AND '"+dtTo+"'\r\nAND Ls.StockTransType IN ('BPROD INPUT','PAINTING & CLEANING','TANYA COMPANY PAINTING','TANYA COMPANY PAINTING ','NMPC Painting work')\r\nGROUP BY TL.LocationType , Ls.DocDate\r\n) x\r\nGROUP BY x.DocDate\r\nORDER BY x.DocDate";

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
