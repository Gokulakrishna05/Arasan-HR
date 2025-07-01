using Arasan.Interface;
using Arasan.Models;
using Microsoft.AspNetCore.Http;
using Oracle.ManagedDataAccess.Client;
using System.Data;

namespace Arasan.Services
{
    public class HolidayMasterService : IHolidayMaster
    {
        private readonly string _connectionString;
        DataTransactions datatrans;
        //private readonly IHttpContextAccessor _httpContextAccessor;

        public HolidayMasterService(IConfiguration _configuratio)


        {
            _connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            datatrans = new DataTransactions(_connectionString);
            //_httpContextAccessor = httpContextAccessor;
        }
        public DataTable GetHolidayMasterEdit(string id)
        {
            string SvSql = string.Empty;

            SvSql = "Select HOLIDAYID,HOLIDAYNAME,to_char(HOLIDAYDATE,'dd-MON-yyyy')HOLIDAYDATE ,DAYOFWEEK,HOLIDAYTYPE,REMARKS,CREATEDDATE,CREATEDBY,MODIFIEDDATE,MODIFIEDBY  from HOLIDAYMASTER WHERE HOLIDAYID='" + id + "'";

            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetAllHolidayMaster(string status)
        {
            string SvSql = string.Empty;
            if (status == "Y" || status == null)
            {
                SvSql = "Select HOLIDAYID,HOLIDAYNAME,to_char(HOLIDAYDATE,'dd-MON-yyyy')HOLIDAYDATE,DAYOFWEEK,HOLIDAYMASTER.IS_ACTIVE from HOLIDAYMASTER WHERE HOLIDAYMASTER.IS_ACTIVE='Y' ORDER BY HOLIDAYMASTER.HOLIDAYID DESC ";

            }
            else
            {
                SvSql = "Select HOLIDAYID,HOLIDAYNAME,to_char(HOLIDAYDATE,'dd-MON-yyyy')HOLIDAYDATE,DAYOFWEEK,HOLIDAYMASTER.IS_ACTIVE from HOLIDAYMASTER WHERE HOLIDAYMASTER.IS_ACTIVE='N' ORDER BY HOLIDAYMASTER.HOLIDAYID DESC ";

            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }




        public string GetHMaster(HolidayMaster Em)
        {
            string msg = "";
            string svSQL = "";
            string updateCMd = "";
            string Pid = "";
            try
            {
                //var userid = _httpContextAccessor.HttpContext?.Request.Cookies["UserId"];
                using (OracleConnection objconn = new OracleConnection(_connectionString))


                {
                    objconn.Open();
                    if (Em.ID == null)
                    {
                        svSQL = "Insert into HOLIDAYMASTER (HOLIDAYID,HOLIDAYNAME,HOLIDAYDATE,DAYOFWEEK,HOLIDAYTYPE,REMARKS,CREATEDDATE,CREATEDBY) values ('" + Em.ID + "','" + Em.Hname + "','" + Em.Hdate + "','" + Em.DWeek + "','" + Em.HType + "','" + Em.Rmk + "','" + Em.Cdate + "','" + Em.Cby + "') ";
                    }

                    else
                    {
                        svSQL = " UPDATE HOLIDAYMASTER SET HOLIDAYNAME = '" + Em.Hname + "',HOLIDAYDATE = '" + Em.Hdate + "',DAYOFWEEK = '" + Em.DWeek + "',HOLIDAYTYPE = '" + Em.HType + "',REMARKS='" + Em.Rmk + "',MODIFIEDDATE='" + Em.Mdate + "',MODIFIEDBY='" + Em.Mby + "'  Where HOLIDAYID = '" + Em.ID + "' ";

                    }
                    OracleCommand oracleCommand = new OracleCommand(svSQL, objconn);
                    oracleCommand.Parameters.Add("OUTID", OracleDbType.Int64, ParameterDirection.ReturnValue);
                    oracleCommand.ExecuteNonQuery();

                }

            }
            catch (Exception ex)
            {
                msg = ex.Message;
                throw ex;
            }
            return msg;
        }

        public string StatusChange(string tag, string id)
        {
            try
            {
                string svSQL = string.Empty;
                using (OracleConnection objConnT = new OracleConnection(_connectionString))
                {

                    if (tag == "Del")
                    {
                        svSQL = "UPDATE HolidayMaster SET IS_ACTIVE ='N' WHERE HOLIDAYID='" + id + "'";
                    }
                    else
                    {
                        svSQL = "UPDATE HolidayMaster SET IS_ACTIVE ='Y' WHERE HOLIDAYID='" + id + "'";
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


    }
}
