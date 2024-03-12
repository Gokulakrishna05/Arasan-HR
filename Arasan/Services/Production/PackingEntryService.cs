using Arasan.Interface;
 
using Arasan.Models;
using Microsoft.Extensions.Configuration;
using NuGet.Protocol;
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
        public DataTable GetPackNote(string wcid)
        {
            string SvSql = string.Empty;
            SvSql = @"Select  Distinct B.PackNoteBasicID, B.DocID, B.DocDate,i.itemid , Decode(I.Purcat,'NAMCO CONVERSION',B.PACKCONSYN,'Yes') PAckCons
From PackNoteBasic B, PackNoteInpDetail D,itemmaster i,WcBasic W
Where B.PackNoteBasicID = D.PackNoteBasicID
and b.oitemid=i.itemmasterid
And B.WCID=W.WCBASICID
AND B.WCBASICID = '" + wcid + "'";
            SvSql += @"And B.PACKNOTEBASICID not in (Select packingnote from packbasic) Order By 3,2 Desc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GETWC()
        {
            string SvSql = string.Empty;
            SvSql = @"Select W.WCBasicID, W.WCID, W.ILocation, W.RLocation, W.QCLocation, W.RejLocation , L.LocationType , w.Cost
From WCBasic W , LocDetails L
Where W.WCType = 'INTERNAL'
And W.ILocation = L.LocDetailsID
And L.LocationType = 'PACKING'
order by W.wcid";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        
        public DataTable GetNoteDetail(string Note)
        {
            string SvSql = string.Empty;
            SvSql = "Select to_char(PACKNOTEBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,to_char(PACKNOTEBASIC.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(PACKNOTEBASIC.ENDDATE,'dd-MON-yyyy')ENDDATE,STARTTIME,ENDTIME,TOLOCDETAILSID,TOTHRS,PACKNOTEBASIC.OITEMID,PACKNOTEBASIC.WCID as work,PSBASIC.DOCID,WCBASIC.WCID,ITEMMASTER.ITEMID,PACKNOTEBASIC.SHIFT,SHIFTMAST.SHIFTNO,PACKNOTEBASIC.PSCHNO,PACKCONSYN,PACKNOTEBASICID,WCBASIC.ILOCATION from PACKNOTEBASIC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PACKNOTEBASIC.OITEMID LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKNOTEBASIC.WCID LEFT OUTER JOIN PSBASIC ON PSBASICID=PACKNOTEBASIC.PSCHNO LEFT OUTER JOIN SHIFTMAST ON SHIFTMASTID=PACKNOTEBASIC.SHIFT WHERE PACKNOTEBASICID='" + Note +"'";
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

        public DataTable GetItem(string locid)
        {
            string SvSql = string.Empty;
            string Docdate = DateTime.Now.ToString("dd-MMM-yyyy");
            SvSql = @"SELECT I.ITEMMASTERID,I.ITEMID,SUM(DECODE(s.PLUSORMINUS,'p',s.QTY,-s.QTY)) qty,I.VALMETHOD ,I.ITEMACC ,U.UNITID
FROM Stockvalue S,Itemmaster I,UnitMast U WHERE s.ITEMID=i.ITEMMASTERID AND S.DOCDATE<='" + Docdate + "' ";
            SvSql += @" AND I.IGROUP IN ('PACKING MATERIALS') And S.LOCID='"+ locid + "' And U.Unitmastid=I.PRIUNIT ";
 SvSql += @" And I.Sncategory='PACKING CONSUMABLES'
GROUP BY I.ITEMMASTERID,I.ITEMID,I.VALMETHOD,I.ITEMACC,U.UNITID  
Having SUM(DECODE(s.PLUSORMINUS,'p',s.QTY,-s.QTY))>0
Order by 2";
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

                                       string SvSql1 = "Insert into LSTOCKVALUE (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,DOCID,DOCDATE,LOTNO,PLUSQTY,MINUSQTY,DRUMNO,RATE,STOCKVALUE,ITEMID,LOCID,BINNO,FROMLOCID,STOCKTRANSTYPE) VALUES ('0','0','F','" + Pid + "','750292868','" + cy.Docid+ "','" + cy.Docdate + "','" + cp.batchno + "' ,'0','" + cp.iqty + "','" + cp.drum + "','" + cp.rate + "','" + cp.amount + "','" + cy.Itemid + "','" + loc + "','0','0','PACKING INPUT' )";
                                        OracleCommand objCmdss = new OracleCommand(SvSql1, objConn);
                                        objCmdss.ExecuteNonQuery();
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

                                        string qcyn = datatrans.GetDataString("SELECT QCCOMPFLAG FROM ITEMMASTER WHERE ITEMMASTERID='" + item + "'");
                                        string sttime = datatrans.GetDataString("SELECT FROMTIME FROM SHIFTMAST WHERE SHIFTNO='" + cy.Shift + "'");
                                        string etime = datatrans.GetDataString("SELECT TOTIME FROM SHIFTMAST WHERE SHIFTNO='" + cy.Shift + "'");
                                        string process = datatrans.GetDataString("SELECT PROCESSMAST.PROCESSID FROM WCBASIC LEFT OUTER JOIN PROCESSMAST ON PROCESSMAST.PROCESSMASTID=WCBASIC.PROCESSID  WHERE wcid='" + cy.Work + "'");
                                        string ins = "";
                                        string sflag = "1";
                                        if (qcyn == "YES")
                                        {
                                            ins = "0";
                                        }
                                        else { ins = "1"; }
                                        string SvSql = " INSERT INTO PLOTMAST (APPROVAL, MAXAPPROVED, CANCEL, T1SOURCEID, LATEMPLATEID, ITEMID, PARTYID, RATE, DOCID, DOCDATE, QTY, LOTNO, DRUMNO, LOCATION, INSFLAG, RCFLAG, PRODTYPE, AMOUNT, QCRELASEFLAG, ESTATUS, COMPFLAG, PACKFLAG, SHIFT, STARTTIME, ENDTIME, CURINWFLAG, CUROUTFLAG, PACKINSFLAG, PSCHNO, MATCOST, MCCOST, EMPCOST, OTHERCOST, ADMINCOST, GENSETCOST, EBCOST, EBUNITRATE, DIESELRATE, TESTINSFLAG, FIDRMS, DRMPRF, PACKDRMNO, WCID, ProcessID,SHEDNO) Values(0, 0, 'F', '" + Pid + "', '738073639', '" + cy.Itemid + "', '0', '"+cp.rate+"', '" + cy.Docid + "', '" + cy.Docdate + "','" + cp.dqty + "','" + batch + "', '" + cp.drum + "', '" + cy.toloc + "', '" + ins + "', '0', 'PACK','"+cp.amount+"', '0', '0','" + sflag + "',' 0', '" + cy.Shift + "', '" + sttime + "', '" + etime + "', '0', '0', '1', '" + cy.ProdSchNo + "', 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, '', '', '" + cy.Work + "','" + process + "','')";
                                        objCmds = new OracleCommand(SvSql, objConn);
                                        objCmds.ExecuteNonQuery();
                                        SvSql = "INSERT INTO PLSTOCKVALUE (APPROVAL, MAXAPPROVED, CANCEL, T1SOURCEID, LATEMPLATEID, DOCID, DOCDATE, Drumno, LOTNO, PLUSQTY, MINUSQTY, RATE, STOCKVALUE, ITEMID, LOCID, BINNO, FROMLOCID, StockTranstype, batch) VALUES ('0', '0', 'F', '" + Pid + "', '', '" + cy.Docid + "',  '" + cy.Docdate + "', '" + cp.drum + "','" + batch + "','" + cp.dqty + "', '0', '"+cp.rate+"','"+cp.amount+"', '" + cy.Itemid + "','" + cy.toloc + "', '0', '0', '', '')";
                                        objCmds = new OracleCommand(SvSql, objConn);
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
                SvSql = "select PACKBASIC.DOCID,to_char(PACKBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PACKNOTEBASIC.DOCID as pack,PACKBASICID,PACKBASIC.BRANCH,ITEMMASTER.ITEMID,LOCDETAILS.LOCID,SHIFTMAST.SHIFTNO from PACKBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKBASIC.LOCDETAILSID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PACKBASIC.OITEMID LEFT OUTER JOIN SHIFTMAST ON SHIFTMASTID=PACKBASIC.SHIFT LEFT OUTER JOIN PACKNOTEBASIC ON PACKNOTEBASICID=PACKBASIC.PACKINGNOTE WHERE PACKBASIC.IS_ACTIVE='Y' ORDER BY  PACKBASICID DESC";
            }
            else
            {
                SvSql = "select PACKBASIC.DOCID,to_char(PACKBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PACKNOTEBASIC.DOCID as pack,PACKBASICID,PACKBASIC.BRANCH,ITEMMASTER.ITEMID,LOCDETAILS.LOCID,SHIFTMAST.SHIFTNO from PACKBASIC LEFT OUTER JOIN LOCDETAILS ON LOCDETAILS.LOCDETAILSID=PACKBASIC.LOCDETAILSID LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PACKBASIC.OITEMID LEFT OUTER JOIN SHIFTMAST ON SHIFTMASTID=PACKBASIC.SHIFT LEFT OUTER JOIN PACKNOTEBASIC ON PACKNOTEBASICID=PACKBASIC.PACKINGNOTE WHERE PACKBASIC.IS_ACTIVE='N' ORDER BY  PACKBASICID DESC";

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
            SvSql = "Select BRANCHMAST.BRANCHID,to_char(PACKBASIC.DOCDATE,'dd-MON-yyyy')DOCDATE,PACKBASIC.DOCID,WCBASIC.WCID,PACKBASIC.WCBASICID,to_char(PACKBASIC.PNDATE,'dd-MON-yyyy')PNDATE,PSBASIC.DOCID as prod,PACKBASIC.PSCHNO,SHIFTMAST.SHIFTNO,ITEMMASTER.ITEMID,to_char(PACKBASIC.STARTDATE,'dd-MON-yyyy')STARTDATE,to_char(PACKBASIC.ENDDATE,'dd-MON-yyyy')ENDDATE,PACKBASIC.STARTTIME, PACKBASIC.ENDTIME,PACKBASIC.LOCDETAILSID,PACKBASIC.TOTISSQTY,PACKBASIC.ISSRATE,PACKBASIC.ISSAMT,PACKBASIC.TOTOPQTY,PACKBASIC.OPRATE,PACKBASIC.OPAMOUNT,PACKNOTEBASIC.DOCID as packnote,PACKINGNOTE,PACKBASIC.ENTEREDBY,PACKBASIC.REMARKS,PACKBASIC.TOTALCAMOUNT,PACKBASIC.PSBASICID,PACKBASIC.TOTALIAMOUNT,PACKBASIC.PACKCONSYN,PACKBASIC.TOLOCDETAILSID,PACKBASIC.TOTHRS from PACKBASIC LEFT OUTER JOIN ITEMMASTER ON ITEMMASTERID=PACKBASIC.OITEMID LEFT OUTER JOIN WCBASIC ON WCBASIC.WCBASICID=PACKBASIC.WCID LEFT OUTER JOIN PSBASIC ON PSBASIC.PSBASICID=PACKBASIC.PSCHNO LEFT OUTER JOIN SHIFTMAST ON SHIFTMASTID=PACKBASIC.SHIFT LEFT OUTER JOIN BRANCHMAST ON BRANCHMASTID=PACKBASIC.BRANCH LEFT OUTER JOIN PACKNOTEBASIC ON PACKNOTEBASICID=PACKBASIC.PACKINGNOTE WHERE PACKBASICID='" + Note + "'";
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
        public DataTable GetEmployeeDetails(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPID,EMPNAME,EMPMASTID,EMPDEPT, DDBASIC.DEPTNAME,EMPCOST,OTPERHR from EMPMAST LEFT OUTER JOIN DDBASIC ON DDBASICID=EMPMAST.EMPDEPT where EMPMASTID='" + id + "' ";
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
        public DataTable GetPackItem()
        {
            string SvSql = string.Empty;
            SvSql = @" Select ItemMasterID , ItemID, ItemDesc, U.UnitID, ItemMasterID, ItemAcc, ValMethod, LotYN
From ItemMaster I, UnitMast U
Where I.PriUnit = U.UnitMastID
And I.Igroup='PACKING MATERIALS'
And I.Subcategory='PACK DRUM'
Order By ItemID";
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
