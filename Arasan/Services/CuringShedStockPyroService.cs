using Arasan.Interface.Report;
using System.Collections.Generic;
using System.Data;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc;
using Oracle.ManagedDataAccess.Client;
using System.IO;
using Arasan.Interface;
using Nest;
using Microsoft.VisualBasic;
using System.Globalization;
using Microsoft.Reporting.Map.WebForms.BingMaps;
using ClosedXML.Excel;
using Microsoft.CodeAnalysis.Operations;

namespace Arasan.Services
{
    public class CuringShedStockPyroService : ICuringShedStockPyro
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public CuringShedStockPyroService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllCuringShedStockPyro(string dtFrom, string dtTo)
        {
            try
            {
                string SvSql = "";
                //SvSql = " SELECT x.DocDate , SUM(x.OpQty) OQ , SUM(x.RQty) RQ , SUM(x.PIQty) IQ , SUM(x.RIQty) RIQ , SUM(x.RmIQty) RmIQ FROM  ( SELECT  '" + dtFrom + "'  DocDate , SUM(Ls.PlusQty)-SUM(Ls.MinusQty) OpQty , 0 RQty , 0 PIQty , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND TL.LocationType in  ('MIXING','PACKING') AND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE') AND Ls.DocDate < '" + dtFrom + "' GROUP BY TL.LocID UNION ALL SELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE') AND TL.LocationType in ('MIXING','PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND Ls.StockTransType in ('BPROD OUTPUT','Direct Addition') GROUP BY TL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty  , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL  WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType IN ('DRUM ISSUE','PACKING ISSUE','STORES PROD ISSUE') AND Ls.LocID = TL.LocDetailsID AND FL.LocationType NOT in ('MIXING') AND Ls.FromLocID = FL.LocDetailsID AND TL.LocationType  IN ('MIXING','PACKING')  AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE') GROUP BY TL.LocID , FL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , SUM(Ls.MinusQty) PIQty  , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType = 'PACKING INPUT' AND Ls.LocID = TL.LocDetailsID AND TL.LocationType in ('PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE') GROUP BY TL.LocID , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty  , SUM(Ls.PlusQty) RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL  WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType = 'DRUM ISSUE' AND Ls.LocID = TL.LocDetailsID AND TL.LocationType NOT IN ('MIXING','PACKING') AND Ls.FromLocID = FL.LocDetailsID AND FL.LocationType  IN ('MIXING','PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND I.SNCategory IN ('CAKE' , 'PASTE','PIG-CAKE') GROUP BY TL.LocID , FL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty , 0 RIQty , SUM(Ls.MinusQty) RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE') AND TL.LocationType IN ('MIXING','PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND Ls.StockTransType IN ('BPROD INPUT','PAINTING & CLEANING','TANYA COMPANY PAINTING','TANYA COMPANY PAINTING ','NMPC Painting work') GROUP BY TL.LocationType , Ls.DocDate ) x GROUP BY x.DocDate ORDER BY x.DocDate";
                SvSql = " SELECT x.DocDate , SUM(x.OpQty) OQ , SUM(x.RQty) RQ , SUM(x.PIQty) IQ , SUM(x.RIQty) RIQ , SUM(x.RmIQty) RmIQ FROM  (SELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE') AND TL.LocationType in ('MIXING','PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND Ls.StockTransType in ('BPROD OUTPUT','Direct Addition') GROUP BY TL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate ,0 OpQty , SUM(Ls.PlusQty) RQty , 0 PIQty  , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL  WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType IN ('DRUM ISSUE','PACKING ISSUE','STORES PROD ISSUE') AND Ls.LocID = TL.LocDetailsID AND FL.LocationType NOT in ('MIXING') AND Ls.FromLocID = FL.LocDetailsID AND TL.LocationType  IN ('MIXING','PACKING')  AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE') GROUP BY TL.LocID , FL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , SUM(Ls.MinusQty) PIQty  , 0 RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType = 'PACKING INPUT' AND Ls.LocID = TL.LocDetailsID AND TL.LocationType in ('PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND I.SNCategory IN ('PASTE' , 'CAKE','PIG-CAKE') GROUP BY TL.LocID , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty  , SUM(Ls.PlusQty) RIQty , 0 RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL  WHERE Ls.ItemID = I.ItemMasterID AND Ls.StockTransType = 'DRUM ISSUE' AND Ls.LocID = TL.LocDetailsID AND TL.LocationType NOT IN ('MIXING','PACKING') AND Ls.FromLocID = FL.LocDetailsID AND FL.LocationType  IN ('MIXING','PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND I.SNCategory IN ('CAKE' , 'PASTE','PIG-CAKE') GROUP BY TL.LocID , FL.LocationType , Ls.DocDate UNION ALL SELECT Ls.DocDate , 0 OpQty , 0 RQty , 0 PIQty , 0 RIQty , SUM(Ls.MinusQty) RmIQty FROM LStockValue Ls , ItemMaster I , LocDetails TL WHERE Ls.ItemID = I.ItemMasterID AND Ls.LocID = TL.LocDetailsID AND I.SNCategory IN ('PASTE','CAKE','PIG-CAKE') AND TL.LocationType IN ('MIXING','PACKING') AND Ls.DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "' AND Ls.StockTransType IN ('BPROD INPUT','PAINTING & CLEANING','TANYA COMPANY PAINTING','TANYA COMPANY PAINTING ','NMPC Painting work') GROUP BY TL.LocationType , Ls.DocDate ) x GROUP BY x.DocDate ORDER BY x.DocDate";

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
