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

namespace Arasan.Services.Report
{
    public class PurchaseRepPartyReportService : IPurchaseRepPartyReportService
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public PurchaseRepPartyReportService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetItem(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER Where SUBGROUPCODE='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllPurchasePartyReport(string dtFrom, string dtTo, string Branch, string Customer, string Item)
        {
            try
            {
                string SvSql = "Select BranchID,PartyID,Docid,to_char(Docdate,'dd-MON-yyyy')Docdate,RefNo,RefDt,ItemID,Unit,Qty,Rate,Amount,SGST,CGST,IGST,Rem,Inddept,Decode(Docid,Ndoc,0,Net) Net from (SELECT Br.BranchID ,P.PartyID , B.DocID , B.DocDate , to_char(B.RefNo) RefNo , B.RefDt ,  I.ItemID , U.UnitID Unit , D.Qty , D.Rate , D.Amount,D.SGST,D.CGST,D.IGST, Pd.Narration Rem,l.locid inddept,b.NET,lead(B.DOCID,1) Over(Order by P.partyid,B.docdate,B.docid) Ndoc FROM DpBasic B , PartyMast P , BranchMast Br , DpDetail D , ItemMaster I , UnitMast U , PindDetail Pd,locdetails l WHERE B.PartyID = P.PartyMastID AND B.DpBasicID = D.DpBasicID AND B.BranchID = Br.BranchMastID AND D.ItemID = I.ItemMasterID AND D.Unit = U.UnitMastID AND Pd.PindDetailID = D.PindDetailID AND pd.DEPARTMENT = l.LOCDETAILSID)";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and Docdate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Branch != null)
                {
                    SvSql += " and BranchID='" + Branch + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Customer != null)
                {
                    SvSql += " and PartyID='" + Customer + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Item != null)
                {
                    SvSql += " and ItemID='" + Item + "'";
                }
                else
                {
                    SvSql += "";
                }
                SvSql += "Union Select BranchID,PartyID,Docid,to_char(Docdate,'dd-MON-yyyy')Docdate,RefNo,RefDt,ItemID,Unit,Qty,Rate,Amount,SGST,CGST,IGST,Rem,Inddept,Decode(Docid,Ndoc,0,Net) Net from (SELECT Br.BranchID ,P.PartyID , B.DocID , B.DocDate , to_char(B.RefNo) RefNo , B.RefDt ,  I.ItemID , U.UnitID Unit , D.Qty , D.Rate , D.Amount,D.SGST,D.CGST,D.IGST , Pd.Narration Rem,l.locid inddept,b.NET,lead(B.DOCID,1) Over(Order by P.partyid,B.docdate,B.docid) Ndoc FROM grnBLBasic B , PartyMast P , BranchMast Br , grnBLDetail D , ItemMaster I , UnitMast U , PoDetail Pod , PindDetail Pd,locdetails l WHERE B.PartyID = P.PartyMastID AND B.grnBLBasicID = D.grnBLBasicID AND B.BranchID = Br.BranchMastID AND D.ItemID = I.ItemMasterID AND D.Unit = U.UnitMastID AND D.PoDetailID = Pod.PoDetailID AND Pd.PindDetailID = Pod.PindDetailID AND pd.DEPARTMENT = l.LOCDETAILSID) ";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Branch != null)
                {
                    SvSql += " and BranchID='" + Branch + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Customer != null)
                {
                    SvSql += " and PartyID='" + Customer + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Item != null)
                {
                    SvSql += " and ItemID='" + Item + "'";
                }
                else
                {
                    SvSql += "";
                }
                SvSql += "Union Select BranchID,PartyID,Docid,to_char(Docdate,'dd-MON-yyyy')Docdate,RefNo,RefDt,ItemID,Unit,Qty,Rate,Amount,0 GST,0,0,Rem,Inddept,Decode(Docid,Ndoc,0,Net) Net from (SELECT Br.BranchID , P.PartyID , B.DocID , B.DocDate , B.RefNo , B.RefDate refdt , I.ItemID , U.UnitID Unit , D.Qty , D.Rate , D.Amount, Pd.Narration Rem,l.locid inddept ,b.NET,lead(B.DOCID,1) Over(Order by P.partyid,B.docdate,B.docid) Ndoc FROM IGrnBasic B,IGrndetail D,PartyMast P , BranchMast Br,ItemMaster I, UnitMast U , IPodetail Pod,IPinddetail Pd,IPindbasic Pb,IPobasic PoB,locdetails l WHERE B.PartyID = P.PartyMastID AND PD.itemid = Pod.Itemid AND Pb.IPindbasicid = Pd.IPindbasicid AND Pod.Indentno = Pb.Docid AND B.igrnBasicID = D.IgrnBasicID AND B.BranchID = Br.BranchMastID AND D.ItemmasterID = I.Itemmasterid AND D.Unit = U.UnitMastID AND Pob.Ipobasicid = Pod.Ipobasicid AND D.Refnod = Pob.Ipobasicid AND pd.DEPARTMENT = l.LOCDETAILSID)";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Branch != null)
                {
                    SvSql += " and BranchID='" + Branch + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Customer != null)
                {
                    SvSql += " and PartyID='" + Customer + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Item != null)
                {
                    SvSql += " and ItemID='" + Item + "'";
                }
                else
                {
                    SvSql += "";
                }
                SvSql += "Union Select BranchID,PartyID,Docid,to_char(Docdate,'dd-MON-yyyy')Docdate,RefNo,RefDt,ItemID,Unit,Qty,Rate,Amount,0,0,0 GST,Rem,Inddept,Decode(Docid,Ndoc,0,Net) Net from (SELECT B.Branchid,P.Partyid,S.Docid,S.Docdate ,S.Refno,S.Refdate refdt,I.Itemid,SD.MUnit unit,SD.MrQty Qty,SD.BRate Rate,(SD.MrQty*SD.BRate) amount ,S.Narration Rem,'CONVERSION' Inddept ,0 net,lead(S.DOCID,1) Over(Order by P.partyid,S.docdate,S.docid) Ndoc FROM SubMRBasic S,Branchmast B,Partymast P,subactmrdet SD,Itemmaster I WHERE B.Branchmastid = S.Branch AND P.Partymastid = S.Partyid AND S.SubMRBasicid = SD.SubMRBasicid AND SD.Mitemid = I.Itemmasterid)";
                if (dtFrom != null && dtTo != null)
                {
                    SvSql += " and DocDate BETWEEN '" + dtFrom + "' AND '" + dtTo + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Branch != null)
                {
                    SvSql += " and BranchID='" + Branch + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Customer != null)
                {
                    SvSql += " and PartyID='" + Customer + "'";
                }
                else
                {
                    SvSql += "";
                }
                if (Item != null)
                {
                    SvSql += " and ItemID='" + Item + "'";
                }
                else
                {
                    SvSql += "";
                }
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
