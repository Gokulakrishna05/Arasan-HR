using Arasan.Interface;
using Arasan.Models;
//using DocumentFormat.OpenXml.Office2010.Excel;
//using DocumentFormat.OpenXml.Wordprocessing;
//using GrapeCity.DataVisualization.Chart;
using Microsoft.CodeAnalysis.Host.Mef;
using Microsoft.CodeAnalysis.VisualBasic.Syntax;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Data;
using System.Security.Cryptography;

namespace Arasan.Services
{
    public class MaterialRequisitionService : IMaterialRequisition
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        public MaterialRequisitionService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetWorkCenter(string value)
        {
            string SvSql = string.Empty;
            SvSql = "Select WCID,WCBASICID from WCBASIC where ILOCATION='" + value + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable BindProcess(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PROCESSMAST.PROCESSNAME,WCBASIC.PROCESSID from WCBASIC left outer join PROCESSMAST on PROCESSMASTID=WCBASIC.PROCESSID where WCBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetmaterialReqDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHID ,FROMLOCID,PROCESSID,WCID,DOCID,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,REQTYPE,STORESREQBASICID  from STORESREQBASIC where STORESREQBASICID=" + id + "";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetmaterialReqItemDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESREQDETAIL.UNIT,ITEMMASTER.ITEMMASTERID,UNITMAST.UNITID,STORESREQDETAIL.STOCK,STORESREQDETAIL.ITEMID,STORESREQDETAIL.QTY,NARR from STORESREQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESREQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESREQDETAIL.UNIT WHERE STORESREQDETAIL.STORESREQBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemGrp()
        {
            string SvSql = string.Empty;
            SvSql = "Select GROUPTYPE,ITEMGROUPID FROM ITEMGROUP  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLoc()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCATIONDETAILSID FROM LOCATIONDETAILS  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMID,ITEMGROUP FROM ITEMMASTER where ITEMMASTERID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
            //if (!string.IsNullOrEmpty(value) && value != "0")
            //{
            //    SvSql += " Where ITEMGROUP='" + value + "'";
            //}
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLocation(string id)
        {
            string SvSql = string.Empty;
            SvSql = " select locdetails.LOCID ,EMPLOYEELOCATION.LOCID loc from EMPLOYEELOCATION  left outer join locdetails on locdetails.locdetailsid=EMPLOYEELOCATION.LOCID where EMPID='" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetMatbyID(string MatId)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESREQBASIC.BRANCHID as BRANCHIDS,STORESREQBASIC.WCID as work,STORESREQBASIC.FROMLOCID,BRANCHMAST.BRANCHID,STORESREQBASIC.BRANCHID as BRANCH,LOCDETAILS.LOCID,PROCESSMAST.PROCESSNAME,WCBASIC.WCID,REQTYPE,DOCID,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,STORESREQBASICID  from STORESREQBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=STORESREQBASIC.WCID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=STORESREQBASIC.PROCESSID LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESREQBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=STORESREQBASIC.FROMLOCID where STORESREQBASICID='" + MatId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetMatItemByID(string MatId)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESREQDETAILID,STORESREQBASICID,STORESREQDETAIL.UNIT,ITEMMASTER.ITEMMASTERID,SCHCLQTY,UNITMAST.UNITID,ITEMMASTER.ITEMID,STORESREQDETAIL.QTY,ISSQTY from STORESREQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESREQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESREQDETAIL.UNIT WHERE STORESREQDETAIL.STORESREQBASICID='" + MatId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetIndMatItemByID(string MatId)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESREQDETAILID,STORESREQDETAIL.UNIT,ITEMMASTER.ITEMMASTERID,UNITMAST.UNITID,ITEMMASTER.ITEMID,STORESREQDETAIL.STOCK,STORESREQDETAIL.QTY from STORESREQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESREQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESREQDETAIL.UNIT WHERE STORESREQDETAIL.STORESREQBASICID='" + MatId + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,UNITMAST.UNITMASTID from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public IEnumerable<MaterialRequisition> GetAllMaterial(string status, string st, string ed)
        {
            if (string.IsNullOrEmpty(status))
            {
                status = "OPEN";
            }
            List<MaterialRequisition> cmpList = new List<MaterialRequisition>();
            using (OracleConnection con = new OracleConnection(_connectionString))
            {
                if (st != null && ed != null)
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = "Select BRANCHMAST.BRANCHID,STORESREQBASIC.DOCID,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,PROCESSID,REQTYPE,STORESREQBASICID from STORESREQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESREQBASIC.BRANCHID LEFT OUTER JOIN  LOCDETAILS on STORESREQBASIC.FROMLOCID=LOCDETAILS.LOCDETAILSID WHERE STORESREQBASIC.DOCDATE BETWEEN '" + st + "'  AND '" + ed + "'  order by STORESREQBASICID desc";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            MaterialRequisition cmp = new MaterialRequisition
                            {
                                ID = rdr["STORESREQBASICID"].ToString(),
                                Branch = rdr["BRANCHID"].ToString(),
                                Location = rdr["LOCID"].ToString(),
                                Process = rdr["PROCESSID"].ToString(),
                                RequestType = rdr["REQTYPE"].ToString(),
                                DocId = rdr["DOCID"].ToString(),
                                DocDa = rdr["DOCDATE"].ToString()



                            };
                            cmpList.Add(cmp);
                        }
                    }

                }
                else
                {
                    using (OracleCommand cmd = con.CreateCommand())
                    {
                        con.Open();
                        cmd.CommandText = "Select BRANCHMAST.BRANCHID,STORESREQBASIC.DOCID,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,PROCESSID,REQTYPE,STORESREQBASICID from STORESREQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESREQBASIC.BRANCHID LEFT OUTER JOIN  LOCDETAILS on STORESREQBASIC.FROMLOCID=LOCDETAILS.LOCDETAILSID WHERE STORESREQBASIC.DOCDATE > sysdate-30  order by STORESREQBASICID desc";
                        OracleDataReader rdr = cmd.ExecuteReader();
                        while (rdr.Read())
                        {
                            MaterialRequisition cmp = new MaterialRequisition
                            {
                                ID = rdr["STORESREQBASICID"].ToString(),
                                Branch = rdr["BRANCHID"].ToString(),
                                Location = rdr["LOCID"].ToString(),
                                Process = rdr["PROCESSID"].ToString(),
                                RequestType = rdr["REQTYPE"].ToString(),
                                DocId = rdr["DOCID"].ToString(),
                                DocDa = rdr["DOCDATE"].ToString()



                            };
                            cmpList.Add(cmp);
                        }
                    }

                }
            }
            return cmpList;
        }

        public DataTable Getstkqty(string ItemId, string locid )
        {
            string lot = datatrans.GetDataString("SELECT LOTYN FROM ITEMMASTER WHERE ITEMMASTERID='"+ItemId+"'");
            string SvSql = string.Empty;
            if(lot=="YES")
            {
                SvSql = "select SUM(S.PLUSQTY-S.MINUSQTY) as QTY  from LSTOCKVALUE S  where S.LOCID='" + locid + "' AND S.ITEMID='" + ItemId + "' HAVING SUM(S.PLUSQTY-S.MINUSQTY) > 0";

            }
            else
            {
                SvSql = "select SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) as QTY  from STOCKVALUE S  where S.LOCID='" + locid + "' AND S.ITEMID='" + ItemId + "' HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemLot(string ItemId, string locid )
        {
            string SvSql = string.Empty;
            SvSql = "select S.LOTNO,SUM(S.PLUSQTY-S.MINUSQTY) as QTY,ITEMMASTER.ITEMID ,S.ITEMID as item  from LSTOCKVALUE S LEFT OUTER JOIN ITEMMASTER on ITEMMASTERID=S.ITEMID,LOTMAST L  where S.LOTNO=L.LOTNO AND S.LOCID='" + locid + "' AND S.ITEMID='" + ItemId + "' HAVING SUM(S.PLUSQTY-S.MINUSQTY) > 0  GROUP BY ITEMMASTER.ITEMID,S.LOTNO,S.ITEMID  ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        //public DataTable Getstkstoreqty(string ItemId,  string brid,string locid)
        //{
        //    string SvSql = string.Empty;
        //    SvSql = "select SUM(BALANCE_QTY) as QTY from INVENTORY_ITEM where BALANCE_QTY > 0 AND LOCATION_ID='" + locid + "' AND BRANCH_ID='" + brid + "' AND ITEM_ID='" + ItemId + "'";
        //    DataTable dtt = new DataTable();
        //    OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
        //    OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
        //    adapter.Fill(dtt);
        //    return dtt;
        //}

        //public MaterialRequisition GetMaterialById(string eid)
        //{
        //    MaterialRequisition Material = new MaterialRequisition();
        //    using (OracleConnection con = new OracleConnection(_connectionString))
        //    {
        //        using (OracleCommand cmd = con.CreateCommand())
        //        {
        //            con.Open();
        //            cmd.CommandText = "Select BRANCHID,DOCID,DOCDATE,FROMLOCID,PROCESSID,REQTYPE,STORESREQBASICID from STORESREQBASIC where STORESREQBASICID=" + eid + "";
        //            OracleDataReader rdr = cmd.ExecuteReader();
        //            while (rdr.Read())
        //            {
        //                MaterialRequisition cmp = new MaterialRequisition
        //                {
        //                    ID = rdr["STORESREQBASICID"].ToString(),

        //                   Branch = rdr["BRANCHID"].ToString(),
        //                   Location = rdr["FROMLOCID"].ToString(),
        //                    Process = rdr["PROCESSID"].ToString(),
        //                    RequestType = rdr["REQTYPE"].ToString(),
        //                    DocId = rdr["DOCID"].ToString(),
        //                    DocDa = rdr["DOCDATE"].ToString()


        //               };

        //                Material = cmp;
        //            }
        //        }
        //   } 
        //    return Material;
        //}
        public string IssuetoIndent(MaterialRequisition cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    foreach (MaterialRequistionItem cp in cy.MRlst)
                    {
                        if (cp.ItemId != "0")
                        {
                            if (cp.IndQty > 0)
                            {
                                /////////////////////////Indent creation
                                datatrans = new DataTransactions(_connectionString);
                                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'Ind-' AND ACTIVESEQUENCE = 'T'");
                                string docid = string.Format("{0}{1}", "Ind-", (idc + 1).ToString());

                                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='Ind-' AND ACTIVESEQUENCE ='T'";
                                try
                                {
                                    datatrans.UpdateStatus(updateCMd);
                                }
                                catch (Exception ex)
                                {
                                    throw ex;
                                }
                                OracleCommand objCmd = new OracleCommand("PIPROC", objConn);

                                objCmd.CommandType = CommandType.StoredProcedure;
                                //if (cy.ID == null)
                                //{
                                StatementType = "Insert";
                                objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                //}
                                //else
                                //{
                                //    StatementType = "Update";
                                //    objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                                //}

                                objCmd.Parameters.Add("Branch", OracleDbType.NVarchar2).Value = cy.BranchId;
                                objCmd.Parameters.Add("Location", OracleDbType.NVarchar2).Value = cy.LocationId;
                                objCmd.Parameters.Add("IndentNo", OracleDbType.NVarchar2).Value = docid;
                                objCmd.Parameters.Add("IndentDate", OracleDbType.Date).Value = DateTime.Now;
                                objCmd.Parameters.Add("RefDate", OracleDbType.Date).Value = DateTime.Now;
                                //objCmd.Parameters.Add("Erecation", OracleDbType.NVarchar2).Value = "";
                               // objCmd.Parameters.Add("PurchaseType", OracleDbType.NVarchar2).Value = "";
                                objCmd.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value =cy.Entered;
                                objCmd.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                objCmd.Parameters.Add("STOREREQID", OracleDbType.NVarchar2).Value = cy.ID;
                                objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;

                                objConn.Open();
                                objCmd.ExecuteNonQuery();
                                Object Pid = objCmd.Parameters["OUTID"].Value;

                                OracleCommand objCmds = new OracleCommand("PIDETAILPROC", objConn);
                                objCmds.CommandType = CommandType.StoredProcedure;
                                StatementType = "Insert";
                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                               
                                objCmds.Parameters.Add("PIID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("ITEMIDS", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("QUANTITY", OracleDbType.NVarchar2).Value = cp.IndQty;
                                objCmds.Parameters.Add("UNITP", OracleDbType.NVarchar2).Value = cp.UnitID;
                                objCmds.Parameters.Add("QC", OracleDbType.NVarchar2).Value = "";
                                objCmds.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = cp.Narration;
                                objCmds.Parameters.Add("DUE_DATE", OracleDbType.Date).Value = DateTime.Now;
                                objCmds.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.LocationId;
                                objCmds.Parameters.Add("ITEMGROUPID", OracleDbType.NVarchar2).Value = cp.ItemGroupId;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmds.ExecuteNonQuery();

                                /////////////////////////Indent creation
                            }

                        }
                    }

                    svSQL = "UPDATE STORESREQBASIC SET STATUS ='Indent' WHERE STORESREQBASICID='" + cy.MaterialReqId + "'";
                    OracleCommand objCmdst = new OracleCommand(svSQL, objConn);
                    objCmdst.ExecuteNonQuery();
                    objConn.Close();


                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public string ApproveMaterial(MaterialRequisition cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                //bool ispending = false;
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    objConn.Open();
                    int l = 0;
                    foreach (MaterialRequistionItem cp in cy.MRlst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {


                          
                           
                            if (cp.InvQty > 0)
                            {
                                string lot = datatrans.GetDataString("SELECT LOTYN FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.ItemId+"'");
                                string masterid = datatrans.GetDataString("SELECT ITEMACC FROM ITEMMASTER WHERE ITEMMASTERID='" + cp.ItemId+"'");
                                if(lot=="YES")
                                {

                                    string[] lotno = cp.lotno.Split(',');
                                    string[] lotqty = cp.inventryQty.Split('-');

                                    for(int k=0; k<lotno.Length; k++)
                                    {
                                        string ilot = lotno[k];
                                        double ilotqty = Convert.ToDouble(lotqty[k]);
                                        double rate = datatrans.GetDataId("SELECT RATE FROM LSTOCKVALUE WHERE LOTNO ='"+ ilot + "' AND LOCID='"+cy.LocationId+"' AND ITEMID='"+cp.ItemId+"' order by DOCDATE desc fetch first 1 rows only ");
                                        double amt=ilotqty*rate;
                                        string SvSql3 = "Insert into LSTOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,DOCID,DOCDATE,LOTNO,PLUSQTY,MINUSQTY,RATE,STOCKVALUE,ITEMID,LOCID,BINNO,FROMLOCID,STOCKTRANSTYPE ) VALUES ('0','0','F','" + cp.detid + "','749342390','" + cy.DocId + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + ilot + "' ,'0','" + ilotqty + "' ,'" + rate + "','" + amt + "','" + cp.ItemId + "','10001000000827','0','0','STORES PROD ISSUE')";
                                        OracleCommand objCmdss = new OracleCommand(SvSql3, objConn);
                                        objCmdss.ExecuteNonQuery();
                                         SvSql3 = "Insert into LSTOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,DOCID,DOCDATE,LOTNO,PLUSQTY,MINUSQTY,RATE,STOCKVALUE,ITEMID,LOCID,BINNO,FROMLOCID,STOCKTRANSTYPE ) VALUES ('0','0','F','" + cp.detid + "','749342390','" + cy.DocId + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + ilot + "' ,'" + ilotqty + "','0' ,'" + rate + "','" + amt + "','" + cp.ItemId + "','" + cy.LocationId + "','0','0','STORES PROD ISSUE')";
                                         objCmdss = new OracleCommand(SvSql3, objConn);
                                        objCmdss.ExecuteNonQuery();
                                    }

                                    double srate = datatrans.GetDataId("select SS.RATE  from STOCKVALUE S,STOCKVALUE2 SS  where S.STOCKVALUEID=SS.STOCKVALUEID AND S.LOCID='" + cy.LocationId + "' AND S.ITEMID='"+cp.ItemId+"' HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 GROUP BY  SS.RATE ");
                                    double amount = cp.InvQty * srate;
                                    string SvSql1 = "Insert into STOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE,MASTERID) VALUES ('0','0','F','" + cp.detid + "','749342921','m','" + cp.ItemId + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cp.InvQty + "' ,'10001000000827','0','0','0','0','0','0','0','0','" + amount + "','"+ masterid +"')RETURNING STOCKVALUEID INTO :STKID";
                                    OracleCommand objCmdsss = new OracleCommand(SvSql1, objConn);
                                    objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    objCmdsss.ExecuteNonQuery();
                                    string narr = "Issued to " + cy.Location;
                                    string stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                    string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + cy.DocId + "','" + narr + "','"+ srate + "')";
                                    OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                    objCmddts.ExecuteNonQuery();
                                      SvSql1 = "Insert into STOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE,MASTERID) VALUES ('0','0','F','" + cp.detid + "','749342921','p','" + cp.ItemId + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cp.InvQty + "' ,'"+cy.LocationId+"','0','0','0','0','0','0','0','0','" + amount + "','" + masterid + "')RETURNING STOCKVALUEID INTO :STKID";
                                      objCmdsss = new OracleCommand(SvSql1, objConn);
                                    objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    objCmdsss.ExecuteNonQuery();
                                    
                                      stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                      SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + cy.DocId + "','" + narr + "','" + srate + "')";
                                      objCmddts = new OracleCommand(SvSql2, objConn);
                                    objCmddts.ExecuteNonQuery();

                                    SvSql2 = "UPDATE STORESREQDETAIL SET SCHCLQTY='"+ cp.IndQty + "' WHERE STORESREQDETAILID='"+ cp.detid +"'";
                                    objCmddts = new OracleCommand(SvSql2, objConn);
                                    objCmddts.ExecuteNonQuery();

                                }
                                else
                                {
                                    string srate = datatrans.GetDataString("select SS.RATE  from STOCKVALUE S,STOCKVALUE2 SS  where S.STOCKVALUEID=SS.STOCKVALUEID AND S.LOCID='10001000000827' AND S.ITEMID='" + cp.ItemId+"' HAVING SUM(DECODE(S.PlusOrMinus,'p',S.qty,-S.qty)) > 0 GROUP BY  SS.RATE ");
                                    double amt = cp.InvQty * Convert.ToDouble(srate);
                                    double amount = Math.Round(amt, 2);
                                    string SvSql1 = "Insert into STOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE) VALUES ('0','0','F','" + cp.detid + "','749342921','m','" + cp.ItemId + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cp.InvQty + "' ,'10001000000827','0','0','0','0','0','0','0','0','" + amount + "') RETURNING STOCKVALUEID INTO :STKID";
                                    OracleCommand objCmdsss = new OracleCommand(SvSql1, objConn);
                                    objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    objCmdsss.ExecuteNonQuery();
                                    string narr = "Issued to " + cy.Location;
                                    string stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                    string SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + cy.DocId + "','" + narr + "','" + srate + "')";
                                    OracleCommand objCmddts = new OracleCommand(SvSql2, objConn);
                                    objCmddts.ExecuteNonQuery();
                                    SvSql1 = "Insert into STOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,PLUSORMINUS,ITEMID,DOCDATE,QTY,LOCID,BINID,RATEC,PROCESSID,SNO,SCSID,SVID,FROMLOCID,SINSFLAG,STOCKVALUE) VALUES ('0','0','F','" + cp.detid + "','749342921','p','" + cp.ItemId + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cp.InvQty + "' ,'" + cy.LocationId + "','0','0','0','0','0','0','0','0','" + amount + "') RETURNING STOCKVALUEID INTO :STKID";
                                    objCmdsss = new OracleCommand(SvSql1, objConn);
                                    objCmdsss.Parameters.Add("STKID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                    objCmdsss.ExecuteNonQuery();

                                    stkid = objCmdsss.Parameters["STKID"].Value.ToString();
                                    SvSql2 = "Insert into STOCKVALUE2 (STOCKVALUEID,DOCID,NARRATION,RATE) VALUES ('" + stkid + "','" + cy.DocId + "','" + narr + "','" + srate + "')";
                                    objCmddts = new OracleCommand(SvSql2, objConn);
                                    objCmddts.ExecuteNonQuery();

                                    SvSql2 = "UPDATE STORESREQDETAIL SET SCHCLQTY='" + cp.IndQty + "',ISSQTY='"+ cp.InvQty + "',SCHISSQTY='"+ cp.InvQty +"' WHERE STORESREQDETAILID='" + cp.detid + "'";
                                    objCmddts = new OracleCommand(SvSql2, objConn);
                                    objCmddts.ExecuteNonQuery();
                                }
                               
                                /////////////////////////Inventory Update
                                DataTable lotnogen = datatrans.GetData("Select LOTYN  FROM ITEMMASTER where LOTYN ='YES' AND ITEMMASTERID='"+cp.ItemId+"'");
                                string lotnumber = "";
                                if (lotnogen.Rows.Count>0)
                                {
                                    string item = cp.Item;
                                    string Docid = cy.DocId;
                                    string DocDate = cy.DocDa;
                                    
                                     lotnumber = string.Format("{0}-{1}-{2}-{3}", item, DocDate, Docid, l.ToString());
                                    l++;
                                }

                                double qty = cp.InvQty;
                                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRNID,GRN_DATE,LOT_NO from INVENTORY_ITEM where INVENTORY_ITEM.INVENTORY_ITEM_ID IN(" + cp.invenid + ")");
                                //string[] inventryqty = cp.inventryQty.Split('-');
                                if (dt.Rows.Count > 0)
                                {
                                    double rqty = Convert.ToDouble(dt.Rows[0]["BALANCE_QTY"].ToString());
                                    if (cp.lotno == "")
                                    {
                                        DataTable dtt1 = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRNID,GRN_DATE,LOT_NO from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID ='" + cp.ItemId + "',INVENTORY_ITEM.LOCATION_ID='10001000000827'");
                                        if (dtt1.Rows.Count > 0)
                                        {
                                            for (int i = 0; i < dtt1.Rows.Count; i++)
                                            {

                                                rqty = Convert.ToDouble(dtt1.Rows[i]["BALANCE_QTY"].ToString());


                                                if (rqty >= qty)
                                                {
                                                    double bqty = rqty - qty;

                                                    string Sql = string.Empty;
                                                    Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dtt1.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                    OracleCommand objCmdss = new OracleCommand(Sql, objConn);

                                                    objCmdss.ExecuteNonQuery();


                                                }

                                                if (rqty >= qty)
                                                {
                                                    //double bqty = rqty - lotqty;

                                                    //string Sql = string.Empty;
                                                    //Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                    //OracleCommand objCmdss = new OracleCommand(Sql, objConn);
                                                    //objConn.Open();
                                                    //objCmdss.ExecuteNonQuery();


                                                    OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConn);
                                                    objCmdI.CommandType = CommandType.StoredProcedure;
                                                    objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                    objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                    objCmdI.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.indentid;
                                                    objCmdI.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdI.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                                    objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = qty;
                                                    objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                                    objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                    objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                    objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = 0;
                                                    objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LocationId;
                                                    objCmdI.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenterid;
                                                    objCmdI.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;

                                                    objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                    objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = lotnumber;
                                                    objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                    objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                                    objCmdI.ExecuteNonQuery();
                                                    Object Invid = objCmdI.Parameters["OUTID"].Value;


                                                    OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                                    objCmdIn.CommandType = CommandType.StoredProcedure;
                                                    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                    objCmdIn.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.indentid;
                                                    objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                                    objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                                    objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "MREQ";
                                                    objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                                    objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                                    objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "MREQ";
                                                    objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                    objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                    objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                    objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LocationId;
                                                    objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                                    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                    objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                    objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                    objCmdIn.ExecuteNonQuery();

                                                    break;
                                                }

                                            }
                                        }
                                        
                                    }
                                    else
                                    {
                                        for (int i = 0; i < dt.Rows.Count; i++)
                                        {
                                            string[] inventryqty = cp.inventryQty.Split('-');
                                            rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());

                                            string[] df = inventryqty[i].Split('#');
                                            string lotno = df[i];
                                            double lotqty = Convert.ToDouble(df[i]);



                                            if (rqty >= qty)
                                            {
                                                double bqty = rqty - lotqty;

                                                string Sql = string.Empty;
                                                Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                OracleCommand objCmdss = new OracleCommand(Sql, objConn);

                                                objCmdss.ExecuteNonQuery();


                                            }

                                            if (rqty >= qty)
                                            {
                                                //double bqty = rqty - lotqty;

                                                //string Sql = string.Empty;
                                                //Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                //OracleCommand objCmdss = new OracleCommand(Sql, objConn);
                                                //objConn.Open();
                                                //objCmdss.ExecuteNonQuery();


                                                OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConn);
                                                objCmdI.CommandType = CommandType.StoredProcedure;
                                                objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                objCmdI.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.indentid;
                                                objCmdI.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                                objCmdI.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                                objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = lotqty;
                                                objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = lotqty;
                                                objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = 0;
                                                objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LocationId;
                                                objCmdI.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenterid;
                                                objCmdI.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "0";
                                                objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;

                                                objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = lotnumber;
                                                objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                                objCmdI.ExecuteNonQuery();
                                                Object Invid = objCmdI.Parameters["OUTID"].Value;


                                                OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                objCmdIn.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.indentid;
                                                objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                                objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRNID"].ToString();
                                                objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                                objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "MREQ";
                                                objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                                objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = lotqty;
                                                objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "MREQ";
                                                objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                                objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                                objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                                objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LocationId;
                                                objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                                objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                                objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                                objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                                objCmdIn.ExecuteNonQuery();

                                                //break;
                                            }
                                        }
                                        
                                    }

                                    //if (rqty >= qty)
                                    //    {
                                    //        //double bqty = rqty - lotqty;

                                    //        //string Sql = string.Empty;
                                    //        //Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                    //        //OracleCommand objCmdss = new OracleCommand(Sql, objConn);
                                    //        //objConn.Open();
                                    //        //objCmdss.ExecuteNonQuery();


                                    //        OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConn);
                                    //        objCmdI.CommandType = CommandType.StoredProcedure;
                                    //        objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    //        objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    //        objCmdI.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.indentid;
                                    //    objCmdI.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";    
                                    //        objCmdI.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                    //        objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                    //        objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = qty;
                                    //        objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                    //        objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                    //        objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                    //        objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                    //        objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = 0;
                                    //        objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LocationId;
                                    //        objCmdI.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenterid;
                                    //        objCmdI.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;

                                    //        objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                    //        objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                    //        objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                    //        objCmdI.ExecuteNonQuery();
                                    //        Object Invid = objCmdI.Parameters["OUTID"].Value;


                                    //        OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                    //        objCmdIn.CommandType = CommandType.StoredProcedure;
                                    //        objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    //        objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    //        objCmdIn.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.indentid;
                                    //        objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                    //        objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                    //        objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "MREQ";
                                    //        objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                    //        objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                    //        objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "MREQ";
                                    //        objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                    //        objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                    //        objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                    //        objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                    //        objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LocationId;
                                    //        objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                    //        objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                    //        objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                    //        objCmdIn.ExecuteNonQuery();

                                    //        break;
                                    //    }



                                    //    else
                                    //    {
                                    //    DataTable dt1 = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRNID,GRN_DATE,LOT_NO from INVENTORY_ITEM where INVENTORY_ITEM.INVENTORY_ITEM_ID IN(" + cp.invenid + ")");
                                    //    //string[] inventryqty = cp.inventryQty.Split('-');
                                         
                                    //         rqty = Convert.ToDouble(dt.Rows[0]["BALANCE_QTY"].ToString());
                                    //        for (int i = 0; i < dt1.Rows.Count; i++)
                                    //        {
                                    //            string[] inventryqty = cp.inventryQty.Split('-');
                                    //            rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());

                                    //            string[] df = inventryqty[i].Split('#');
                                    //            string lotno = df[i];
                                    //            double lotqty = Convert.ToDouble(df[1]);



                                    //            if (rqty >= qty)
                                    //            {
                                    //                double bqty = rqty - lotqty;

                                    //                string Sql = string.Empty;
                                    //                Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                    //                OracleCommand objCmdss = new OracleCommand(Sql, objConn);
                                    //                objConn.Open();
                                    //                objCmdss.ExecuteNonQuery();


                                    //            }


                                    //        }

                                    //        OracleCommand objCmdI = new OracleCommand("INVENTORYITEMPROC", objConn);
                                    //        objCmdI.CommandType = CommandType.StoredProcedure;
                                    //        objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    //        objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    //        objCmdI.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.indentid;
                                    //        objCmdI.Parameters.Add("T1SOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                    //        objCmdI.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value ="0";
                                    //        objCmdI.Parameters.Add("GRN_DATE", OracleDbType.Date).Value = DateTime.Now;
                                    //        objCmdI.Parameters.Add("REC_GOOD_QTY", OracleDbType.NVarchar2).Value = qty;
                                    //        objCmdI.Parameters.Add("BALANCE_QTY", OracleDbType.NVarchar2).Value = qty;
                                    //        objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                    //        objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                    //        objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                    //        objCmdI.Parameters.Add("WASTAGE", OracleDbType.NVarchar2).Value = 0;
                                    //        objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LocationId;
                                    //        objCmdI.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdI.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;

                                    //        objCmdI.Parameters.Add("INV_OUT_ID", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                    //        objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdI.Parameters.Add("LOT_NO", OracleDbType.NVarchar2).Value ="0";
                                    //        objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                    //        objCmdI.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                    //        objCmdI.ExecuteNonQuery();
                                    //        Object Invid = objCmdI.Parameters["OUTID"].Value;

                                    //        OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                                    //        objCmdIn.CommandType = CommandType.StoredProcedure;
                                    //        objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                    //        objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                    //        objCmdIn.Parameters.Add("T1SOURCEID", OracleDbType.NVarchar2).Value = cp.indentid;
                                    //        objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                                    //        objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = Invid;
                                    //        objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "MREQ";
                                    //        objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "I";
                                    //        objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = qty;
                                    //        objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "MREQ";
                                    //        objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                                    //        objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                                    //        objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                                    //        objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                                    //        objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = cy.LocationId;
                                    //        objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.BranchId;
                                    //        objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                                    //        objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                                    //        objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                                    //        objCmdIn.ExecuteNonQuery();
                                    //        objConn.Close();

                                    //    }

                                        //}





                                    }
                                    /////////////////////////Inventory Update
                                    if (cp.IndQty > 0)
                                    {
                                        //svSQL = "UPDATE STORESREQDETAIL SET PENDING_QTY ='" + cp.IndQty + "',STATUS='Pending' WHERE STORESREQDETAILID='" + cp.indentid + "'";
                                        //OracleCommand objCmdsa = new OracleCommand(svSQL, objConn);
                                        //objCmdsa.ExecuteNonQuery();
                                       
                                        //ispending = true;
                                        svSQL = "UPDATE STORESREQBASIC SET STATUS ='Pending'  WHERE STORESREQBASICID='" + cy.ID + "'";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                         

                                    }
                                    else
                                    {
                                    DataTable status = datatrans.GetData("Select STATUS ,STORESREQDETAILID FROM STORESREQDETAIL where STORESREQBASICID ='" + cy.ID+"' AND STATUS IN ('OPEN','Pending')");
                                    if (status.Rows.Count > 0)
                                    {
                                        svSQL = "UPDATE STORESREQBASIC SET STATUS ='Pending' WHERE STORESREQBASICID='" + cy.ID + "'";
                                        OracleCommand objCmdsaa = new OracleCommand(svSQL, objConn);
                                        objCmdsaa.ExecuteNonQuery();
                                    }
                                    else
                                    {
                                        svSQL = "UPDATE STORESREQBASIC SET STATUS ='Issued' WHERE STORESREQBASICID='" + cy.ID + "'";
                                        OracleCommand objCmdsaa = new OracleCommand(svSQL, objConn);
                                        objCmdsaa.ExecuteNonQuery();
                                    }
                                        svSQL = "UPDATE STORESREQDETAIL SET STATUS ='Issued' WHERE STORESREQDETAILID='" + cp.indentid + "'";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                         
                                        //j++;
                                    }


                                }
                            }

                            //if (ispending == true)
                            //{

                            //}
                            //if (cp.IndQty > 0)
                            //{
                            //    using (OracleConnection objConnT = new OracleConnection(_connectionString))
                            //    {
                            //        string Sql = string.Empty;
                            //        svSQL = "UPDATE STORESREQDETAIL SET PENDING_QTY ='" + cp.IndQty + "' WHERE STORESREQDETAILID='" + cp.indentid + "'";
                            //        OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                            //        objConnT.Open();
                            //        objCmds.ExecuteNonQuery();
                            //        objConnT.Close();
                            //    }
                            //}

                        }
                    objConn.Close();
                }
                
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public string MaterialCRUD(MaterialRequisition cy)
        {
            string msg = "";
            string entat = DateTime.Now.ToString("dd\\/MM\\/yyyy hh:mm:ss tt");
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                
                    datatrans = new DataTransactions(_connectionString);
                string updateCMd = "";
                if (cy.ID == null)
                {
                    int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'SRq-' AND ACTIVESEQUENCE = 'T'");
                    string docid = string.Format("{0}{1}", "SRq-", (idc + 1).ToString());

                     updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='SRq-' AND ACTIVESEQUENCE ='T'";
                    datatrans.UpdateStatus(updateCMd);
                    cy.matno = docid;
                }

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {

                   
                    OracleCommand objCmd = new OracleCommand("MATERIALREQPROC", objConn);
                    
                    objCmd.CommandType = CommandType.StoredProcedure;
                    if (cy.ID == null)
                    {
                        StatementType = "Insert";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    }
                    else
                    {
                        StatementType = "Update";
                        objCmd.Parameters.Add("ID", OracleDbType.NVarchar2).Value = cy.ID;
                    }
                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Branch;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.matno;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDa;
                    objCmd.Parameters.Add("FROMLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    if(string.IsNullOrEmpty(cy.WorkCenter))
                    {
                        objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = "0";
                    }
                    else { objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenter; }
                    if (string.IsNullOrEmpty(cy.Process))
                    {
                        objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = "0";
                    }
                    else { objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Process; }
                   
                    objCmd.Parameters.Add("REQTYPE", OracleDbType.NVarchar2).Value = "Other Issue";
                     objCmd.Parameters.Add("STAUS", OracleDbType.NVarchar2).Value = "OPEN";
                    if(cy.ID==null)
                    {
                        objCmd.Parameters.Add("ENTBY", OracleDbType.NVarchar2).Value = cy.Entered;
                        objCmd.Parameters.Add("CREATEDBY", OracleDbType.NVarchar2).Value = cy.EnteredId;
                        objCmd.Parameters.Add("ENTAT", OracleDbType.NVarchar2).Value = entat;
                    }
                    else
                    {
                        objCmd.Parameters.Add("ENTBY", OracleDbType.NVarchar2).Value = cy.Entered;

                        objCmd.Parameters.Add("UPDATEDBY", OracleDbType.NVarchar2).Value = cy.EnteredId;

                        objCmd.Parameters.Add("UPDATEDON", OracleDbType.NVarchar2).Value = entat;
                    }
                        
                  
                    objCmd.Parameters.Add("TOLOCID", OracleDbType.NVarchar2).Value = "10001000000827";
                    objCmd.Parameters.Add("LOCIDCONS", OracleDbType.NVarchar2).Value = cy.Location;
                  
                   
                    //if (cy.Reason == "")
                    //{
                    //    objCmd.Parameters.Add("PRIORITY", OracleDbType.NVarchar2).Value = "0";
                    //}
                    //else
                    //{
                    //    objCmd.Parameters.Add("PRIORITY", OracleDbType.NVarchar2).Value = "1";
                    //}
                    //objCmd.Parameters.Add("REASON", OracleDbType.NVarchar2).Value = cy.Reason;
                     
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    try
                    {
                       
                    }
                    catch (Exception ex)
                    {
                        throw ex;
                    }
                    Object reqid = objCmd.Parameters["OUTID"].Value;
                    if (cy.ID == null)
                    {
                        foreach (MaterialRequistionItem cp in cy.MRlst)
                        {
                            string desc = datatrans.GetDataString("SELECT ITEMDESC FROM ITEMMASTER where ITEMMASTERID='" + cp.ItemId + "' ");


                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                string unitID = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.UnitID + "' ");
                                string VALMETHOD = datatrans.GetDataString("Select VALMETHOD from ITEMMASTER where ITEMMASTERID='" + cp.ItemId + "' ");


                                svSQL = "Insert into STORESREQDETAIL (STORESREQBASICID,ITEMID,UNIT,QTY,STOCK,NARR,ITEMMASTERID,ITEMDESC,STATUS,STORESREQDETAILROW,ISSQTY,SGCODE,TYPE,VALMETHOD,SCHCLQTY,SCHISSQTY,SCHQTY) VALUES ('" + reqid + "','" + cp.ItemId + "','" + unitID + "','" + cp.ReqQty + "','" + cp.ClosingStock + "','" + cy.Narration + "','" + cp.ItemId + "','" + desc + "','OPEN','0','0','0','Stores Issue','"+ VALMETHOD + "','" + cp.ReqQty + "','0','0') RETURNING STORESREQDETAILID INTO :LASTCID";
                                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                objCmds.Parameters.Add("LASTCID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                                objCmds.ExecuteNonQuery();
                                string detailid = objCmds.Parameters["LASTCID"].Value.ToString();
                                double stock = Convert.ToDouble(cp.ReqQty);
                                double clstock = Convert.ToDouble(cp.ClosingStock);
                                if (stock > clstock)
                                {

                                    DateTime currentdate = DateTime.Now;
                                    DateTime expiry = currentdate.AddDays(10);
                                    string notifidate = currentdate.ToString("dd-MMM-yyyy");
                                    string expirydate = expiry.ToString("dd-MMM-yyyy");

                                    svSQL = "Insert into PURNOTIFICATION (T1SOURCEID,TYPE,NOTIFYDATE,DISPLAY,ACK,EXPIRYDATE) VALUES ('" + detailid + "','MR','" + notifidate + "','INDENT CREATE','N','" + expirydate + "')";

                                    objCmds = new OracleCommand(svSQL, objConn);
                                    objCmds.ExecuteNonQuery();
                                }
                            }
                        }
                    }
                    else
                    {

                        svSQL = "Delete STORESREQDETAIL WHERE STORESREQBASICID='" + cy.ID + "'";
                        OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                        objCmdd.ExecuteNonQuery();

                        foreach (MaterialRequistionItem cp in cy.MRlst)
                        {

                            string desc = datatrans.GetDataString("SELECT ITEMDESC FROM ITEMMASTER where ITEMMASTERID='" + cp.ItemId + "' ");

                            if (cp.Isvalid == "Y" && cp.ItemId != "0")
                            {
                                string unitID = datatrans.GetDataString("Select UNITMASTID from UNITMAST where UNITID='" + cp.UnitID + "' ");
                                string VALMETHOD = datatrans.GetDataString("Select VALMETHOD from ITEMMASTER where ITEMMASTERID='" + cp.ItemId + "' ");


                                svSQL = "Insert into STORESREQDETAIL (STORESREQBASICID,ITEMID,UNIT,QTY,STOCK,NARR,ITEMMASTERID,ITEMDESC,STATUS,STORESREQDETAILROW,ISSQTY,SGCODE,TYPE,VALMETHOD,SCHCLQTY,SCHISSQTY,SCHQTY) VALUES ('" + cy.ID + "','" + cp.ItemId + "','" + unitID + "','" + cp.ReqQty + "','" + cp.ClosingStock + "','" + cy.Narration + "','" + cp.ItemId + "','" + desc + "','OPEN','0','0','0','Stores Issue','" + VALMETHOD + "','" + cp.ReqQty + "','0','0')";
                                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                objCmds.ExecuteNonQuery();

                            }
                        }
                    }
                    objConn.Close();
                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public string MaterialStatus(MaterialRequisition cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                if (cy.status == "CLOSE")
                {
                    using (OracleConnection objConnT = new OracleConnection(_connectionString))
                    {
                        string Sql = string.Empty;
                        svSQL = "UPDATE STORESREQBASIC SET STATUS ='CLOSE' WHERE STORESREQBASICID='" + cy.ID + "'";
                        OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                        objConnT.Open();
                        objCmds.ExecuteNonQuery();
                        foreach(MaterialRequistionItem cp in cy.MRlst)
                        {
                            svSQL = "UPDATE STORESREQDETAIL SET SCHCLQTY ='"+cp.ReqQty+ "' WHERE STORESREQDETAILID='" + cp.detid + "'";
                             objCmds = new OracleCommand(svSQL, objConnT);

                            objCmds.ExecuteNonQuery();
                        }
                      
                        objConnT.Close();
                    }



                }
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public DataTable GetMatStabyID(string MatId)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESREQBASIC.BRANCHID as BRANCHIDS,STORESREQBASIC.FROMLOCID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,PROCESSMAST.PROCESSNAME,WCBASIC.WCID,REQTYPE,DOCID,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,STORESREQBASICID  from STORESREQBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=STORESREQBASIC.WCID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=STORESREQBASIC.PROCESSID LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESREQBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=STORESREQBASIC.FROMLOCID where STORESREQBASICID='" + MatId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetMatStaItemByID(string MatId)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESREQDETAIL.UNIT,ITEMMASTER.ITEMMASTERID,UNITMAST.UNITID,ITEMMASTER.ITEMID,STORESREQDETAIL.QTY,STOCK,STORESREQDETAILID from STORESREQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESREQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESREQDETAIL.UNIT WHERE STORESREQDETAIL.STORESREQBASICID='" + MatId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, string id)
        {

            try
            {

                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE STORESREQBASIC SET IS_ACTIVE ='N' WHERE STORESREQBASICID='" + id + "'";
                    OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                    objConnT.Open();
                    objCmds.ExecuteNonQuery();
                    objConnT.Close();
                }

            }
            catch (Exception ex)
            {
                throw ex;
            }
            return "";

        }

    
        public DataTable GetAllMaterialRequItems(string strStatus, string strfrom, string strTo)
        {
            string SvSql = string.Empty;
            
            if (strStatus == "Y" || strStatus == null && strfrom==null && strTo == null)
            {
                SvSql = " Select BRANCHMAST.BRANCHID,STORESREQBASIC.DOCID,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,LOCDETAILS.LOCID,STORESREQBASIC.PROCESSID,REQTYPE,STORESREQBASIC.STATUS,STORESREQBASICID,STORESREQBASIC.ENTBY,STORESREQBASIC.ENTAT from STORESREQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESREQBASIC.BRANCHID LEFT OUTER JOIN  LOCDETAILS on STORESREQBASIC.FROMLOCID=LOCDETAILS.LOCDETAILSID LEFT OUTER JOIN  WCBASIC on WCBASIC.WCBASICID=STORESREQBASIC.WCID WHERE STORESREQBASIC.DOCDATE >=TRUNC(SYSDATE) - 7  ORDER BY STORESREQBASIC.DOCID DESC";
            }
            else if(strStatus == "Y" || strStatus == null && strfrom != null && strTo != null)
            {
                SvSql = " Select BRANCHMAST.BRANCHID,STORESREQBASIC.DOCID,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,LOCDETAILS.LOCID,STORESREQBASIC.PROCESSID,REQTYPE,STORESREQBASIC.STATUS,STORESREQBASICID,STORESREQBASIC.ENTBY,STORESREQBASIC.ENTAT from STORESREQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESREQBASIC.BRANCHID LEFT OUTER JOIN  LOCDETAILS on STORESREQBASIC.FROMLOCID=LOCDETAILS.LOCDETAILSID LEFT OUTER JOIN  WCBASIC on WCBASIC.WCBASICID=STORESREQBASIC.WCID WHERE STORESREQBASIC.DOCDATE between '" + strfrom+"' and '"+ strTo + "' ORDER BY STORESREQBASIC.DOCID DESC";

            }
            else
            {
                SvSql = "Select BRANCHMAST.BRANCHID,STORESREQBASIC.DOCID,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,WCBASIC.WCID,LOCDETAILS.LOCID,STORESREQBASIC.PROCESSID,REQTYPE,STORESREQBASIC.STATUS,STORESREQBASICID,STORESREQBASIC.ENTBY,STORESREQBASIC.ENTAT from STORESREQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESREQBASIC.BRANCHID LEFT OUTER JOIN  LOCDETAILS on STORESREQBASIC.FROMLOCID=LOCDETAILS.LOCDETAILSID LEFT OUTER JOIN  WCBASIC on WCBASIC.WCBASICID=STORESREQBASIC.WCID WHERE STORESREQBASIC.IS_ACTIVE='N' AND STORESREQBASIC.STATUS NOT IN ('Issued','CLOSE')  ORDER BY STORESREQBASIC.DOCID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllMaterialDetailRequItems(string strStatus)
        {
            string SvSql = string.Empty;
            
                SvSql = " Select QTY,ITEMID,STORESREQBASICID from STORESREQDETAIL where STORESREQBASICID='" + strStatus + "'";
           
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string WholeStockGURD(MaterialRequisition cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                     
                        foreach (StockItem cp in cy.stklst)
                        {


                            if (cp.select == true && cp.itemid != "0")
                            {
                            svSQL = "Insert into INVENTORYITEMREQ(ITEM_ID,REQ_DATE,REQ_QTY,CREATED_BY,CREATED_ON,LOCATION_ID,REQ_LOCID,BRANCH_ID,INVNTORY_ID) VALUES ('" + cp.itemid + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cp.reqqty + "','" + cy.Entered + "','" + DateTime.Now.ToString("dd-MMM-yyyy") + "','" + cp.locationid + "','10001000000827','10001000000001','" + cp.invid + "')";
                                OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                            objConn.Open();
                                objCmds.ExecuteNonQuery();


                            }
                        }
                    

                }

                
            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }

        public DataTable GetAllInventoryReq(string strStatus)
        {
            string SvSql = string.Empty;
            
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = " Select  ITEMMASTER.ITEMID,to_char(INVENTORYITEMREQ.REQ_DATE,'dd-MON-yyyy')REQ_DATE,LOCDETAILS.LOCID,loc.LOCID as location,REQ_QTY,INVENTORYITEMREQ.STATUS,INVENTORYITEMREQID from INVENTORYITEMREQ LEFT OUTER JOIN LOCDETAILS loc ON loc.LOCDETAILSID=INVENTORYITEMREQ.REQ_LOCID LEFT OUTER JOIN  LOCDETAILS on INVENTORYITEMREQ.LOCATION_ID=LOCDETAILS.LOCDETAILSID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=INVENTORYITEMREQ.ITEM_ID WHERE INVENTORYITEMREQ.ISACTIVE='Y' ORDER BY INVENTORYITEMREQ.INVENTORYITEMREQID DESC";
            }
            else
            {
                SvSql = " Select  ITEMMASTER.ITEMID,to_char(INVENTORYITEMREQ.REQ_DATE,'dd-MON-yyyy')REQ_DATE,LOCDETAILS.LOCID,loc.LOCID as location,REQ_QTY,INVENTORYITEMREQ.STATUS,INVENTORYITEMREQID from INVENTORYITEMREQ LEFT OUTER JOIN LOCDETAILS loc ON loc.LOCDETAILSID=INVENTORYITEMREQ.REQ_LOCID LEFT OUTER JOIN  LOCDETAILS on INVENTORYITEMREQ.LOCATION_ID=LOCDETAILS.LOCDETAILSID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=INVENTORYITEMREQ.ITEM_ID WHERE INVENTORYITEMREQ.ISACTIVE='N' ORDER BY INVENTORYITEMREQ.INVENTORYITEMREQID DESC";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetReqMatItemByID(string MatId)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMID,to_char(INVENTORYITEMREQ.REQ_DATE,'dd-MON-yyyy')REQ_DATE,LOCDETAILS.LOCID,INVENTORYITEMREQ.REQ_LOCID,INVNTORY_ID,INVENTORYITEMREQ.LOCATION_ID,INVENTORYITEMREQ.ITEM_ID,loc.LOCID as location,REQ_QTY,INVENTORYITEMREQ.STATUS,INVENTORYITEMREQID,BRANCHMAST.BRANCHID,BRANCH_ID from INVENTORYITEMREQ LEFT OUTER JOIN LOCDETAILS loc ON loc.LOCDETAILSID=INVENTORYITEMREQ.REQ_LOCID LEFT OUTER JOIN  LOCDETAILS on INVENTORYITEMREQ.LOCATION_ID=LOCDETAILS.LOCDETAILSID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=INVENTORYITEMREQ.ITEM_ID LEFT OUTER JOIN BRANCHMAST ON BRANCHMAST.BRANCHMASTID=INVENTORYITEMREQ.BRANCH_ID WHERE INVENTORYITEMREQID='" + MatId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetMRItem(string strfrom, string strTo)
        {
            string SvSql = string.Empty;
            if ( strfrom == null && strTo == null)
            {
                SvSql = " Select SD.QTY,ITEMMASTER.ITEMID,SD.STORESREQBASICID,SD.STORESREQDETAILID ,UNITMAST.UNITID from STORESREQBASIC S,STORESREQDETAIL SD LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SD.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=SD.UNIT WHERE S.DOCDATE >=TRUNC(SYSDATE) - 7";
            }
            else  
            {
                SvSql = " Select SD.QTY,ITEMMASTER.ITEMID,SD.STORESREQBASICID,SD.STORESREQDETAILID ,UNITMAST.UNITID from STORESREQBASIC S,STORESREQDETAIL SD LEFT OUTER JOIN ITEMMASTER ON ITEMMASTER.ITEMMASTERID=SD.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=SD.UNIT WHERE S.STORESREQBASICID=SD.STORESREQBASICID AND S.DOCDATE between '" + strfrom + "' and '" + strTo + "' ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string MaterialReqGURD(MaterialReq cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {

                    
                            svSQL = "UPDATE INVENTORYITEMREQ SET REQ_QTY='"+ cy.reqqty + "' , UPDATED_BY='"+ cy.user + "',UPDATED_ON='" + DateTime.Now.ToString("dd-MMM-yyyy") + "' WHERE INVENTORYITEMREQID='" + cy.ID+"'";
                            OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                            objConn.Open();
                            objCmds.ExecuteNonQuery();

                }


            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
        public string ApproveReqGURD(MaterialReq cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    DataTable inv = datatrans.GetData("Select ITEMMASTER.ITEMID,ITEM_ID,LOCDETAILS.LOCID,GRNID,INVENTORY_ITEM_ID,LOCATION_ID,to_char(GRN_DATE,'dd-MON-yyyy')GRN_DATE,ITEM_ID,BALANCE_QTY from INVENTORY_ITEM left outer join ITEMMASTER ON ITEMMASTERID=INVENTORY_ITEM.ITEM_ID left outer join LOCDETAILS ON LOCDETAILSID=INVENTORY_ITEM.LOCATION_ID where INVENTORY_ITEM_ID='" + cy.invid + "' ");
                    double sqty = cy.reqqty;
                    double iqty = Convert.ToDouble(inv.Rows[0]["BALANCE_QTY"].ToString());
                    double tqry = iqty - sqty;
                    svSQL = "UPDATE INVENTORY_ITEM SET BALANCE_QTY='" + tqry + "'   WHERE INVENTORY_ITEM_ID='" + cy.invid + "'";
                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                    objConn.Open();
                    objCmds.ExecuteNonQuery();
                    DataTable inventory = datatrans.GetData("Select ITEMMASTER.ITEMID,ITEM_ID,LOCDETAILS.LOCID,INVENTORY_ITEM_ID,GRNID,LOCATION_ID,to_char(GRN_DATE,'dd-MON-yyyy')GRN_DATE,ITEM_ID,BALANCE_QTY from INVENTORY_ITEM left outer join ITEMMASTER ON ITEMMASTERID=INVENTORY_ITEM.ITEM_ID left outer join LOCDETAILS ON LOCDETAILSID=INVENTORY_ITEM.LOCATION_ID where ITEM_ID='" + cy.itemid + "' AND LOCATION_ID='10001000000827' AND BRANCH_ID='" + cy.branchid + "' ");
                    double pqty = cy.reqqty;
                    double aqty = Convert.ToDouble(inventory.Rows[0]["BALANCE_QTY"].ToString());
                    double kqry = aqty + pqty;
                    svSQL = "UPDATE INVENTORY_ITEM SET BALANCE_QTY='" + kqry + "'  WHERE INVENTORY_ITEM_ID='" + inventory.Rows[0]["INVENTORY_ITEM_ID"].ToString() + "'";
                    OracleCommand objCmdsa = new OracleCommand(svSQL, objConn);
            
                    objCmdsa.ExecuteNonQuery();

                    OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConn);
                    objCmdIn.CommandType = CommandType.StoredProcedure;
                    objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cy.itemid;
                    objCmdIn.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = "0";
                    objCmdIn.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmdIn.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = inv.Rows[0]["GRNID"].ToString();
                    objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = inv.Rows[0]["INVENTORY_ITEM_ID"].ToString();
                    objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "MATREQ";
                    objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                    objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = tqry;
                    objCmdIn.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "MATREQ";
                    objCmdIn.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                    objCmdIn.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                    objCmdIn.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                    objCmdIn.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmdIn.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = inv.Rows[0]["LOCATION_ID"].ToString(); 
                    objCmdIn.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.branchid;
                    objCmdIn.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                    objCmdIn.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                    objCmdIn.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                    objCmdIn.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                    objCmdIn.ExecuteNonQuery();

                    OracleCommand objCmdI = new OracleCommand("INVITEMTRANSPROC", objConn);
                    objCmdI.CommandType = CommandType.StoredProcedure;
                    objCmdI.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                    objCmdI.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cy.itemid;
                    objCmdI.Parameters.Add("TSOURCEID", OracleDbType.NVarchar2).Value = "0";
                    objCmdI.Parameters.Add("TSOURCEBASICID", OracleDbType.NVarchar2).Value = cy.ID;
                    objCmdI.Parameters.Add("GRNID", OracleDbType.NVarchar2).Value = inventory.Rows[0]["GRNID"].ToString();
                    objCmdI.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = inventory.Rows[0]["INVENTORY_ITEM_ID"].ToString();
                    objCmdI.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "MATREQ";
                    objCmdI.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                    objCmdI.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = kqry;
                    objCmdI.Parameters.Add("TRANS_NOTES", OracleDbType.NVarchar2).Value = "MATREQ";
                    objCmdI.Parameters.Add("TRANS_DATE", OracleDbType.Date).Value = DateTime.Now;
                    objCmdI.Parameters.Add("FINANCIAL_YEAR", OracleDbType.NVarchar2).Value = datatrans.GetFinancialYear(DateTime.Now);
                    objCmdI.Parameters.Add("CREATED_BY", OracleDbType.NVarchar2).Value = "1"; /*HttpContext.*/
                    objCmdI.Parameters.Add("CREATED_ON", OracleDbType.Date).Value = DateTime.Now;
                    objCmdI.Parameters.Add("LOCATION_ID", OracleDbType.NVarchar2).Value = "10001000000827";
                    objCmdI.Parameters.Add("BRANCH_ID", OracleDbType.NVarchar2).Value = cy.branchid;
                    objCmdI.Parameters.Add("DRUM_NO", OracleDbType.NVarchar2).Value = "";
                    objCmdI.Parameters.Add("RATE", OracleDbType.NVarchar2).Value = "0";
                    objCmdI.Parameters.Add("AMOUNT", OracleDbType.NVarchar2).Value = "0";
                    objCmdI.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = "Insert";
                    objCmdI.ExecuteNonQuery();

                    svSQL = "UPDATE INVENTORYITEMREQ SET APPROVED_BY='"+cy.user+"',STATUS='Approved'   WHERE INVENTORYITEMREQID='" + cy.ID + "'";
                    OracleCommand objCmdss = new OracleCommand(svSQL, objConn);
            
                    objCmdss.ExecuteNonQuery();
                }


            }
            catch (Exception ex)
            {
                msg = "Error Occurs, While inserting / updating Data";
                throw ex;
            }

            return msg;
        }
    }
}
