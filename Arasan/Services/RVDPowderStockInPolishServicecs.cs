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
    public class RVDPowderStockInPolishService : IRVDPowderStockInPolish
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public RVDPowderStockInPolishService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllRVDPowderStockInPolish(string dtFrom, string dtTo)
        {
            try
            {
                string SvSql = "";
               // SvSql = "Select x.DocDate , Sum(x.OpQty) OQ , Sum(x.RQty) RQ ,Sum(x.PQty) PQ, Sum(x.IQty) IQ\r\nFrom \r\n(\r\nSelect :SD DocDate , Sum(Qty) OpQty , 0 RQty ,0 PQty, 0 IQty\r\nFrom\r\n(\r\nSelect TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo , Sum(Ls.PlusQty)-Sum(Ls.MinusQty) Qty\r\nFrom LStockValue Ls , LocDetails TL , ItemMaster I\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'POLISH'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nAnd Ls.DrumNo Is Not Null\r\nAnd Ls.DocDate < :SD\r\nGroup By TL.LocID , Ls.LotNo , I.ItemID , Ls.DrumNo\r\nHaving Sum(Ls.PlusQty)-Sum(Ls.MinusQty)  > 0\r\n)\r\n\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty , Sum(Ls.PlusQty) RQty ,0 PQty, 0 IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd FL.LocationType <> 'POLISH'\r\nAnd Ls.DocDate Between :SD And :ED\r\nAnd I.SnCategory In ('RVD POWDER')\r\nGroup by TL.LocationType , FL.LocationType , Ls.DocDate\r\n\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty ,0 RQty, Sum(Ls.MinusQty) PQty , 0 IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd I.SnCategory In ('POLISH POWDER')\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between :SD And :ED\r\nAnd Ls.StockTransType = 'BPROD INPUT'\r\nGroup by TL.LocationType , Ls.DocDate\r\nUnion All\r\nSelect Ls.DocDate , 0 OpQty , 0 RQty ,0 PQty, Sum(Ls.PlusQty) IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType <> 'POLISH'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd FL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between :SD And :ED\r\nAnd I.SnCategory In ('RVD POWDER')\r\nGroup by TL.LocationType , FL.LocationType , Ls.DocDate\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty , 0 RQty , 0 PQty, Sum(Ls.MinusQty) IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd I.SnCategory In ('RVD POWDER','POLISH POWDER')\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between :SD And :ED\r\nAnd Ls.StockTransType = 'BPROD INPUT'\r\nGroup by TL.LocationType , Ls.DocDate\r\n) x\r\nGroup By x.DocDate\r\nOrder By x.DocDate";
                SvSql = "Select Ls.DocDate , 0 OpQty , Sum(Ls.PlusQty) RQty ,0 PQty, 0 IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd FL.LocationType <> 'POLISH'\r\nAnd Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nGroup by TL.LocationType , FL.LocationType , Ls.DocDate\r\n\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty ,0 RQty, Sum(Ls.MinusQty) PQty , 0 IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd I.SnCategory In ('POLISH POWDER')\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"'\r\nAnd Ls.StockTransType = 'BPROD INPUT'\r\nGroup by TL.LocationType , Ls.DocDate\r\nUnion All\r\nSelect Ls.DocDate , 0 OpQty , 0 RQty ,0 PQty, Sum(Ls.PlusQty) IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL , LocDetails FL \r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.StockTransType = 'DRUM ISSUE'\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd TL.LocationType <> 'POLISH'\r\nAnd Ls.FromLocID = FL.LocDetailsID\r\nAnd FL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"'\r\nAnd I.SnCategory In ('RVD POWDER')\r\nGroup by TL.LocationType , FL.LocationType , Ls.DocDate\r\nUnion All\r\n\r\nSelect Ls.DocDate , 0 OpQty , 0 RQty , 0 PQty, Sum(Ls.MinusQty) IQty\r\nFrom LStockValue Ls , ItemMaster I , LocDetails TL\r\nWhere Ls.ItemID = I.ItemMasterID\r\nAnd Ls.LocID = TL.LocDetailsID\r\nAnd I.SnCategory In ('RVD POWDER','POLISH POWDER')\r\nAnd TL.LocationType = 'POLISH'\r\nAnd Ls.DocDate Between '"+dtFrom+"' And '"+dtTo+"'\r\nAnd Ls.StockTransType = 'BPROD INPUT'\r\nGroup by TL.LocationType , Ls.DocDate";
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
