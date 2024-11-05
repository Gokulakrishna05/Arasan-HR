using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;

namespace Arasan.Interface
{
	public interface IExportEnquiry
	{
		string Export_EnquiryCRUD(ExportEnquiry cy);
        DataTable GetAllListExportEnquiry(string strStatus);
        DataTable GetCustomerDetails(string id);
        DataTable GetItem();
        DataTable GetItemDetails(string id);
        DataTable GetSupplier();
        string StatusChange(string tag, string id);
        string ActStatusChange(string tag, string id);
        IEnumerable<ExportItem> GetAllExportEnquiryItem(string id);
        DataTable GetExportEnquiry(string id);
        DataTable GetExportEnquiryItem(string id);
        DataTable GetExportItem(string id);
        DataTable GetParty(string id);
        DataTable GetPartyName(string id);
        DataTable GetExRateDetails(string id);
        DataTable GetExportEnquiryView(string id);
        DataTable GetCondition();
        DataTable Gettemplete();
    }
}
