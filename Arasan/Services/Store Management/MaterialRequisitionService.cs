using Arasan.Interface;
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using Org.BouncyCastle.Asn1;
using System;
using System.Collections.Generic;
using System.Data;
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
            SvSql = "Select WCID,WCBASICID from WCBASIC where ILOCATION='" + value +"' ";
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
            SvSql = "Select STORESREQDETAIL.UNIT,ITEMMASTER.ITEMMASTERID,UNITMAST.UNITID,STORESREQDETAIL.ITEMID,STORESREQDETAIL.QTY from STORESREQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESREQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESREQDETAIL.UNIT WHERE STORESREQDETAIL.STORESREQBASICID='" + id + "'";
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
        public DataTable GetItemGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMID,ITEMGROUP FROM ITEMMASTER where ITEMMASTERID='"+ id +"' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItem(string value)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,ITEMMASTERID from ITEMMASTER";
            if (!string.IsNullOrEmpty(value) && value != "0")
            {
                SvSql += " Where ITEMGROUP='" + value + "'";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLocation(string id)
        {
            string SvSql = string.Empty;
            SvSql = " select locdetails.LOCID ,EMPLOYEELOCATION.LOCID loc from EMPLOYEELOCATION  left outer join locdetails on locdetails.locdetailsid=EMPLOYEELOCATION.LOCID where EMPID='" + id +"' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetMatbyID(string MatId)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESREQBASIC.BRANCHID as BRANCHIDS,STORESREQBASIC.FROMLOCID,BRANCHMAST.BRANCHID,LOCDETAILS.LOCID,PROCESSMAST.PROCESSNAME,WCBASIC.WCID,REQTYPE,DOCID,to_char(STORESREQBASIC.DOCDATE,'dd-MON-yyyy') DOCDATE,STORESREQBASICID  from STORESREQBASIC LEFT OUTER JOIN WCBASIC ON WCBASICID=STORESREQBASIC.WCID  LEFT OUTER JOIN PROCESSMAST ON PROCESSMASTID=STORESREQBASIC.PROCESSID LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESREQBASIC.BRANCHID LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=STORESREQBASIC.FROMLOCID where STORESREQBASICID='" + MatId +"'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetMatItemByID(string MatId)
        {
            string SvSql = string.Empty;
            SvSql = "Select STORESREQDETAILID,STORESREQDETAIL.UNIT,ITEMMASTER.ITEMMASTERID,UNITMAST.UNITID,ITEMMASTER.ITEMID,STORESREQDETAIL.QTY from STORESREQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESREQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESREQDETAIL.UNIT WHERE STORESREQDETAIL.STORESREQBASICID='" + MatId + "'";
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
        public IEnumerable<MaterialRequisition> GetAllMaterial()
        {
            List<MaterialRequisition> cmpList = new List<MaterialRequisition>();
           using (OracleConnection con = new OracleConnection(_connectionString))
           {

                using (OracleCommand cmd = con.CreateCommand())
                {
                   con.Open();
                   cmd.CommandText = "Select BRANCHMAST.BRANCHID,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,LOCDETAILS.LOCID,PROCESSID,REQTYPE,STORESREQBASICID,STATUS from STORESREQBASIC LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=STORESREQBASIC.BRANCHID LEFT OUTER JOIN  LOCDETAILS on STORESREQBASIC.FROMLOCID=LOCDETAILS.LOCDETAILSID where STATUS='OPEN'";
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
                            DocDa = rdr["DOCDATE"].ToString(),
                           status= rdr["STATUS"].ToString()


                        };
                        cmpList.Add(cmp);
                    }
               }
            }
           return cmpList;
        }

        public DataTable Getstkqty(string ItemId,string locid, string brid)
        {
            string SvSql = string.Empty;
            SvSql = "select SUM(BALANCE_QTY) as QTY from INVENTORY_ITEM where BALANCE_QTY > 0 AND LOCATION_ID='" + locid + "' AND BRANCH_ID='" + brid + "' AND ITEM_ID='"+ ItemId + "'";
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
                                DateTime theDate = DateTime.Now;
                                DateTime todate; DateTime fromdate;
                                string t; string f;
                                if (DateTime.Now.Month >= 4)
                                {
                                    todate = theDate.AddYears(1);
                                }
                                else
                                {
                                    todate = theDate;
                                }
                                if (DateTime.Now.Month >= 4)
                                {
                                    fromdate = theDate;
                                }
                                else
                                {
                                    fromdate = theDate.AddYears(-1);
                                }
                                t = todate.ToString("yy");
                                f = fromdate.ToString("yy");
                                string disp = string.Format("{0}-{1}", f, t);

                                int idc = datatrans.GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'PI' AND IS_ACTIVE = 'Y'");
                                string Indentno = string.Format("{0} - {1} / {2}", "IND", (idc + 1).ToString(), disp);

                                string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='PI' AND IS_ACTIVE ='Y'";
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
                                objCmd.Parameters.Add("IndentNo", OracleDbType.NVarchar2).Value = Indentno;
                                objCmd.Parameters.Add("IndentDate", OracleDbType.Date).Value = DateTime.Now;
                                objCmd.Parameters.Add("RefDate", OracleDbType.Date).Value = DateTime.Now;
                                objCmd.Parameters.Add("Erecation", OracleDbType.NVarchar2).Value = "";
                                objCmd.Parameters.Add("PurchaseType", OracleDbType.NVarchar2).Value ="";
                                objCmd.Parameters.Add("EnterBy", OracleDbType.NVarchar2).Value = "10032000091118";
                                objCmd.Parameters.Add("EnterDate", OracleDbType.Date).Value = DateTime.Now;
                                objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                                objConn.Open();
                                objCmd.ExecuteNonQuery();
                                Object Pid = objCmd.Parameters["OUTID"].Value;

                                OracleCommand objCmds = new OracleCommand("PIDETAILPROC", objConn);
                                StatementType = "Insert";
                                objCmds.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                objCmds.CommandType = CommandType.StoredProcedure;
                                objCmds.Parameters.Add("PIID", OracleDbType.NVarchar2).Value = Pid;
                                objCmds.Parameters.Add("ITEMIDS", OracleDbType.NVarchar2).Value = cp.ItemId;
                                objCmds.Parameters.Add("QUANTITY", OracleDbType.NVarchar2).Value = cp.IndQty;
                                objCmds.Parameters.Add("UNITP", OracleDbType.NVarchar2).Value = cp.UnitID;
                                objCmds.Parameters.Add("QC", OracleDbType.NVarchar2).Value = "";
                                objCmds.Parameters.Add("NARRATION", OracleDbType.NVarchar2).Value = "";
                                objCmds.Parameters.Add("DUE_DATE", OracleDbType.Date).Value = DateTime.Now;
                                objCmds.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.LocationId;
                                objCmds.Parameters.Add("ITEMGROUPID", OracleDbType.NVarchar2).Value = cp.ItemGroupId;
                                objCmds.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                                objCmds.ExecuteNonQuery();

                                /////////////////////////Indent creation
                            }

                        }
                    }

                    svSQL = "UPDATE STORESREQBASIC SET STATUS ='CLOSE' WHERE STORESREQBASICID='" + cy.MaterialReqId + "'";
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
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    foreach (MaterialRequistionItem cp in cy.MRlst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {

                            if (cp.InvQty > 0)
                            {
                                /////////////////////////Inventory Update

                                double qty = cp.InvQty;
                                DataTable dt = datatrans.GetData("Select INVENTORY_ITEM.BALANCE_QTY,INVENTORY_ITEM.ITEM_ID,INVENTORY_ITEM.LOCATION_ID,INVENTORY_ITEM.BRANCH_ID,INVENTORY_ITEM_ID,GRN_ID,GRN_DATE from INVENTORY_ITEM where INVENTORY_ITEM.ITEM_ID='" + cp.ItemId + "' AND INVENTORY_ITEM.LOCATION_ID='" + cy.LocationId + "' and INVENTORY_ITEM.BRANCH_ID='" + cy.BranchId + "' and BALANCE_QTY!=0 order by GRN_DATE ASC");
                                if (dt.Rows.Count > 0)
                                {
                                    for (int i = 0; i < dt.Rows.Count; i++)
                                    {
                                        double rqty = Convert.ToDouble(dt.Rows[i]["BALANCE_QTY"].ToString());
                                        if (rqty >= qty)
                                        {
                                            double bqty = rqty - qty;
                                            using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                            {
                                                string Sql = string.Empty;
                                                Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + bqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                OracleCommand objCmdss = new OracleCommand(Sql, objConnT);
                                                objConnT.Open();
                                                objCmdss.ExecuteNonQuery();

                                                OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConnT);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRN_ID"].ToString();
                                                objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "MREQ";
                                                objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                                objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = bqty;
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
                                                objConnT.Close();



                                            }
                                            break;
                                        }
                                        else
                                        {
                                            qty = qty - rqty;
                                            using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                            {
                                                string Sql = string.Empty;
                                                Sql = "Update INVENTORY_ITEM SET  BALANCE_QTY='" + rqty + "' WHERE INVENTORY_ITEM_ID='" + dt.Rows[i]["INVENTORY_ITEM_ID"].ToString() + "'";
                                                OracleCommand objCmdss = new OracleCommand(Sql, objConnT);
                                                objConnT.Open();
                                                objCmdss.ExecuteNonQuery();


                                                OracleCommand objCmdIn = new OracleCommand("INVITEMTRANSPROC", objConnT);
                                                objCmdIn.CommandType = CommandType.StoredProcedure;
                                                objCmdIn.Parameters.Add("ID", OracleDbType.NVarchar2).Value = DBNull.Value;
                                                objCmdIn.Parameters.Add("INVENTORY_ITEM_ID", OracleDbType.NVarchar2).Value = cp.ItemId;
                                                objCmdIn.Parameters.Add("GRN_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["GRN_ID"].ToString();
                                                objCmdIn.Parameters.Add("ITEM_ID", OracleDbType.NVarchar2).Value = dt.Rows[i]["INVENTORY_ITEM_ID"].ToString();
                                                objCmdIn.Parameters.Add("TRANS_TYPE", OracleDbType.NVarchar2).Value = "MREQ";
                                                objCmdIn.Parameters.Add("TRANS_IMPACT", OracleDbType.NVarchar2).Value = "O";
                                                objCmdIn.Parameters.Add("TRANS_QTY", OracleDbType.NVarchar2).Value = rqty;
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
                                                objConnT.Close();
                                            }
                                        }
                                    }
                                }
                                /////////////////////////Inventory Update
                            }
                            if (cp.IndQty > 0)
                            {
                                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                                {
                                    string Sql = string.Empty;
                                    svSQL = "UPDATE STORESREQDETAIL SET PENDING_QTY ='" + cp.IndQty + "' WHERE STORESREQDETAILID='" + cp.indentid + "'";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objConnT);
                                    objConnT.Open();
                                    objCmds.ExecuteNonQuery();
                                    objConnT.Close();
                                }
                            }

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
        public string MaterialCRUD(MaterialRequisition cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);
                DateTime theDate = DateTime.Now;
                DateTime todate; DateTime fromdate;
                string t; string f;
                if (DateTime.Now.Month >= 4)
                {
                    todate = theDate.AddYears(1);
                }
                else
                {
                    todate = theDate;
                }
                if (DateTime.Now.Month >= 4)
                {
                    fromdate = theDate;
                }
                else
                {
                    fromdate = theDate.AddYears(-1);
                }
                t = todate.ToString("yy");
                f = fromdate.ToString("yy");
                string disp = string.Format("{0}-{1}", f, t);

                int idc = datatrans.GetDataId(" SELECT COMMON_TEXT FROM COMMON_MASTER WHERE COMMON_TYPE = 'MAT' AND IS_ACTIVE = 'Y'");
                string MATNo = string.Format("{0} - {1} / {2}", "MAT", (idc + 1).ToString(), disp);

                string updateCMd = " UPDATE COMMON_MASTER SET COMMON_TEXT ='" + (idc + 1).ToString() + "' WHERE COMMON_TYPE ='MAT' AND IS_ACTIVE ='Y'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = MATNo;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDa;
                    objCmd.Parameters.Add("FROMLOCID", OracleDbType.NVarchar2).Value = cy.Location;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.WorkCenter;
                    objCmd.Parameters.Add("PROCESSID", OracleDbType.NVarchar2).Value = cy.Process;
                    objCmd.Parameters.Add("REQTYPE", OracleDbType.NVarchar2).Value = cy.RequestType;
                    objCmd.Parameters.Add("STATUS", OracleDbType.NVarchar2).Value = "OPEN";
                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    objConn.Open();
                    objCmd.ExecuteNonQuery();
                    Object reqid = objCmd.Parameters["OUTID"].Value;
                    foreach (MaterialRequistionItem cp in cy.MRlst)
                    {
                        if (cp.Isvalid == "Y" && cp.ItemId != "0")
                        {
                            svSQL = "Insert into STORESREQDETAIL (STORESREQBASICID,ITEMID,UNIT,QTY,STOCK,NARR) VALUES ('" + reqid + "','"+ cp.ItemId  + "','"+ cp.UnitID +"','"+ cp.ReqQty + "','"+ cp.ClosingStock + "','"+ cy.Narration + "')";
                            OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                            objCmds.ExecuteNonQuery();
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
            SvSql = "Select STORESREQDETAIL.UNIT,ITEMMASTER.ITEMMASTERID,UNITMAST.UNITID,ITEMMASTER.ITEMID,STORESREQDETAIL.QTY from STORESREQDETAIL LEFT OUTER JOIN ITEMMASTER on ITEMMASTER.ITEMMASTERID=STORESREQDETAIL.ITEMID LEFT OUTER JOIN UNITMAST ON UNITMAST.UNITMASTID=STORESREQDETAIL.UNIT WHERE STORESREQDETAIL.STORESREQBASICID='" + MatId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string StatusChange(string tag, int id)
        {

            try
            {

                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE STORESREQBASIC SET STATUS ='CLOSE' WHERE STORESREQBASICID='" + id + "'";
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

    }
}
