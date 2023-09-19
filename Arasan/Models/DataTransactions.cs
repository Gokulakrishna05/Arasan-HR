using System.Data;
using Oracle.ManagedDataAccess.Client;
using MimeKit;
using System.IO;
using System.Threading.Tasks;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using Arasan.Interface;
using Arasan.Models;
using Org.BouncyCastle.Crypto.Macs;
using System.Net.Mail;
using System.Net.Mime;
using System.Text;

namespace Arasan.Models
{
    public class DataTransactions
    {
        private readonly string _connectionString;
        public DataTransactions(string connectionString)
        {
            //_connectionString = _configuratio.GetConnectionString("OracleDBConnection");
            _connectionString = connectionString;// _configuratio.GetConnectionString("OracleDBConnection");
        }
        public DataTable GetData(string sql)
        {
            DataTable _Dt = new DataTable();
            try
            {
                OracleDataAdapter adapter = new OracleDataAdapter(sql, _connectionString);
                OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
                adapter.Fill(_Dt);
            }
            catch (Exception ex)
            {

            }
            return _Dt;
        }

      
        public int GetFinancialYear(DateTime Date1)
        {
            if (Date1.Year > 2000)
            {
                if (Date1.Month > 3)
                {
                    return new DateTime(Date1.Year, 4, 1).Year;
                }
                else
                {
                    return new DateTime(Date1.Year - 1, 4, 1).Year;
                }
            }
            return Date1.Year;
        }
        public int GetDataId(String sql)
        {
            DataTable _dt = new DataTable();
            int Id = 0;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt32(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }

        public long GetDataIdlong(String sql)
        {
            DataTable _dt = new DataTable();
            long Id = 0;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    Id = Convert.ToInt64(_dt.Rows[0][0].ToString() == string.Empty ? "0" : _dt.Rows[0][0].ToString());
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return Id;
        }
        public DataTable GetBranch()
        {
            string SvSql = string.Empty;
            SvSql = "select BRANCHMASTID,BRANCHID from BRANCHMAST where STATUS='ACTIVE'   order by BRANCHMASTID asc ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetSequence(string vtype)
        {
            string SvSql = string.Empty;
            SvSql = "select PREFIX,LASTNO+1 as last from sequence where TRANSTYPE='" + vtype  + "' AND ACTIVESEQUENCE='T'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetLedger()
        {
            string SvSql = string.Empty;
            SvSql = "select DISPLAY_NAME, LEDGERID from LEDGER";
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

        public DataTable GetLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS ";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetPyroLocation()
        {
            string SvSql = string.Empty;
            SvSql = "Select LOCID,LOCDETAILSID from LOCDETAILS where LOCATIONTYPE='BALL MILL' order by LOCDETAILSID asc";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetcuringSet()
        {
            string SvSql = string.Empty;
            SvSql = "select * from BINBASIC WHERE ACTIVE='Y' AND ISCURINGSET='Y'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetEmailConfig()
        {
            string SvSql = "Select ID,SMTP_HOST,PORT_NO,SIGNATURE,SSL,EMAIL_ID,PASSWORD from EMAIL_CONFIG where IS_ACTIVE = 'Y'";

            DataTable dtCity = new DataTable();
            dtCity = GetData(SvSql);
            return dtCity;
        }
        public DataTable GetSupplier()
        {
            string SvSql = string.Empty;
            SvSql = "Select PARTYMAST.PARTYMASTID,PARTYMAST.PARTYNAME from PARTYMAST  Where PARTYMAST.TYPE IN ('Supplier','BOTH') AND PARTYMAST.PARTYNAME IS NOT NULL";
            DataTable dtt = new DataTable(); OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);

            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetCurency()
        {
            string SvSql = string.Empty;
            SvSql = "Select MAINCURR || ' - ' || SYMBOL  as Cur,CURRENCYID from CURRENCY";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetEmp()
        {
            string SvSql = string.Empty;
            SvSql = "Select EMPID,EMPNAME,EMPMASTID from EMPMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable GetItemSubGrp()
        {
            string SvSql = string.Empty;
            SvSql = "Select SGCODE,ITEMSUBGROUPID FROM ITEMSUBGROUP";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdLog()
        {
            string SvSql = string.Empty;
            SvSql = "select LPRODBASICID,DOCID from LPRODBASIC";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdLogsearch(string term)
        {
            string SvSql = string.Empty;
            SvSql = "select LPRODBASICID,DOCID from LPRODBASIC WHERE DOCID like '%"+ term + "%'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetProdSch()
        {
            string SvSql = string.Empty;
            SvSql = "select PSBASICID,DOCID from PSBASIC   WHERE PSCHSTATUS='ACTIVE'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetBatch()
        {
            string SvSql = string.Empty;
            SvSql = "select BCPRODBASICID,DOCID from BCPRODBASIC ";
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
            if(!string.IsNullOrEmpty(value) && value != "0")
            {
                SvSql += " Where SUBGROUPCODE='" + value + "'";
            }
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetItemSubGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ITEMID,SUBGROUPCODE from ITEMMASTER WHERE ITEMMASTERID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetVType()
        {
            string SvSql = string.Empty;
            SvSql = "select VCHTYPEID,DESCRIPTION from VCHTYPE";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        
        public DataTable GetItemDetails(string ItemId)
        {
            string SvSql = string.Empty;
            SvSql = "select UNITMAST.UNITID,ITEMID,ITEMDESC,UNITMAST.UNITMASTID,LATPURPRICE,ITEMMASTERPUNIT.CF,ITEMMASTER.LATPURPRICE,QCCOMPFLAG from ITEMMASTER LEFT OUTER JOIN UNITMAST  on ITEMMASTER.PRIUNIT=UNITMAST.UNITMASTID LEFT OUTER JOIN ITEMMASTERPUNIT ON  ITEMMASTER.ITEMMASTERID=ITEMMASTERPUNIT.ITEMMASTERID Where ITEMMASTER.ITEMMASTERID='" + ItemId + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccType(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCOUNTTYPE,ACCOUNTCODE from ACCTYPE where ACCOUNTTYPEID='"+ id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccGroup(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUP.ACCOUNTGROUP,ACCGROUP.GROUPCODE,ACCTYPE.ACCOUNTTYPE,ACCTYPE.ACCOUNTCODE from ACCGROUP LEFT OUTER JOIN ACCTYPE on ACCTYPE.ACCOUNTTYPEID=ACCGROUP.ACCTYPE  Where ACCGROUP.STATUS='Active' AND ACCGROUPID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public DataTable GetAccLedger(string id)
        {
            string SvSql = string.Empty;
            SvSql = "select ACCGROUP.ACCOUNTGROUP,ACCGROUP.GROUPCODE,ACCTYPE.ACCOUNTTYPE,ACCTYPE.ACCOUNTCODE,LEDGER.LEDNAME,LEDGER.DISPLAY_NAME,LEDGER.CATEGORY from LEDGER LEFT OUTER JOIN ACCGROUP ON LEDGER.ACCGROUP=ACCGROUP.ACCGROUPID LEFT OUTER JOIN ACCTYPE on ACCTYPE.ACCOUNTTYPEID=ACCGROUP.ACCTYPE  Where ACCGROUP.STATUS='Active' AND LEDGERID='" + id + "'";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }

        public DataTable ShiftDeatils()
        {
            string SvSql = string.Empty;
            SvSql = "Select SHIFTMASTID,SHIFTNO from SHIFTMAST";
            DataTable dtt = new DataTable();
            OracleDataAdapter adapter = new OracleDataAdapter(SvSql, _connectionString);
            OracleCommandBuilder builder = new OracleCommandBuilder(adapter);
            adapter.Fill(dtt);
            return dtt;
        }
        public string GetDataString(String sql)
        {
            DataTable _dt = new DataTable();
            string str = string.Empty;
            try
            {
                _dt = GetData(sql);
                if (_dt.Rows.Count > 0)
                {
                    str = _dt.Rows[0][0].ToString();
                }
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return str;
        }
        public void sendemail(string Subject, string Message, string EmailID, string SenderID, string SenderPassword, string CompanySMTPPort, string SmtpEnableSsl, string sSmtpServer, string CompanyName)
        {
            string strerr = "";
            if (EmailID != "" && EmailID != null)
            {
                try
                {
                    MailMessage mail = new MailMessage();
                    System.Net.Mail.SmtpClient SmtpServer = new System.Net.Mail.SmtpClient(sSmtpServer);
                    mail.From = new MailAddress(SenderID, CompanyName);
                    mail.To.Add(EmailID);
                    mail.Subject = Subject;
                    StringBuilder sb3 = new StringBuilder();
                    sb3.Append(Message);
                    mail.Body = sb3.ToString();
                    AlternateView avHtml = AlternateView.CreateAlternateViewFromString(sb3.ToString(), null, MediaTypeNames.Text.Html);
                    mail.AlternateViews.Add(avHtml);
                    mail.IsBodyHtml = true;
                    SmtpServer.Port = Convert.ToInt32(CompanySMTPPort);
                    SmtpServer.Credentials = new System.Net.NetworkCredential(SenderID, SenderPassword);
                    SmtpServer.EnableSsl = Convert.ToBoolean(SmtpEnableSsl);
                    //SmtpServer.Timeout = 10000;
                    SmtpServer.Send(mail);
                    mail.Dispose();
                }
                catch (Exception ex)
                {
                    strerr = ex.Message;
                }
            }
        }
        public bool UpdateStatus(string query)
        {
            bool Saved = true;
            try
            {
                OracleConnection objConn = new OracleConnection(_connectionString);
                OracleCommand objCmd = new OracleCommand(query, objConn);
                objCmd.Connection.Open();
                objCmd.ExecuteNonQuery();
                objCmd.Connection.Close();
            }
            catch (Exception ex)
            {

                Saved = false;
            }
            return Saved;
        }

       
    }
}
