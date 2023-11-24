using Arasan.Interface;
 
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using Oracle.ManagedDataAccess.Client;
using System;
using System.Collections.Generic;
using System.Data;
namespace Arasan.Services
{
    public class PackingEntryService :IPackingEntry
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public PackingEntryService(IConfiguration _configuratio)
        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }
        public DataTable GetPackNote()
        {
            string SvSql = string.Empty;
            SvSql = "Select PACKNOTEBASICID,DOCID from PACKNOTEBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetNoteDetail(string Note)
        {
            string SvSql = string.Empty;
            SvSql = "Select to_char(PACKNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(PACKNOTEBASIC.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(PACKNOTEBASIC.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,TOLOCDETAILSID,TOTHRS,PACKNOTEBASIC.OITEMID,PACKNOTEBASIC.WCID as work,PSBASIC.DOCID,WCBASIC.WCID,ITEMMASTER.ITEMID,PACKNOTEBASIC.SHIFT,SHIFTMAST.SHIFTNO,PACKNOTEBASIC.PSCHNO,PACKCONSYN,PACKNOTEBASICID from PACKNOTEBASIC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PACKNOTEBASIC.OITEMID LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID LEFT OUTER JOIN PSBASIC ON PSBASICID=PACKNOTEBASIC.PSCHNO LEFT OUTER JOIN SHIFTMAST ON SHIFTMASTID=PACKNOTEBASIC.SHIFT WHERE PACKNOTEBASICID='" + Note +"'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetPackDetails(string Note)
        {
            string SvSql = string.Empty;
            SvSql = "Select PACKNOTEINPDETAIL.IDRUMNO,DRUMMAST.DRUMNO,PACKNOTEINPDETAIL.IBATCHNO,IBATCHQTY,COMBNO,IQTY,IRATE,IAMOUNT,PACKNOTEINPDETAILID from PACKNOTEINPDETAIL LEFT OUTER JOIN DRUMMAST ON DRUMMASTID=PACKNOTEINPDETAIL.IDRUMNO WHERE PACKNOTEBASICID='" + Note + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackOutDetails(string Note)
        {
            string SvSql = string.Empty;
            SvSql = "Select PDADETAILID,PDABASICID,TDRUMNO,DRUMNO,TSTATUS from PDADETAIL  WHERE PDABASICID='" + Note + "' And TSTATUS='N' ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem()
        {
            string SvSql = string.Empty; 
            SvSql = "select ITEMMASTERID,ITEMID from ITEMMASTER WHERE IGROUP IN ('Other Consumables','Consumables')";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetMachine()
        {
            string SvSql = string.Empty;
            SvSql = "Select MNAME,MCODE,MACHINEINFOBASICID from MACHINEINFOBASIC ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string PackingEntryCRUD(PackingEntry cy)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";
                datatrans = new DataTransactions(_connectionString);


                int idc = datatrans.GetDataId(" SELECT LASTNO FROM SEQUENCE WHERE PREFIX = 'PACK' AND ACTIVESEQUENCE = 'T'");
                string docid = string.Format("{0}{1}", "PACK", (idc + 1).ToString());

                string updateCMd = " UPDATE SEQUENCE SET LASTNO ='" + (idc + 1).ToString() + "' WHERE PREFIX ='PACK' AND ACTIVESEQUENCE ='T'";
                try
                {
                    datatrans.UpdateStatus(updateCMd);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                cy.Docid = docid;
                string[] sdateList = cy.startdate.Split(" - ");
                string sdate = "";
                string stime = "";
                if (sdateList.Length > 0)
                {
                    sdate = sdateList[0];
                    stime = sdateList[1];
                }
                string[] edateList = cy.enddate.Split(" - ");
                string endate = "";
                string endtime = "";
                if (sdateList.Length > 0)
                {
                    endate = edateList[0];
                    endtime = edateList[1];
                }
                string user = datatrans.GetDataString("Select USERID from EMPMAST where EMPMASTID='" + cy.user + "' ");
                string loc = datatrans.GetDataString("Select ILOCATION from WCBASIC where WCBASICID='" + cy.Work + "' ");
               
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    OracleCommand objCmd = new OracleCommand("PACKINGENTRYPROC", objConn);
                    /*objCmd.Connection = objConn;
                    objCmd.CommandText = "DIRECTPURCHASEPROC";*/

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
                    objCmd.Parameters.Add("DOCID", OracleDbType.NVarchar2).Value = cy.Docid;
                    objCmd.Parameters.Add("DOCDATE", OracleDbType.NVarchar2).Value = cy.Docdate;
                    objCmd.Parameters.Add("WCID", OracleDbType.NVarchar2).Value = cy.Work;
                   
                    objCmd.Parameters.Add("WCBASICID", OracleDbType.NVarchar2).Value = cy.Work;
                    objCmd.Parameters.Add("PNDATE", OracleDbType.NVarchar2).Value = cy.NoteDate;
                    objCmd.Parameters.Add("PSCHNO", OracleDbType.NVarchar2).Value = cy.Schid;
                    objCmd.Parameters.Add("SHIFT", OracleDbType.NVarchar2).Value = cy.Shiftid;
                    objCmd.Parameters.Add("OITEMID", OracleDbType.NVarchar2).Value = cy.Itemid;
                    objCmd.Parameters.Add("STARTDATE", OracleDbType.NVarchar2).Value = sdate;
                    objCmd.Parameters.Add("ENDDATE", OracleDbType.NVarchar2).Value = endate;
                    objCmd.Parameters.Add("STARTTIME", OracleDbType.NVarchar2).Value = stime;
                    objCmd.Parameters.Add("ENDTIME", OracleDbType.NVarchar2).Value = endtime;
                    objCmd.Parameters.Add("LOCDETAILSID", OracleDbType.NVarchar2).Value = loc;
                    objCmd.Parameters.Add("TOTISSQTY", OracleDbType.NVarchar2).Value = cy.Totinpqty;
                    objCmd.Parameters.Add("ISSRATE", OracleDbType.NVarchar2).Value = cy.Totinprate;
                    objCmd.Parameters.Add("ISSAMT", OracleDbType.NVarchar2).Value = cy.totalisspamt;
                    objCmd.Parameters.Add("TOTOPQTY", OracleDbType.NVarchar2).Value = cy.Totoutqty;
                    objCmd.Parameters.Add("OPRATE", OracleDbType.NVarchar2).Value = cy.Totoutrate;
                    objCmd.Parameters.Add("OPAMOUNT", OracleDbType.NVarchar2).Value = cy.Totoutamount;
                    objCmd.Parameters.Add("PACKINGNOTE", OracleDbType.NVarchar2).Value = cy.Packnote;
                    objCmd.Parameters.Add("ENTEREDBY", OracleDbType.NVarchar2).Value = user;
                    objCmd.Parameters.Add("REMARKS", OracleDbType.NVarchar2).Value = cy.Remark;
                    objCmd.Parameters.Add("USERID", OracleDbType.NVarchar2).Value = user;
                    objCmd.Parameters.Add("TOTALCAMOUNT", OracleDbType.NVarchar2).Value = cy.TotConamount;
                    objCmd.Parameters.Add("PSBASICID", OracleDbType.NVarchar2).Value = cy.Schid;
                    objCmd.Parameters.Add("TOTALIAMOUNT", OracleDbType.NVarchar2).Value = cy.totalinpamt;
                    objCmd.Parameters.Add("PACKCONSYN", OracleDbType.NVarchar2).Value = cy.PackYN;
                    objCmd.Parameters.Add("TOLOCDETAILSID", OracleDbType.NVarchar2).Value = cy.toloc;
                    objCmd.Parameters.Add("TOTHRS", OracleDbType.NVarchar2).Value = cy.tothrs;
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
                        string unit = datatrans.GetDataString("Select UNITMAST.UNITID from ITEMMASTER LEFT OUTER JOIN UNITMAST on UNITMASTID=ITEMMASTER.PRIUNIT where ITEMMASTERID='" + cy.Itemid + "' ");
                        if (cy.Inplst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PackInp cp in cy.Inplst)
                                {
                                    if (cp.drum != "0")
                                    {
                                        svSQL = "Insert into PACKINPDETAIL (PACKBASICID,PACKINPDETAILROW,IITEMID,IUNIT,ICOMBNO,IDRUMNO,IBATCHNO,IBATCHQTY,IQTY,IRATE,IAMOUNT,PACKNOTEDETID,IITEMMASTERID,IDOCDATE,IDOCID,LLOCID,DRUMNO) VALUES ('" + Pid + "','0','" + cy.Itemid + "','" + unit + "','" + cp.comp + "','" + cp.drumid + "','" + cp.batchno + "','" + cp.bqty + "','" + cp.iqty + "','" + cp.rate + "','" + cp.amount + "','" + cp.packid + "','" + cy.Itemid + "','" + cy.Docdate + "','" + cy.Docid + "','" + loc + "','" + cp.drum + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                            else
                            {
                                svSQL = "Delete PACKINPDETAIL WHERE PACKBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (PackInp cp in cy.Inplst)
                                {
                                    if (cp.drum != "0")
                                    {
                                        svSQL = "Insert into PACKINPDETAIL (PACKBASICID,PACKINPDETAILROW,IITEMID,IUNIT,ICOMBNO,IDRUMNO,IBATCHNO,IBATCHQTY,IQTY,IRATE,IAMOUNT,PACKNOTEDETID,IITEMMASTERID,IDOCDATE,IDOCID,LLOCID,DRUMNO) VALUES ('" + Pid + "','0','" + cy.Itemid + "','" + unit + "','" + cp.comp + "','" + cp.drumid + "','" + cp.batchno + "','" + cp.bqty + "','" + cp.iqty + "','" + cp.rate + "','" + cp.amount + "','" + cp.packid + "','" + cy.Itemid + "','" + cy.Docdate + "','" + cy.Docid + "','" + loc + "','" + cp.drum + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                        }
                        if (cy.Matlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PackMat cp in cy.Matlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.item != "0")
                                    {
                                        svSQL = "Insert into PACKCONSDETAIL (PACKBASICID,PACKCONSDETAILROW,CITEMID,CUNIT,LOTYN,SUBQTY,CONSQTY,CONSRATE,CONSAMOUNT) VALUES ('" + Pid + "','0','" + cp.item + "','" + cp.unit + "','" + cp.lotyn + "','0','" + cp.consqty + "','" + cp.conrate + "','" + cp.conamount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                            else
                            {
                                svSQL = "Delete PACKCONSDETAIL WHERE PACKBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (PackMat cp in cy.Matlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.item != "0")
                                    {
                                        svSQL = "Insert into PACKCONSDETAIL (PACKBASICID,PACKCONSDETAILROW,CITEMID,CUNIT,LOTYN,SUBQTY,CONSQTY,CONSRATE,CONSAMOUNT) VALUES ('" + Pid + "','0','" + cp.item + "','" + cp.unit + "','" + cp.lotyn + "','0','" + cp.consqty + "','" + cp.conrate + "','" + cp.conamount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                        }
                        if (cy.Emplst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PackEmp cp in cy.Emplst)
                                {
                                    if (cp.Isvalid == "Y" && cp.empname != "0")
                                    {
                                        svSQL = "Insert into PACKEMPDETAIL (PACKBASICID,PACKEMPDETAILROW,EMPCODE,EMPNAME,DEPARTMENT,EMPCOST) VALUES ('" + Pid + "','0','" + cp.empcode + "','" + cp.empname + "','" + cp.department + "','"+ cp.empcost + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                            else
                            {
                                svSQL = "Delete PACKEMPDETAIL WHERE PACKBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (PackEmp cp in cy.Emplst)
                                {
                                    if (cp.Isvalid == "Y" && cp.empname != "0")
                                    {
                                        svSQL = "Insert into PACKEMPDETAIL (PACKBASICID,PACKEMPDETAILROW,EMPCODE,EMPNAME,DEPARTMENT,EMPCOST) VALUES ('" + Pid + "','0','" + cp.empcode + "','" + cp.empname + "','" + cp.department + "','" + cp.empcost + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                        }
                        if (cy.oconlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (Packothcon cp in cy.oconlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.item != "0")
                                    {
                                        svSQL = "Insert into PACKOTHCONSDETAIL (PACKBASICID,COITEMID,COITEMMASTERID,VALMETHOD,COUNIT,COCLSTK,CONSSTK,COQTY,CORATE,COAMOUNT) VALUES ('" + Pid + "','" + cp.item + "','" + cp.item + "','" + cp.value + "','" + cp.unit + "','0','" + cp.clstk + "','" + cp.conqty + "','" + cp.conrate + "','" + cp.conamount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                            else
                            {
                                svSQL = "Delete PACKOTHCONSDETAIL WHERE PACKBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (Packothcon cp in cy.oconlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.item != "0")
                                    {
                                        svSQL = "Insert into PACKOTHCONSDETAIL (PACKBASICID,COITEMID,COITEMMASTERID,VALMETHOD,COUNIT,COCLSTK,CONSSTK,COQTY,CORATE,COAMOUNT) VALUES ('" + Pid + "','" + cp.item + "','" + cp.item + "','" + cp.value + "','" + cp.unit + "','0','" + cp.clstk + "','" + cp.conqty + "','" + cp.conrate + "','" + cp.conamount + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                        }
                        if (cy.machlst != null)
                        {
                            if (cy.ID == null)
                            {
                                foreach (PackMach cp in cy.machlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.macid != "0")
                                    {
                                        svSQL = "Insert into PACKMACDETAIL (PACKBASICID,PACKMACDETAILROW,MACHINEID,MACHINECOST) VALUES ('" + Pid + "','0','" + cp.macid + "','" + cp.maccost + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                            else
                            {
                                svSQL = "Delete PACKMACDETAIL WHERE PACKBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (PackMach cp in cy.machlst)
                                {
                                    if (cp.Isvalid == "Y" && cp.macid != "0")
                                    {
                                        svSQL = "Insert into PACKMACDETAIL (PACKBASICID,PACKMACDETAILROW,MACHINEID,MACHINECOST) VALUES ('" + Pid + "','0','" + cp.macid + "','" + cp.maccost + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }

                                }
                            }
                        }
                        if (cy.Packdetlst != null)
                        {
                            string toloc = datatrans.GetDataString("Select LOCID from LOCDETAILS where LOCDETAILSID='" + cy.toloc + "' ");

                            if (cy.ID == null)
                            {
                                foreach (PackDet cp in cy.Packdetlst)
                                {
                                    string item = cy.Item;
                                    string drum = cp.drum;
                                    string date = cy.Docdate;
                                    string shift = cy.Shift;

                                    string batch = string.Format("{0} - {1} - {2} - {3}", item, drum, date, shift.ToString());
                                    int i = 1;
                                    if (cp.Isvalid == "Y" && cp.drum != "0")
                                    {
                                        svSQL = "Insert into PACKPDETAIL (PACKBASICID,PACKPDETAILROW,PITEMID,PLOCATION,SLNO,PCOMBNO,PDRUMNO,PBATCHNO,PDRUMQTY,POXQTY,PRATE,PAMOUNT,PDADETID,PITEMIDN,DRMPRF,TEMPDRM,LSCH,LSHIFT,ITEMMASTERID,TOLOCID,LWCID) VALUES ('" + Pid + "','"+i+"','" + cy.Item + "','" + toloc + "','" +i + "','" + cp.comp + "','" + cp.drum + "','" + batch + "','" + cp.dqty + "','" + cp.eqty + "','" + cp.rate + "','" + cp.amount + "','" + cp.pdaid + "','" + cy.Itemid + "','" + cp.prefix + "','" + cp.drumid + "','" + cy.ProdSchNo + "','" + cy.Shift + "','" + cy.Itemid + "','" + cy.toloc + "','" + cy.WorkId + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();

                                        i++;
                                    }
                                  
                                }
                            }
                            else
                            {
                                svSQL = "Delete PACKPDETAIL WHERE PACKBASICID='" + cy.ID + "'";
                                OracleCommand objCmdd = new OracleCommand(svSQL, objConn);
                                objCmdd.ExecuteNonQuery();
                                foreach (PackDet cp in cy.Packdetlst)
                                {
                                    string item = cy.Item;
                                    string drum = cp.drum;
                                    string date = cy.Docdate;
                                    string shift = cy.Shift;

                                    string batch = string.Format("{0}-{1}-{2}-{3}", item, drum, date, shift.ToString());
                                    int i = 1;
                                    if (cp.Isvalid == "Y"  && cp.drum != "0")
                                    {
                                        svSQL = "Insert into PACKPDETAIL (PACKBASICID,PACKPDETAILROW,PITEMID,PLOCATION,SLNO,PCOMBNO,PDRUMNO,PBATCHNO,PDRUMQTY,POXQTY,PRATE,PAMOUNT,PDADETID,PITEMIDN,DRMPRF,TEMPDRM,LSCH,LSHIFT,ITEMMASTERID,TOLOCID,LWCID) VALUES ('" + Pid + "','" + i + "','" + cy.Item + "','" + toloc + "','" + i + "','" + cp.comp + "','" + cp.drum + "','" + batch + "','" + cp.dqty + "','" + cp.eqty + "','" + cp.rate + "','" + cp.amount + "','" + cp.pdaid + "','" + cy.Itemid + "','" + cp.prefix + "','" + cp.drumid + "','" + cy.ProdSchNo + "','" + cy.Shift + "','" + cy.Itemid + "','" + cy.toloc + "','" + cy.WorkId + "')";
                                        OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                                        objCmds.ExecuteNonQuery();


                                    }
                                    i++;
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
        public DataTable GetAllPackingentry(string strStatus)
        {
            string SvSql = string.Empty;
            if (strStatus == "Y" || strStatus == null)
            {
                SvSql = "select PACKBASIC.DOCID,to_char(PACKBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PACKNOTEBASIC.DOCID as pack,PACKBASICID,PACKBASIC.BRANCH,ITEMMASTER.ITEMID,LOCDETAILS.LOCID,SHIFTMAST.SHIFTNO from PACKBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKBASIC.LOCDETAILSID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PACKBASIC.OITEMID LEFT OUTER JOIN SHIFTMAST ON SHIFTMASTID=PACKBASIC.SHIFT LEFT OUTER JOIN PACKNOTEBASIC ON PACKNOTEBASICID=PACKBASIC.PACKINGNOTE WHERE IS_ACTIVE='Y' ORDER BY  PACKBASICID DESC";
            }
            else
            {
                SvSql = "select PACKBASIC.DOCID,to_char(PACKBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PACKNOTEBASIC.DOCID as pack,PACKBASICID,PACKBASIC.BRANCH,ITEMMASTER.ITEMID,LOCDETAILS.LOCID,SHIFTMAST.SHIFTNO from PACKBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKBASIC.LOCDETAILSID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PACKBASIC.OITEMID LEFT OUTER JOIN SHIFTMAST ON SHIFTMASTID=PACKBASIC.SHIFT LEFT OUTER JOIN PACKNOTEBASIC ON PACKNOTEBASICID=PACKBASIC.PACKINGNOTE WHERE IS_ACTIVE='N' ORDER BY  PACKBASICID DESC";

            }
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
                    svSQL = "UPDATE PACKBASIC SET IS_ACTIVE ='N' WHERE PACKBASICID='" + id + "'";
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

        public DataTable GetPacking(string Note)
        {
            string SvSql = string.Empty;
            SvSql = "Select BRANCHMAST.BRANCHID,to_char(PACKBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PACKBASIC.DOCID,WCBASIC.WCID,PACKBASIC.WCBASICID,to_char(PACKBASIC.PNDATE,'dd-MON-yyyy')PNDATE,PSBASIC.DOCID as prod,PACKBASIC.PSCHNO,SHIFTMAST.SHIFTNO,ITEMMASTER.ITEMID,to_char(PACKBASIC.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(PACKBASIC.ENDDATE,'dd-MON-yyyy')ENDDATE,PACKBASIC.STARTTIME, PACKBASIC.ENDTIME,PACKBASIC.LOCDETAILSID,PACKBASIC.TOTISSQTY,PACKBASIC.ISSRATE,PACKBASIC.ISSAMT,PACKBASIC.TOTOPQTY,PACKBASIC.OPRATE,PACKBASIC.OPAMOUNT,PACKNOTEBASIC.DOCID as packnote,PACKINGNOTE,PACKBASIC.ENTEREDBY,PACKBASIC.REMARKS,PACKBASIC.TOTALCAMOUNT,PACKBASIC.PSBASICID,PACKBASIC.TOTALIAMOUNT,PACKBASIC.PACKCONSYN,PACKBASIC.TOLOCDETAILSID,PACKBASIC.TOTHRS from PACKBASIC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PACKBASIC.OITEMID LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKBASIC.WCID LEFT OUTER JOIN PSBASIC ON PSBASIC.PSBASICID=PACKBASIC.PSCHNO LEFT OUTER JOIN SHIFTMAST ON SHIFTMASTID=PACKBASIC.SHIFT LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PACKBASIC.BRANCH LEFT OUTER JOIN PACKNOTEBASIC ON PACKNOTEBASICID=PACKBASIC.PACKINGNOTE\r\n WHERE PACKBASICID='" + Note + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackinp(string Note)
        {
            string SvSql = string.Empty;
            SvSql = " Select PACKBASICID,ICOMBNO,IDRUMNO,DRUMMAST.DRUMNO,IBATCHNO,IBATCHQTY,IQTY,IRATE,IAMOUNT,PACKNOTEDETID from PACKINPDETAIL LEFT OUTER JOIN DRUMMAST ON DRUMMASTID=PACKINPDETAIL.IDRUMNO  WHERE PACKBASICID='" + Note + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackMat(string Note)
        {
            string SvSql = string.Empty;
            SvSql = "  Select PACKBASICID,ITEMMASTER.ITEMID,CUNIT,PACKCONSDETAIL.LOTYN,SUBQTY,CONSQTY,CONSRATE,CONSAMOUNT from PACKCONSDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PACKCONSDETAIL.CITEMID  WHERE PACKBASICID='" + Note + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackEmp(string Note)
        {
            string SvSql = string.Empty;
            SvSql = "  Select PACKBASICID,PACKEMPDETAILROW,EMPCODE,EMPNAME,DEPARTMENT,EMPCOST from PACKEMPDETAIL WHERE PACKBASICID='" + Note + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackCons(string Note)
        {
            string SvSql = string.Empty;
            SvSql = "  Select PACKBASICID,COITEMID,COITEMMASTERID,PACKOTHCONSDETAIL.VALMETHOD,COUNIT,COCLSTK,CONSSTK,COQTY,CORATE,COAMOUNT,ITEMMASTER.ITEMID from PACKOTHCONSDETAIL LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PACKOTHCONSDETAIL.COITEMID  WHERE PACKBASICID='" + Note + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackMac(string Note)
        {
            string SvSql = string.Empty;
            SvSql = " SELECT PACKBASICID,PACKMACDETAILROW,MACHINEINFOBASIC.MNAME,MACHINECOST from PACKMACDETAIL LEFT OUTER JOIN MACHINEINFOBASIC ON MACHINEINFOBASICID=PACKMACDETAIL.MACHINEID  WHERE PACKBASICID='" + Note + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPackingDetail(string Note)
        {
            string SvSql = string.Empty;
            SvSql = "  Select PACKBASICID,PACKPDETAILROW,PITEMID,PLOCATION,SLNO,PCOMBNO,PDRUMNO,PBATCHNO,PDRUMQTY,POXQTY,PRATE,PAMOUNT,PDADETID,PITEMIDN,DRMPRF,TEMPDRM,LSCH,LSHIFT,ITEMMASTERID,TOLOCID,LWCID from PACKPDETAIL WHERE PACKBASICID='" + Note + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
    }
}
