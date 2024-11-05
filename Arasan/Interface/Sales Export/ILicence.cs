using System.Collections.Generic;
using System.Collections;
using System.Data;
using Arasan.Models;


namespace Arasan.Interface
{
    public interface ILicence
    {
        DataTable GetItem();
        DataTable GetItemDetails(string id);
        DataTable GetExportDetails(string id);
        DataTable GetExportItem();
        DataTable GetSupplier();
        DataTable GetAllListLicenceItems(string strStatus);
        string StatusChange(string tag, string id);
        string ActStatusChange(string tag, string id);
        string LicenceCRUD(Licence cy);
        DataTable GetEditLicence(string id);
        DataTable GetLicenceImport(string id);
        DataTable GetLicenceExport(string id);
        DataTable GetLicenceImportView(string id);
        DataTable GetLicenceExportView(string id);
    }
}
