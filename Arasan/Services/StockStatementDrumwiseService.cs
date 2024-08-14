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
  
namespace Arasan.Services
{
    public class StockStatementDrumwiseService : IStockStatementDrumwise
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public StockStatementDrumwiseService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetLocation(string LocID)
        {
            string SvSql = string.Empty;
            SvSql = "Select L.LocDetailsID , L.LocID as Location From LocDetails L Union All Select 1 ID , 'ALL LOCATIONS'From Dual Order By 1";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSN(string SNID)
        {
            string SvSql = string.Empty;
            SvSql = "Select I.SNCATEGORY From ITEMMASTER I WHERE I.SNCATEGORY IS NOT NULL GROUP BY I.SNCATEGORY Union All Select 'ALL' From Dual Order By 1";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllStockStatementDrumwise(string dtFrom, string WEDyn, string SN, string Location)
        {


            try
            {
                string SvSql = "";
                SvSql = "Select x.DrumMastID,x.DrumNo, x.ItemID,X.batchno,Sum(X.qty) Qty, X.locid,x.Completed,X.Inspection,x.CuringInward,x.CuringES,x.CuringOutward,x.Recharge,x.NCRelease,x.Packing, x.PackingIns,x.DocDate,x.subcategory,x.Batch,x.FiDrms,x.CURDAYS,x.idle,x.igroup,Decode(FiDrms, 0, Decode(Sign(sum(x.idle) + 1), 1, Decode(x.Igroup, 'FINISHED', Decode(x.Recharge, 'No', 'Ready For Packing', 'Ready For Re-Process'), 'Ready for Next Process'), 'Not Ready'), 'Not Ready') Status From(Select M.DrumMastID, M.DrumNo, I.ItemID, Nvl(B.BatchNo, 'Empty') BatchNo, Nvl(Sum(B.CLQty), 0) Qty,Nvl((B.Rate), 0) Rate, L.LocID, Decode(Nvl(LM.Compflag, 0), 0, 'No', 'Yes') Completed,Decode(Nvl(LM.InsFlag, 0), 0, 'No', 'Yes') Inspection,Decode(Nvl(LM.CurInwFlag, 0), 0, 'No', 'Yes') CuringInward,Decode(Nvl(LM.EStatus, 0), 0, 'No', 'Yes') CuringES,Decode(Nvl(LM.CurOutFlag, 0), 0, 'No', 'Yes') CuringOutward,Decode(Nvl(LM.RCFlag, 0), 0, 'No', 'Yes') Recharge,Decode(Nvl(LM.QcRelaseFlag, 0), 0, 'No', 'Yes') NCRelease,Decode(Nvl(LM.PackFlag, 0), 0, 'No', 'Yes') Packing,Decode(Nvl(LM.PackInsFlag, 0), 0, 'No', 'Yes') PackingIns,LM.DocDate, Initcap(i.SubCategory) subcategory, LM.Batch, Nvl(LM.FIDRMS, 0) FiDrms, IC.CURDAYS, (((sysdate) - LM.DocDate) - nvl(IC.CURDAYS, 0)) IDLE, I.IGROUP From  DrumMast M, LocDetails L, ItemMaster I, LotMast LM, itemmCurInfo IC,(Select S.DrumNo, S.CDrumNo, S.Location, Sum(Decode(PlusOrMinus, 'p', S.Qty, -S.Qty)) DStkQty From  DrumStock S Where S.DocDate <= '" + dtFrom + "'Group By S.DrumNo, S.CDrumNo, S.Location Having Sum(Decode(PlusOrMinus, 'p', S.Qty, -S.Qty)) > 0 order by 2) S, (Select B.DrumNo,  B.LotNo BatchNo, B.LocID, B.ItemID, Sum(B.PlusQty) - Sum(B.MinusQty) ClQty,Sum(B.StockValue) / (Sum(B.PlusQty) + Sum(B.MinusQty)) Rate From LStockValue B Where B.DrumNo Is Not Null And B.DocDate <= '" + dtFrom + "'Group By B.LotNo, B.DrumNo, B.LocID, B.ItemID Having(Sum(B.PlusQty-B.MinusQty)) > 0)  B Where  S.CDrumNo = B.Drumno(+) And L.LocDetailsID = S.Location And S.Location = B.LocID(+) And S.DrumNo = M.DrumMastID And B.ItemID = I.ItemMasterID(+) AND I.ITEMMASTERID = IC.ITEMMASTERID(+) AND(I.SNCATEGORY = '" + SN + "' OR 'ALL' = '" + SN + "') And B.BatchNo = LM.LotNo(+) And(L.LocID = '" + Location + "' Or '" + Location + "' = 'ALL LOCATIONS') Group By M.DrumMastID, M.DrumNo, I.ItemID, B.BatchNo, M.Partial, S.Location, B.ItemID, B.LOCID, L.LocID, B.Rate, LM.Compflag,LM.InsFlag, LM.CurInwFlag,LM.EStatus,LM.CurOutFlag, LM.RCFlag,LM.QcRelaseFlag,LM.PackFlag,LM.PackInsFlag,LM.DocDate,i.SubCategory,LM.Batch,LM.FIDRMS,IC.CURDAYS,I.IGROUP Having(Sum(DStkQty) > 0 And  Sum(Nvl(B.ClQty, 0)) = 0) Or(Sum(Nvl(B.ClQty, 0)) > 0) Order By L.LocID, I.ItemID , DrumNo) x Where(('" + WEDyn + "' = 'Yes' ) Or(x.BatchNo<>'Empty'  And '" + WEDyn + "' = 'No')) Group by x.DrumMastID,x.DrumNo, x.ItemID,X.batchno,x.Completed,X.Inspection,x.CuringInward,x.CuringES,x.CuringOutward,x.Recharge,x.NCRelease,x.Packing,x.PackingIns,x.DocDate,x.subcategory,x.Batch,x.FiDrms,x.CURDAYS,x.idle,x.igroup,X.locid Order By 24,x.FIDRMS, x.LocID, x.ItemID , x.docdate, x.DrumNo";

                //else
                //{
                //SvSql = "Select x.DrumMastID,x.DrumNo, x.ItemID,X.batchno,Sum(X.qty) Qty,round(Sum(x.rate), 2) rate,X.locid,x.Completed,X.Inspection,x.CuringInward,x.CuringES,x.CuringOutward,x.Recharge,x.NCRelease,x.Packing, x.PackingIns,x.DocDate,x.subcategory,x.Batch,x.FiDrms,x.CURDAYS,x.idle,x.igroup,Decode(FiDrms, 0, Decode(Sign(sum(x.idle) + 1), 1, Decode(x.Igroup, 'FINISHED', Decode(x.Recharge, 'No', 'Ready For Packing', 'Ready For Re-Process'), 'Ready for Next Process'), 'Not Ready'), 'Not Ready') Status From(Select M.DrumMastID, M.DrumNo, I.ItemID, Nvl(B.BatchNo, 'Empty') BatchNo, Nvl(Sum(B.CLQty), 0) Qty,Nvl((B.Rate), 0) Rate, L.LocID, Decode(Nvl(LM.Compflag, 0), 0, 'No', 'Yes') Completed,Decode(Nvl(LM.InsFlag, 0), 0, 'No', 'Yes') Inspection,Decode(Nvl(LM.CurInwFlag, 0), 0, 'No', 'Yes') CuringInward,Decode(Nvl(LM.EStatus, 0), 0, 'No', 'Yes') CuringES,Decode(Nvl(LM.CurOutFlag, 0), 0, 'No', 'Yes') CuringOutward,Decode(Nvl(LM.RCFlag, 0), 0, 'No', 'Yes') Recharge,Decode(Nvl(LM.QcRelaseFlag, 0), 0, 'No', 'Yes') NCRelease,Decode(Nvl(LM.PackFlag, 0), 0, 'No', 'Yes') Packing,Decode(Nvl(LM.PackInsFlag, 0), 0, 'No', 'Yes') PackingIns,LM.DocDate, Initcap(i.SubCategory) subcategory, LM.Batch, Nvl(LM.FIDRMS, 0) FiDrms, IC.CURDAYS, (((sysdate) - LM.DocDate) - nvl(IC.CURDAYS, 0)) IDLE, I.IGROUP From  DrumMast M, LocDetails L, ItemMaster I, LotMast LM, itemmCurInfo IC,(Select S.DrumNo, S.CDrumNo, S.Location, Sum(Decode(PlusOrMinus, 'p', S.Qty, -S.Qty)) DStkQty From  DrumStock S Where S.DocDate <= '" + dtFrom + "'Group By S.DrumNo, S.CDrumNo, S.Location Having Sum(Decode(PlusOrMinus, 'p', S.Qty, -S.Qty)) > 0 order by 2) S, (Select B.DrumNo,  B.LotNo BatchNo, B.LocID, B.ItemID, Sum(B.PlusQty) - Sum(B.MinusQty) ClQty,Sum(B.StockValue) / (Sum(B.PlusQty) + Sum(B.MinusQty)) Rate From LStockValue B Where B.DrumNo Is Not Null And B.DocDate <= '" + dtFrom + "'Group By B.LotNo, B.DrumNo, B.LocID, B.ItemID Having(Sum(B.PlusQty-B.MinusQty)) > 0)  B Where  S.CDrumNo = B.Drumno(+) And L.LocDetailsID = S.Location And S.Location = B.LocID(+) And S.DrumNo = M.DrumMastID And B.ItemID = I.ItemMasterID(+) AND I.ITEMMASTERID = IC.ITEMMASTERID(+) AND(I.SNCATEGORY = '" + SN + "' OR 'ALL' = '" + SN + "') And B.BatchNo = LM.LotNo(+) And(L.LocID = '" + Location + "' Or '" + Location + "' = 'ALL LOCATIONS') Group By M.DrumMastID, M.DrumNo, I.ItemID, B.BatchNo, M.Partial, S.Location, B.ItemID, B.LOCID, L.LocID, B.Rate, LM.Compflag,LM.InsFlag, LM.CurInwFlag,LM.EStatus,LM.CurOutFlag, LM.RCFlag,LM.QcRelaseFlag,LM.PackFlag,LM.PackInsFlag,LM.DocDate,i.SubCategory,LM.Batch,LM.FIDRMS,IC.CURDAYS,I.IGROUP Having(Sum(DStkQty) > 0 And  Sum(Nvl(B.ClQty, 0)) = 0) Or(Sum(Nvl(B.ClQty, 0)) > 0) Order By L.LocID, I.ItemID , DrumNo) x Where(('" + WEDyn + "' = 'Yes' ) Or(x.BatchNo<>'Empty'  And '" + WEDyn + "' = 'No')) Group by x.DrumMastID,x.DrumNo, x.ItemID,X.batchno,x.Completed,X.Inspection,x.CuringInward,x.CuringES,x.CuringOutward,x.Recharge,x.NCRelease,x.Packing,x.PackingIns,x.DocDate,x.subcategory,x.Batch,x.FiDrms,x.CURDAYS,x.idle,x.igroup,X.locid Order By 24,x.FIDRMS, x.LocID, x.ItemID , x.docdate, x.DrumNo";


                //}
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
