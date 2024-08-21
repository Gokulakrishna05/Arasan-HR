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
    public class ReceiptAgainstReturnableGoodsService : IReceiptAgainstReturnableGoods
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public ReceiptAgainstReturnableGoodsService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetAllReceiptAgainstReturnableGoods(string dtFrom,string dtTo, string Branch)
        {
            try
            {
                string SvSql = "";
                SvSql = " SELECT BR.BRANCHID , L.LOCID , B.DOCID , B.DOCDATE , P.PARTYID , RB.DOCID DCNO , RB.DOCDATE DCDT, B.REFNO , B.REFDATE , D.ITEMID , U.UnitID Unit , D.QTY , D.REJQTY , D.ACCQTY  , Rd.Qty DcQty FROM RECDCBASIC B , RECDCDETAIL D, BRANCHMAST BR , LOCDETAILS L, RDELBASIC RB , UnitMast U, RDelDetail Rd,Partymast P WHERE B.RECDCBASICID = D.RECDCBASICID AND RB.DOCDATE BETWEEN '"+dtFrom+"' AND '"+dtTo+"' AND B.BRANCHID = BR.BRANCHMASTID And D.Unit = U.UnitMastID And P.partymastid = RB.Partyid AND(BR.BRANCHID = '"+Branch+"' OR 'ALL BRANCHES' ='"+Branch+"') AND B.LOCID(+) = L.LOCDETAILSID AND B.DCNO = RB.RDELBASICID(+) And Rb.RDelBasicID = Rd.RDelBasicID And Rd.ItemID = D.ItemID ORDER BY 7,6,2";

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
