using Arasan.Interface.Master;
using Arasan.Models;
using Oracle.ManagedDataAccess.Client;
using System.Data;


namespace Arasan.Services.Master
{
    public class MchineServices : IMchine 
    {
        private readonly string _connectionString;
        DataTransactions datatrans;

        public MchineServices(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
        }


        public DataTable GetUnit()
        {
            string SvSql = string.Empty;
            SvSql = "Select UNITID,UNITMASTID from UNITMAST WHERE IS_ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAllMach(string id)
        {
            string SvSql = string.Empty;
            SvSql = "Select MCODE,MNAME,MLOCATION,MSERIALNO,MMODEL from MACHINEINFOBASIC WHERE MACHINEINFOBASICID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }




        public string Homereturn(Machine ka)
        {
            string msg = "";
            string svSQL = "";
            try
            {
                using (OracleConnection objconn = new OracleConnection(_connectionString))
                {
                    objconn.Open();
                    if (ka.ID == null)
                    {
                     svSQL = "Insert into MACHINEINFOBASIC (APPROVAL,MAXAPPROVED,CANCEL,T1SOURCEID,LATEMPLATEID,MCODE,MNAME,MLOCATION,MSERIALNO,MMODEL,MMANFNAME,MMADEIN,MMODEOFPUR,MSERVDET,MSERCOMP,MINCHARGE,MPURPOSE,DELCTRL1,DELCTRL,MCAPACITY,ESTLIFE,POWKVA,POWUTILFAC,MPURDT,MSUPDT,MINSTDT,MLAMAINDT,MNEMAINDT,MACLCOST,POWUTILHR,DEPPER,DEPVAL,INTPER,INTVAL,INSPER,INSVAL,SALOP,MAINCOST,AREAOCC,RATESQFT,RENTMAC,TOT,POWRATEUNIT,POWCOSTHR,RUNHR,FIXCOSTHR,COSTRATEHR,MUNIT,EUNIT,PUNIT,MWCID,AUXYN,MCID,SUNIT,SOFARLIFE,MAINTHRS,MAINTUNI,LEADUNIT,LEADHRS,MSUPPLY,MTUNIT,MAINTHOURS,MTYPE,MSPROCESSID,MMODE) values ('0','0','F','0','0','" + ka.MId + "','" + ka.MName + "','" + ka.MLoc + "','" + ka.MSerial + "','" + ka.MModel + "','" + ka.MManname + "','" + ka.MMade + "','" + ka.MPur + "','" + ka.MSer + "','" + ka.MSerCmp + "','" + ka.MIncharge + "','','0','T','" + ka.MCap + "','" + ka.MELife + "','" + ka.MPower + "','" + ka.MPFactor + "','" + ka.DOP + "','" + ka.DOS + "','" + ka.InsDate + "','" + ka.DOLMain + "','" + ka.NMainDate + "','" + ka.MLCost + "','" + ka.PCostH + "','" + ka.Dep + "','" + ka.DepValue + "','" + ka.Int + "','" + ka.IntValue + "','','" + ka.InsValue + "','" + ka.SOYear + "','" + ka.MCYear + "','0','" + ka.CostRH + "','" + ka.Rent + "','" + ka.Tot + "','" + ka.PCUnit + "','','" + ka.MRun + "','" + ka.FixCost + "','','" + ka.MCapUnit + "','" + ka.MELifeUnit + "','" + ka.MPowerUnit + "','" + ka.MWrkCent + "','" + ka.Aux + "','" + ka.MRunHUnit + "','','','" + ka.MMaintain + "','" + ka.MMaintainUnit + "','" + ka.MLeadUnit + "','" + ka.MLead + "','" + ka.MSupply + "','0','','','" + ka.MSubProc + "','" + ka.MMMode + "') RETURNING MACHINEINFOBASICID  INTO: OUTID";
                    }

                    else
                    {
                        svSQL = " UPDATE MACHINEINFOBASIC SET MCODE = '" + ka.MId + "', MNAME = '" + ka.MName + "',  MLOCATION =  '" + ka.MLoc + "', MWCID = '" + ka.MWrkCent + "'  , MSUPPLY = '" + ka.MSupply + "', MSERIALNO = '" + ka.MSerial + "', MMODEL = '" + ka.MModel + "' , MMANFNAME = '" + ka.MManname + "', MTYPE = '" + ka.MSubProc + "', MMADEIN = '" + ka.MMade + "', MMODEOFPUR = '" + ka.MPur + "', MSERVDET = '" + ka.MSer + "', MSERCOMP = '" + ka.MSerCmp + "', MINCHARGE = '" + ka.MIncharge + "', AUXYN = '" + ka.Aux + "', MMODE = '" + ka.MMMode + "', MCAPACITY = '" + ka.MCap + "', MUNIT = '" + ka.MCapUnit + "', ESTLIFE = '" + ka.MELife + "', EUNIT = '" + ka.MELifeUnit + "', POWKVA = '" + ka.MPower + "', PUNIT = '" + ka.MPowerUnit + "', POWUTILFAC = '" + ka.MPFactor + "', RUNHR = '" + ka.MRun + "', MCID = '" + ka.MRunHUnit + "', LEADHRS = '" + ka.MLead + "', LEADUNIT = '" + ka.MLeadUnit + "', MAINTHRS = '" + ka.MMaintain + "', MAINTUNI = '" + ka.MMaintainUnit + "', MPURDT = '" + ka.DOP + "', MSUPDT = '" + ka.DOS + "', MINSTDT = '" + ka.InsDate + "', MLAMAINDT = '" + ka.DOLMain + "', MNEMAINDT = '" + ka.NMainDate + "', MACLCOST = '" + ka.MLCost + "', DEPPER = '" + ka.Dep + "', INTPER = '" + ka.Int + "', SALOP = '" + ka.SOYear + "', MAINCOST = '" + ka.MCYear + "', RENTMAC = '" + ka.Rent + "', TOT = '" + ka.Tot + "', POWRATEUNIT = '" + ka.PCUnit + "', FIXCOSTHR = '" + ka.FixCost + "', RATESQFT = '" + ka.CostRH + "', DEPVAL = '" + ka.DepValue + "', INTVAL = '" + ka.IntValue + "', INSVAL = '" + ka.InsValue + "', POWUTILHR = '" + ka.PCostH + "', MSPROCESSID = '" + ka.AddMCost + "'  Where MACHINEINFOBASICID = '" + ka.ID + "'";

                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();
                 string Pid = oracleCommand.Parameters["OUTID"].Value.ToString();
                    if (ka.Complst != null)
                    {
                        if (ka.ID == null)
                        {
                            foreach (Compdetails cp in ka.Complst)
                            {
                                if (cp.Isvalid == "Y" && cp.PartNumber!="")
                                {

                                    svSQL = "Insert into COMPDETAILS (MACHINEINFOBASICID,PARTNO,LIFETIME,WARRANTYTILLDT) VALUES ('" + Pid + "','" + cp.PartNumber + "','" + cp.DateOfIssue + "','" + cp.LifeTimeInHrs + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                            }

                        }
                        else
                        {
                            svSQL = "Delete COMPDETAILS WHERE MACHINEINFOBASICID='" + ka.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                            objCmdd.ExecuteNonQuery();
                            foreach (Compdetails cp in ka.Complst)
                            {
                                if (cp.Isvalid == "Y" && cp.PartNumber != "")
                                {
                                    svSQL = "Insert into COMPDETAILS (MACHINEINFOBASICID,PARTNO,LIFETIME,WARRANTYTILLDT) VALUES ('" + Pid + "','" + cp.PartNumber + "','" + cp.DateOfIssue + "','" + cp.LifeTimeInHrs + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                            }
                        }
                    }
                    if (ka.Majorlst != null)
                    {
                        if (ka.ID == null)
                        {
                            int r = 1;
                            foreach (Majorpart cp in ka.Majorlst)
                            {
                                if (cp.Isvalid == "Y" && cp.Majorparts!="")
                                {

                                    svSQL = "Insert into MCMAJORPARTS (MACHINEINFOBASICID,MCMAJORPARTSROW,MPARTID,ACTIVEYN,CRYN) VALUES ('" + Pid + "','"+r+"','" + cp.Majorparts + "','" + cp.Active + "','" + cp.Critical + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();
                                    
                                }
                                r++;
                            }

                        }
                        else
                        {
                            svSQL = "Delete COMPDETAILS WHERE MACHINEINFOBASICID='" + ka.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                            objCmdd.ExecuteNonQuery();
                            foreach (Majorpart cp in ka.Majorlst)
                            {
                                int r = 1;
                                if (cp.Isvalid == "Y" && cp.Majorparts != "")
                                {
                                    svSQL = "Insert into MCMAJORPARTS (MACHINEINFOBASICID,MCMAJORPARTSROW,MPARTID,ACTIVEYN,CRYN) VALUES ('" + Pid + "','"+r+"','" + cp.Majorparts + "','" + cp.Active + "','" + cp.Critical + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                                r++;
                            }
                        }
                    }
                    if (ka.Checklistlst != null)
                    {
                        if (ka.ID == null)
                        {
                            int r = 1;
                            foreach (Checklistdetails cp in ka.Checklistlst)
                            {
                                if (cp.Isvalid == "Y" && cp.Service!="")
                                {

                                    svSQL = "Insert into MACHINECHECK (MACHINEINFOBASICID,MACHINECHECKROW,SERVICE,CHTYPE) VALUES ('" + Pid + "','"+r+"','" + cp.Service + "','" + cp.Type + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                                r++;
                            }

                        }
                        else
                        {
                            svSQL = "Delete COMPDETAILS WHERE MACHINEINFOBASICID='" + ka.ID + "'";
                            OracleCommand objCmdd = new OracleCommand(svSQL, objconn);
                            objCmdd.ExecuteNonQuery();
                            foreach (Checklistdetails cp in ka.Checklistlst)
                            {
                                int r = 1;
                                if (cp.Isvalid == "Y" && cp.Service != "")
                                {
                                    svSQL = "Insert into MACHINECHECK (MACHINEINFOBASICID,MACHINECHECKROW,SERVICE,CHTYPE) VALUES ('" + Pid + "','"+r+"','" + cp.Service + "','" + cp.Type + "')";
                                    OracleCommand objCmds = new OracleCommand(svSQL, objconn);
                                    objCmds.ExecuteNonQuery();

                                }
                                r++;
                            }
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw ex;
            }
            return msg;
        }


        public DataTable GetAllMachine(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select MACHINEINFOBASICID,MCODE,MNAME,MLOCATION,MSERIALNO,MMODEL,LOCDETAILS.LOCID,MACHINEINFOBASIC.IS_ACTIVE from MACHINEINFOBASIC left outer join LOCDETAILS ON LOCDETAILSID=MACHINEINFOBASIC.MLOCATION  WHERE MACHINEINFOBASIC.IS_ACTIVE='Y' ORDER BY MACHINEINFOBASIC.MACHINEINFOBASICID DESC ";

            }
            else
            {
                SvSql = "Select MACHINEINFOBASICID,MCODE,MNAME,MLOCATION,MSERIALNO,MMODEL,LOCDETAILS.LOCID,MACHINEINFOBASIC.IS_ACTIVE from MACHINEINFOBASIC left outer join LOCDETAILS ON LOCDETAILSID=MACHINEINFOBASIC.MLOCATION  WHERE MACHINEINFOBASIC.IS_ACTIVE='N' ORDER BY MACHINEINFOBASIC.MACHINEINFOBASICID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }


        public DataTable GetMachineEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select MACHINEINFOBASICID,MCODE,MNAME,MLOCATION,MWCID,MSUPPLY,MSERIALNO,MMODEL,MMANFNAME,MSPROCESSID,MMADEIN,MMODEOFPUR,MSERVDET,MSERCOMP,MINCHARGE,AUXYN,MMODE,MCAPACITY,MUNIT,ESTLIFE,EUNIT,POWKVA,PUNIT,POWUTILFAC,RUNHR,MCID,LEADHRS,LEADUNIT,MAINTHRS,MAINTUNI,to_char(MPURDT,'dd-MON-yyyy')MPURDT,to_char(MSUPDT,'dd-MON-yyyy')MSUPDT,to_char(MINSTDT,'dd-MONyyyy')MINSTDT,to_char(MLAMAINDT,'dd-MON-yyyy')MLAMAINDT,to_char(MNEMAINDT,'dd-MON-yyyy')MNEMAINDT,MACLCOST,DEPPER,INTPER,SALOP,MAINCOST,RENTMAC,TOT,POWRATEUNIT,FIXCOSTHR,RATESQFT,DEPVAL,INTVAL,INSVAL,POWUTILHR,MSPROCESSID from MACHINEINFOBASIC WHERE MACHINEINFOBASICID='" + id + "'";


            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItem()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMMASTERID,ITEMMASTER.ITEMID  from ITEMMASTER  WHERE ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetMajor()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMMASTERID,ITEMMASTER.ITEMID  from ITEMMASTER  WHERE ACTIVE='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetCheck()
        {
            string SvSql = string.Empty;
            SvSql = "Select ITEMMASTER.ITEMMASTERID,ITEMMASTER.ITEMID  from ITEMMASTER  WHERE ACTIVE='Y'";
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

                    if(tag=="Del")
                    {
                        svSQL = "UPDATE MACHINEINFOBASIC SET IS_ACTIVE ='N' WHERE MACHINEINFOBASICID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE MACHINEINFOBASIC SET IS_ACTIVE ='Y' WHERE MACHINEINFOBASICID='" + id + "'";
                    }
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



        public string AddPurchaseCRUD(string category)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                svSQL = " SELECT Count(COMMON_VALUE) as cnt FROM COMMONMASTER WHERE COMMON_VALUE =LTRIM(RTRIM('" + category + "')) AND COMMON_TEXT='MODEOFPURCHASE'";
                if (datatrans.GetDataId(svSQL) > 0)
                {
                    msg = "Purchase CATEGORY Already Existed";
                    return msg;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    svSQL = "Insert into COMMONMASTER (COMMON_TEXT,COMMON_VALUE)  VALUES('MODEOFPURCHASE','" + category + "')";

                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                    objConn.Open();
                    objCmds.ExecuteNonQuery();
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

        public string AddMadeCRUD(string category)
        {
            string msg = "";
            try
            {
                string StatementType = string.Empty; string svSQL = "";

                svSQL = " SELECT Count(COMMON_VALUE) as cnt FROM COMMONMASTER WHERE COMMON_VALUE =LTRIM(RTRIM('" + category + "')) AND COMMON_TEXT='MADEIN'";
                if (datatrans.GetDataId(svSQL) > 0)
                {
                    msg = "SUB RAWMATERIAL CATEGORY Already Existed";
                    return msg;
                }
                using (OracleConnection objConn = new OracleConnection(_connectionString))
                {
                    svSQL = "Insert into COMMONMASTER (COMMON_TEXT,COMMON_VALUE)  VALUES('MADEIN','" + category + "')";

                    OracleCommand objCmds = new OracleCommand(svSQL, objConn);
                    objConn.Open();
                    objCmds.ExecuteNonQuery();
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
    }
}

