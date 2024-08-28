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
    public class FGBalancePowderStockService : IFGBalancePowderStock
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public FGBalancePowderStockService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllFGBalancePowderStock(string dtFrom, string dtTo)
        {
            try
            {
                string SvSql = "";
                SvSql = "Select I.ItemID , Sum(x.OQty) OQ , Sum(x.RQty) RQ, Sum(x.ORec) ORQ , Sum(x.PkQty) PkQ , Sum(x.OIss) Oiss \r\nFrom \r\n(\r\nSelect ItemID,Sum(Oqty) Oqty,SUm(RQty) Rqty,Sum(Orec) Orec, Sum(PkQty) PkQty,Sum(Oiss) Oiss From ( \r\nSelect  S.Itemid,Sum(S.PlusQty-S.MinusQty) OQty , 0 RQty,0 Orec , 0 PkQty , 0 OIss \r\nFrom LStockValue S,LOCDETAILS L , DrumMast D\r\nWhere S.LocID = L.LocDetailsID\r\n       And S.DrumNo = D.DrumNo\r\n       And L.LocID in ('PYRO PACKING','PYRO MACHINE PACKING','POLISH MACHINE PACK','POLISH PACKING','PYRO MACHINE PACK-1','PYRO MACHINE PACK-2')\r\n       And S.DocDate < '"+dtFrom+"'\r\nGroup By S.itemid\r\nHaving ( (Sum(S.PlusQty-S.MinusQty) > 0))\r\nUnion All\r\nSelect  S.Itemid,0 OQty , Sum(S.PlusQty) RQty,0 Orec, 0 PkQty , 0 Oiss\r\nFrom LStockValue S,LOCDETAILS L , DrumMast D\r\nWhere S.LocID = L.LocDetailsID\r\n       And s.Stocktranstype='CURING PACK'\r\n       And S.DrumNo = D.DrumNo and S.Plusqty>0\r\n       And L.LocID in ('PYRO PACKING','PYRO MACHINE PACKING','POLISH MACHINE PACK','POLISH PACKING','PYRO MACHINE PACK-1','PYRO MACHINE PACK-2')\r\n       And S.DocDate Between '"+dtFrom+"' And '"+dtTo+"'\r\nGroup By S.itemid\r\nUnion All\r\nSelect  S.Itemid,0 OQty ,0 Rqty, Sum(S.PlusQty) Orec , 0 PkQty , 0 Oiss \r\nFrom LStockValue S,LOCDETAILS L , DrumMast D\r\nWhere S.LocID = L.LocDetailsID\r\n       And s.Stocktranstype<>'CURING PACK'\r\n       And S.DrumNo = D.DrumNo and S.Plusqty>0\r\n       And L.LocID in ('PYRO PACKING','PYRO MACHINE PACKING','POLISH MACHINE PACK','POLISH PACKING','PYRO MACHINE PACK-1','PYRO MACHINE PACK-2')\r\n       And S.DocDate Between '"+dtFrom+"' And '"+dtTo+"'\r\nGroup By S.itemid\r\nUnion All\r\nSelect  S.Itemid,0 OQty , 0 RQty , 0, Sum(S.MinusQty) PkQty , 0 BQty\r\nFrom LStockValue S,LOCDETAILS L , DrumMast D\r\nWhere S.LocID = L.LocDetailsID\r\n       And S.DrumNo = D.DrumNo and S.MinusQty>0\r\n        And S.STOCKTRANSTYPE='PACKING INPUT'\r\n       And L.LocID in ('PYRO PACKING','PYRO MACHINE PACKING','POLISH MACHINE PACK','POLISH PACKING','PYRO MACHINE PACK-1','PYRO MACHINE PACK-2')\r\n       And S.DocDate Between '"+dtFrom+"' And '"+dtTo+"'\r\nGroup By S.itemid\r\nUnion All\r\nSelect  S.Itemid,0 OQty , 0 RQty ,0, 0 PkQty ,Sum(S.MinusQty) OIss \r\nFrom LStockValue S,LOCDETAILS L , DrumMast D\r\nWhere S.LocID = L.LocDetailsID\r\n       And S.DrumNo = D.DrumNo\r\n        And S.STOCKTRANSTYPE<>'PACKING INPUT' and S.MinusQty>0\r\n       And L.LocID in ('PYRO PACKING','PYRO MACHINE PACKING','POLISH MACHINE PACK','POLISH PACKING','PYRO MACHINE PACK-1','PYRO MACHINE PACK-2')\r\n       And S.DocDate Between '"+dtFrom+"' And '"+dtTo+"'\r\nGroup By S.itemid)Group By ItemID\r\n) x , ItemMaster I \r\nWhere x.ItemID = I.ItemMasterID\r\nGroup By I.ItemID\r\nOrder By I.ItemID";
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
