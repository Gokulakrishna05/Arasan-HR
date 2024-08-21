using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services
{
    public class ActivitiesCOService : IActivitiesCO
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public ActivitiesCOService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }

        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMASTID,BRANCHID,IS_ACTIVE from BRANCHMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCDETAILSID,LOCID,IS_ACTIVE from LOCDETAILS  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetActivity()
        {
            string SvSql = string.Empty;
            SvSql = "Select SCHMAINBASICID,DOCID,IS_ACTIVE from SCHMAINBASIC  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetMachine()
        {
            string SvSql = string.Empty;
            SvSql = "Select MACHINEINFOBASICID,MCODE,IS_ACTIVE from MACHINEINFOBASIC  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTERID,ITEMID,IS_ACTIVE from ITEMMASTER  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmplId()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPMASTID,EMPID  from EMPMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetParty()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYID,PARTYMASTID  from PARTYMAST  WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public string ActivitiesCOCRUD(ActivitiesCO cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("ACTCOPROC", objConn);


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

                    objCmd.Parameters.Add("BRANCHID", OracleDbType.NVarchar2).Value = cy.Brc;
                    objCmd.Parameters.Add("LOCID", OracleDbType.NVarchar2).Value = cy.Loc;
                    objCmd.Parameters.Add("ENTRYTYPE", OracleDbType.NVarchar2).Value = cy.EnTy;
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Docno;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.DocDate;
                    objCmd.Parameters.Add("ACTIVITYID", OracleDbType.NVarchar2).Value = cy.ActId;
                    objCmd.Parameters.Add("REFDATE", OracleDbType.NVarchar2).Value = cy.AcDat;
                    objCmd.Parameters.Add("CFROMTIME", OracleDbType.NVarchar2).Value = cy.FrTim;
                    objCmd.Parameters.Add("CTOTIME", OracleDbType.NVarchar2).Value = cy.ToTim;
                    objCmd.Parameters.Add("CDATE", OracleDbType.NVarchar2).Value = cy.FrDat;
                    objCmd.Parameters.Add("SFROMTIME", OracleDbType.NVarchar2).Value = cy.FrTimTy;
                    objCmd.Parameters.Add("CARRIEDHRS", OracleDbType.NVarchar2).Value = cy.CaHrs;
                    objCmd.Parameters.Add("CTODATE", OracleDbType.NVarchar2).Value = cy.ToDat;
                    objCmd.Parameters.Add("STOTIME", OracleDbType.NVarchar2).Value = cy.ToTimTy;
                    objCmd.Parameters.Add("PREORBER", OracleDbType.NVarchar2).Value = cy.PB;
                    objCmd.Parameters.Add("MCCODE", OracleDbType.NVarchar2).Value = cy.MtId;
                    objCmd.Parameters.Add("DTYPE", OracleDbType.NVarchar2).Value = cy.DeTyp;
                    objCmd.Parameters.Add("MTYPE", OracleDbType.NVarchar2).Value = cy.MaTyp;
                    objCmd.Parameters.Add("MPREORBRE", OracleDbType.NVarchar2).Value = cy.PrePB;
                    objCmd.Parameters.Add("MCSFRUNHR", OracleDbType.NVarchar2).Value = cy.SfHr;
                    objCmd.Parameters.Add("RUNHRYN", OracleDbType.NVarchar2).Value = cy.MhYN;
                    objCmd.Parameters.Add("ACTIVITY", OracleDbType.NVarchar2).Value = cy.Plac;
                    objCmd.Parameters.Add("ACTIVIYDONE", OracleDbType.NVarchar2).Value = cy.Acdo;

                    objCmd.Parameters.Add("CAMOUNT", OracleDbType.NVarchar2).Value = cy.Conam;
                    objCmd.Parameters.Add("OTHERAMT", OracleDbType.NVarchar2).Value = cy.Oth;
                    objCmd.Parameters.Add("TOTALAMT", OracleDbType.NVarchar2).Value = cy.Tamo;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Rem;
                    objCmd.Parameters.Add("CAPAMT", OracleDbType.NVarchar2).Value = cy.Camo;
                    objCmd.Parameters.Add("SAMOUNT", OracleDbType.NVarchar2).Value = cy.Samo;

                    objCmd.Parameters.Add("StatementType", OracleDbType.NVarchar2).Value = StatementType;
                    objCmd.Parameters.Add("OUTID", OracleDbType.Int64).Direction = ParameterDirection.Output;
                    try
                    {
                        objConn.Open();
                        objCmd.ExecuteNonQuery();
                        Object Pid = objCmd.Parameters["OUTID"].Value;
                        //string Pid = "0";
                        if (cy.ID != null)
                        {
                            Pid = cy.ID;
                        }
                        if (cy.ConLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Cons cp in cy.ConLst)
                                {
                                    if (cp.Isvalid1 == "Y" && cp.Itm != "")
                                    {

                                        svSQL = "Insert into ACTCARRIEDDETAIL (ACTCARRIEDHEADID,ITEMID,UNIT,CONSYN,CLSTK,QTY,RATE,AMOUNT) VALUES ('" + Pid + "','" + cp.Itm + "','" + cp.Unit + "','" + cp.Active + "','" + cp.Curst + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amo + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete ACTCARRIEDDETAIL WHERE ACTCARRIEDHEADID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Cons cp in cy.ConLst)
                                {
                                    if (cp.Isvalid1 == "Y" && cp.Itm != "")
                                    {

                                        svSQL = "Insert into ACTCARRIEDDETAIL (ACTCARRIEDHEADID,ITEMID,UNIT,CONSYN,CLSTK,QTY,RATE,AMOUNT) VALUES ('" + cy.ID + "','" + cp.Itm + "','" + cp.Unit + "','" + cp.Active + "','" + cp.Curst + "','" + cp.Qty + "','" + cp.Rate + "','" + cp.Amo + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                        if (cy.EmpLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Emp cp in cy.EmpLst)
                                {
                                    if (cp.Isvalid2 == "Y" && cp.EmplId != "")
                                    {

                                        svSQL = "Insert into EMPACTDET (ACTCARRIEDHEADID,EMPID,EMPNAME,EMPDEPT,ENHRS,OTHRS,EMPWHRS,EMPCOST) VALUES ('" + Pid + "','" + cp.EmplId + "','" + cp.EmpName + "','" + cp.Edes + "','" + cp.Nhr + "','" + cp.Ohr + "','" + cp.Whr + "','" + cp.Ecost + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete EMPACTDET WHERE ACTCARRIEDHEADID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Emp cp in cy.EmpLst)
                                {
                                    if (cp.Isvalid2 == "Y" && cp.EmplId != "")
                                    {

                                        svSQL = "Insert into EMPACTDET (ACTCARRIEDHEADID,EMPID,EMPNAME,EMPDEPT,ENHRS,OTHRS,EMPWHRS,EMPCOST) VALUES ('" + cy.ID + "','" + cp.EmplId + "','" + cp.EmpName + "','" + cp.Edes + "','" + cp.Nhr + "','" + cp.Ohr + "','" + cp.Whr + "','" + cp.Ecost + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        if (cy.SerLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Ser cp in cy.SerLst)
                                {
                                    if (cp.Isvalid3 == "Y" && cp.Pid != "")
                                    {

                                        svSQL = "Insert into ACTSERVICEDET (ACTCARRIEDHEADID,PARTYID,SERDESC,SERQTY,SERRATE,SERAMOUNT) VALUES ('" + Pid + "','" + cp.Pid + "','" + cp.Sedec + "','" + cp.SQty + "','" + cp.SRate + "','" + cp.SAmo + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete ACTSERVICEDET WHERE ACTCARRIEDHEADID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Ser cp in cy.SerLst)
                                {
                                    if (cp.Isvalid3 == "Y" && cp.Pid != "")
                                    {

                                        svSQL = "Insert into ACTSERVICEDET (ACTCARRIEDHEADID,PARTYID,SERDESC,SERQTY,SERRATE,SERAMOUNT) VALUES ('" + cy.ID + "','" + cp.Pid + "','" + cp.Sedec + "','" + cp.SQty + "','" + cp.SRate + "','" + cp.SAmo + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                        }

                        if (cy.ChkLst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Chk cp in cy.ChkLst)
                                {
                                    if (cp.Isvalid4 == "Y" && cp.Ser != "")
                                    {

                                        svSQL = "Insert into CHECKLIST (ACTCARRIEDHEADID,SERVICE) VALUES ('" + Pid + "','" + cp.Ser + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }

                            }
                            else
                            {
                                svSQL = "Delete CHECKLIST WHERE ACTCARRIEDHEADID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Chk cp in cy.ChkLst)
                                {
                                    if (cp.Isvalid4 == "Y" && cp.Ser != "")
                                    {

                                        svSQL = "Insert into CHECKLIST (ACTCARRIEDHEADID,SERVICE) VALUES ('" + cy.ID + "','" + cp.Ser + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();
                                    }
                                }
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        //System.Console.WriteLine("Exception: {0}", ex.ToString());
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

        public DataTable GetAllActivitiesCO(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "SELECT ACTCARRIEDHEAD.DOCID,to_char(ACTCARRIEDHEAD.DOCDATE,'dd-MON-yyyy')DOCDATE,SCHMAINBASIC.DOCID ACTID,LOCDETAILS.LOCID,ACTCARRIEDHEAD.ENTRYTYPE,ACTCARRIEDHEAD.IS_ACTIVE,ACTCARRIEDHEAD.ACTCARRIEDHEADID FROM ACTCARRIEDHEAD left outer join SCHMAINBASIC ON SCHMAINBASICID=ACTCARRIEDHEAD.ACTIVITYID left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=ACTCARRIEDHEAD.LOCID WHERE ACTCARRIEDHEAD.IS_ACTIVE = 'Y' ";

            }
            else
            {
                SvSql = "SELECT ACTCARRIEDHEAD.DOCID,to_char(ACTCARRIEDHEAD.DOCDATE,'dd-MON-yyyy')DOCDATE,SCHMAINBASIC.DOCID ACTID,LOCDETAILS.LOCID,ACTCARRIEDHEAD.ENTRYTYPE,ACTCARRIEDHEAD.IS_ACTIVE,ACTCARRIEDHEAD.ACTCARRIEDHEADID FROM ACTCARRIEDHEAD left outer join SCHMAINBASIC ON SCHMAINBASICID=ACTCARRIEDHEAD.ACTIVITYID left outer join LOCDETAILS on LOCDETAILS.LOCDETAILSID=ACTCARRIEDHEAD.LOCID WHERE ACTCARRIEDHEAD.IS_ACTIVE = 'N' ";

            }

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditActivityCo(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHID,LOCID,ENTRYTYPE,DOCID,to_char(DOCDATE,'dd-MON-yyyy')DOCDATE,ACTIVITYID,CFROMTIME,to_char(REFDATE,'dd-MON-yyyy')REFDATE,CTOTIME,SFROMTIME,CARRIEDHRS,STOTIME,PREORBER,MCCODE,DTYPE, MTYPE, to_char(CDATE,'dd-MON-yyyy')CDATE,to_char(CTODATE,'dd-MON-yyyy')CTODATE,MPREORBRE, MCSFRUNHR, RUNHRYN, ACTIVITY,ACTIVIYDONE,CAMOUNT,OTHERAMT,TOTALAMT,REMARKS,CAPAMT,SAMOUNT from ACTCARRIEDHEAD where ACTCARRIEDHEADID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditCons(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,UNIT,CONSYN,CLSTK,QTY,RATE,AMOUNT from ACTCARRIEDDETAIL where ACTCARRIEDHEADID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditEmp(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select EMPID,EMPNAME,EMPDEPT,ENHRS,OTHRS,EMPWHRS,EMPCOST from EMPACTDET where ACTCARRIEDHEADID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEditSerdetail(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select PARTYID,SERDESC,SERQTY,SERRATE,SERAMOUNT from ACTSERVICEDET where ACTCARRIEDHEADID = '" + id + "' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEditChkl(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select SERVICE from CHECKLIST where ACTCARRIEDHEADID = '" + id + "' ";
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
                    svSQL = "UPDATE ACTCARRIEDHEAD SET IS_ACTIVE ='N' WHERE ACTCARRIEDHEADID='" + id + "'";
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
        public string RemoveChange(string tag, string id)
        {

            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {
                    svSQL = "UPDATE ACTCARRIEDHEAD SET IS_ACTIVE ='Y' WHERE ACTCARRIEDHEADID='" + id + "'";
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
