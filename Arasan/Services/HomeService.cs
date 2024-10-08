﻿using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
using Microsoft.AspNetCore.Http;
namespace Arasan.Services
{
    public class HomeService : IHomeService
    {
        private readonly string _connectionString;
        DataTransactions _dtransactions;
        public HomeService(IConfiguration _configuratio)
        {
        _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
        }

        //public IEnumerable<MenuList> GetAllMenu(string userid)
        //{
        //    List<MenuList> cmpList = new List<MenuList>();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {

        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "select S.TITLE,S.PARENT,S.GROUP_ID,S.IS_HEAD from user_privileges U,USERPRIVDETAIL P,SITEMAP S WHERE U.PRIVILEGESID=P.PRIVILEGESID AND P.SITEMAPID=P.SITEMAPID AND EMPID='" + userid + "'";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                MenuList cmp = new MenuList
        //                {
        //                    Title = rdr["TITLE"].ToString(),
        //                    Parent = rdr["PARENT"].ToString(),
        //                    Groupid = rdr["GROUP_ID"].ToString(),
        //                    IsHead = rdr["IS_HEAD"].ToString()
        //                };
        //                cmpList.Add(cmp);
        //            }
        //        }
        //    }
        //    return cmpList;
        //}
        public DataTable GetAllMenu(string userid)
        {
            string SvSql = string.Empty;
            SvSql = "select S.TITLE,S.PARENT,S.GROUP_ID,S.IS_HEAD from user_privileges U,USERPRIVDETAIL P,SITEMAP S WHERE U.PRIVILEGESID=P.PRIVILEGESID AND P.SITEMAPID=S.SITEMAPID AND EMPID='" + userid + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable CuringGroup()
        {
            string SvSql = string.Empty;
            SvSql = "select SUBGROUP from CURINGMASTER GROUP BY SUBGROUP  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable curingsubgroup(string curingset)
        {
            string SvSql = string.Empty;
            SvSql = "select SHEDNUMBER,CAPACITY,STATUS,OCCUPIED from CURINGMASTER where SUBGROUP='" + curingset + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

       
        public DataTable GetquoteFollowupnextReport()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT QUO_ID, FOLLOWED_BY,FOLLOW_DATE ,SYSDATE,SYSDATE + 2,TO_CHAR(NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE  from quotation_follow_up  where NEXT_FOLLOW_DATE  between SYSDATE  and  SYSDATE + 2  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;

        }
        public DataTable GetEnqFollowupnextReport()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT ENQ_ID,FOLLOWED_BY,FOLLOW_DATE ,SYSDATE,SYSDATE + 2,TO_CHAR(NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE  from ENQUIRY_FOLLOW_UP  where NEXT_FOLLOW_DATE  between SYSDATE  and  SYSDATE + 2  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSalesQuoteFollowupnextReport()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT SALESQUOFOLLOWID,QUOTE_NO,FOLLOW_BY,FOLLOW_DATE,FOLLOW_STATUS ,SYSDATE,SYSDATE + 2,TO_CHAR(NEXT_FOLLOW_DATE,'dd-MON-yyyy')NEXT_FOLLOW_DATE  from SALESQUOFOLLOWUP  where NEXT_FOLLOW_DATE  between SYSDATE  and  SYSDATE + 2  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCurInward()
        {
            string SvSql = string.Empty;
            SvSql = "SELECT CURINPBASICID,CURINPDETAILID,DRUMNO,ITEMMASTER.ITEMID ,SYSDATE,SYSDATE + 2,TO_CHAR(DUEDATE,'dd-MON-yyyy')DUEDATE  from CURINPDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=CURINPDETAIL.ITEMID  where DUEDATE  between SYSDATE  and  SYSDATE + 2  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCurInwardDoc(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT CURINPBASICID,DOCID  from CURINPBASIC  where CURINPBASICID='"+id+"'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetMaterialnot()
        {
            string SvSql = string.Empty;
            SvSql = "Select STOCK,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,UNITMAST.UNITID,ITEMMASTER.ITEMID,STORESREQDETAIL.QTY,LOCDETAILS.LOCID from STORESREQDETAIL LEFT OUTER JOIN STORESREQBASIC ON STORESREQBASIC.STORESREQBASICID=STORESREQDETAIL.STORESREQBASICID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=STORESREQBASIC.FROMLOCID LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESREQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESREQDETAIL.UNIT where STOCK < STORESREQDETAIL.QTY  ORDER  BY to_date(DOCDATE) DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetGRN(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT to_char(GRNBLBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,DOCID,PARTYMAST.PARTYNAME  from GRNBLBASIC LEFT OUTER JOIN  PARTYMAST ON PARTYMAST.PARTYMASTID=GRNBLBASIC.PARTYID where GRNBLBASICID='" + id + "' ORDER BY GRNBLBASIC.GRNBLBASICID ASC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetDamageGRN()
        {
            string SvSql = string.Empty;
            SvSql = "Select T1SOURCEID,TYPE,to_char(NOTIFYDATE,'dd-MON-yyyy')NOTIFYDATE,DISPLAY,EXPIRYDATE from PURNOTIFICATION  where TYPE='GRN' and EXPIRYDATE  between SYSDATE  and  SYSDATE +10 ORDER BY NOTIFYID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetDamageGRNDetail()
        {
            string SvSql = string.Empty;
            SvSql = "Select GD.GRNBLBASICID,ITEMMASTER.ITEMID,GD.DAMAGE_QTY,G.DOCID,G.PARTYNAME,to_char(G.DOCDATE,'dd-MON-yyyy')DOCDATE from GRNBLBASIC G, GRNBLDETAIL GD LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=GD.ITEMID WHERE GD.DAMAGE_QTY >0 AND G.GRNBLBASICID=GD.GRNBLBASICID ORDER BY to_date(G.DOCDATE) DESC  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndent()
        {
            string SvSql = string.Empty;
            SvSql = "Select T1SOURCEID,TYPE,to_char(NOTIFYDATE,'dd-MON-yyyy')NOTIFYDATE,DISPLAY,EXPIRYDATE from PURNOTIFICATION  where TYPE='MR' and EXPIRYDATE  between SYSDATE  and  SYSDATE +10 ORDER BY NOTIFYID DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetMatDetail()
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESREQDETAILID,STORESREQBASICID,ITEMMASTER.ITEMID,STORESREQDETAIL.QTY from STORESREQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESREQDETAIL.ITEMID where QTY>STOCK ORDER BY STORESREQDETAIL.STORESREQDETAILID ASC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetMat(string id)
        {
            string SvSql = string.Empty;
            SvSql = "SELECT to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,DOCID,LOCDETAILS.LOCID  from STORESREQBASIC LEFT OUTER JOIN  LOCDETAILS ON LOCDETAILS.LOCDETAILSID=STORESREQBASIC.FROMLOCID where STORESREQBASICID='" + id + "'  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIssMatDetail()
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESREQDETAIL.STORESREQDETAILID,STORESREQDETAIL.STORESREQBASICID,ITEMMASTER.ITEMID,STORESREQDETAIL.QTY,STORESREQDETAIL.STOCK,STORESREQDETAIL.STATUS,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,STORESREQBASIC.DOCID,LOCDETAILS.LOCID  from STORESREQBASIC LEFT OUTER JOIN  LOCDETAILS ON LOCDETAILS.LOCDETAILSID=STORESREQBASIC.FROMLOCID,STORESREQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESREQDETAIL.ITEMID where STORESREQDETAIL.STORESREQBASICID=STORESREQBASIC.STORESREQBASICID AND STORESREQDETAIL.QTY<=STORESREQDETAIL.STOCK and STORESREQDETAIL.STATUS ='OPEN' ORDER BY to_date(STORESREQBASIC.DOCDATE) DESC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable Getinddentapprove()
        {
            string SvSql = string.Empty;
            SvSql = "select P.DOCID,to_char(P.DOCDATE,'dd-MON-yyyy')DOCDATE,ITEMMASTER.ITEMID,PD.QTY,LOCDETAILS.LOCID from PINDBASIC P,PINDDETAIL PD LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=PD.ITEMID  LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PD.DEPARTMENT WHERE P.PINDBASICID=PD.PINDBASICID AND PD.APPROVED1 is null AND PD.MODIFY_BY IS NULL";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
