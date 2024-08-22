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
    public class ApsIdleStockService : IApsIdleStock
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ApsIdleStockService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllApsIdleStock(string dtFrom)
        {
            try
            {
                string SvSql = "";
                SvSql = " select LocID,DrumNo,Docdate,ItemId,BatchNo,Qty,RC,asondt,Days from (Select M.DrumMastID,LM.Docdate,  M.DrumNo, I.ItemID, Nvl(B.BatchNo, 'Empty') BatchNo, Nvl(Sum(B.CLQty), 0) Qty ,  Nvl((B.Rate), 0) Rate, L.LocID, Decode(Nvl(LM.RCFlag, 0), 0, 'No', 'Yes') RC,'" + dtFrom + "' asondt, To_date('" + dtFrom + "', 'DD/MM/YYYY')-To_Date(lm.docdate, 'DD/MM/YYYY') Days From  DrumMast M, LocDetails L, ItemMaster I, LotMast LM, (Select S.DrumNo, S.CDrumNo, S.Location, Sum(Decode(PlusOrMinus, 'p', S.Qty, -S.Qty)) DStkQty From  DrumStock S WHERE s.docdate <= '" + dtFrom + "'Group By S.DrumNo, S.CDrumNo, S.Location Having Sum(Decode(PlusOrMinus, 'p', S.Qty, -S.Qty)) > 0 ) S, (Select B.DrumNo,  B.LotNo BatchNo, B.LocID, B.ItemID, Sum(B.PlusQty) - Sum(B.MinusQty) ClQty, Sum(B.StockValue) / (Sum(B.PlusQty) + Sum(B.MinusQty)) Rate From LStockValue B Where B.DrumNo Is Not Null and B.DOCDATE <= '" + dtFrom + "' Group By B.LotNo, B.DrumNo, B.LocID, B.ItemID Having(Sum(B.PlusQty)-Sum(B.MinusQty)) > 0)  B Where  S.CDrumNo = B.Drumno(+) And L.LocDetailsID = S.Location And S.Location = B.LocID(+) And L.LOCATIONTYPE in ('AP MILL', 'SIEVE & BLEND')And S.DrumNo = M.DrumMastID And B.ItemID = I.ItemMasterID(+) And B.BatchNo = LM.LotNo(+) Group By M.DrumMastID,LM.Docdate, M.DrumNo, I.ItemID, B.BatchNo, M.Partial, S.Location, B.ItemID, B.LOCID, L.LocID, B.Rate, LM.RCFlag Having(Sum(DStkQty) > 0 And  Sum(Nvl(B.ClQty, 0)) = 0) Or(Sum(Nvl(B.ClQty, 0)) > 0)Order By L.LocID, DrumNo) Where BatchNo<>'Empty' And Days >= 30 Order By LocID, ItemID , docdate, DrumNo";

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
